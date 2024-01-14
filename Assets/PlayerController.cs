using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] 
    private float velocity;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.F))
        {
            //rb.AddForce(new Vector2(0, velocity));
            transform.position = new Vector2(0, velocity);
        }
        else if (Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.K))
        {
            //rb.AddForce(new Vector2(0, -velocity));
            transform.position = new Vector2(0, -velocity);
        }
    }
}
