using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Door : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Sprite OpenSprite;

    [SerializeField]
    private string NextScene;

    private bool IsOpen = false;
    private int ExitCount = 0;

    List<Photon.Realtime.Player> ExitPlayers = new List<Photon.Realtime.Player>();

    [SerializeField]
    private bool isLast = false;

    [SerializeField]
    private GameObject cutton;


    public void OnTriggerEnter2D(Collider2D other)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PlayerController pc = other.gameObject.GetComponent<PlayerController>();

            if (pc)
            {
                if (!IsOpen && pc.isKey)
                {
                    PhotonView.Get(this).RPC("OpenDoor", RpcTarget.AllBuffered);
                    IsOpen = true;
                    if(isLast)
                        cutton.SetActive(true);
                }

                if (IsOpen && !ExitPlayers.Contains(PhotonView.Get(pc.gameObject).Owner))
                {
                    ExitPlayers.Add(PhotonView.Get(pc.gameObject).Owner);
                    PhotonView.Get(pc).RPC("DestroyRPC", RpcTarget.AllBuffered);

                    PhotonView.Get(GameObject.Find("GlobalSound").GetComponent<GlobalSOund>()).RPC("PlaySound", PhotonView.Get(pc.gameObject).Owner, 0);

                    ExitCount++;
                    if (ExitCount == PhotonNetwork.CurrentRoom.PlayerCount)
                    {
                        StartCoroutine(invincibilityTime());
                        PhotonView.Get(this).RPC("OpenClearUI", RpcTarget.AllBuffered);
                        PhotonView.Get(GameObject.Find("GlobalSound").GetComponent<GlobalSOund>()).RPC("PlaySound", RpcTarget.AllBuffered, 1);
                    }
                }
            }

        }
    }
    public IEnumerator invincibilityTime()
    {
        yield return new WaitForSeconds(5.0f);
        PhotonNetwork.LoadLevel(NextScene);
    }

    [PunRPC]
    public void OpenDoor() 
    {
        GetComponent<SpriteRenderer>().sprite = OpenSprite;
        IsOpen = true;
    }

    [PunRPC]
    public void OpenClearUI() 
    {
        GameObject.Find("Canvas").transform.Find("ClearUI").gameObject.SetActive(true);
    }

}
