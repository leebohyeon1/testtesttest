using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlatformUP : MonoBehaviourPunCallbacks
{
    public List<Enemy> InEnemys;

    public void OnTriggerStay2D(Collider2D other) 
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (other.gameObject.GetComponent<PlayerController>())
            {
                foreach (Enemy enemy in InEnemys)
                { 
                    if(!enemy.TrackingPlayers.Contains(other.gameObject))
                        enemy.TrackingPlayers.Add(other.gameObject);
                }
            }
        
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (other.gameObject.GetComponent<PlayerController>())
            {
                foreach (Enemy enemy in InEnemys)
                {
                    if (enemy.TrackingPlayers.Contains(other.gameObject))
                        enemy.TrackingPlayers.Remove(other.gameObject);
                }
            }

        }
    }
}
