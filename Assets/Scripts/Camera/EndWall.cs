using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EndWall : MonoBehaviour
{
    public bool OnScreen;
    public CameraMove Cameramove;
    Camera cameraa;
    Transform target;
    // Start is called before the first frame update
    void Start()
    {
        target = GetComponent<Transform>();
        Cameramove = GameObject.FindWithTag("MainCamera").GetComponent<CameraMove>();
        cameraa = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }


    // Update is called once per frame
    void Update()
    {
        if (OnScreen)
        {
            Cameramove.isEnd = true;
        }
        Vector3 viewPos = cameraa.WorldToViewportPoint(target.position);

        if (viewPos.x >= 0 && viewPos.x <= 1 &&
            viewPos.y >= 0 && viewPos.y <= 1 &&
            viewPos.z > 0)
        {
            OnScreen = true;
        }
    }
}
