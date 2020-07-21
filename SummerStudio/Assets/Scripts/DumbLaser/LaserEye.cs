﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO:
//LaserEye could not see behind it.

public enum LaserState
{
    Hide,
    Sleeping,
    Alerting,
    Aiming,
    Locked,
    Shoot
}

public class LaserEye : MonoBehaviour
{
    public LaserState mLaserState = LaserState.Hide;

    private Transform target;

    private bool left = false; //represent the eye's position

    private bool shotingleft = false; //The Shoting Moment represent the eye's position

    private float _time = 0;

    private Vector2 _shotLastPosition = Vector2.zero; //the last position player stayed at

    [Header("General")]

    public LineRenderer lineRenderer;
    public float range = 7.5f;
    public Transform firePoint;
    public float AimToLockTime = 1; //AimToLockTime
    public float AlertToAimTime = 3; // AwakeToAimTime
    public Animator animator;
    public float LockedToShootTime = 0;

    [Header("Temperary")]

    public GameObject Bursteffect;
    public LineRenderer shotlineRenderer;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        changeState(LaserState.Hide);
    }

    private void OnEnable()
    {
        changeState(LaserState.Sleeping);
        Debug.Log("there is enable");
    }

    private void OnDisable()
    {
        changeState(LaserState.Hide);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.enabled && mLaserState == LaserState.Hide)
            changeState(LaserState.Sleeping);

        target = GameObject.FindGameObjectWithTag("Player").transform;
        Vector2 FP = firePoint.position;
        Vector2 TP = target.position;
        switch (mLaserState)
        {
            case LaserState.Hide:
                //when unHide go sleep;
                break;
            case LaserState.Sleeping:
                //Lasereye would close
                if(Vector2.Distance(FP, TP) < range)
                {
                    changeState(LaserState.Alerting);
                }
                break;
            case LaserState.Alerting:
                //when player inside there range they would awake
                if (target != null)
                {
                    
                    _time += Time.deltaTime;

                    InvokeRepeating("CheckPlayer", 0.0f, 1.0f); //every second check player's position to look at player

                    if(_time > AlertToAimTime)
                    {
                        changeState(LaserState.Aiming);
                    }

                }
                else
                {
                    changeState(LaserState.Sleeping);
                }
                // if player get out of the detect range
                if (Vector2.Distance(FP, TP) > range)
                {
                    changeState(LaserState.Sleeping);
                }
                //look left and right
                break;
            case LaserState.Aiming:
                Laser();
                _time += Time.deltaTime;
                if (_time > AimToLockTime)
                {
                    //if (shotingleft == left)
                    //{
                        _shotLastPosition = target.position - firePoint.position;
                        renderline(target.position, firePoint.position);
                        Invoke("Burst", LockedToShootTime);
                        changeState(LaserState.Locked);
                    //}
                }
                if (Vector2.Distance(FP, TP) > range)
                {
                    changeState(LaserState.Sleeping);
                }
                break;
            case LaserState.Locked:
                if (Vector2.Distance(FP, TP) > range)
                {
                    changeState(LaserState.Sleeping);
                }
                break;
            case LaserState.Shoot:
                // shot state is a process
                // the shot actually happens at the last moment of the state;
                //shotingleft = (target.position.x - this.transform.position.x) < 0;

                /*
                if(shotingleft == left) // this stuff make sure the laser eye could shot like when player stay at the same side with detect
                {
                    Laser();
                }
                else
                {
                    lineRenderer.enabled = false;
                }
                */

                //_time += Time.deltaTime;

                /*
                if(_time > LockToShootTime)
                {
                    if (shotingleft == left)
                    {
                        _shotLastPosition = target.position - firePoint.position;
                        renderline(target.position, firePoint.position);
                        Invoke("Burst", Shotdelay);
                        changeState(LaserState.Alerting);
                    }
                }
                */
                changeState(LaserState.Alerting);
                //deal damage

                if (Vector2.Distance(FP, TP) > range)
                {
                    changeState(LaserState.Sleeping);
                }
                break;
            default:
                
                break;
        }
    }

    private void Burst()
    {
        // this is when shot happens;
        RaycastHit2D hit2D;

        shotlineRenderer.enabled = false;

        hit2D = Physics2D.Raycast(firePoint.position, _shotLastPosition );
        Instantiate(Bursteffect, hit2D.point, Quaternion.identity);
        changeState(LaserState.Shoot);
    }

    private void renderline(Vector2 v1, Vector2 v2) //the moment it shot
    {
        shotlineRenderer.enabled = true;
        shotlineRenderer.SetPosition(1, v1);
        shotlineRenderer.SetPosition(0, v2);
    }

    public void changeState(LaserState newLaserState)
    {
        // OnExit()
        switch (mLaserState)
        {
            case LaserState.Hide:
                break;
            case LaserState.Sleeping:
                animator.SetBool("Sleep", false);
                break;
            case LaserState.Alerting:
                CancelInvoke("CheckPlayer");
                break;
            case LaserState.Aiming:
                lineRenderer.enabled = false;
                break;
            case LaserState.Locked:
                break;
            case LaserState.Shoot:
                lineRenderer.enabled = false;
                break;
            default:
                break;
        }

        //  change state
        mLaserState = newLaserState;

        // OnEnter()
        switch (mLaserState)
        {
            case LaserState.Hide:
                Debug.Log("Laser: Hide State");
                break;
            case LaserState.Sleeping:
                Debug.Log("Laser: Sleep State");
                animator.SetBool("Sleep", true);
                break;
            case LaserState.Alerting:
                Debug.Log("Laser: Awake State");
                _time = 0;
                break;
            case LaserState.Aiming:
                Debug.Log("Laser: Aiming State");
                _time = 0;
                break;
            case LaserState.Locked:
                Debug.Log("Laser: Locked State");
                break;
            case LaserState.Shoot:
                Debug.Log("Laser: Shoot State");
               // _time = 0;
                break;
            default:
                break;
        }
    }
    private void CheckPlayer()
    {
        if(target != null)
        {
            left = (target.position.x - this.transform.position.x) < 0;
            animator.SetBool("Left", left);
        }
    }

    private void Laser()
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);
    }
}
