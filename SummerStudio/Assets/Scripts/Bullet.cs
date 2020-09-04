using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    [SerializeField] private int bulletDirection; //1 = up, 2 = down, 3 = left, 4 = right
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch(bulletDirection)
        {
            case 1:
                transform.position += new Vector3(0f, 1f, 0f) * Time.deltaTime * speed;
                break;
            case 2:
                transform.position += new Vector3(0f, -1f, 0f) * Time.deltaTime * speed;
                break;
            case 3:
                transform.position += new Vector3(-1f, 0f, 0f) * Time.deltaTime * speed;
                break;
            case 4:
                transform.position += new Vector3(1f, 0f, 0f) * Time.deltaTime * speed;
                break;
            default:
                Debug.Log("Direction error");
                break;
        }
    }

    public void SetBulletDirection(int direction)
    {
        bulletDirection = direction;
    }

    public void SetBulletSpeed(float bspeed)
    {
        speed = bspeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
