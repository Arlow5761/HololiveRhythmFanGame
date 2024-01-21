using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] 
    private float verticalUp;
    [SerializeField]
    private float verticalDown;

    private float xFixedPos = -9.5f;
    private float delay = 0.5f;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.F))
        {
            transform.position = new Vector2(xFixedPos, verticalUp);
            StartCoroutine(DownDelay());
        }
        else if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K))
        {
            transform.position = new Vector2(xFixedPos, -verticalDown);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Beat")
        {
            Destroy(collision.gameObject);
        }
    }

    IEnumerator DownDelay()
    {
        yield return new WaitForSeconds(delay);

        transform.position = new Vector2(xFixedPos, -verticalDown);
    }
}
