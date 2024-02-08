using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthDisplayer : MonoBehaviour
{
    [SerializeField] private RectTransform healthSlider;

    public void OnHealthChanged(int health)
    {
        healthSlider.anchorMax = new Vector2((float) health / PlayerController.instance.maxHealth, 1);
    }
}
