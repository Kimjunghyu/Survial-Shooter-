using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : LivingEntity
{
    public LayerMask whatIsTarget;
    private NavMeshAgent navMesh;
    public Transform player;
    private Animator animator;
    public ParticleSystem hitEffect;
    private Rigidbody rigid;

    private GameObject playerGo;
    private LivingEntity targetEntity;
    //public AudioClip hitSound;
    private bool isTargeting = false;
    public float speed = 0.2f;


    private bool hasTarget
    {
        get
        {
            if (targetEntity != null && !targetEntity.dead)
            {
                return true;
            }

            return false;
        }
    }
    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        navMesh = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        targetEntity = GameObject.FindWithTag("Player").GetComponent<LivingEntity>();
        StartCoroutine(UpdatePath());
        dead = false;
    }
    private void Update()
    {
        animator.SetBool("SetTarget", hasTarget);
        if (player != null)
        {
            navMesh.speed = this.speed;
            Vector3 pos = player.position - transform.position;
            pos.Normalize();
            transform.rotation = Quaternion.LookRotation(transform.forward + pos);
        }
    }
    private IEnumerator UpdatePath()
    {
        while (!dead)
        {
            if (hasTarget)
            {
                navMesh.isStopped = false;
                navMesh.SetDestination(player.position);
            }
            else
            {
                navMesh.isStopped = true;
                var cols = Physics.OverlapSphere(transform.position, 10f, whatIsTarget);
                foreach (var col in cols)
                {
                    LivingEntity livingEntity = col.GetComponent<LivingEntity>();
                    if (livingEntity != null && !livingEntity.dead)
                    {
                        targetEntity = livingEntity;
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(0.25f);
        }
    }
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        hitEffect.transform.position = hitPoint;
        hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
        hitEffect.Play();
        //enemyAudioPlayer.PlayOneShot(hitSound);
        base.OnDamage(damage, hitPoint, hitNormal);
    }

    public override void Die()
    {
        base.Die();
        var cols = GetComponentsInChildren<Collider>();
        foreach (var col in cols)
        {
            col.enabled = false;
        }

        navMesh.isStopped = true;
        navMesh.enabled = false;

        animator.SetTrigger("Die");
        // enemyAudioPlayer.PlayOneShot(deathSound);
    }

    public void StartSinking()
    {
        StartCoroutine(OnDieEnemy());
    }

    private IEnumerator OnDieEnemy()
    {
        var cols = gameObject.GetComponentsInChildren<Collider>();
        foreach (var col in cols)
        {
            col.enabled = false;
        }

        rigid.isKinematic = false;
        rigid.useGravity = true;
        yield return new WaitForSeconds(4f);
        gameObject.SetActive(false);
    }
}
