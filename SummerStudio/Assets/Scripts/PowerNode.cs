using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerNode : MonoBehaviour
{
    [SerializeField] private float attractRadius = 50f;
    [SerializeField] private FlyMovement[] flyList;
    [SerializeField] private float activationDuration = 5f;
    private float timeLastActivated = 0.0f;
    private SpriteRenderer sprite;

    void Start() {
        sprite = gameObject.GetComponent<SpriteRenderer>();
    }

    void FixedUpdate() {
        if (Time.time - timeLastActivated > activationDuration) {
            Deactivate();
        }
    }

    private void Activate() {
        if (sprite != null) {
            sprite.color = new Color (0, 0.5f, 1, 1);
        }
    }

    private void Deactivate() {
        if (sprite != null) {
            sprite.color = new Color (0, 0, 1, 1);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            // Debug.Log("power node hit");
            Activate();
            timeLastActivated = Time.time;
            System.Array.Clear(flyList,0,flyList.Length); //clear fly list
            flyList = FindObjectsOfType<FlyMovement>(); //find all FlyMovement objects within radius attractRadius
            //add those objects to flyList
            if (flyList != null) {
                // Debug.Log("Fly List not empty");
                foreach (FlyMovement fly in flyList) {
                    if ((transform.position - fly.transform.position).sqrMagnitude < attractRadius * attractRadius) {
                        // Debug.Log("fly distracted");
                        fly.Distract(this.gameObject);
                    }
                    
                }
            }

        }
    }

}
