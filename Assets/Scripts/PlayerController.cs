using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    //Movement Variables
    [SerializeField] 
    private float verticalUp;
    [SerializeField]
    private float verticalDown;
    [SerializeField]
    private float delay;
    private float xFixedPos = -7;

    //PlayerController's Combo and Health Variables
    [SerializeField]
    private int hitDamage;
    [HideInInspector]
    public int health = 100;

    // How much air time the player has left before falling back down
    private float airTime;
    
    // Whether or not an air attack whiffs
    public bool whiff;

    // Update is called once per frame
    void Update()
    {
        DownDelay();
    }

    void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (instance != this && instance != null) return;

        instance = this;
    }

    private void Jump()
    {
        if (whiff) return;
        whiff = true;
        transform.position = new Vector2(xFixedPos, verticalUp);
        if(Time.time - Time.deltaTime >= 0.5f) // Idk what this if does
        {
            airTime = delay;
        }
    }

    private void Land()
    {
        whiff = false;
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
