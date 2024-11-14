using UnityEngine;

public class ToySoldierAttack : MonoBehaviour
{
    void OnTriggerEnter (Collider other)
    {
        Debug.Log("Name: " + other.gameObject.name);
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<IDamageable>().Damage(1);
        }
        Destroy(this.gameObject);
    }
}
