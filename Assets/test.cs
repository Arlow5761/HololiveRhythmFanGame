using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(FilesPanel.OpenFilePanel());
    }
}
