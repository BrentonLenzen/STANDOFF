using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class gunButton : NetworkBehaviour {

    public bool shot;
    public GameManager manager;

    void Start()
    {
        if (isLocalPlayer)
        {
            transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform, false);
            transform.SetAsFirstSibling();
        }
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        manager.AddToList(this);
    }


    public void OnMouseDown()
    {
        shot = true;
        NetworkManager.singleton.client.Disconnect();
        manager.EndGame();
    }

    //check game over
    public void End()
    {
        if (manager.IsOver() && isLocalPlayer)
        {
            manager.players.Remove(this);
            //lose
            if (shot)
            {
                SceneManager.LoadScene(4);
            }
            //win
            else
            {
                SceneManager.LoadScene(3);
            }
        }
    }
}
