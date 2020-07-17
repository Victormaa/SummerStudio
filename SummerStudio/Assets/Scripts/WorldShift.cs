using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D;
using UnityEngine.U2D;

public class WorldShift : MonoBehaviour
{
    public bool w_Type = true; //true = physical world; false = kenos world
    private bool ignore_Layer = true;
    public CharacterController2D playerController;
    public GameObject physical_world;
    public GameObject kenos_world;
    public GameObject PostProcessVolume;
    public GameObject gameManager;
    public GameObject[] physical_platforms;
    public GameObject[] kenos_platforms;
    [SerializeField] [Range(0, 1)] private float phys_world_opacity = 0.3f;
    [SerializeField] [Range(0, 1)] private float kenos_world_opacity = 0f;
    

    // Start is called before the first frame update
    void Start()
    {
        Transform[] physical_transforms = physical_world.GetComponentsInChildren<Transform>();
        Transform[] kenos_transforms = kenos_world.GetComponentsInChildren<Transform>();
        physical_platforms = new GameObject[physical_transforms.Length];
        kenos_platforms = new GameObject[kenos_transforms.Length];
        int value = 0;
        foreach(Transform trans in physical_transforms) {
            value++;
            physical_platforms.SetValue (trans.gameObject, value - 1);
        }
        value = 0;
        foreach(Transform trans in kenos_transforms) {
            value++;
            kenos_platforms.SetValue (trans.gameObject, value - 1);
        }
        SetWorldTransparency(w_Type);
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
            SetWorldTransparency(w_Type);
            gameManager.GetComponent<BulletTime>().EnableBulletTimeWithDuration(0.2f);
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

    void ShowHideWorld(bool world_state)
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

    void SetWorldTransparency(bool world_state)
    {
        if(world_state) //if shifting to phyical world
        {
            foreach (GameObject physical_platform in physical_platforms) //set all physical objects to full opacity
            {
                if (physical_platform.GetComponent<SpriteRenderer>() != null) {
                    Color tmp = physical_platform.GetComponent<SpriteRenderer>().color;
                    tmp.a = 1f;
                    physical_platform.GetComponent<SpriteRenderer>().color = tmp;
                }
                else if (physical_platform.GetComponent<SpriteShapeRenderer>() != null) {
                    Color tmp = physical_platform.GetComponent<SpriteShapeRenderer>().color;
                    tmp.a = 1f;
                    physical_platform.GetComponent<SpriteShapeRenderer>().color = tmp;
                }
            }
            if (playerController != null) {
                playerController.m_WhatIsGround = playerController.m_WhatIsGround | (1<<10); //add physical world to ground check
            }
            foreach (GameObject kenos_platform in kenos_platforms)  //set all kenos objects to partial opacity
            {
                if (kenos_platform.GetComponent<SpriteRenderer>() != null) {
                    Color tmp = kenos_platform.GetComponent<SpriteRenderer>().color;
                    tmp.a = kenos_world_opacity;
                    kenos_platform.GetComponent<SpriteRenderer>().color = tmp;
                }
                else if (kenos_platform.GetComponent<SpriteShapeRenderer>() != null) {
                    Color tmp = kenos_platform.GetComponent<SpriteShapeRenderer>().color;
                    tmp.a = kenos_world_opacity;
                    kenos_platform.GetComponent<SpriteShapeRenderer>().color = tmp;
                }
            }
            if (playerController != null) {
                playerController.m_WhatIsGround = playerController.m_WhatIsGround & ~(1<<11); //remove kenos objects from ground check
            }
        }

        else //if shifting to kenos world
        {
            foreach (GameObject physical_platform in physical_platforms) //set all physical objects to partial opacity
            {
                if (physical_platform.GetComponent<SpriteRenderer>() != null) {
                    Color tmp = physical_platform.GetComponent<SpriteRenderer>().color;
                    tmp.a = phys_world_opacity;
                    physical_platform.GetComponent<SpriteRenderer>().color = tmp;
                }
                else if (physical_platform.GetComponent<SpriteShapeRenderer>() != null) {
                    Color tmp = physical_platform.GetComponent<SpriteShapeRenderer>().color;
                    tmp.a = phys_world_opacity;
                    physical_platform.GetComponent<SpriteShapeRenderer>().color = tmp;
                }
            }
            if (playerController != null) {
                playerController.m_WhatIsGround = playerController.m_WhatIsGround & ~(1<<10); //remove physical objects from ground check
            }

            foreach (GameObject kenos_platform in kenos_platforms) //set all kenos objects to full opacity
            {
                if (kenos_platform.GetComponent<SpriteRenderer>() != null) {
                    Color tmp = kenos_platform.GetComponent<SpriteRenderer>().color;
                    tmp.a = 1f;
                    kenos_platform.GetComponent<SpriteRenderer>().color = tmp;
                }
                else if (kenos_platform.GetComponent<SpriteShapeRenderer>() != null) {
                    Color tmp = kenos_platform.GetComponent<SpriteShapeRenderer>().color;
                    tmp.a = 1f;
                    kenos_platform.GetComponent<SpriteShapeRenderer>().color = tmp;
                }
            }
            if (playerController != null) {
                playerController.m_WhatIsGround = playerController.m_WhatIsGround | (1<<11); //add kenos world to ground check
            }
        }
    }
}
