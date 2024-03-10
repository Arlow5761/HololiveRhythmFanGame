using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EditorBeatDivisorUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField divisorField;

    // Sets the UI value to the internal value
    public void OnBeatDivisorModified(int beatDivisor)
    {
        divisorField.SetTextWithoutNotify(beatDivisor.ToString());
    }

    // Sets the internal value to the UI value
    public void OnBeatDivisorChanged(string beatDivisor)
    {
        EditorTimeController.instance.SetBeatDivisor(int.Parse(beatDivisor));
    }
}
