using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTime : MonoBehaviour
{
    public GameObject enemy;
    public GameObject player;
    public bool bulletTimeEnabled = false;
    public float speed = 1;
    public float slowdown = 0.5f;
    public float gravity;
    public Vector2 velocity;
    bool slowdownDurationEnabled = false; //checks if slowdown is active
    float slowdownDuration; //duration of slowdown
    float timeDurationEnabled; //records the time that worldshift was enabled (or disabled) for duration based slowdown

    // Start is called before the first frame update
    void Start()
    {
        gravity = player.GetComponent<Rigidbody2D>().gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        enemy.transform.Translate(Vector3.right * Time.deltaTime * speed * -1);
        
        if(Input.GetKeyDown(KeyCode.X))
        {
            ToggleBulletTime();
        }

        if(slowdownDurationEnabled && Time.time - timeDurationEnabled >= slowdownDuration) {
            slowdownDurationEnabled = false;
            bulletTimeEnabled = false;
            DisableBulletTime();
        }
    }


    public void ToggleBulletTime() { //function that toggles bullet time
        if(bulletTimeEnabled == false) {
            bulletTimeEnabled = true;
            EnableBulletTime();
            }
        else {
            bulletTimeEnabled = false;
            DisableBulletTime();
        }
    }

    public void EnableBulletTimeWithDuration(float duration) { //function that enables/disables bullet time for a set duration
        timeDurationEnabled = Time.time;
        EnableBulletTime();
        slowdownDuration = duration / Time.timeScale;
        slowdownDurationEnabled = true;
    }

    public void EnableBulletTime() { //function that enables bullet time
        Time.timeScale = slowdown;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        velocity = player.GetComponent<Rigidbody2D>().velocity;
        player.GetComponent<Rigidbody2D>().velocity = velocity * (slowdown);
        player.GetComponent<Rigidbody2D>().gravityScale = gravity * (slowdown * slowdown);
    }

    public void DisableBulletTime() { //function that disables bullet time
        Time.timeScale = 1;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        velocity = player.GetComponent<Rigidbody2D>().velocity;
        player.GetComponent<Rigidbody2D>().velocity = velocity / (slowdown);
        player.GetComponent<Rigidbody2D>().gravityScale = gravity;
    }
}
