using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class InGamePhotonManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.Instantiate("Player", new Vector3(0,0,0), new Quaternion());
    }

}
