using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : LivingEntity
{
    private float rotCamXAxisSpeed = 5f;
    private float rotCamYAxisSpeed = 3f;
    private float minX = -80;
    private float maxX = 50;

    private float currX;
    private float currY;

    public Gun gun;
    public float speed = 5f;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        if(Time.timeScale > 0)
        {
            if (Input.GetButton("Fire1"))
            {
                gun.Fire();
            }

            float mouseX = Input.GetAxis("Mouse X") * rotCamXAxisSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotCamYAxisSpeed;
            CalculateRotation(mouseX, mouseY);

            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");
            Vector3 moveDirection = transform.forward * moveZ + transform.right * moveX;
            moveDirection.Normalize();
            animator.SetFloat("Blend", Mathf.Abs(moveX) + Mathf.Abs(moveZ));
            transform.position += moveDirection * speed * Time.deltaTime;

        }
    }

    public void CalculateRotation(float mouseX, float mouseY)
    {
        currY += mouseX;
        currX -= mouseY;
        currX = Mathf.Clamp(currX, minX, maxX);
        transform.rotation = Quaternion.Euler(currX, currY, 0);
    }
}
