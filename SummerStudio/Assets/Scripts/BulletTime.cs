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
    // Start is called before the first frame update
    void Start()
    {
        gravity = player.GetComponent<Rigidbody2D>().gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        enemy.transform.Translate(Vector3.right * Time.deltaTime * speed * -1);
        
        if(Input.GetKey(KeyCode.X))
        {
            if(bulletTimeEnabled == false) {
                bulletTimeEnabled = true;
                Time.timeScale = slowdown;
                Time.fixedDeltaTime = Time.timeScale * 0.02f;
                player.GetComponent<Rigidbody2D>().gravityScale = gravity * (slowdown * slowdown) ;
            }
            else {
                bulletTimeEnabled = false;
                Time.timeScale = 1;
                Time.fixedDeltaTime = Time.timeScale * 0.02f;
                player.GetComponent<Rigidbody2D>().gravityScale = gravity;
            }
        }
    }
}
