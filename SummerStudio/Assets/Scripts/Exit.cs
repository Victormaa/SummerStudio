using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    private int collected_parts;
    private int required_parts;
    public GameObject level_complete;

    private void Start()
    {
        level_complete.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Exit")
        {
            collected_parts = this.gameObject.GetComponent<Collectables>().machine_parts;
            required_parts = this.gameObject.GetComponent<Collectables>().required_machine_parts;
            Debug.Log(collected_parts);

            if(collected_parts >= required_parts)
            {
                level_complete.SetActive(true);
            }
        }
    }

}
