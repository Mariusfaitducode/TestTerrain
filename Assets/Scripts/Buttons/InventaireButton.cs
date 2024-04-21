using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventaireButton : MonoBehaviour
{
    public GameObject invent;

    public RawImage isSelect;

    public bool open = false;
    public bool lastAction;

    public void OnClick()
    {
        if (!open)
        {
            invent.SetActive(true);
            isSelect.enabled = true;
            open = true;
        }
        else
        {
            invent.SetActive(false);
            isSelect.enabled = false;
            open = false;
        }
    }
   
    void Start()
    {
        open = false;
    }
    
    
    
    void Update()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            if (!open)
            {
                invent.SetActive(true);
                isSelect.enabled = true;
            }
            else
            {
                invent.SetActive(false);
                isSelect.enabled = false;
            }

            lastAction = true;
        }
        else
        {
            if (lastAction)
            {
                open = !open;
                lastAction = false;
            }
        }
    }
}
