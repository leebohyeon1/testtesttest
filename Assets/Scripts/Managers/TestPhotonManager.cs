using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class TestPhotonManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }


    public override void OnConnectedToMaster()
    {
        Debug.Log("joined to Server");
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.JoinOrCreateRoom("TestRoom8", new Photon.Realtime.RoomOptions { MaxPlayers = 4 }, null); 
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("joined to room");
        PhotonNetwork.Instantiate("Player",new Vector3(0,0,0),new Quaternion());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
