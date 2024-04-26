using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiManager : MonoBehaviour
{
    public TextMeshProUGUI scoreUi;

    public static UiManager Instance
    {
        get
        {
            if (uInstance == null)
            {
                uInstance = FindObjectOfType<UiManager>();
            }

            return uInstance;
        }
    }
    private static UiManager uInstance;
    public void UpdateScoreUi(int i)
    {
        scoreUi.text = "Score : " + i;
    }
}
