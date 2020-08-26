using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyMovement : MonoBehaviour
{
    [SerializeField] private GameObject leader;
    [SerializeField] private float speed = 5f;
    [SerializeField] public GameObject distracter;
    [SerializeField] private float distractDuration = 5f;
    private Vector3 lastPosition;
    private float timeDistracted;
    private bool isDistracted = false;

    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDistracted == false) {
            Vector2 currentDirection = (transform.position - lastPosition).normalized;
            Vector2 leaderDirection = (leader.transform.position - transform.position).normalized;
            leaderDirection = leaderDirection * 0.5f;
            Vector2 newDirection = currentDirection + leaderDirection;
            // newDirection.Normalize();
            // Vector2 newDirection = ((8f *(((transform.position - lastPosition).normalized)) + (1f *((leader.transform.position - transform.position).normalized))).normalized;
            lastPosition = transform.position;
            transform.position = (Vector2)transform.position + (Vector2)(newDirection * speed * Time.deltaTime);
            // transform.position = Vector3.MoveTowards(transform.position, leader.transform.position, speed * Time.deltaTime);
        }
        else {
            if (distracter != null) {
                Vector2 currentDirection = (transform.position - lastPosition).normalized;
                Vector2 distracterDirection = (distracter.transform.position - transform.position).normalized;
                distracterDirection = distracterDirection * 1f;
                Vector2 newDirection = currentDirection + distracterDirection;
                // newDirection.Normalize();
                // Vector2 newDirection = ((1f *((transform.position - lastPosition).normalized)) + (1f *((distracter.transform.position - transform.position).normalized))).normalized;
                lastPosition = transform.position;
                transform.position = (Vector2)transform.position + (Vector2)(newDirection * speed * Time.deltaTime);
                // transform.position = Vector3.MoveTowards(transform.position, distracter.transform.position, speed * Time.deltaTime);
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
