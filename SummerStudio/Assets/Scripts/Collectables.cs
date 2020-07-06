using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collectables : MonoBehaviour
{
    //this script contains interactions for all collectables EXCEPT for health pick ups

    public int machine_parts;
    public int total_machine_parts;
    public Image[] mparts;

    private void Start()
    {
        machine_parts = 0;

        for (int i = 0; i < total_machine_parts; i++)
        {
            mparts[i].enabled = false;
        }
    }

    public void PickUp(GameObject obj)
    {
        machine_parts++;
        Destroy(obj);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Machine Parts")
        {
            PickUp(collision.gameObject);
            mparts[machine_parts - 1].enabled = true;
        }
    }
}
