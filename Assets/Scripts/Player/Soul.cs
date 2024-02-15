using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviourPunCallbacks
{
    Camera cameraa;
    Animator animator;
    void Start()
    {
        cameraa = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        animator = GetComponent<Animator>();
        if (photonView.IsMine)
        {
            StartCoroutine(Respawn());
        }
    }

    public void Update()
    {
        Vector3 v = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        transform.position = new Vector3(v.x, v.y, transform.position.z);
    }

    [PunRPC]
    public void Die()
    {
        Destroy(gameObject);
    }
    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(5);     
        transform.position = new Vector3(cameraa.transform.position.x, cameraa.transform.position.y);
        animator.SetTrigger("isResurr");
    }
    public void respawn()
    {
        if (!photonView.IsMine)
            return;
        PhotonNetwork.Instantiate("Player", new Vector3(cameraa.transform.position.x, cameraa.transform.position.y), Quaternion.identity);
        PhotonView.Get(this).RPC("Die", RpcTarget.AllBuffered);
    }
}
