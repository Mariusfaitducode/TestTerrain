using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{

    public GameObject planeStart;
    public GameObject newPlane;
    public Canvas canvasMenu;
    public GameObject explorateur;

    public void NextWorld()
    {
        planeStart.SetActive(false);
        newPlane.SetActive(true);
        
        explorateur.SetActive(true);
        
        canvasMenu.gameObject.SetActive(false);
    }

}
