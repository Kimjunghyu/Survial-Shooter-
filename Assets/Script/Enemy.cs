using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public AudioClip deadSound;
    public AudioClip hitSound;
    private AudioSource audioPlayer;

    private GameObject playerGo;
    private LivingEntity targetEntity;
    public float speed = 0.2f;
    public float damage = 20f;
    public float lastAttackTime;

    private GameManager gameManager;

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
        audioPlayer = GetComponent<AudioSource>();
    }

    private void Start()
    {
        gameManager = GetComponent<GameManager>();
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
            if (hasTarget && isActiveAndEnabled)
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
    public void Setup(float newHealth, float newDamage, float newSpeed)
    {
        startingHealth = newHealth;
        damage = newDamage;
        speed = newSpeed;
    }
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        audioPlayer.PlayOneShot(hitSound);
        hitEffect.transform.position = hitPoint;
        hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
        hitEffect.Play();
        base.OnDamage(damage, hitPoint, hitNormal);
    }

    public override void Die()
    {
        base.Die();
        audioPlayer.PlayOneShot(deadSound);
        var cols = GetComponentsInChildren<Collider>();
        foreach (var col in cols)
        {
            col.enabled = false;
        }

        navMesh.isStopped = true;
        navMesh.enabled = false;
        rigid.isKinematic = true;
        rigid.useGravity = true;
        animator.SetTrigger("Die");
        StopCoroutine(UpdatePath());
        GameManager.instance.AddScore(10);
    }

    public void Respawn()
    {
        gameObject.SetActive(true);
        var cols = GetComponentsInChildren<Collider>();
        foreach (var col in cols)
        {
            col.enabled = true;
        }
        rigid.isKinematic = false;
        rigid.useGravity = false;
        navMesh.enabled = true;
        navMesh.isStopped = false;

        StartCoroutine(UpdatePath());
    }

    public void StartSinking()
    {
        StartCoroutine(OnDieEnemy());
    }

    private IEnumerator OnDieEnemy()
    {
        rigid.isKinematic = false;
        rigid.useGravity = true;
        yield return new WaitForSeconds(4f);
        gameObject.SetActive(false);
    }
    private void OnTriggerStay(Collider other)
    {
        if (!dead && Time.time > lastAttackTime + 0.5f)
        {
            var entity = other.GetComponent<LivingEntity>();
            if (entity != null && entity == targetEntity)
            {
                var pos = transform.position;
                pos.y += 1f;
                var hitPoint = other.ClosestPoint(pos);
                var hitNormal = transform.position - other.transform.position;
                entity.OnDamage(damage, hitPoint, hitNormal.normalized);

                lastAttackTime = Time.time;
            }
        }
    }
}
