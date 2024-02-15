using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject weapon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    [PunRPC]
    void GetWeapon(string weaponName)
    {
        weapon = Resources.Load<GameObject>(weaponName);
        Instantiate(weapon);
    }
}
