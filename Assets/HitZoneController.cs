using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitZoneController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Pencet");
            Destroy(collision.gameObject);
            Debug.Log("Deleted");
        }
    }
}
