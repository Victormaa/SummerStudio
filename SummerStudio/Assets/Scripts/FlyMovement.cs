using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyMovement : MonoBehaviour
{
    [SerializeField] private GameObject leader;
    [SerializeField] private float speed = 5f;
    [SerializeField] public GameObject distracter;
    [SerializeField] private float distractDuration = 5f;
    private float timeDistracted;
    private bool isDistracted = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDistracted == false) {
            transform.position = Vector3.MoveTowards(transform.position, leader.transform.position, speed * Time.deltaTime);
        }
        else {
            if (distracter != null) {
                transform.position = Vector3.MoveTowards(transform.position, distracter.transform.position, speed * Time.deltaTime);
            }
            else {
                isDistracted = false;
            }
            if (Time.time - timeDistracted > distractDuration) {
                // Debug.Log("distraction end");
                isDistracted = false;
            }
        }
    }

    public void Distract(GameObject distraction) {
        // Debug.Log("going to distraction");
        isDistracted = true;
        timeDistracted = Time.time;
        distracter = distraction;
    }
    
    public void Distract(GameObject distraction, float duration) {
        // Debug.Log("going to distraction");
        isDistracted = true;
        timeDistracted = Time.time;
        distracter = distraction;
        distractDuration = duration;
    }


}
