using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingGame : MonoBehaviour
{
    public Image LoadingBar;
    public TextMeshProUGUI text;
    void Start()
    {
        StartCoroutine(LoadingProgress());
    }

    IEnumerator LoadingProgress()
    {
        float progress = 0; 
        while(progress < 1)
        {
            progress += Time.deltaTime / 8;
            LoadingBar.fillAmount = progress;
            text.text = (int)(progress * 100) + "%";
            yield return null;
        }
        SceneManager.LoadScene("Main menu");
    }
    
}
