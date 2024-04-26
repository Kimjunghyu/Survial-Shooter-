using System.Collections;
using UnityEngine;

// ���� �����Ѵ�
public class Gun : MonoBehaviour
{
    public Transform fireTransform;

    public ParticleSystem muzzleFlashEffect;
    private LineRenderer bulletLineRenderer;

    private AudioSource gunAudioPlayer;

    private float fireDistance = 70f;
    private float lastFireTime; // ���� ���������� �߻��� ����
    private float timer = 0;

    private void Awake()
    {
        // ����� ������Ʈ���� ������ ��������
        bulletLineRenderer = GetComponent<LineRenderer>();
        gunAudioPlayer = GetComponent<AudioSource>();

        bulletLineRenderer.enabled = false;
        bulletLineRenderer.positionCount = 2;
    }

    private void OnEnable()
    {
        lastFireTime = 0f;
    }

    // �߻� �õ�
    public void Fire()
    {
        if (Time.time > lastFireTime + 0.1f)
        {
            lastFireTime = Time.time;
            Shot();
        }
    }

    // ���� �߻� ó��
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

    // �߻� ����Ʈ�� �Ҹ��� ����ϰ� �Ѿ� ������ �׸���
    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        // ���� �������� Ȱ��ȭ�Ͽ� �Ѿ� ������ �׸���
        bulletLineRenderer.SetPosition(0, fireTransform.position);
        bulletLineRenderer.SetPosition(1, hitPosition);
        bulletLineRenderer.enabled = true;

        muzzleFlashEffect.Play();
       // gunAudioPlayer.PlayOneShot(gunData.shotClip);
        yield return new WaitForSeconds(0.03f);
        bulletLineRenderer.enabled = false;
    }
}