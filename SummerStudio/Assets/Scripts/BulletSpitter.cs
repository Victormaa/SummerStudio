using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpitter : MonoBehaviour
{
    public GameObject bullet;
    public GameObject spawnPoint;
    public int bulletDirection; //1 = up, 2 = down, 3 = left, 4 = right
    public float bulletSpeed;
    public bool isActivated;
    private GameObject bulletObj;
    [SerializeField] private float currentTime;
    [SerializeField] private float spawnTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isActivated)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= spawnTime)
            {
                Fire();
                currentTime = 0;
            }
        }
        
    }

    void Fire()
    {
        if(bulletDirection == 1 || bulletDirection == 2)
        {
            bulletObj = Instantiate(bullet, spawnPoint.transform.position, Quaternion.identity);
        }
        else if(bulletDirection == 3 || bulletDirection == 4)
        {
            bulletObj = Instantiate(bullet, spawnPoint.transform.position, Quaternion.identity);
            bulletObj.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        }
        bulletObj.name = this.gameObject.name + "bullet";
        bulletObj.GetComponent<Bullet>().SetBulletDirection(bulletDirection);
        bulletObj.GetComponent<Bullet>().SetBulletSpeed(bulletSpeed);
    }
}
