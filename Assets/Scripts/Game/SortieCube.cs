using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortieCube : MonoBehaviour
{
    private bool success = false;
    public GameObject player;
    private void OnTriggerEnter(Collider other)
    {
        //print("enter ?");
        //print(gameObject.tag);
        if (other.tag.Equals("Player") && gameObject.tag.Equals("Sortie"))
        {
            //print("Innnn");
            
            Inventaire invent = other.GetComponent<Inventaire>();

            Inventaire.Item item = invent.FindWithName("pierre");
            Inventaire.Item item2 = invent.FindWithName("bois");
            Inventaire.Item item3 = invent.FindWithName("sable");

            if (item.quantite > 20 && item2.quantite > 20
                                   && item3.quantite > 20)
            {
                print("Level 0 complete");
                player = other.gameObject;
                player.GetComponent<Personnage>().exit = true;
                success = true;

            }/*
            if (!success)
            {
                print("Level 0 complete");
                player = other.gameObject;
                player.GetComponent<Personnage>().exit = true;
                success = true;
            }*/
            
        }
    }

    public void Update()
    {
        if (success)
        {
            //transform.Translate(Time.deltaTime * 10 * Vector3.down);
            //player.transform.Translate(Time.deltaTime * 10 * Vector3.down);
            
        }
    }
}

