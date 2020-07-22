using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashScript : MonoBehaviour
{
    public float Speed;
    public float timescale;
    private bool timepause;
    private bool move;
    private Vector3 worldPosition;

    // Start is called before the first frame update
    void Start()
    {
        Speed = 5f;
        timepause = false;
        move = false;
        worldPosition = Vector3.zero;

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if(!timepause)
            {
                Time.timeScale = timescale;
                timepause = true;
            }

            else if(timepause)
            {
                timepause = false;  
                Time.timeScale = 1f;
                Debug.Log(Time.deltaTime);
                Debug.Log("Time scale" + Time.timeScale);
                move = true;
                worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            }
        }

        if(move)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, new Vector2(worldPosition.x, worldPosition.y), Speed * Time.deltaTime);
            if(Vector2.Distance(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2 (worldPosition.x, worldPosition.y)) < 0.2f)
            {
                move = false;
            }
        }
    }

   
}
