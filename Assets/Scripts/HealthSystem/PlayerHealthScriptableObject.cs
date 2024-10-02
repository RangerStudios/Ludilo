using UnityEngine;

[CreateAssetMenu(fileName = "PlayerHealthScriptableObject", menuName = "PlayerHealthTracker")]
public class PlayerHealthScriptableObject : ScriptableObject
{
    public int currentHealth = 6;
    public int currentStuffing = 0;
}
