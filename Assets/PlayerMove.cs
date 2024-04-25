using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
   // public string fireButtonName = "Fire1";

    public float speed = 5f;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

    }
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            Vector3 direction = point - transform.position;
            direction.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        animator.SetFloat("Blend", Math.Abs(moveX) + Math.Abs(moveZ));
        //bool fire = Input.GetButton(fireButtonName);
        Vector3 pos = new Vector3(moveX, 0f, moveZ);
        pos.Normalize();
        transform.position += pos * speed * Time.deltaTime;

    }
}
