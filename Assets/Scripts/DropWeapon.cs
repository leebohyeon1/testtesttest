using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DropWeapon : MonoBehaviourPunCallbacks
{
    public string WeaponName;

    private bool IsGeted = false;

    private float moveLife = 2;
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (PhotonNetwork.IsMasterClient && !IsGeted)
        {
            PlayerController pc = other.gameObject.GetComponent<PlayerController>();
            if (pc)
            {
                IsGeted = true;
                //¹«±â¸ÔÀÌ±â
                PhotonView.Get(pc.gameObject).RPC("GetWeapon", RpcTarget.AllBuffered,WeaponName);

                PhotonView.Get(this).RPC("Die", RpcTarget.AllBuffered);
            }
        }
    }

    public void Update()
    {
        if (PhotonNetwork.IsMasterClient && moveLife > 0)
        {
            transform.Translate(0, - Time.deltaTime * 1.5f, 0);
            moveLife -= Time.deltaTime;
        }
    }

    [PunRPC]
    public void Die()
    {
        Destroy(gameObject);
    }
}
