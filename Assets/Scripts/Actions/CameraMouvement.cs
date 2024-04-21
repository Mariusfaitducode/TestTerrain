using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouvement : MonoBehaviour
{
    //public bool activate;

    public bool KeyDeplacement()
    {
        
        
        if (Input.GetKey(KeyCode.U))
        {
            transform.Translate(Vector3.forward * 15f * Time.fixedDeltaTime); //Avance sur z
            
            return true;
        }
        else if (Input.GetKey(KeyCode.H))
        {
            transform.Translate(Vector3.forward * -15f * Time.fixedDeltaTime); //Avance sur x
            
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool KeyRotation()
    {
        if (Input.GetKey(KeyCode.Y))
        {
            
            transform.Rotate(Vector3.right * 90f * Time.fixedDeltaTime); //Tourne autour de y
            //transform.Rotate(Vector3.right * 90f * Time.fixedDeltaTime * Input.GetAxis("Vertical"));
            return true;
        }
        else if (Input.GetKey(KeyCode.G))
        {
            transform.Rotate(Vector3.right * -90f * Time.fixedDeltaTime);
            return true;
        }
        else
        {
            return false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        KeyRotation();
        KeyDeplacement();
    }
}
