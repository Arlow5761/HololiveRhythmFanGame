using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    //Movement Variables
    [SerializeField]
    private float delay;
    [SerializeField]
    private int lane;


    //PlayerController's Health Variables
    public int maxHealth;
    [HideInInspector]
    public int health = 100;
    public UnityEvent<int> onHealthChanged;

    // How much air time the player has left before falling back down
    private float airTime;
    
    // Whether or not an air attack whiffs
    private bool whiff;

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
        whiff = true;
        lane = 1;
        transform.position = new Vector2(GameplayLayout.playerPosX, GameplayLayout.airLaneY);
        if(Time.time - Time.deltaTime >= 0.5f) // Idk what this if does
        {
            airTime = delay;
        }
    }

    private void Land()
    {
        whiff = false;
        lane = 0;
        transform.position = new Vector2(GameplayLayout.playerPosX, GameplayLayout.groundLaneY);
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

    public int GetLane()
    {
        return lane;
    }

    // The following functions are used to respond inputs from InputHandler

    public void PressUp()
    {
        if (whiff) return;
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

    // This function is called when the player hits a note
    public void OnHitNote(Grade grade)
    {
        whiff = false;
    }

    public void Damage(int damage)
    {
        health = math.clamp(health - damage, 0, maxHealth);
        onHealthChanged.Invoke(health);
        Debug.Log(health);
    }

    public void Heal(int amount)
    {
        health = math.clamp(health + amount, 0, maxHealth);
        onHealthChanged.Invoke(health);
        Debug.Log(health);
    }
}
