using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    // Start is called before the first frame update

    int z1, z2;

    void Start()
    {
        z1 = -10;
        z2 = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(this.transform.position.z < z2)
            {
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, z2);
            }
            else if(this.transform.position.z > z1)
            {
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, z1);
            }
        }
    }
}
