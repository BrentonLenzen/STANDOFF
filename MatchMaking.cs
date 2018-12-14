using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;

public class MatchMaking : MonoBehaviour {

    NetworkManager network;
    GameManager manager;

    public void StartMatchMaking()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        NetworkManager.singleton.StartMatchMaker();
    }
	
    //hosting
    public void CreateRoom(string roomName)
    {
        NetworkManager.singleton.matchMaker.CreateMatch(roomName, 2, true, "", "", "", 0, 1, OnRoomCreate);
    }

    void OnRoomCreate(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        print("call back");
        if (success)
        {
            Debug.Log("Room Created");

            MatchInfo hostInfo = matchInfo;

            NetworkServer.Listen(hostInfo, 9000);

            NetworkManager.singleton.StartHost(hostInfo);
        }
        else
        {
            Debug.LogError("Create room failed");
            SceneManager.LoadScene(5);
        }
    }

    //joining
    public void FindRoom(string roomName)
    {
        NetworkManager.singleton.matchMaker.ListMatches(0, 20, roomName, false, 0, 1, OnRoomList);
    }

    void OnRoomList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {
        if (success)
        {
            if (matchList.Count != 0)
            {
                //Debug.Log("A list of matches was returned");

                //join most recently created
                NetworkManager.singleton.matchMaker.JoinMatch(matchList[matchList.Count - 1].networkId, "", "", "", 0, 1, OnRoomJoined);
            }
            else
            {
                Debug.Log("No matches in requested room!");
                SceneManager.LoadScene(5);
            }
        }
    }

    void OnRoomJoined(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (success)
        {
            //Debug.Log("Able to join a match");

            MatchInfo hostInfo = matchInfo;
            NetworkManager.singleton.StartClient(hostInfo);
        }
        else
        {
            Debug.LogError("Join match failed");
            SceneManager.LoadScene(5);
        }
    }
}
