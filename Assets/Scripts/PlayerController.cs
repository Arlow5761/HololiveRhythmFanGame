using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Movement Variables
    [SerializeField] 
    private float verticalUp;
    [SerializeField]
    private float verticalDown;
    [SerializeField]
    private float delay;
    private float xFixedPos = -9.5f;

    //PlayerController's Combo and Health Variables
    [SerializeField]
    private int hitDamage;
    [HideInInspector]
    public int health = 100;
    [HideInInspector]
    public int comboBeat = 0;

    // How much air time the player has left before falling back down
    private float airTime;

    // Update is called once per frame
    void Update()
    {
        DownDelay();
    }

    private void Jump()
    {
        transform.position = new Vector2(xFixedPos, verticalUp);
        if(Time.time - Time.deltaTime >= 0.5f) // Idk what this if does
        {
            airTime = delay;
        }
    }

    private void Land()
    {
        transform.position = new Vector2(xFixedPos, -verticalDown);
        airTime = 0;
    }

    private void DownDelay()
    {
        if (airTime > 0)
        {
            airTime -= Time.deltaTime;

            if (airTime <= 0)
            {
                Land();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Beat")
        {
            comboBeat = 0;
            Destroy(collision.gameObject);
            health -= hitDamage;
        }
    }

    // The following functions are used to respond inputs from InputHandler

    public void PressUp()
    {
        Jump();
        ProcessInput.instance.PollInputDown(1);
    }

    public void ReleaseUp()
    {
        ProcessInput.instance.PollInputUp(1);
    }

    public void PressDown()
    {
        Land();
        ProcessInput.instance.PollInputDown(0);
    }

    public void ReleaseDown()
    {
        ProcessInput.instance.PollInputUp(0);
    }
}
