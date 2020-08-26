using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float jumpPadForce = 600f;

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy" )
        {
            Rigidbody2D objRigidBody = collision.gameObject.GetComponent<Rigidbody2D>();
            if (objRigidBody != null) {
                Vector3 vel2 = objRigidBody.velocity;
                vel2.y = 0f;
                objRigidBody.velocity = vel2;
                objRigidBody.AddForce(new Vector2(0f, jumpPadForce * Time.timeScale));    
            }
        }

    }

}
