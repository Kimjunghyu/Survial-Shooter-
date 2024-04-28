using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    public Slider healthSlider;
    public Animator playerAnimator;
    private Rigidbody rb;
    public PlayerMove playerMovement;
    public event Action OnRespawn;
    private AudioSource playerAudioSource;

    public AudioClip hitClip;
    public AudioClip deadClip;


    private void Awake()
    {
       // healthSlider = GetComponent<Slider>();
       rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMove>();
        playerAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        healthSlider.gameObject.SetActive(true);
        healthSlider.value = startingHealth;
        playerMovement.enabled = true;
    }
    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        if (dead)
            return;
        Debug.Log(health);
        playerAudioSource.PlayOneShot(hitClip);
        base.OnDamage(damage, hitPoint, hitDirection);
        healthSlider.value = health;
        Debug.Log(healthSlider.value);
    }   

    // »ç¸Á Ã³¸®
    public override void Die()
    {
        base.Die();
        playerAudioSource.PlayOneShot(deadClip);
        rb.isKinematic = true;
        healthSlider.gameObject.SetActive(false);
        playerAnimator.SetTrigger("Die");
        playerMovement.enabled = false;
    }
    public void RestartLevel()
    {
        if (OnRespawn != null)
        {
            OnRespawn();
        }
        rb.isKinematic = false;
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
}
