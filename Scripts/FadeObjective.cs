using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeObjective : MonoBehaviour
{
    //Private Variable
    private CanvasGroup canvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (SceneManager.GetActiveScene().name == "BestTime" || SceneManager.GetActiveScene().name == "Timer")
        {
            StartCoroutine(DoFade());
        }
    }

    public void StartFade()
    {
        StartCoroutine(DoFade());
    }

    public void ResetFade()
    {
        canvasGroup.alpha = 1;
        gameObject.SetActive(false);
    }

    public IEnumerator DoFade()
    {
        yield return new WaitForSeconds(2.5f);

        while(canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime / 2;
            yield return null;
        }
    }
}
