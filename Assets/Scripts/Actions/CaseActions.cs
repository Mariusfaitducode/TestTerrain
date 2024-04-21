using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class CaseActions : MonoBehaviour
{
    public Personnage player;

    //public PlateauJeu plateau;

    public Inventaire invent;

    private bool cutTree = false;
    private bool putTree = false;

    public AfficheInvent.ItemOnScreen selected;

    public bool firstAction;
    private bool lastAction;

    public EditObject edit;

    public GameObject fox;

    private bool putFox;

   

    public bool GetCutTree(){return cutTree;}
    public bool GetPutTree(){return putTree;}
    
    public void SetCutTree(bool cut){cutTree = cut;}
    public void SetPutTree(bool put){putTree = put;}

    void Start()
    {
        //player = gameObject.GetComponent<Personnage>();
        selected.name = default;
        firstAction = true;
        
    }
    
    // Update is called once per frame
    void Update()
    {
        
       PutObject();
       if (player.level == 2)
       {
           PutFox();
           TransformationFox();
       }
       
       
    }

    public void OnTriggerStay(Collider other)
    {
        
        TakeObject(other);
        
    }

    public void TakeObject(Collider other)
    {
        //cutTree = false;
        if (selected.name == default && Input.GetKey(KeyCode.Space) &&
                      !other.gameObject.CompareTag("CaseCube") && !other.gameObject.CompareTag("Sortie"))
        {
            print("tag = "+other.tag+"  name = "+other.name);
            if (other.gameObject.layer.Equals(20)) //layer animal 
            {
                print("catch fox");
                Inventaire.Item item = invent.FindWithName("fox");
                
                invent.IncrementQuantite(item);
            
                //invent.DispList();
            
                Destroy(fox); //destroy fox
                cutTree = true;

                player.exit = true;
            }
            else
            {
                Inventaire.Item item = invent.FindWithName(other.gameObject.tag);
                //print(item.name);

                invent.IncrementQuantite(item);
            
                //invent.DispList();
            
                Destroy(other.gameObject);
            }
            
            //print("destroy");

            

            //invent.actualize = true;

            cutTree = true;
        }
    }

    public void PutObject()
    {
        lastAction = true;
        //  putTree = false;
        if (selected.name != default && selected.name != "fox")
        {
            if (Input.GetKey(KeyCode.Space) && firstAction)
            {
                Inventaire.Item obj = invent.FindWithName(selected.name);

                if (obj.quantite > 0)
                {
                    GameObject objet = Instantiate(obj.prefab);

                    
                    objet.transform.position = gameObject.transform.position;

                    Vector3 position = objet.transform.position;

                    int size = Constants.GetConstant(player.level);

                    Case actualCase = player.ReturnCase(size);
                    
                    edit.Edit(objet, actualCase.typeRegion);
                    
                    edit.Biodiversite(actualCase, player.plateau);


                    invent.DecrementQuantite(obj);
                    putTree = true;
                    firstAction = false;
                    lastAction = false;
                    //objet.transform.parent = gameObject.transform;
                }
            }
            else if (lastAction && !Input.GetKey(KeyCode.Space))
            {
                
                firstAction = true;
            }

            
        }
    }

    public void PutFox()
    {
        lastAction = true;
        
        
        //  putTree = false;
        if (selected.name == "fox")
        {
            print("should put fox");
            if (Input.GetKey(KeyCode.Space))
            {
                print("okk");
                Inventaire.Item obj = invent.FindWithName(selected.name);
                print(obj.name);

                if (obj.quantite > 0)
                {
                    obj.prefab.SetActive(true);
                    obj.prefab.transform.position = gameObject.transform.position;
                    
                    invent.DecrementQuantite(obj);
                    putTree = true;
                    firstAction = false;
                    lastAction = false;
                    //objet.transform.parent = gameObject.transform;
                }
            }
            else if (lastAction && !Input.GetKey(KeyCode.Space))
            {
                
                firstAction = true;
            }

            
        }
    }

    public void TransformationFox()
    {
        if (selected.name == "fox")
        {
            print("should put fox");
            if (Input.GetKey(KeyCode.Space))
            {
                fox.SetActive(true);
                fox.transform.position = gameObject.transform.position;

                gameObject.SetActive(false);
            }
        }
    }
}
