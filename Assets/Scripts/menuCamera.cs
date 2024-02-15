using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuCamera : MonoBehaviour
{
    // Start is called before the first frame update

    Vector3 startPosition;
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(3 * Time.deltaTime, 0, 0);
        if (transform.position.x > 100)
            transform.position = startPosition;
    }
}
