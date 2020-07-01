using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTime : MonoBehaviour
{
    public GameObject enemy;
    public float speed = 1;
    public float slowdown = 0.05f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        enemy.transform.Translate(Vector3.right * Time.deltaTime * speed * -1);
        if(Input.GetKey(KeyCode.X))
        {
            Time.timeScale = slowdown;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }
    }
}
