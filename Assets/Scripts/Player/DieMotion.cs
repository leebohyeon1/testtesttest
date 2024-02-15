using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieMotion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DIe()
    {
        if(PhotonView.Get(this).IsMine)
            PhotonView.Get(this).RPC("Die", RpcTarget.AllBuffered);
    }
    [PunRPC]
    public void Die()
    {
        Destroy(gameObject);
    }
}
