using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatField : MonoBehaviour
{
    [SerializeField] private float floatGrav = -2.0f;
    [SerializeField] private float floatForce = -2.0f;

    private void OnTriggerStay2D(Collider2D collision)
    {

        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy" )
        {
            Debug.Log("player detected");
            Rigidbody2D objRigidBody = collision.transform.parent.GetComponent<Rigidbody2D>();
            if (objRigidBody != null) {
                objRigidBody.gravityScale = floatGrav * Time.timeScale * Time.timeScale;
                Debug.Log("playergravity at " + floatGrav);
                // Vector3 vel2 = objRigidBody.velocity;
                // vel2.y = 0f;
                // objRigidBody.velocity = vel2;
                // objRigidBody.AddForce(new Vector2(0f, floatForce * Time.timeScale));    
            }
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {

        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy" )
        {
            Debug.Log("player leaves field");
            Rigidbody2D objRigidBody = collision.transform.parent.GetComponent<Rigidbody2D>();
            if (objRigidBody != null) {
                objRigidBody.gravityScale = 2f * Time.timeScale * Time.timeScale;
                Debug.Log("playergravity at " + 2);
                // Vector3 vel2 = objRigidBody.velocity;
                // vel2.y = 0f;
                // objRigidBody.velocity = vel2;
                // objRigidBody.AddForce(new Vector2(0f, floatForce));    
            }
        }

    }

}
