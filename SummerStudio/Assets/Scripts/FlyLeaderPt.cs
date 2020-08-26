using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyLeaderPt : MonoBehaviour
{
    const float PI = 3.14159265359f;
    // [SerializeField] private GameObject Boundaries;
    [SerializeField] private Rigidbody2D mRigidbody2D;

    [SerializeField] private float speed = 7f;
    [SerializeField] private float startingAngle;
    [SerializeField] private Vector2 direction;
    private bool anglePos;
    private bool magnitudePos;

    // Start is called before the first frame update
    void Start()
    {
        anglePos = (Random.value<0.5f);
        magnitudePos = Random.value<0.5f;

        startingAngle = Random.value * (float) PI/6f + (float) PI/6f;
        if (!anglePos) {
            startingAngle = -1f * startingAngle;
        }
        if (!magnitudePos) {
            speed = -1f * speed;
        }
        direction = new Vector2(Mathf.Cos(startingAngle), Mathf.Sin(startingAngle));
        direction = speed * direction;

        mRigidbody2D.velocity = direction;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.tag == "FlyFieldLimits") {
    //         Debug.Log("exit detected");
    //         Vector2 normal = (transform.position - collision.transform.position).normalized;
    //         mRigidbody2D.velocity = Vector2.Reflect(mRigidbody2D.velocity, collision.GetContact(0).normal);
    //     }
        
    // }
}
