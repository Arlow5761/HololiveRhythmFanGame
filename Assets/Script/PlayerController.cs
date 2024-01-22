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

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.F))
        {
            transform.position = new Vector2(xFixedPos, verticalUp);
            if(Time.time - Time.deltaTime >= 0.5f)
            {
                StartCoroutine(DownDelay());
            }
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
            comboBeat = 0;
            Destroy(collision.gameObject);
            health -= hitDamage;
        }
    }

    IEnumerator DownDelay()
    {
        yield return new WaitForSeconds(delay);

        transform.position = new Vector2(xFixedPos, -verticalDown);
    }
}
