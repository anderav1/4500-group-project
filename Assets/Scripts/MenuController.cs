using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuController : MonoBehaviourPunCallbacks
{
    string gameVersion = "1";

    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    [SerializeField]
    private byte maxPlayersPerRoom = 4;

    [Tooltip("The Ui Panel to let the user enter name, connect and play")]
    [SerializeField]
    private GameObject controlPanel;

    public Text playerDisplay;


    void Awake()
    {
        print("MENU AWAKE");
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        print("MENU LOADED");
        controlPanel.SetActive(true);

        if (DBManager.LoggedIn)
        {
            playerDisplay.text = "Logged in as: " + DBManager.username;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Starts game if player hits button
    public void onStartButtonPress()
    {
        GameManager.inEditor = false; //Sets inEditor to false, because when we are starting the game at the start screen, we are either running a full build of the game outside of the editor, or we are wanting to test the game in actual multiplayer.
        
        if (DBManager.LoggedIn)
        {
            Connect();
        }
        else
        {
            playerDisplay.text = "You must log in to play";
        }
    }
    
    public void onHelpButtonPress() {
        SceneManager.LoadScene("Help");
    }

    void Connect()
    {
        print("MENU CONNECTED");
        controlPanel.SetActive(false);
        if(PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("MENU OnConnectedToMaster() was called by PUN");
        PhotonNetwork.JoinRandomRoom();
    }


    public override void OnDisconnected(DisconnectCause cause)
    {
        controlPanel.SetActive(true);
        Debug.LogWarningFormat("MENU OnDisconnected() was called by PUN with reason {0}", cause);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("MENU OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

        PhotonNetwork.CreateRoom(null, new RoomOptions {MaxPlayers = maxPlayersPerRoom });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("MENU OnJoinedRoom() called by PUN. Now this client is in a room.");
        PhotonNetwork.LoadLevel("Game");
    }

    public void GoToRegister()
    {
        SceneManager.LoadScene(1);
    }

    public void GoToLogin()
    {
        SceneManager.LoadScene(2);
    }
}
