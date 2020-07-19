using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroller : MonoBehaviour
{
    public Transform[] PatrolPoints;
    //public GameObject StartPoint;
    //public GameObject EndPoint;
    private int currentdestid;
    public float speed = 5f;
    public float Waittime;
    private float currenttime;
    // Start is called before the first frame update
    void Start()
    {
        currenttime = Waittime;
        currentdestid = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, PatrolPoints[currentdestid].position, speed * Time.deltaTime);
        Debug.Log(PatrolPoints[currentdestid].position);
        if (Vector2.Distance(this.transform.position, PatrolPoints[currentdestid].position) < 0.2f)
        {
            if (currenttime <= 0)
            {
                currentdestid = (currentdestid + 1) % PatrolPoints.Length;
                currenttime = Waittime;
            }

            else
            {
                currenttime -= Time.deltaTime;
            }
        }
    }
}
