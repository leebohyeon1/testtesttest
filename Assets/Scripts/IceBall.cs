using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class IceBall : MonoBehaviourPunCallbacks
{

    public float Direction = 1;

    public float lifeTime = 3;

    private bool IsDead = false;

    // Update is called once per frame
    void Update()
    {
        if (PhotonView.Get(this).IsMine)
        {
            if (Direction == 1)
                transform.rotation = Quaternion.Euler(0, 0, -90);
            else
                transform.rotation = Quaternion.Euler(0, 0, 90);
            transform.Translate(new Vector3(0, Time.deltaTime * 15, 0));
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0 && !IsDead)
            {
                PhotonView.Get(this).RPC("Die", RpcTarget.AllBuffered);
                IsDead = true;
            }
        }
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (PhotonView.Get(this).IsMine && !IsDead)
        {
            PlayerController pc = other.gameObject.GetComponent<PlayerController>();
            if (pc && PhotonView.Get(this).Owner != PhotonView.Get(pc).Owner && !pc.Dontmove)
            {
                PhotonView.Get(pc).RPC("Freeze", RpcTarget.AllBuffered);
                PhotonView.Get(this).RPC("Die", RpcTarget.AllBuffered);
                IsDead = true;
            }

            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy)
            {
                PhotonView.Get(enemy).RPC("GetDamage", RpcTarget.MasterClient, 1.0f);
                PhotonView.Get(this).RPC("Die", RpcTarget.AllBuffered);
                IsDead = true;
            }

        }
    }

    [PunRPC]
    public void Die()
    {
        Destroy(gameObject);
    }
}
