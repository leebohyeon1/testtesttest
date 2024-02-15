using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlatformCorner : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private float Direct = 1;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if(enemy != null)
                enemy.MoveDirection=Direct;
        }
    }



}
