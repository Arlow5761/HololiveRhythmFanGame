using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class EditorOffsetUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField offsetField;

    // Modifies the field's value to match the offset value
    public void OnOffsetModified(double offset)
    {
        offsetField.SetTextWithoutNotify(((int) (offset * 1000)).ToString());
    }

    // Modifies the offset value to match the field's value
    public void OnOffsetChanged(string offset)
    {
        EditorTimeController.instance.SetFirstTimestamp(double.Parse(offset) / 1000);
    }
}
