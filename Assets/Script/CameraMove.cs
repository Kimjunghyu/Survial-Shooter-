using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset = new Vector3(0f, 0.6f, 0.3f);
    void Update()
    {
        transform.position = player.transform.position + offset;
        transform.rotation = Quaternion.LookRotation(player.transform.forward);
    }
}
