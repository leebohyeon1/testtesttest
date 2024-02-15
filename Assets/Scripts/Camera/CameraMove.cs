using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    CameraMove instance;
    public bool isEnd;
    public float Speed;
    public float StartTime;
    public Transform cameraTransform;
    void Start()
    {
        cameraTransform = Camera.main.transform;
        StartTime = 3;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
        if(StartTime >= 0 )
        StartTime -= Time.deltaTime;
    }
    public void MoveCamera()
    {
        if(!isEnd && StartTime <= 0)
        cameraTransform.position = this.transform.position + new Vector3(Time.deltaTime * Speed, 0);
    }

}
