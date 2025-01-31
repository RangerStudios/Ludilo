using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lure : MonoBehaviour
{
    // Float for current timer value. Used in update.
    [SerializeField] float currentTimer;
    // Float for how long timer should be. Default is 3 seconds. Can be modified.
    public float pingTime = 3.0f;
    // Float for spherecast radius. Default is 3. Can be modified.
    public float castRadius = 3.0f;
    // Color of Gizmo sphere.
    public Color sphereColor = Color.white;
    // Layermask of just enemies.
    public LayerMask enemyMask;
    // Layermask for line of sight raycast. Can be adjusted.
    public LayerMask sightMask;

    // Ensures currentTimer is set properly on start.
    void Start()
    {
        ResetTimer();
    }

    // Increments timer. When it exceeds the specified timer, perform a "Ping" and reset the timer.
    void Update()
    {
        currentTimer += Time.deltaTime;
        if (currentTimer >= pingTime)
        {
            Ping();
            ResetTimer();
        }
    }

    // Sends out a lure to any lurable enemies in range. First checks range, then checks sight.
    void Ping()
    {
        RaycastHit[] enemies= Physics.SphereCastAll(transform.position, castRadius, Vector3.forward, 0f, enemyMask);

        foreach(RaycastHit hit in enemies)
        {
            RaycastHit sight;
            if (Physics.Raycast(transform.position, (hit.collider.transform.position - transform.position).normalized, out sight, castRadius, sightMask))
            {

                ILureable lureable = sight.collider.GetComponent<ILureable>();
                if (lureable != null)
                {
                    lureable.OnLure(transform);
                    //Debug.Log("Lure Target Located");
                }
                //Debug.Log("Ping!");
            }
            
        }
        
    }

    // Resets the current timer to zero.
    void ResetTimer()
    {
        currentTimer = 0f;
    }

    // Gizmo for seeing sphere
    void OnDrawGizmos()
    {
        sphereColor.a = 0.5f;
        Gizmos.color = sphereColor;
        Gizmos.DrawSphere(transform.position, castRadius);
    }
}
