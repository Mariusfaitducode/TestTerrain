using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraActions : MonoBehaviour
{
    //Deplacements mouv;
    private bool _isSelect;
    private bool _canMouv = false;
    public GameObject camera;
    
    void FixedUpdate()
    {
        if ( camera.GetComponent<Camera>().enabled )
        {
            _isSelect = true;
            if (_canMouv == false)
            {
                //mouv = gameObject.AddComponent<Deplacements>();
                _canMouv = true;
            }
            if (KeyRotation())
            {
                KeyDeplacement2(50f);
            }
        }
        else
        {
            _isSelect = false;
            Destroy(gameObject.GetComponent<Deplacements>());
            _canMouv = false;
        }
    }
    
    public void KeyDeplacement2(float vitesse)
    {
        if (!Input.GetKey(KeyCode.RightShift) && Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            

            transform.Translate(Vector3.forward * vitesse * Time.fixedDeltaTime *
                                Input.GetAxis("Vertical")); //Avance sur z

            transform.Translate(Vector3.right * vitesse * Time.fixedDeltaTime *
                                Input.GetAxis("Horizontal")); //Avance sur x
        }
    }
    
    public bool KeyRotation()
    {
        if (Input.GetKey(KeyCode.T))
        {
            
            transform.Rotate(Vector3.up * 90f * Time.fixedDeltaTime); //Tourne autour de y
            //transform.Rotate(Vector3.right * 90f * Time.fixedDeltaTime * Input.GetAxis("Vertical"));
            return true;
        }
        else if (Input.GetKey(KeyCode.R))
        {
            transform.Rotate(Vector3.up * -90f * Time.fixedDeltaTime);
            return true;
        }
        else
        {
            return false;
        }
    }
}
