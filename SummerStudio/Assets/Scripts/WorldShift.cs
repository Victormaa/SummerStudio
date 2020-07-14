using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D;
using UnityEngine.U2D;

public class WorldShift : MonoBehaviour
{
    public bool w_Type = true; //true = physical world; false = kenos world
    private bool ignore_Layer = true;
    public GameObject physical_world;
    public GameObject kenos_world;
    public GameObject PostProcessVolume;
    public GameObject gameManager;
    [SerializeField] private bool hideOtherWorld = true;
    
    private Color solidColor = new Color(255, 255, 255, 255);
    private Color transColor = new Color(255, 255, 255, 128);
    Renderer[] physicalPlatforms;
    Renderer[] kenosPlatforms;

    // Start is called before the first frame update
    void Start()
    {
        ToggleWorldVisibility(w_Type);
        Physics2D.IgnoreLayerCollision(8, 11, ignore_Layer);
        physicalPlatforms = physical_world.GetComponentsInChildren<Renderer>();
        kenosPlatforms = kenos_world.GetComponentsInChildren<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            w_Type = !w_Type; //toggle world type on each button press
            Physics2D.IgnoreLayerCollision(8, 10, ignore_Layer); //toggle ignore on physical layer
            Physics2D.IgnoreLayerCollision(8, 11, !ignore_Layer); //toggle ignore on kenos layer
            ignore_Layer = !ignore_Layer; //toggle ignore field
            ToggleWorldVisibilityWithSlowdown(w_Type);
        }

        if(w_Type == true)
        {
            PostProcessVolume.SetActive(false);
        }

        else if(w_Type == false)
        {
            PostProcessVolume.SetActive(true);
        }
    }

    void ToggleWorldVisibility(bool world_state)
    {
        if (hideOtherWorld) {
            if(world_state)
            {
                physical_world.SetActive(true);
                kenos_world.SetActive(false);
            }
            else
            {
                physical_world.SetActive(false);
                kenos_world.SetActive(true);
            }
        }
        else {
            if(world_state)
            {
                foreach(Renderer platform in physicalPlatforms) {
                    platform.material.color = solidColor;
                }
                foreach(Renderer platform in kenosPlatforms) {
                    platform.material.color = transColor;
                }
            }
            else
            {
                foreach(Renderer platform in physicalPlatforms) {
                    platform.material.color = transColor;
                }
                foreach(Renderer platform in kenosPlatforms) {
                    platform.material.color = solidColor;
                }
            }
        }
    }

    void ToggleWorldVisibilityWithSlowdown(bool world_state)
    {
        ToggleWorldVisibility(world_state);
        gameManager.GetComponent<BulletTime>().EnableBulletTimeWithDuration(0.2f);
    }

}
