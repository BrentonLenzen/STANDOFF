using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameManager : NetworkBehaviour
{

    public bool gameOver;
    public bool hosting;
    public bool joining;
    public bool started;
    public List<gunButton> players = new List<gunButton>();
    public int numConnected;
    public string roomName;
    NetworkManager network;
    MatchMaking match;
    GameObject[] ui;
    List<GameObject> uiNetwork = new List<GameObject>();
    GameObject[] uiWait;

    public AudioSource bgAudio;
    public AudioClip menu;
    public AudioClip battle;


    // Use this for initialization
    void Start()
    {
        //stop phone from sleeping
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        //setup
        gameOver = false;
        started = false;
        numConnected = 0;
        network = GameObject.FindGameObjectWithTag("Network").GetComponent<NetworkManager>();
        match = GameObject.FindGameObjectWithTag("Network").GetComponent<MatchMaking>();
        match.StartMatchMaking();
        ui = GameObject.FindGameObjectsWithTag("UI");
        GameObject[] temp = GameObject.FindGameObjectsWithTag("NetworkUI");
        foreach (var i in temp)
        {
            uiNetwork.Add(i);
        }
        uiNetwork.Add(GameObject.FindGameObjectWithTag("Input"));
        uiWait = GameObject.FindGameObjectsWithTag("WaitUI");
        HideNetwork();
        HideWaiting();

        //audio
        bgAudio.clip = menu;
        bgAudio.Play();
    }

    //add players to list
    public void AddToList(gunButton player)
    {
        players.Add(player);
        AddPlayer();
    }

    //add gun to scene
    public void AddPlayer()
    {
        numConnected++;
        CheckGameStart();
    }

    //check if both players are connected
    void CheckGameStart()
    {
        if (numConnected >= 2)
        {
            HideWaiting();
            started = true;
        }
    }

    //called by gun button to end game
    public void EndGame()
    {
        gameOver = true;
        foreach (var player in players)
        {
            player.End();
        }
    }

    //called by gun button to check win conditions
    public bool IsOver()
    {
        return gameOver;
    }

    //remove buttons/logo
    void HideUI()
    {
        foreach (var obj in ui)
        {
            obj.SetActive(false);
        }
        ShowNetwork();
    }

    //show/hide network related UI
    void ShowNetwork()
    {
        foreach (var obj in uiNetwork)
        {
            obj.SetActive(true);
        }
    }

    void HideNetwork()
    {
        foreach (var obj in uiNetwork)
        {
            obj.SetActive(false);
        }
    }

    //show/hide waiting for oppenent UI
    void ShowWaiting()
    {
        foreach (var obj in uiWait)
        {
            obj.SetActive(true);
        }
    }

    void HideWaiting()
    {
        foreach (var obj in uiWait)
        {
            obj.SetActive(false);
        }
        bgAudio.clip = battle;
        bgAudio.Play();
    }

    //set if hosting or joining
    public void Hosting()
    {
        hosting = true;
        HideUI();
    }

    public void Joining()
    {
        joining = true;
        HideUI();
    }

    //have player enter room based on name
    public void EnterRoom()
    {
        InputField field = GameObject.FindGameObjectWithTag("Input").GetComponent<InputField>();
        roomName = field.text;
        HideNetwork();
        ShowWaiting();
        if (hosting)
        {
            match.CreateRoom(roomName);
        }
        else if (joining)
        {
            match.FindRoom(roomName);
        }
    }

    //change if game is over
    void Update()
    {
        int numDisconected = 0;
        foreach(var gun in players)
        {
            if (gun == null && started)
                numDisconected++;
        }
        if(numDisconected > 0)
        {
            SceneManager.LoadScene(3);
            NetworkManager.singleton.client.Disconnect();
        }
    }
}