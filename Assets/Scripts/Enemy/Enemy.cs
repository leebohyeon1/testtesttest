using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Enemy : MonoBehaviourPunCallbacks, IPunObservable
{
    SpriteRenderer spriteRenderer;
    Animator animator;
    [SerializeField]
    private float Speed = 10;

    public float MoveDirection = -1;

    public List<GameObject> TrackingPlayers;

    public float HP = 3;

    public float AttackDamage = 1;

    public bool HasKey = false;

    public string[] DropWeapons = {"sword","hammer","staff" };

    private bool IsDown = false;

    public PlatformUP BottomPlatform;

    // Start is called before the first frame update

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.rotation.eulerAngles.y);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, (float)stream.ReceiveNext(),0 );
        }
    }
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        if (HasKey && !IsDown)
        {
            Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
            if (viewPos.x < 0)
            {
                transform.position = new Vector3(BottomPlatform.transform.position.x, BottomPlatform.transform.position.y, transform.position.z);
                BottomPlatform.InEnemys.Add(this);
                IsDown = true;
            }

        }


        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (TrackingPlayers.Count == 0)
        {
            rb.velocity = new Vector3(Speed * MoveDirection, 0, 0);
            if(MoveDirection == 1)
                transform.rotation = Quaternion.Euler(0, 0, 0);
            else
                transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            GameObject TrackingPlayer = TrackingPlayers[TrackingPlayers.Count - 1];

            if (transform.position.x < TrackingPlayer.transform.position.x)
            {
                rb.velocity = new Vector3(Speed, 0, 0);
                MoveDirection = 1;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                rb.velocity = new Vector3(-Speed, 0, 0);
                MoveDirection = -1;
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
        if (PhotonNetwork.IsMasterClient)
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                PhotonView.Get(player.gameObject).RPC("GetDamage", player.gameObject.GetComponent<PhotonView>().Owner, AttackDamage);
            }
        }
    }

    [PunRPC]
    public void GetDamage(float Damage)
    {
        if (HP <= 0)
            return;

        HP -= Damage;
        if (HP <= 0)
        {
            animator.SetTrigger("IsDead");
            //PhotonView.Get(this).RPC("Die", RpcTarget.AllBuffered);
            if (HasKey)
                PhotonNetwork.Instantiate("Key", transform.position, new Quaternion());

            if (Random.Range(0.0f, 1.0f) > 0.0f)
            {
                PhotonNetwork.Instantiate(DropWeapons[Random.Range(0, DropWeapons.Length)] + "Drop", transform.position + new Vector3(0,3,0), new Quaternion());
            }

        }
    }
    public void ghostdead()
    {
        StartCoroutine(GhostDead());
    }
    public IEnumerator GhostDead()
    {
        if (PhotonNetwork.IsMasterClient)
            transform.GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveDirection * 15, 0);
        AttackDamage = 0;
        float fade = 1;
        for(int i = 0; i< 10; i++)
        {
            fade /= 1.1f;
            Color a = transform.GetComponent<SpriteRenderer>().color;
            a.a = fade;
            transform.GetComponent<SpriteRenderer>().color = a;
            yield return new WaitForSeconds(0.1f);
        }

        if (PhotonNetwork.IsMasterClient)
            PhotonView.Get(this).RPC("Die", RpcTarget.AllBuffered);
    }
    public void mummydead()
    {
        if(PhotonNetwork.IsMasterClient)
            PhotonView.Get(this).RPC("Die", RpcTarget.AllBuffered);
    }
    public void CandleDeath()
    {
        if (PhotonNetwork.IsMasterClient)
            transform.GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveDirection * 15, 0);

        if (PhotonNetwork.IsMasterClient)
            PhotonView.Get(this).RPC("Die", RpcTarget.AllBuffered);
    }
    public void bookdeath()
    {
        StartCoroutine(BookDeath());
    }
    public IEnumerator BookDeath()
    {
         if (PhotonNetwork.IsMasterClient)       
        transform.GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveDirection * 15, 0);
        if (PhotonNetwork.IsMasterClient)
            transform.GetChild(0).gameObject.SetActive(true);
        if (PhotonNetwork.IsMasterClient)
            transform.GetChild(0).GetComponent<Rigidbody2D>().velocity = new Vector2(-MoveDirection * 6, 0);     
        yield return new WaitForSeconds(1);
        if (PhotonNetwork.IsMasterClient)
            PhotonView.Get(this).RPC("Die", RpcTarget.AllBuffered);
    }
    [PunRPC]
    public void Die()
    {
        Destroy(gameObject);
    }
}
