using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collectables : MonoBehaviour
{
    //this script contains interactions for all collectables EXCEPT for health pick ups

    public int machine_parts; //parts player has collected
    public int total_machine_parts; //total machine parts in the level
    public int required_machine_parts; //required machine parts to advance to the next level
    [SerializeField] private AudioClip collectSound;
    public Image[] mparts;
    public Image[] mparts_outline;
    public Image[] mparts_shadow;

    private void Start()
    {
        machine_parts = 0;

        for (int i = 0; i < total_machine_parts; i++)
        {
            mparts[i].enabled = false;
            mparts_outline[i].enabled = false;
            mparts_shadow[i].enabled = false;
        }
    }

    public void PickUp(GameObject obj)
    {
        machine_parts++;
        if (collectSound != null) {
            AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }
        obj.SetActive(false);
    }

    public void EnableMachineParts()
    {
        for (int i = 0; i < total_machine_parts; i++)
        {
            mparts_outline[i].enabled = true;
            mparts_shadow[i].enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Machine Parts")
        {
            PickUp(collision.gameObject);
            mparts[machine_parts - 1].enabled = true;
        }

        if(collision.gameObject.tag == "Tutorial Level 4")
        {
            EnableMachineParts();
        }
    }
}
