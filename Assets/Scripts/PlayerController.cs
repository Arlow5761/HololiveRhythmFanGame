using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

// Class that controls player behaviour
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
    public UnityEvent onDeath;
    public bool isDead;

    // Fever Variables
    public float feverDuration;
    [HideInInspector]
    public int maxFever;
    public int fever;
    public UnityEvent<int> onFeverChanged;
    public UnityEvent onFeverStarted;
    public UnityEvent onFeverEnded;
    private float feverTime;

    // How much air time the player has left before falling back down
    private float airTime;
    
    // Player state
    private bool whiff;
    private bool[] isSliding = {false, false};

    [SerializeField] private Animator animator;

    // Update is called once per frame
    void Update()
    {
        DownDelay();
        FeverDelay();
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
        if (isSliding[0])
        {
            return;
        }

        animator.SetInteger("RandomAttack", (animator.GetInteger("RandomAttack") + 1) % 3);
        animator.SetTrigger("Jump");

        AudioSource audioSource = AudioSystem.instance.GetAudio("sfx", "airattack");
        audioSource.PlayOneShot(audioSource.clip);

        whiff = true;
        lane = 1;
        transform.position = new Vector2(GameplayLayout.playerPosX, GameplayLayout.airLaneY);
        airTime = delay;
    }

    private void Land()
    {
        if (isSliding[1])
        {
            return;
        }

        AudioSource audioSource = AudioSystem.instance.GetAudio("sfx", "groundattack");
        audioSource.PlayOneShot(audioSource.clip);

        whiff = false;
        lane = 0;
        transform.position = new Vector2(GameplayLayout.playerPosX, GameplayLayout.groundLaneY);
        airTime = 0;
    }

    private void DownDelay()
    {
        if (isSliding[1])
        {
            airTime = delay;
            return;
        }

        if (airTime > 0)
        {
            airTime -= Time.deltaTime;

            if (airTime <= 0)
            {
                Land();
                animator.SetTrigger("Run");
            }
        }
    }

    private void FeverDelay()
    {
        if (feverTime > 0)
        {
            feverTime -= Time.deltaTime;

            if (feverTime <= 0)
            {
                feverTime = 0;
                fever = 0;
                onFeverEnded.Invoke();
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
        animator.SetTrigger("Hit");
    }

    public void ReleaseDown()
    {
        ProcessInput.instance.PollInputUp(0);
    }

    public void SetSliding(int slidingLane, bool sliding)
    {
        isSliding[slidingLane] = sliding;

        if (!isSliding[0] && !isSliding[1])
        {
            Land();

            animator.SetBool("Hold", false);
            animator.SetTrigger("Run");
            
            AudioSystem.instance.GetAudio("sfx", "holdloop").Stop();

            return;
        }

        if (!sliding && isSliding[1 - slidingLane])
        {
            switch (slidingLane)
            {
                case 0:
                    Jump();
                    break;
                case 1:
                    Land();
                    break;
                default:
                    break;
            }
        }

        if (isSliding[0] && isSliding[1])
        {
            transform.position = new Vector2(
                GameplayLayout.playerPosX, 
                (GameplayLayout.airLaneY + GameplayLayout.groundLaneY) / 2
            );
        }

        animator.SetBool("Hold", true);
        
        AudioSystem.instance.GetAudio("sfx", "holdloop").Play();
    }

    public void Damage(int damage)
    {
        if (isDead) return;

        health = math.clamp(health - damage, 0, maxHealth);
        onHealthChanged.Invoke(health);

        if (health == 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (isDead) return;

        health = math.clamp(health + amount, 0, maxHealth);
        onHealthChanged.Invoke(health);
    }

    public void Die()
    {
        isDead = true;
        onDeath.Invoke();
    }

    public void IncreaseFever(int amount)
    {
        fever = math.clamp(fever + amount, 0, maxFever);
        onFeverChanged.Invoke(fever);

        if (fever == maxFever)
        {
            feverTime = feverDuration;
            onFeverStarted.Invoke();
        }
    }

    // Function that is fired when the player misses a note
    public void OnNoteMiss(NoteData noteData, Grade grade)
    {

    }

    // Function that is fired when the player releases a note
    public void OnNoteRelease(NoteData noteData, Grade grade)
    {
        switch (noteData.NoteType)
        {
            case "Hold":
                SetSliding(noteData.RowNumber, false);
                if (grade.name != "Miss")
                {
                    IncreaseFever(Song.Instance.baseFeverIncrease);
                }
            break;
            default:
            break;
        }
    }

    // Function that is fired when the player presses a note
    public void OnNotePress(NoteData noteData, Grade grade)
    {
        switch (noteData.NoteType)
        {
            case "Hold":
                SetSliding(noteData.RowNumber, true);
                IncreaseFever(Song.Instance.baseFeverIncrease);
            break;
            default:
            break;
        }

        whiff = false;
    }

    // Function that is fired when the player passes a note
    public void OnNotePass(NoteData noteData, Grade grade)
    {
        switch (noteData.NoteType)
        {
            case "Normal":
                if (noteData.RowNumber == lane) Damage(Song.Instance.baseDamage);
            break;
            default:
            break;
        }
    }
}
