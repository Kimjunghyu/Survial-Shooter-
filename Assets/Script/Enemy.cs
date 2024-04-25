using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent navMesh;
    public Transform player;
    private Animator animator;
    public float speed = 0.2f;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        navMesh = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            animator.SetBool("SetTarget", true);
        }
        navMesh.SetDestination(player.position);
        navMesh.speed = this.speed;

        Vector3 pos = player.position - transform.position;
        pos.Normalize();
        transform.rotation = Quaternion.LookRotation(transform.forward + pos);
        //transform.position = Vector3.Lerp(transform.position, player.position, Time.deltaTime *speed);
    }
}
