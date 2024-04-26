using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private UiManager uiManager;
    private int score = 0;

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
