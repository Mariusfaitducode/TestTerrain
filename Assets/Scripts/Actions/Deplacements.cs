using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Actions;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Deplacements : MonoBehaviour
{
    private float height;
    public float vitesse;

    public int level;

    public PlateauJeu plateau;

    public GameObject player;

    private void Update()
    {
        // Calcul de la position de l'obstacle et de l'objet
        Vector3 obstaclePos = player.transform.position;
        Vector3 objectPos = transform.position;
        
        int size = Constants.GetConstant(level);
        Case actualCase = ReturnCase(size);

        RectifHeight(actualCase);

        
        // Calcul de la direction vers laquelle l'objet doit se déplacer pour éviter l'obstacle
        Vector3 avoidanceDirection = objectPos - obstaclePos;
        //avoidanceDirection = Vector3.ProjectOnPlane(avoidanceDirection, Vector3.up);

        if (avoidanceDirection.magnitude < 40)
        {
            print("Vector");
            print(avoidanceDirection);
            // Appliquer une force dans la direction d'évitement
            //GetComponent<Rigidbody>().AddForce(avoidanceDirection.normalized * 10f * vitesse);
            RotateAndTranslate2(avoidanceDirection.normalized);
            
            
        }
        
    }
    
    

    public void RotateAndTranslate(Vector3 vect)
    {
        // Faire pivoter le personnage vers l'avant en utilisant Rigidbody.MoveRotation()
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward);
        GetComponent<Rigidbody>().MoveRotation(targetRotation);

        // Appliquer une force vers l'avant en utilisant Rigidbody.AddForce()
        GetComponent<Rigidbody>().AddForce(vect.normalized * (40 - vect.magnitude) * Time.deltaTime * vitesse);
    }
    public void RotateAndTranslate2(Vector3 vect)
    
    { // Faire pivoter le personnage vers l'avant en utilisant Transform.LookAt()
        transform.LookAt(transform.position + vect);

        // Déplacer le personnage vers l'avant en utilisant Transform.Translate()
        transform.Translate(Vector3.forward * vitesse * Time.deltaTime);
    }

    /*void FixedUpdate()
    {
        int size = Constants.GetConstant(level);
        Case actualCase = ReturnCase(size);
        
        //Key Mouvement
        
        //KeyDeplacement1(actualCase);
        //KeyRotation();

        IA_mouv2(actualCase);

        RectifHeight(actualCase);
    }*/

    public void RectifHeight(Case actualCase)
    {
        float newHeight = actualCase.surfaceHeight;
        
        print("height fox = "+newHeight);
        print("region = "+ actualCase.typeRegion.height);
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, newHeight,
            gameObject.transform.position.z);
    }

    public void IA_mouv(Case actualCase)
    {
        Vector3 vect = transform.position - player.transform.position;

        if (vect.magnitude < 40)
        {
            transform.Translate(vect * vitesse * Time.fixedDeltaTime);
            
            RectifHeight(actualCase);
            
            double cos = Vector3.forward.magnitude / vect.magnitude;

            //double alpha = Math.Acos(cos);
        }
        double alpha = VectorAngle.AngleBetween(Vector3.forward, vect);
        print("alpha = " + alpha);
        
        if (alpha > 0.5f)
        {
            transform.Rotate(Vector3.up * -10f * Time.fixedDeltaTime);
        }

        


    }
    
    public void IA_mouv2(Case actualCase)
    {
        Vector3 vect = transform.position - player.transform.position;
        double alpha = VectorAngle.AngleBetween(Vector3.forward, vect);
        print("alpha = " + alpha);

        if (vect.magnitude < 40)
        {

            if (alpha < 1.5)
            {
                transform.Translate(vect * vitesse * Time.fixedDeltaTime);
            
                RectifHeight(actualCase);
            }
            else
            {
                transform.Rotate(Vector3.up * -10f * (float)alpha * Time.fixedDeltaTime);
            }
        }
        transform.Rotate(Vector3.up * -5f * Time.fixedDeltaTime);
   
    }
    
    
    

    public void KeyDeplacement1(Case actualCase)
    {
        if (!Input.GetKey(KeyCode.RightShift) && Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            print("mouv");
            Vector3 lastPos = transform.position;

            transform.Translate(Vector3.forward * vitesse * Time.fixedDeltaTime *
                                Input.GetAxis("Vertical")); //Avance sur z

            transform.Translate(Vector3.right * vitesse * Time.fixedDeltaTime *
                                Input.GetAxis("Horizontal")); //Avance sur x

            height = transform.position.y;

            float newHeight = actualCase.caseCube.transform.position.y * 2;

            //print(height + " // " + newHeight);

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
                gameObject.transform.position = lastPos;
            }
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


    public void KeyRotation()
    {
        if (Input.GetKey(KeyCode.T))
        {
            
            transform.Rotate(Vector3.up * 90f * Time.fixedDeltaTime); //Tourne autour de y
            //transform.Rotate(Vector3.right * 90f * Time.fixedDeltaTime * Input.GetAxis("Vertical"));
        }
        else if (Input.GetKey(KeyCode.R))
        {
            transform.Rotate(Vector3.up * -90f * Time.fixedDeltaTime);
        }
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
}
