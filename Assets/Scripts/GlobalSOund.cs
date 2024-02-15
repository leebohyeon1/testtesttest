using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class GlobalSOund : MonoBehaviourPunCallbacks
{
    public AudioClip[] sounds;
    [PunRPC]
    void PlaySound(int i) 
    { 
        GetComponent<AudioSource>().clip = sounds[i];
        GetComponent<AudioSource>().Play();
    }

    public void Update()
    {
        transform.position = Camera.main.transform.position;
    }
}
