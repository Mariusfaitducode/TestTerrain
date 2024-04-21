using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventaire : MonoBehaviour
{
    [System.Serializable]
    public struct Item 
    {
        public string name;
        public int quantite;
        public GameObject prefab;
    }

    public int totalObj;
    public bool actualize;
    public GameObject itemDispPF;

    //public List<Item> collection;
    public Item[] collection;
    // Start is called before the first frame update
    void Start()
    {
        //collection = new Item[totalObj];
    }

    public Item newItem(string name, int quantite)
    {
        Item item = new Item();
        item.name = name;
        item.quantite = quantite;

        return item;
    }

    public Item FindWithName(string name)
    {
        foreach (Item var in collection)
        {
            if (var.name.Equals(name))
            {
                return var;
            }
        }
        return newItem(name, 1);
    }

    public void DispList()
    {
        foreach (Item var in collection)
        {
            print(var.name + " == " + var.quantite);
        }
    }

    public void IncrementQuantite(Item item)
    {
        bool isInList = false;
        for (int i = 0; i < collection.Length; i++)
        {
            Item var = collection[i];
            if (var.name.Equals(item.prefab.tag))
            {
                isInList = true;
                collection[i].quantite += 1;
                //print(var.quantite);
            }
        }

        if (!isInList)
        {
            print("append");
            collection.Append(item);
        }
        
    }
    
    public void DecrementQuantite(Item item)
    {
        bool isInList = false;
        for (int i = 0; i < collection.Length; i++)
        {
            Item var = collection[i];
            if (var.name.Equals(item.name))
            {
                isInList = true;
                collection[i].quantite -= 1;
                //print(var.quantite);
            }
        }

        if (!isInList)
        {
            print("append");
            collection.Append(item);
        }
        
    }

}
