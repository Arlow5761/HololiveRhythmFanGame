using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class that handles the in progress screen for unfinished content
public class InProgressDisplayer : MonoBehaviour
{
    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
