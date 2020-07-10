using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldShift : MonoBehaviour
{
    public bool w_Type = true; //true = physical world; false = kenos world
    private bool ignore_Layer = true;
    public GameObject physical_world;
    public GameObject kenos_world;
    public GameObject PostProcessVolume;
    public GameObject gameManager;

    // Start is called before the first frame update
    void Start()
    {
        ToggleWorldVisibility(w_Type);
        Physics2D.IgnoreLayerCollision(8, 11, ignore_Layer);
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

    void ToggleWorldVisibilityWithSlowdown(bool world_state)
    {
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
        gameManager.GetComponent<BulletTime>().EnableBulletTimeWithDuration(0.2f);
    }

}
