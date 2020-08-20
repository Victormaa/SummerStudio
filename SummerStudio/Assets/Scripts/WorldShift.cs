using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D;
using UnityEngine.U2D;
using UnityEngine.UI;

public class WorldShift : MonoBehaviour
{
    public bool w_Type = true; //true = physical world; false = kenos world
    private bool ignore_Layer = true;
    public CharacterController2D playerController;
    /*[SerializeField]*/ private Movement movement;
    public GameObject physical_world;
    public GameObject kenos_world;
    public GameObject PostProcessVolume;
    public GameObject gameManager;
    public GameObject[] physical_platforms;
    public GameObject[] kenos_platforms;
    private AudioSource[] tracks;
    private bool time_start = false;
    private bool shift_cooldown_start = false; //new  field
    private bool queueShift = false;
    [SerializeField] private float max_time = 3f;
    [SerializeField] private float current_time = 0f;
    [SerializeField] private float current_cooldown_time =3f; //new field
    private float timeShiftPressed = -1f;
    [SerializeField] [Range(0,3)] private float shiftBufferTime; //new field
    [SerializeField] [Range(0,3)] private float shiftInputBuffer = 0.1f; //input buffer; if player tries to shift before cooldown or shiftbuffer ends it queues it up
    [SerializeField] [Range(0, 1)] private float phys_world_opacity = 0.3f;
    [SerializeField] [Range(0, 1)] private float kenos_world_opacity = 0f;
    [SerializeField] [Range(0,1)] private float bulletTimeDuration = 0.2f;
    [SerializeField] [Range(0,0.5f)] private float worldshiftTransitionDuration = 0.05f;
    [SerializeField] [Range(0,2f)] private float musicChangeDuration = 0.25f;
    [SerializeField] [Range(0, 3)] private float shiftCooldownTime; //new field
    public float musicVolume = 1f;
    [SerializeField] private AudioSource worldShiftSound;

    //new fields for shift counter
    public Image[] portalBG;
    public Image[] portal;
    public int max_shift;
    public int current_shift;
    public float shift_refresh_time;
    //private bool start_shift_timer = true;
    [SerializeField] private float shift_timer = 0;

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
        if (movement == null) {
            movement = (Movement) GameObject.FindObjectOfType (typeof(Movement));
        }
        tracks = this.gameObject.GetComponents<AudioSource>();
        //tracks[0].clip = physical_bgm;
        tracks[0].volume = musicVolume;
        //tracks[0].loop = true;

        //tracks[1].clip = kenos_bgm;
        tracks[1].volume = 0;
        //tracks[1].loop = true;

