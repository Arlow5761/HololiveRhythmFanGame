using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterInput : MonoBehaviour
{
    public ProcessInput ProcessTarget;
    void Update()
    {
        if (Input.GetButtonDown("D")) 
        {
            ProcessTarget.Down(0);
        }

        if (Input.GetButtonDown("F"))
        {
            ProcessTarget.Down(0);
        }

        if (Input.GetButtonDown("J"))
        {
            ProcessTarget.Down(1);
        }
        
        if (Input.GetButtonDown("K"))
        {
            ProcessTarget.Down(1);
        }

        if (Input.GetButtonUp("D"))
        {
            ProcessTarget.Release(0);
        }
        
        if (Input.GetButtonUp("F"))
        {
            ProcessTarget.Release(0);
        }

        if (Input.GetButtonUp("J"))
        {
            ProcessTarget.Release(1);
        }
        
        if (Input.GetButtonUp("K"))
        {
            ProcessTarget.Release(1);
        }
    }
}
