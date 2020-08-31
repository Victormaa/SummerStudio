using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpitter : MonoBehaviour
{
    public GameObject bullet;
    public GameObject spawnPoint;
    public bool bulletDirection; //true = up, false = down
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
        bulletObj = Instantiate(bullet, spawnPoint.transform.position, Quaternion.identity);
        bulletObj.name = this.gameObject.name + "bullet";
        bulletObj.GetComponent<Bullet>().SetBulletDirection(bulletDirection);
    }
}
