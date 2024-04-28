using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform fireTransform;

    public ParticleSystem muzzleFlashEffect;
    private LineRenderer bulletLineRenderer;

    private AudioSource gunAudioPlayer;
    public AudioClip fireClip;
    private float fireDistance = 70f;
    private float lastFireTime;
    private void Awake()
    {
        bulletLineRenderer = GetComponent<LineRenderer>();
        gunAudioPlayer = GetComponent<AudioSource>();

        bulletLineRenderer.enabled = false;
        bulletLineRenderer.positionCount = 2;
    }

    private void OnEnable()
    {
        lastFireTime = 0f;
    }
    public void Fire()
    {
        if (Time.time > lastFireTime + 0.1f)
        {
            lastFireTime = Time.time;
            gunAudioPlayer.PlayOneShot(fireClip);
            Shot();
        }
    }

    private void Shot()
    {
        if(Time.timeScale > 0)
        {
        var hitPoint = Vector3.zero;
        var ray = new Ray(fireTransform.position, fireTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, fireDistance))
        {
            hitPoint = hitInfo.point;
            var damagable = hitInfo.collider.GetComponent<Damagable>();
            if (damagable != null)
            {
                damagable.OnDamage(20, hitPoint, hitInfo.normal);
            }
        }
        else
        {
            hitPoint = fireTransform.position + fireTransform.forward * fireDistance;
        }
        StartCoroutine(ShotEffect(hitPoint));
        }
        
    }

    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        bulletLineRenderer.SetPosition(0, fireTransform.position);
        bulletLineRenderer.SetPosition(1, hitPosition);
        bulletLineRenderer.enabled = true;

        muzzleFlashEffect.Play();
        yield return new WaitForSeconds(0.03f);
        bulletLineRenderer.enabled = false;
    }
}