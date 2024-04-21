using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personnage : MonoBehaviour
{
    public PlateauJeu plateau;
    
    private string name;
    
    //private enum type {Explorateur};
    //private float life;
    //private int mouvements; //portée du joueur
    //private int actions; //nbr d'actions du joueur
    
    private bool _isSelect;

    public float vitesse = 150f;

    private float height;

    private Vector3 m_input;

    private Rigidbody m_rigid;
    //Deplacements mouv;

    public Camera defaultCam;
    public Camera playerCam;

    public bool exit;

    public int level;
    
/*
    public Vector3 ReturnPlateauPos()
    {
        float tabX = transform.position.x + Constants.MapWidth * Constants.CaseSize / 2;
        float tabY = transform.position.z + Constants.MapWidth * Constants.CaseSize / 2;
        Vector3 position = new Vector3( tabX, transform.position.y, tabY);

        return position;
    }*/

    public void Start()
    {
        transform.position = new Vector3(0, 60, 0);
        m_rigid = gameObject.GetComponent<Rigidbody>();
    }

    public Vector2 ReturnCaseRef(int size)
    {
        int tabX = (int)((transform.position.x ) / Constants.CaseSize + size/2)  ;
        int tabY = (int)((transform.position.z )/ Constants.CaseSize + size/2);
        Vector2 position = new Vector3( tabX, tabY);

        return position;
    }

    public Case ReturnCase(int size)
    {
        int i = (int)ReturnCaseRef(size).x;
        int j = (int)ReturnCaseRef(size).y;
        return plateau.grid[i, j];
    }
    private void OnMouseDown() //Choix de la caméra -> controle du personnage
    {
        if (!_isSelect)
        {
            defaultCam.enabled = false;
            playerCam.enabled = true;
            _isSelect = true;
            //mouv = gameObject.AddComponent<Deplacements>();
        }
        else
        {
            playerCam.enabled = false;
            defaultCam.enabled = true;
            _isSelect = false;
            Destroy(gameObject.GetComponent<Deplacements>());
        }
    }

    private void Update()
    {
        //orthographic_vision();
        m_input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        //KeyRotation();
        
        //if (gameObject.)

        if (_isSelect)
        {
            int size = Constants.GetConstant(level);

            height = transform.position.y;

            if (exit)
            {
                transform.position = new Vector3(0, 60, 0);
                exit = false;
                level += 1;
            }
            
            /*float newHeight = ReturnCase(size).caseCube.transform.position.y * 2;
            
            //transform.position.y = newHeight;
            
            if (height + 4f >= newHeight)
            {
                //print("ok");
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, newHeight,
                    gameObject.transform.position.z);
                //print(actualCase.typeRegion.name);
                height = newHeight;
            }
            else
            {
                //print("ko");
                //gameObject.transform.position = lastPos;
            }*/
            
            
            //m_rigid.AddForce(m_input * vitesse, ForceMode.Force);
            KeyDeplacement3(vitesse);
            KeyRotation();
            //KeyZoom();
            
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
    
    public void KeyDeplacement3(float vitesse)
    {
        if (!Input.GetKey(KeyCode.RightShift) && !Input.GetKey(KeyCode.LeftShift) && 
            (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0))
        {
            

            transform.Translate(Vector3.forward * vitesse * Time.fixedDeltaTime *
                                Input.GetAxis("Vertical")); //Avance sur z

            transform.Translate(Vector3.right * vitesse * Time.fixedDeltaTime *
                                Input.GetAxis("Horizontal")); //Avance sur x
        }
    }
}
