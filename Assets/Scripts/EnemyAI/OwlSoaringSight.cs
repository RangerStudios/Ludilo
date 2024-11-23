using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwlSoaringSight : MonoBehaviour
{
    // Bool of whether the owl sees the player
    public bool seesBool;
    // Reference for Owl transform.
    public Transform self;
    // Reference for GameManager script.
    public GameManager gameManager;
    // Layermask for what the Raycast interacts with.
    public LayerMask sightMask;
    // Variable for how long timer should last.
    public float timer = 2f;
    
    // Private variable for current timer.
    [SerializeField]private float currentTimer;

    //Initializes Timer on Start. Initializes game manmager at start.
    public void Start()
    {
        currentTimer = timer;
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

    // Update timer over time.
    public void Update()
    {
        if (seesBool)
        {
            currentTimer -= Time.deltaTime;
            currentTimer = Mathf.Clamp(currentTimer, 0f, timer);
            if (currentTimer <= 0f)
            {
                OwlGrab();
            }
        }
        else
        {
            if (currentTimer < timer)
            {
                currentTimer += Time.deltaTime;
                currentTimer = Mathf.Clamp(currentTimer, 0f, timer);
            }
        }
    } 

    // As long as player remains in the sight and is hit by the raycast, set seesBool to true
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Waow");
            // Shoot Raycast to other
            // if it connects, set seen bool to true
            // if not, set bool to false.
            var direction = (other.transform.position - self.transform.position).normalized;
            var ray = new Ray(self.transform.position, direction);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, sightMask))
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    Debug.Log("Seen!");
                    SeesPlayer();
                }
                else
                {
                    DoesntSeePlayer();
                    Debug.Log("Player in Range, not seen.");
                }
            }
            else
            {
                DoesntSeePlayer();
                Debug.LogError("Raycast hit nothing.");
            }
        }
        else
        {
            DoesntSeePlayer();
            Debug.LogError("Something else is in the trigger that shouldn't.");
        }
    }

    public void SeesPlayer()
    {
        seesBool = true;
    }

    public void DoesntSeePlayer()
    {
        seesBool = false;
    }

    public void OwlGrab()
    {
        // Owl Gets You, call game manager for PlayerFell.
        gameManager.PlayerFell();
        DoesntSeePlayer();
        Debug.Log("Owl got you");
    }
}
