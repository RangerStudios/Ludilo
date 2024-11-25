using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WorldTextMarker : MonoBehaviour
{
    public void ChangeTextColor(Color color)
    {
        var text = GetComponent<TextMeshProUGUI>();
        text.color = color;
    }
}
