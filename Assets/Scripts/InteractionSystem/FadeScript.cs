
using System.Collections;
using UnityEngine;

public class FadeScript : MonoBehaviour //This script is used for trigger based scene transitions, such as the "thanks for playing" as seen in the Vertical Slice.
{                                       
    public Animator fadeAnimator;
    public GameObject thanksMessage;
    //public Animator thanksAnimator; //for said UI element that changes alpha channel to fade in?

    void Update()
    {
        
    }

    public void FadeLoadScene(string sceneToLoad)
    {
        StartCoroutine(FadeToScene(sceneToLoad));
    }
    public IEnumerator FadeToScene(string sceneToLoad)
    {
        fadeAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1.0f);
        //thanksAnimator.SetTrigger("FadeInText");
        yield return new WaitForSeconds(5.0f);
        GameManager.Instance.LoadScene(sceneToLoad);
    }
}
