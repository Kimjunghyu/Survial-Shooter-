using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject player;
    private Vector3 pos;
    private float offsetX = -1f;
    private float offsetY = 5f;
    private float offsetZ = -5f;

    // Update is called once per frame
    void Update()
    {
        pos = new Vector3(player.transform.position.x + offsetX, player.transform.position.y + offsetY, player.transform.position.z + offsetZ);
        transform.position = pos;
    }
}