        //tracks[0].Play();
        //tracks[1].Play();
    }

    // Update is called once per frame
    void Update()
    {
        shift_timer += Time.deltaTime;
        UpdateShiftCounter();

        if (shift_timer >= shift_refresh_time && current_shift < max_shift)
        {
            current_shift++;
            shift_timer = 0f;
        }
        else if (shift_timer >= shift_refresh_time && current_shift >= max_shift)
        {
            shift_timer = 0f;
        }

        if (time_start == true)
        {
            current_time += Time.deltaTime;
        }
        //timer execution
        //new code
        /*
        if(shift_cooldown_start == true)
        {
            current_cooldown_time += Time.deltaTime;
        }

        if (movement == null || movement.characterControlEnabled) {
            if(Input.GetMouseButtonDown(0))
            {
                if (current_cooldown_time <= shiftCooldownTime || current_time <= shiftInputBuffer) {
                    timeShiftPressed = Time.time;
                }
                shift_cooldown_start = true;
            }
            if ((Input.GetMouseButtonDown(0)) && w_Type == true && current_cooldown_time >= shiftCooldownTime)
            {
                ExecuteWorldShift();
                time_start = true;
                shift_cooldown_start = false;
                current_cooldown_time = 0f;
            }
            //old else if((Input.GetMouseButtonDown(0) && w_Type == false) || current_time >= max_time)
            else if (((Input.GetMouseButtonDown(0)  || Time.time - timeShiftPressed <= shiftInputBuffer) && w_Type == false && current_time >= shiftBufferTime) || current_time >= max_time)
            {
                ExecuteWorldShift();
                current_time = 0f;
                time_start = false;
                shift_cooldown_start = true; //new line
            }
        }*/

        if (Input.GetMouseButtonDown(0) && w_Type == true && current_shift > 0)
        {
            ExecuteWorldShift();
            current_shift--;
            portal[current_shift].enabled = false;
            time_start = true;
        }
        else if ((Input.GetMouseButtonDown(0) || Time.time - timeShiftPressed <= shiftInputBuffer) && w_Type == false || current_time >= max_time)
        {
            ExecuteWorldShift();
            current_time = 0f;
            time_start = false;
        }

        if (w_Type == true)
        {
            PostProcessVolume.SetActive(false);
            if (tracks[0].volume < musicVolume) {
                tracks[0].volume += (float) (Time.deltaTime* musicVolume/musicChangeDuration); //increase physical music volume to max 
                if (tracks[0].volume > musicVolume) {
                    tracks[0].volume = musicVolume;
                }
                Debug.Log("increasing 0's volume to "+ tracks[0].volume + ". target: " + musicVolume);
            }
            if (tracks[1].volume > 0f) {
                tracks[1].volume -= (float) (Time.deltaTime* musicVolume/musicChangeDuration); //decrease kenos music volume to 0
                if (tracks[1].volume < 0f ) {
                    tracks[1].volume = 0f;
                }
                Debug.Log("decreasing 1's volume to "+ tracks[1].volume + ". target: 0." );
            }
            
        }

        else if(w_Type == false)
        {
            PostProcessVolume.SetActive(true);
            if (tracks[0].volume > 0f) {
                tracks[0].volume -= (float) (Time.deltaTime* musicVolume/musicChangeDuration); //decrease physical music volume to 0
                if (tracks[0].volume < 0f ) {
                    tracks[0].volume = 0f;
                }
                Debug.Log("decreasing 0's volume to "+ tracks[0].volume + ". target: 0.");
            }
            if (tracks[1].volume < musicVolume) {
                tracks[1].volume += (float) (Time.deltaTime* musicVolume/musicChangeDuration); //increase kenos music volume to max
                if (tracks[1].volume > musicVolume) {
                    tracks[1].volume = musicVolume;
                }
                Debug.Log("increasing 1's volume to "+ tracks[1].volume + ". target: " + musicVolume);
            }
            
            
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



    void ExecuteWorldShift()
    {
        if (worldShiftSound != null) {
            worldShiftSound.Play();
        }
        w_Type = !w_Type; //toggle world type on each button press
        Physics2D.IgnoreLayerCollision(8, 10, ignore_Layer); //toggle ignore on physical layer
        Physics2D.IgnoreLayerCollision(8, 11, !ignore_Layer); //toggle ignore on kenos layer
        ignore_Layer = !ignore_Layer; //toggle ignore field
        SetWorldTransparency(w_Type);
        gameManager.GetComponent<BulletTime>().EnableBulletTimeWithDuration(bulletTimeDuration);
        // if (w_Type)
        // {
        //     float startingPhysVolume = tracks[0].volume;
        //     float startingKenosVolume = tracks[1].volume;
        //     StartCoroutine(ChangeGameVolume(0,startingPhysVolume,musicVolume,musicChangeDuration));
        //     StartCoroutine(ChangeGameVolume(1,startingKenosVolume,0f,musicChangeDuration));
        // } else
        // {
        //     float startingPhysVolume = tracks[0].volume;
        //     float startingKenosVolume = tracks[1].volume;
        //     StartCoroutine(ChangeGameVolume(0,startingPhysVolume,0f,musicChangeDuration));
        //     StartCoroutine(ChangeGameVolume(1,startingKenosVolume,musicVolume,musicChangeDuration));
        // }
    }

    void SetWorldTransparency(bool world_state)
    {
        if(world_state) //if shifting to phyical world
        {
            foreach (GameObject physical_platform in physical_platforms) //set all physical objects to full opacity
            {
                if (physical_platform.GetComponent<SpriteRenderer>() != null) {
                    Color currentOpacity = physical_platform.GetComponent<SpriteRenderer>().color;
                    StartCoroutine(ChangeSpriteOpacity(physical_platform, currentOpacity.a, 1f, worldshiftTransitionDuration));
                    // Color tmp = physical_platform.GetComponent<SpriteRenderer>().color;
                    // tmp.a = 1f;
                    // physical_platform.GetComponent<SpriteRenderer>().color = tmp;
                }
                else if (physical_platform.GetComponent<SpriteShapeRenderer>() != null) {
                    Color currentOpacity = physical_platform.GetComponent<SpriteShapeRenderer>().color;
                    StartCoroutine(ChangeSpriteShapeOpacity(physical_platform, currentOpacity.a, 1f, worldshiftTransitionDuration));
                    // Color tmp = physical_platform.GetComponent<SpriteShapeRenderer>().color;
                    // tmp.a = 1f;
                    // physical_platform.GetComponent<SpriteShapeRenderer>().color = tmp;
                }
            }
            if (playerController != null) {
                playerController.m_WhatIsGround = playerController.m_WhatIsGround | (1<<10); //add physical world to ground check
            }
            foreach (GameObject kenos_platform in kenos_platforms)  //set all kenos objects to partial opacity
            {
                if (kenos_platform.GetComponent<SpriteRenderer>() != null) {
                    Color currentOpacity = kenos_platform.GetComponent<SpriteRenderer>().color;
                    StartCoroutine(ChangeSpriteOpacity(kenos_platform, currentOpacity.a, kenos_world_opacity, worldshiftTransitionDuration));
                    // Color tmp = kenos_platform.GetComponent<SpriteRenderer>().color;
                    // tmp.a = kenos_world_opacity;
                    // kenos_platform.GetComponent<SpriteRenderer>().color = tmp;
                }
                else if (kenos_platform.GetComponent<SpriteShapeRenderer>() != null) {
                    Color currentOpacity = kenos_platform.GetComponent<SpriteShapeRenderer>().color;
                    StartCoroutine(ChangeSpriteShapeOpacity(kenos_platform, currentOpacity.a, kenos_world_opacity, worldshiftTransitionDuration));
                    // Color tmp = kenos_platform.GetComponent<SpriteShapeRenderer>().color;
                    // tmp.a = kenos_world_opacity;
                    // kenos_platform.GetComponent<SpriteShapeRenderer>().color = tmp;
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
                    Color currentOpacity = physical_platform.GetComponent<SpriteRenderer>().color;
                    StartCoroutine(ChangeSpriteOpacity(physical_platform, currentOpacity.a, phys_world_opacity, worldshiftTransitionDuration));
                    // Color tmp = physical_platform.GetComponent<SpriteRenderer>().color;
                    // tmp.a = phys_world_opacity;
                    // physical_platform.GetComponent<SpriteRenderer>().color = tmp;
                }
                else if (physical_platform.GetComponent<SpriteShapeRenderer>() != null) {
                    Color currentOpacity = physical_platform.GetComponent<SpriteShapeRenderer>().color;
                    StartCoroutine(ChangeSpriteShapeOpacity(physical_platform, currentOpacity.a, phys_world_opacity, worldshiftTransitionDuration));
                    // Color tmp = physical_platform.GetComponent<SpriteShapeRenderer>().color;
                    // tmp.a = phys_world_opacity;
                    // physical_platform.GetComponent<SpriteShapeRenderer>().color = tmp;
                }
            }
            if (playerController != null) {
                playerController.m_WhatIsGround = playerController.m_WhatIsGround & ~(1<<10); //remove physical objects from ground check
            }

            foreach (GameObject kenos_platform in kenos_platforms) //set all kenos objects to full opacity
            {
                if (kenos_platform.GetComponent<SpriteRenderer>() != null) {
                    Color currentOpacity = kenos_platform.GetComponent<SpriteRenderer>().color;
                    StartCoroutine(ChangeSpriteOpacity(kenos_platform, currentOpacity.a, 1f, worldshiftTransitionDuration));
                    // Color tmp = kenos_platform.GetComponent<SpriteRenderer>().color;
                    // tmp.a = 1f;
                    // kenos_platform.GetComponent<SpriteRenderer>().color = tmp;
                }
                else if (kenos_platform.GetComponent<SpriteShapeRenderer>() != null) {
                    Color currentOpacity = kenos_platform.GetComponent<SpriteShapeRenderer>().color;
                    StartCoroutine(ChangeSpriteShapeOpacity(kenos_platform, currentOpacity.a, 1f, worldshiftTransitionDuration));
                    // Color tmp = kenos_platform.GetComponent<SpriteShapeRenderer>().color;
                    // tmp.a = 1f;
                    // kenos_platform.GetComponent<SpriteShapeRenderer>().color = tmp;
                }
            }
            if (playerController != null) {
                playerController.m_WhatIsGround = playerController.m_WhatIsGround | (1<<11); //add kenos world to ground check
            }
        }
    }

    //new function
    public void HideShiftCounter()
    {
        for (int i = 0; i < max_shift; i++)
        {
            portalBG[i].enabled = false;
            portal[i].enabled = false;
        }
    }

    public void ShowShiftCounter()
    {
        for (int i = 0; i < max_shift; i++)
        {
            portalBG[i].enabled = true;
        }

        for (int i = 0; i < current_shift; i++)
        {
            portal[i].enabled = true;
        }
    }

    void UpdateShiftCounter()
    {
        for (int i = 0; i < current_shift; i++)
        {
            portal[i].enabled = true;
        }
    }

    IEnumerator ChangeSpriteOpacity(GameObject gameObject, float opacityStart, float opacityTarget, float duration) {
        for (float t=0f; t<duration; t+=Time.deltaTime) {
            float normalizedTime = t/duration;
            Color tmp = gameObject.GetComponent<SpriteRenderer>().color;
            tmp.a = Mathf.SmoothStep(opacityStart, opacityTarget, normalizedTime);
            gameObject.GetComponent<SpriteRenderer>().color = tmp;
            yield return null;
        }
        Color tmp2 = gameObject.GetComponent<SpriteRenderer>().color;
        tmp2.a = opacityTarget;
        gameObject.GetComponent<SpriteRenderer>().color = tmp2;
    }

    IEnumerator ChangeSpriteShapeOpacity(GameObject gameObject, float opacityStart, float opacityTarget, float duration) {
        for (float t=0f; t<duration; t+=Time.deltaTime) {
            float normalizedTime = t/duration;
            Color tmp = gameObject.GetComponent<SpriteShapeRenderer>().color;
            tmp.a = Mathf.SmoothStep(opacityStart, opacityTarget, normalizedTime);
            gameObject.GetComponent<SpriteShapeRenderer>().color = tmp;
            yield return null;
        }
        Color tmp2 = gameObject.GetComponent<SpriteShapeRenderer>().color;
        tmp2.a = opacityTarget;
        gameObject.GetComponent<SpriteShapeRenderer>().color = tmp2;
    }

    // IEnumerator ChangeGameVolume(int trackNumber, float startingVolume, float targetVolume, float duration) {
    //     float v = startingVolume;
    //     float t = 0f;
    //     if (duration<=0) {
    //         tracks[trackNumber].volume = targetVolume;
    //     }
    //     else if (startingVolume<targetVolume) {
    //         while (v<targetVolume && t<duration) {
    //             v += (float) (musicVolume * t/duration);
    //             t+=Time.deltaTime;
    //             if (v>targetVolume) {
    //                 v= targetVolume;
    //             }
    //             tracks[trackNumber].volume = v;
    //             // Debug.Log("increasing " + trackNumber + "'s volume to "+ v + ". Time elapsed: "+ t + "target: " + targetVolume);
    //             yield return null;
    //         }
    //     }
    //     else {
    //         while (v>targetVolume && t<duration) {
    //             v -= (float) (musicVolume * t/duration);
    //             t+=Time.deltaTime;
    //             if (v<targetVolume) {
    //                 v= targetVolume;
    //             }
    //             tracks[trackNumber].volume = v;
    //             // Debug.Log("decreasing " + trackNumber + "'s volume to "+ v + ". Time elapsed: "+ t + "target: " + targetVolume);
    //             yield return null;
    //         }
    //     }
    // }
}
