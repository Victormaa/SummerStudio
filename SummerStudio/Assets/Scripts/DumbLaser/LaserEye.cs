using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO:
//LaserEye could not see behind it.

public enum LaserState
{
    Hide, // should this enemy appear only in one world?
    Sleeping,
    Alerting,
    Aiming,
    Locked,
    Shoot
}

public class LaserEye : MonoBehaviour
{
    public LaserState mLaserState = LaserState.Hide;    //temporary "public" it should be private in most case

    private Transform target;   // the attact target.

    private bool left = false; //Now the laser eye only looks to 2 direction. left and right

    //The behavior I designed was laser wont shot if player run out of his locked area.
    //private bool shotingleft = false; //The Shoting Moment represent the eye's position

    private float _time = 0; // time to record stay at each state.

    private Vector2 _shotLastPosition = Vector2.zero; //the Locked position player stayed at

    [Header("General")]

    public LineRenderer lineRenderer;   //demo's effect
    public float range = 7.5f;  //shooting range, eye go alert when player inside this range around him
    public Transform firePoint; //the position of the eye
    public float AimToLockTime = 1; //AimToLockTime
    public float AlertToAimTime = 3; // AwakeToAimTime
    public Animator animator;   //
    public float LockedToShootTime = 0; // as the name

    public ParticleSystem chargingEffect;   //particle effect when laser hit stuff.
    public AudioSource chargingSound;   //sounds when charging

    [Header("Direction")]
    Vector2 normalVec;
    Vector2 playersDirVec;

    [Header("Temperary")]

    public GameObject Bursteffect;
    public LineRenderer shotlineRenderer;
    public LineRenderer testlineRenderer;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        GameObject root = this.transform.Find("Root").gameObject;

        normalVec = new Vector2(firePoint.position.x, firePoint.position.y) 
            - new Vector2(root.transform.position.x, root.transform.position.y);
        normalVec = normalVec.normalized;

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
        playersDirVec = (TP - FP).normalized;
        switch (mLaserState)
        {
            case LaserState.Hide:
                //when unHide go sleep;
                break;
            case LaserState.Sleeping:
                //Lasereye would close
                if(Vector2.Distance(FP, TP) < range && Vector2.Dot(playersDirVec, normalVec) > 0)
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
                if (Vector2.Distance(FP, TP) > range || Vector2.Dot(playersDirVec, normalVec) < 0)
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
                        _shotLastPosition = target.position - firePoint.position;
                        changeState(LaserState.Locked);
                }
                if (Vector2.Distance(FP, TP) > range)
                {
                    changeState(LaserState.Sleeping);
                }
                break;
            case LaserState.Locked:

                if (Vector2.Distance(FP, TP) > range || Vector2.Dot(playersDirVec, normalVec) < 0)
                {
                    changeState(LaserState.Sleeping);
                }
                break;
            case LaserState.Shoot:
                // shot state is a process
                // the shot actually happens at the last moment of the state;
                

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

                if (Vector2.Distance(FP, TP) > range || Vector2.Dot(playersDirVec, normalVec) < 0)
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
        GameObject Shooteffect = Instantiate(Bursteffect, hit2D.point, Quaternion.identity);

        if (hit2D.collider.gameObject.tag == "Player")
            Health.onTakenDamage.Invoke();  //eventsystem

        Shooteffect.GetComponent<ParticleSystem>().Play();
        DestoryBullet(2, Shooteffect); // there could build a simple queue to hold the bullet not to destory.
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
                
                break;
            case LaserState.Aiming:
                lineRenderer.enabled = false;
                chargingEffect.Stop();
                chargingSound.Stop();
                CancelInvoke("CheckPlayer");
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
                ChargeLaser();
                _time = 0;
                break;
            case LaserState.Locked:
                renderline(target.position, firePoint.position);
                Invoke("Burst", LockedToShootTime);
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

    private void DestoryBullet(int secs, GameObject bullet)
    {
        Destroy(bullet, secs);
    }

    private void ChargeLaser()
    {
        chargingSound.Play();
        chargingEffect.Play();
    }
}
