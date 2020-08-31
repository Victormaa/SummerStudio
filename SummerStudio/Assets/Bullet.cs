using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    [SerializeField] private bool bulletDirection; //true = up, false = down
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(bulletDirection)
        {
            transform.position += new Vector3(0f, 1f, 0f) * Time.deltaTime * speed;
        }
        else
        {
            transform.position += new Vector3(0f, -1f, 0f) * Time.deltaTime * speed;
        }
    }

    public void SetBulletDirection(bool direction)
    {
        bulletDirection = direction;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
