using System.Collections;
using UnityEngine;

// 총을 구현한다
public class Gun : MonoBehaviour
{
    public Transform fireTransform;

    public ParticleSystem muzzleFlashEffect;
    private LineRenderer bulletLineRenderer;

    private AudioSource gunAudioPlayer;

    private float fireDistance = 70f;
    private float lastFireTime; // 총을 마지막으로 발사한 시점
    private float timer = 0;

    private void Awake()
    {
        // 사용할 컴포넌트들의 참조를 가져오기
        bulletLineRenderer = GetComponent<LineRenderer>();
        gunAudioPlayer = GetComponent<AudioSource>();

        bulletLineRenderer.enabled = false;
        bulletLineRenderer.positionCount = 2;
    }

    private void OnEnable()
    {
        lastFireTime = 0f;
    }

    // 발사 시도
    public void Fire()
    {
        if (Time.time > lastFireTime + 0.1f)
        {
            lastFireTime = Time.time;
            Shot();
        }
    }

    // 실제 발사 처리
    private void Shot()
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

    // 발사 이펙트와 소리를 재생하고 총알 궤적을 그린다
    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        // 라인 렌더러를 활성화하여 총알 궤적을 그린다
        bulletLineRenderer.SetPosition(0, fireTransform.position);
        bulletLineRenderer.SetPosition(1, hitPosition);
        bulletLineRenderer.enabled = true;

        muzzleFlashEffect.Play();
       // gunAudioPlayer.PlayOneShot(gunData.shotClip);
        yield return new WaitForSeconds(0.03f);
        bulletLineRenderer.enabled = false;
    }
}