using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour {

    GameManager manager;

    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public void ToHowTo()
    {
        SceneManager.LoadScene(2);
    }

    public void ToMain()
    {
        SceneManager.LoadScene(1);
    }

    public void ToHost()
    {
        manager.Hosting();
    }

    public void ToJoin()
    {
        manager.Joining();
    }

    public void Enter()
    {
        manager.EnterRoom();
    }
}
