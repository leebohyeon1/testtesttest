using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class RoomEntryManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject EntryUI;

    [SerializeField]
    private Text RoomNameText;

    [SerializeField]
    private GameObject EnterButton;

    [SerializeField]
    private Text ErrorText;

    [SerializeField]
    private GameObject RoomImage;

    [SerializeField]
    private Text RoomNameTextBlock;

    [SerializeField]
    private GameObject StartButton;

    bool IsGameLoaded = false;

    public GameObject[] spawns;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    { 
        EntryUI.SetActive(true);
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void OnClickEnterButton() 
    {
        PhotonNetwork.JoinOrCreateRoom(RoomNameText.text,new Photon.Realtime.RoomOptions { MaxPlayers = 4},null);
        EnterButton.SetActive(false);
    }

    public override void OnJoinedRoom()
    {
        RoomImage.SetActive(true);
        RoomNameTextBlock.text = "Room name: " + PhotonNetwork.CurrentRoom.Name;
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        EnterButton.SetActive(true);
        ErrorText.text = message;
    }



    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.InRoom)
        {

            for (int i = 0; i < 4; i++)
                    spawns[i].SetActive(PhotonNetwork.CurrentRoom.PlayerCount > i);

        }

        if (PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient && !IsGameLoaded)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount >= 1)
            {
                StartButton.SetActive(true);
                //PhotonNetwork.LoadLevel("Test1Scene");
                //IsGameLoaded = true;
            }
            else
                StartButton.SetActive(false);


        
        }
    }

    public void OnClickStartButton() 
    {
        if (PhotonNetwork.InRoom && PhotonNetwork.IsMasterClient && !IsGameLoaded)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel("Stage1");
            IsGameLoaded = true;
        }
    }
}
