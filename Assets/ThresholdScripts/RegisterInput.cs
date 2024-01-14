using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterInput : MonoBehaviour
{
    public ProcessInput ProcessTarget;
    void Update()
    {
        

        if (Input.GetKeyDown(KeyCode.D)) 
        {
            ProcessTarget.Down(0);
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            ProcessTarget.Down(0);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            ProcessTarget.Down(1);
        }
        
        if (Input.GetKeyDown(KeyCode.K))
        {
            ProcessTarget.Down(1);
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            ProcessTarget.Release(0);
        }
        
        if (Input.GetKeyUp(KeyCode.F))
        {
            ProcessTarget.Release(0);
        }

        if (Input.GetKeyUp(KeyCode.J))
        {
            ProcessTarget.Release(1);
        }
        
        if (Input.GetKeyUp(KeyCode.K))
        {
            ProcessTarget.Release(1);
        }
    }
}
