using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseCavasTest : MonoBehaviour
{
    private bool activate = false;

    public Personnage perso;
    
    // Update is called once per frame
    public void TogglePause()
    {
        if (!activate)
        {
            perso.enabled = false;
            activate = true;
        }
        else
        {
            perso.enabled = true;
            activate = false;
        }
    }
    
    
    
    
    
}
