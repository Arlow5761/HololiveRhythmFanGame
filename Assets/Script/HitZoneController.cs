using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitZoneController : MonoBehaviour
{
    [SerializeField]
    private PlayerController playerController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.F)) && collision.gameObject.tag == "Beat")
        {
            Destroy(collision.gameObject);
            playerController.comboBeat++;
            Debug.Log(playerController.comboBeat);
        }
    }
}
