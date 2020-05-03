using UnityEngine;
using UnityEngine.SceneManagement;

public class Fading : MonoBehaviour
{
    //Public Variable
    public Animator animator;

    //Private Variable
    private string levelToLoad;

    public void FadeToLevel(string level)
    {
        levelToLoad = level;
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
