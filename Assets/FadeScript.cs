
using System.Collections;
using UnityEngine;

public class FadeScript : MonoBehaviour //DEAD SCRIPT, BASIS FOR SCENE FADING. FUNCTIONALITY NOW IN "MenuController"
{
    public Animator fadeAnimator;

    void Update()
    {
        
    }

    public IEnumerator FadeToScene(string sceneToLoad)
    {
        fadeAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1.0f);
        OnFadeCompletion(sceneToLoad);
    }

    public void OnFadeCompletion(string sceneToLoad)
    {
        GameManager.Instance.LoadScene(sceneToLoad);
    }
}
