using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Class that handles the fever bar
public class FeverDisplayer : MonoBehaviour
{
    [SerializeField] private RectTransform feverSlider;

    [SerializeField] private Color normalColor;
    [SerializeField] private Color feverColor;

    public void OnFeverChanged(int fever)
    {
        feverSlider.anchorMax = new Vector2((float) fever / PlayerController.instance.maxFever, 1);
    }

    public void OnFeverStarted()
    {
        feverSlider.anchorMax = new Vector2(1,1);
        feverSlider.GetComponent<Image>().color = feverColor;
    }

    public void OnFeverEnded()
    {
        feverSlider.anchorMax = new Vector2(0,1);
        feverSlider.GetComponent<Image>().color = normalColor;
    }
}

