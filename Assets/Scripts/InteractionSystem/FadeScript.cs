
using System.Collections;
using UnityEngine;

public class FadeScript : MonoBehaviour //This script is used for trigger based scene transitions, such as the "thanks for playing" as seen in the Vertical Slice.
{                                       
    public Animator fadeOutAnimator;
    public GameObject thanksMessage;
    public GameObject player;
    public PlayerController playerController;
    //public Animator thanksAnimator; //for said UI element that changes alpha channel to fade in?

    void Awake()
    {
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
    }
    void Update()
    {
        
    }

    public void FadeLoadScene(string sceneToLoad)
    {
        StartCoroutine(FadeToScene(sceneToLoad));
    }
    public IEnumerator FadeToScene(string sceneToLoad)
    {
        fadeOutAnimator.SetTrigger("FadeOut");
        player.GetComponent<CapsuleCollider>().enabled = true;
        playerController.canAttack = false;
        playerController.characterController.enabled = false;
        yield return new WaitForSeconds(1.0f);
        fadeOutAnimator.SetTrigger("FadeIn");
        thanksMessage.SetActive(true);
        yield return new WaitForSeconds(5.0f);
        GameManager.Instance.LoadScene(sceneToLoad);
    }
}
