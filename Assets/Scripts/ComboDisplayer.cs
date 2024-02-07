using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ComboDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI comboField;

    public void onComboChanged(int newCombo)
    {
        comboField.SetText(newCombo.ToString());

        if (newCombo == 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
