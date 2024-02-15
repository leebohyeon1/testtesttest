using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Key : MonoBehaviourPunCallbacks
{
    private bool IsGeted = false;

    private float RightPower = 0;
    private float UpPower = 0;

    public void Update()
    {
        if (PhotonNetwork.IsMasterClient && !IsGeted)
        {
            Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
            if (viewPos.x < 0) 
            {
                RightPower = 10;
            }
            if (viewPos.y < 0)
            {
                UpPower = 10;
            }
        }

        if (RightPower > 0)
        { 
            transform.Translate(new Vector3(RightPower * Time.deltaTime,0,0));
            RightPower -= Time.deltaTime * 10;
        }

        if (UpPower > 0)
        {
            transform.Translate(new Vector3(0, UpPower * Time.deltaTime, 0));
            UpPower -= Time.deltaTime * 10;
        }

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (PhotonNetwork.IsMasterClient && !IsGeted)
        {
            PlayerController pc = other.gameObject.GetComponent<PlayerController>();
            if (pc)
            {
                IsGeted = true;
                //pc한테 키 먹이기
                PhotonView.Get(pc.gameObject).RPC("GetKey",RpcTarget.AllBuffered);
   
                PhotonView.Get(this).RPC("Die", RpcTarget.AllBuffered);
            }
        }
    }

    [PunRPC]
    public void Die()
    {
        Destroy(gameObject);
    }
}
