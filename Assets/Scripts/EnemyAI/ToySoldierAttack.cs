using UnityEngine;

public class ToySoldierAttack : MonoBehaviour
{
    public void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<IDamageable>().Damage(1);
        }
        Destroy(gameObject);
    }
}
