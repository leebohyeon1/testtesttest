using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Device;

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    public Rigidbody2D Rigidbody;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    Camera CameraView;
    Transform target;
    public LayerMask Ground;
    public LayerMask Wall;

    public float Speed;
    public float JumpSpeed;
    public bool invincibility;
    public float AttackArrange;
    public float Axis;
    public float HP;
    public bool isGround;
    public bool isKey;
    public GameObject key;
    public GameObject _Freeze;
    public float AttackSpeed;
    float HandPos;
    float attackRotation;
    public string Weapon = "hand";
    bool isattack;
    bool flipx;
    public bool Dontmove;
    public bool isConfused;
    public bool ispush;
    public bool isDamaged;
    public GameObject HandObject;

    public AudioClip[] Sounds;
    Color color1;

    void Awake()
    {
        target = GetComponent<Transform>();
        CameraView = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        Rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Axis = 1;
        HP = 10;
        flipx = spriteRenderer.flipX = Axis == -1;
        attackRotation = 30;
    }
    void Start()
    {
        for (int i = 0; i < 3; i++) { color1[i] = Random.Range(0, 1); }

        if (photonView.IsMine)
        {
            transform.GetComponent<SpriteRenderer>().color = new Color(Random.value, Random.value, Random.value, 1f);
        }
    }
    void Update()
    {
        booleanSystem();
        InputSystem();
        OutMap();
        //flipx = spriteRenderer.flipX = Axis == -1;
        if (attackRotation >= 30)
        {
            isattack = true;   
        }
        else if (attackRotation <= -40)
        {
            isattack = false;       
        }
        if (isattack)
        {
            attackRotation -= Time.deltaTime * AttackSpeed;
        }
        else
        {
            attackRotation += Time.deltaTime * AttackSpeed;
        }

        if (photonView.IsMine)
            if (transform.position.y < -10 || transform.position.y > 10)
                GetDamage(100);

        if (photonView.IsMine)
        {
            if (Axis == 1)
                transform.rotation = Quaternion.Euler(0, 0, 0);
            else
                transform.rotation = Quaternion.Euler(0, 180, 0);
        }

    }
    public void booleanSystem()
    {
        if(Dontmove)
        {
            animator.speed = 0.0f;
            Axis = 0;
        }
        else if (!Dontmove)
        { 
            animator.speed = 1f;
        }
        if (ispush)
        {
            Axis = 1;
        }

    }
    public void InputSystem()
    {
        if (photonView.IsMine)
        {
            float axis = Input.GetAxisRaw("Horizontal");
            if (axis != 0 && !Dontmove && !ispush)
            {
                if (isConfused)
                {
                    Axis = -axis;
                }
                else 
                {
                    Axis = axis;
                }
                photonView.RPC("FlipXRPC", RpcTarget.AllBuffered, Axis);
                //spriteRenderer.flipX = Axis == -1;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) && isGround && !Dontmove && !ispush)
            {
               // Rigidbody.velocity = new Vector2(0, JumpSpeed);
                photonView.RPC("JumpRPC", RpcTarget.All);
                animator.SetBool("isJump", true);
                transform.GetComponent<BoxCollider2D>().isTrigger = true;
                StartCoroutine(JumpReturn(1.0f));
            }

            if(!Input.GetKey(KeyCode.DownArrow) &&  ( !Input.GetKey(KeyCode.UpArrow) || (isGround && GetComponent<Rigidbody2D>().velocity.y < 0) )  )
                transform.GetComponent<BoxCollider2D>().isTrigger = false;



            if (Physics2D.Raycast(new Vector2(transform.position.x,transform.position.y), -transform.up, 0.6f, Ground) || Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), -transform.up,0.6f, Wall)) //�ٴ� ����
            {
                isGround = true;
            }
            else
            {
                isGround = false;            
            }
            if(Input.GetKey(KeyCode.DownArrow) && isGround )
            {
                transform.GetComponent<BoxCollider2D>().isTrigger = true;
                animator.SetBool("isJump", true);
                StartCoroutine(JumpReturn(0.5f));
            }



            if (Axis == -1)
            {
                HandPos = 180;
            }
            else
            {
                HandPos = 0;
            }
        }
        Rigidbody.velocity = new Vector2(Axis * Speed, Rigidbody.velocity.y);
    }
    public void OutMap()
    {
        if (photonView.IsMine)
        {
            Vector3 viewPos = CameraView.WorldToViewportPoint(transform.position);

            if (viewPos.x <= 0)
            {                
                StartCoroutine(outMap());
            }
            if (viewPos.x >= 1)
            {
                //Axis = 1;
                Rigidbody.velocity = new Vector2(-2, 0);

            }
        }
    }
    public IEnumerator outMap()
    {
        if(photonView.IsMine)
            GetDamage(1);
        ispush = true;
        Axis = 1;
        Rigidbody.velocity = new Vector2(15, 15);
        transform.GetComponent<BoxCollider2D>().isTrigger = false;
        yield return new WaitForSeconds(1.5f);
        ispush = false;
        animator.SetBool("isJump", false);
        transform.GetComponent<BoxCollider2D>().isTrigger = false;
    }

    public void HandShake()
    {
        transform.GetChild(0).GetComponent<Transform>().rotation = Quaternion.Euler(0, HandPos, attackRotation);
    }
    public IEnumerator JumpReturn(float time)
    {
        yield return new WaitForSeconds(time);
        animator.SetBool("isJump", false);
        //transform.GetComponent<BoxCollider2D>().isTrigger = false;
    }
    public IEnumerator invincibilityTime() 
    {
        invincibility = true;
        isDamaged = true;
        yield return new WaitForSeconds(0.5f);
        invincibility = false;
        isDamaged = false;
    }
    public IEnumerator EMarkerGrid()
    {
        Color color = transform.GetComponent<SpriteRenderer>().color;
        color.a = 0.5f;
        yield return new WaitForSeconds(0.5f);
        color.a = 1;
    }
    [PunRPC]
    void FlipXRPC(float Axis)
    { 
        //spriteRenderer.flipX = Axis == 1;
    }

    [PunRPC]
    void JumpRPC()
    {
        Rigidbody.velocity = new Vector2(0, JumpSpeed);
    }
    [PunRPC]
    public void GetDamage(float Damage)
    {
        if (invincibility)
        {
            return;
        }
        if (HP <= 0)
            return;

        HP -= Damage;
        StartCoroutine(invincibilityTime());
        StartCoroutine(EMarkerGrid());
        if (HP <= 0)
        {
            if (isKey)
                PhotonView.Get(this).RPC("SpawnKey", RpcTarget.MasterClient);

            isKey = false;

            Die();
            GameObject die = PhotonNetwork.Instantiate("DieMotion", transform.position, Quaternion.identity);
            die.GetComponent<SpriteRenderer>().color = transform.GetComponent<SpriteRenderer>().color;
            PhotonView.Get(this).RPC("DestroyRPC", RpcTarget.AllBuffered);
        }
    }
    [PunRPC]
    public void GetKey()
    {
        isKey = true;
        key.SetActive(true);
    }

   
    public void Die()
    {
        PhotonNetwork.Instantiate("Soul", transform.position, Quaternion.identity);
    }
    [PunRPC]
    public void DestroyRPC()
    {

        Destroy(gameObject);
    }

    [PunRPC]
    void GetWeapon(string weaponName)
    {
        foreach (Transform child in HandObject.transform)
        {
            Destroy(child.gameObject);
        }
        Weapon = weaponName;
        GameObject w = Instantiate(Resources.Load<GameObject>(weaponName));
        w.transform.SetParent(HandObject.transform);
        w.transform.localPosition = Vector3.zero;
        w.transform.localRotation = Quaternion.identity;
    }

    [PunRPC]
    public void SpawnKey()
    {
        PhotonNetwork.Instantiate("Key", transform.position + new Vector3(2, 0, 0), Quaternion.identity);
    }

    [PunRPC]
    public void PlaySound(int soundIndex)
    {
        HandObject.GetComponent<AudioSource>().clip = Sounds[soundIndex];
        HandObject.GetComponent<AudioSource>().Play();
    }

    [PunRPC]
    public void SpawnAttackEffect(Vector3 Position)
    {
        GameObject g = Instantiate(Resources.Load<GameObject>("AttackEffect"));
        g.transform.position = Position + new Vector3(0,0,-10);
        if (Position.x > transform.position.x)
            g.transform.rotation = Quaternion.Euler(0, 180, 0);
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.rotation.eulerAngles.y);
            stream.SendNext(HandPos);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, (float)stream.ReceiveNext(), 0);
            HandPos = (float)stream.ReceiveNext();
        }
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if(flipx)
        { Gizmos.DrawWireSphere(new Vector2(transform.position.x - 1, transform.position.y), AttackArrange); }
        else
        {
            Gizmos.DrawWireSphere(new Vector2(transform.position.x + 1, transform.position.y), AttackArrange);
        }
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x,transform.position.y-0.6f,0));
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
            CircleRPCToPlayer("UnableAct");
        }
        else if (Weapon == "staff")
        {
            PhotonNetwork.Instantiate("IceBall", transform.position, Quaternion.identity).GetComponent<IceBall>().Direction = Axis;
            PhotonView.Get(this).RPC("PlaySound", RpcTarget.AllBuffered, 7);
        }
        else if (Weapon == "bell")
        {
            CircleAttack(1);
            CircleRPCToPlayer("Confused");
            PhotonView.Get(this).RPC("PlaySound", RpcTarget.AllBuffered, 0);
        }
        else if (Weapon == "fan")
        {
            CircleAttack(1);
            CircleRPCToPlayerFan("Pushout");
        }


    }

    void CircleAttack(float Damage)
    {
        Vector2 attackOrigin = new Vector2(transform.position.x + Axis,transform.position.y);

        RaycastHit2D[] hits = Physics2D.CircleCastAll(attackOrigin, 1, new Vector2(), 0);

        foreach (RaycastHit2D hit in hits)
        {
            Enemy t = hit.collider.gameObject.GetComponent<Enemy>();
            if (t)
            {
                PhotonView.Get(t).RPC("GetDamage", RpcTarget.MasterClient, Damage);
                PhotonView.Get(this).RPC("SpawnAttackEffect", RpcTarget.AllBuffered, t.transform.position);

                if (Weapon == "hand")
                    PhotonView.Get(this).RPC("PlaySound", RpcTarget.AllBuffered, 10);
                if (Weapon == "hammer")
                    PhotonView.Get(this).RPC("PlaySound", RpcTarget.AllBuffered, 6);
                if (Weapon == "sword")
                    PhotonView.Get(this).RPC("PlaySound", RpcTarget.AllBuffered, 10);
                if (Weapon == "bell")
                    PhotonView.Get(this).RPC("PlaySound", RpcTarget.AllBuffered, 10);
                if (Weapon == "fan")
                    PhotonView.Get(this).RPC("PlaySound", RpcTarget.AllBuffered, 11);
            }
        }

    }

    void CircleAttackToPlayer(float Damage)
    {
        Vector2 attackOrigin = new Vector2(transform.position.x + Axis, transform.position.y);

        RaycastHit2D[] hits = Physics2D.CircleCastAll(attackOrigin, 1, new Vector2(), 0);

        foreach (RaycastHit2D hit in hits)
        {
            PlayerController t = hit.collider.gameObject.GetComponent<PlayerController>();
            if (t && t != this)
            {
                PhotonView.Get(t).RPC("GetDamage", PhotonView.Get(t).Owner, Damage);
                PhotonView.Get(this).RPC("SpawnAttackEffect", RpcTarget.AllBuffered, t.transform.position);
                if (Weapon == "sword")
                    PhotonView.Get(this).RPC("PlaySound", RpcTarget.AllBuffered, 10);
            }
        }

    }

    void CircleRPCToPlayer(string RPCName)
    {
        Vector2 attackOrigin = new Vector2(transform.position.x + Axis, transform.position.y);

        RaycastHit2D[] hits = Physics2D.CircleCastAll(attackOrigin, 1, new Vector2(), 0);

        foreach (RaycastHit2D hit in hits)
        {
            PlayerController t = hit.collider.gameObject.GetComponent<PlayerController>();
            if (t && t != this)
            {
                if (RPCName == "UnableAct" && t.Dontmove)
                    return;
                if (RPCName == "Confused" && t.isConfused)
                    return;
                PhotonView.Get(t).RPC(RPCName, RpcTarget.AllBuffered);
                PhotonView.Get(this).RPC("SpawnAttackEffect", RpcTarget.AllBuffered, t.transform.position);

                if (Weapon == "hammer")
                    PhotonView.Get(this).RPC("PlaySound", RpcTarget.AllBuffered, 6);
                if (Weapon == "bell")
                    PhotonView.Get(this).RPC("PlaySound", RpcTarget.AllBuffered, 0);
            }
        }

    }

    void CircleRPCToPlayerFan(string RPCName)
    {
        Vector2 attackOrigin = new Vector2(transform.position.x + Axis, transform.position.y);

        RaycastHit2D[] hits = Physics2D.CircleCastAll(attackOrigin, 1, new Vector2(), 0);

        foreach (RaycastHit2D hit in hits)
        {
            PlayerController t = hit.collider.gameObject.GetComponent<PlayerController>();
            if (t && t != this)
            {
                if (RPCName == "Pushout" && t.Dontmove)
                    return;
                PhotonView.Get(t).RPC(RPCName, RpcTarget.AllBuffered,this.transform.position);
                PhotonView.Get(this).RPC("SpawnAttackEffect", RpcTarget.AllBuffered, t.transform.position);
                if (Weapon == "fan")
                    PhotonView.Get(this).RPC("PlaySound", RpcTarget.AllBuffered, 11);
            }
        }

    }


    #region 상태이상
    [PunRPC]
    public void UnableAct()
    {
        Dontmove = true;
        transform.localScale = new Vector3(1, 0.22f);

        if (isKey)
        {
            isKey = false;
            key.SetActive(false);
            if(photonView.IsMine)
                PhotonView.Get(this).RPC("SpawnKey", RpcTarget.MasterClient);
        }
        

        StartCoroutine(unableact());
    }

    [PunRPC]
    public void Freeze()
    {
        Dontmove = true;
        _Freeze.SetActive(true);

        if (isKey)
        {
            isKey = false;
            key.SetActive(false);
            if (photonView.IsMine)
                PhotonView.Get(this).RPC("SpawnKey", RpcTarget.MasterClient);
        }

        StartCoroutine(Warm());
    }
    public IEnumerator Warm()
    {
        yield return new WaitForSeconds(5);
        _Freeze.SetActive(false);
        Axis = 1;
        Dontmove = false;
    }
    public IEnumerator unableact()
    {
        yield return new WaitForSeconds(5);
        Dontmove = false;
        transform.localScale = new Vector3(1, 1);
        Axis = 1;
    }

    [PunRPC]
    public void Pushout(Vector3 Targettransform)
    {
        Vector3 dir = Targettransform - transform.position;
        dir.Normalize();
        dir.z = 0;
        Dontmove = true;
        Rigidbody.velocity = new Vector3(-(dir.x * 100), -(dir.y * 100));
        StartCoroutine(UnPushout());
    }

    public IEnumerator UnPushout()
    {
        yield return new WaitForSeconds(1);
        Dontmove = false;
        Axis = 1;
    }

    [PunRPC]
    public void Slow()
    {
        float speed = Speed;
        Speed /= 2;
        StartCoroutine(slowslow(speed));
    }
    public IEnumerator slowslow(float speed)
    {
        yield return new WaitForSeconds(10);
        Speed = speed;
    }

    [PunRPC]
    public void Confused()
    {
        isConfused = true;
        StartCoroutine(DeConfused());
    }

    public IEnumerator DeConfused()
    {
        yield return new WaitForSeconds(3);
        isConfused = false;
    }
    #endregion
}

