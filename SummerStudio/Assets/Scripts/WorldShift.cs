using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldShift : MonoBehaviour
{
    public bool w_Type = true; //true = physical world; false = other world
    private bool ignore_Layer = false; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            w_Type = !w_Type;
            Physics2D.IgnoreLayerCollision(8, 10, ignore_Layer);
            ignore_Layer = !ignore_Layer;
        }
    }
}
