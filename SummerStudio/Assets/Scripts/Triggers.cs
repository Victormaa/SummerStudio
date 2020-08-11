using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Triggers : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject lifebar;
    // Start is called before the first frame update
    void Start()
    {
        //turn off portal UI
        player.GetComponent<WorldShift>().HideShiftCounter();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Tutorial Level 2")
        {
            player.GetComponent<WorldShift>().enabled = true;
            player.GetComponent<WorldShift>().ShowShiftCounter();
        }
        if(collision.gameObject.tag == "Tutorial Level 3")
        {
            //lifebar.SetActive(true);
            player.GetComponent<Health>().DisplayHealth();
        }
    }
}
