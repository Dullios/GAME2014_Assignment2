using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, -10.0f);
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }
}
