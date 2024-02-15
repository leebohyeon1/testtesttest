using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class tmp : MonoBehaviourPunCallbacks
{
    public string Weapon;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void attack() 
    {
        if (!PhotonView.Get(this).IsMine)
            return;

        if (Weapon == "hand")
        {
            CircleAttack(1);
        }
        else if (Weapon == "sword")
        {
            CircleAttack(1);
            CircleAttackToPlayer(1);
        }
        else if (Weapon == "hammer")
        {
            CircleAttack(1);
            //플레이어 짜부시키기
        }
        else if (Weapon == "staff")
        {
            PhotonNetwork.Instantiate("IceBall", transform.position, new Quaternion()).GetComponent<IceBall>().Direction = 1;
        }


    }

    void CircleAttack(float Damage)
    {
        Vector2 attackOrigin = new Vector2();

        RaycastHit2D[] hits = Physics2D.CircleCastAll(attackOrigin, 10, new Vector2(), 0, 0);

        foreach (RaycastHit2D hit in hits)
        {
            Enemy t = hit.rigidbody.gameObject.GetComponent<Enemy>();
            if (t)
            {
                PhotonView.Get(t).RPC("GetDamage", RpcTarget.MasterClient, Damage);
            }
        }

    }

    void CircleAttackToPlayer(float Damage)
    {
        Vector2 attackOrigin = new Vector2();

        RaycastHit2D[] hits = Physics2D.CircleCastAll(attackOrigin, 10, new Vector2(), 0, 0);

        foreach (RaycastHit2D hit in hits)
        {
            PlayerController t = hit.rigidbody.gameObject.GetComponent<PlayerController>();
            if (t && t != this)
            {
                PhotonView.Get(t).RPC("GetDamage", PhotonView.Get(t).Owner, Damage);
            }
        }

    }

}
