using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Triggers : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject lifebar;
    Scene scene;
    // Start is called before the first frame update
    void Start()
    {
        scene = SceneManager.GetActiveScene();

        Debug.Log(scene.buildIndex);
        if(scene.buildIndex == 1)
        {
            //turn off portal UI
            player.GetComponent<WorldShift>().HideShiftCounter();
            player.GetComponent<WorldShift>().enabled = false;
        }
        else if(scene.buildIndex == 2)
        {
            player.GetComponent<WorldShift>().HideShiftCounter();
            player.GetComponent<WorldShift>().enabled = true;
            player.GetComponent<WorldShift>().ShowShiftCounter();
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //for level 1
        
        if(scene.buildIndex == 1)
        {
            if (collision.gameObject.tag == "Tutorial Level 2")
            {
                player.GetComponent<WorldShift>().enabled = true;
                player.GetComponent<WorldShift>().ShowShiftCounter();
            }
            if (collision.gameObject.tag == "Tutorial Level 3")
            {
                //lifebar.SetActive(true);
                player.GetComponent<Health>().DisplayHealth();
            }
        }
        //for level 2
        else if(scene.buildIndex == 2)
        {
            
        }
    }
}
