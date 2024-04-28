using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private UiManager uiManager;
    private AudioSource audioSource;
    public AudioClip bgm;
    private int score = 0;
    public GameObject go;

    private float maxVol = 100f;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        audioSource.clip = bgm;
        audioSource.maxDistance = maxVol;
        audioSource.Play();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0f;
            go.SetActive(true);
        }
    }

    public static GameManager instance
    {
        get
        {
            if (gInsctance == null)
            {
                gInsctance = FindObjectOfType<GameManager>();
            }

            return gInsctance; 
        }
        
    }
    private static GameManager gInsctance;
    public void AddScore(int i)
    {
        score += i;
        UiManager.Instance.UpdateScoreUi(score);
    }
}
