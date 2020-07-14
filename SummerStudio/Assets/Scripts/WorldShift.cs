using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldShift : MonoBehaviour
{
    public bool w_Type = true; //true = physical world; false = kenos world
    [SerializeField] [Range(0,1)] private float world_opacity;
    [SerializeField] private GameObject[] physical_platforms;
    [SerializeField] private GameObject[] kenos_platforms;
    public bool shift_type; //true = hide and show; false = transparency toggle
    private bool ignore_Layer = true;
    public GameObject physical_world;
    public GameObject kenos_world;
    public GameObject PostProcessVolume;
    public GameObject gameManager;

    // Start is called before the first frame update
    void Start()
    {
        if(shift_type)
        {
            ToggleWorldVisibility(w_Type);
        }
        else
        {
            ToggleWorldVisibilityWithSlowdown(w_Type);
        }
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
            if(shift_type)
            {
                ToggleWorldVisibility(w_Type);
            }
            else
            {
                ToggleWorldVisibilityWithSlowdown(w_Type);
            }
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
        gameManager.GetComponent<BulletTime>().EnableBulletTimeWithDuration(0.2f);
    }

    void ToggleWorldVisibilityWithSlowdown(bool world_state)
    {
        if(world_state)
        {
            foreach (GameObject physical_platform in physical_platforms)
            {
                Color tmp = physical_platform.GetComponent<SpriteRenderer>().color;
                tmp.a = 1f;
                physical_platform.GetComponent<SpriteRenderer>().color = tmp;
            }

            foreach (GameObject kenos_platform in kenos_platforms)
            {
                Color tmp = kenos_platform.GetComponent<SpriteRenderer>().color;
                tmp.a = world_opacity;
                kenos_platform.GetComponent<SpriteRenderer>().color = tmp;
            }
        }
        else
        {
            foreach (GameObject physical_platform in physical_platforms)
            {
                Color tmp = physical_platform.GetComponent<SpriteRenderer>().color;
                tmp.a = world_opacity;
                physical_platform.GetComponent<SpriteRenderer>().color = tmp;
            }

            foreach (GameObject kenos_platform in kenos_platforms)
            {
                Color tmp = kenos_platform.GetComponent<SpriteRenderer>().color;
                tmp.a = 1f;
                kenos_platform.GetComponent<SpriteRenderer>().color = tmp;
            }
        }
        gameManager.GetComponent<BulletTime>().EnableBulletTimeWithDuration(0.2f);
    }
}
