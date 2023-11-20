using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ValueToText : MonoBehaviour
{
    TextMeshProUGUI text;
    public string valueDefinition;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void SetText(float value)
    {
        text.text = valueDefinition + ": " + value.ToString();
    }
}
