using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashScript : MonoBehaviour
{
    public float Speed;
    public float timescale;
    private bool timepause;
    private bool rbcheck;
    private Rigidbody2D rb;
    private Vector3 worldPosition;

    // Start is called before the first frame update
    void Start()
    {
        Speed = 5f;
        timepause = false;
        rbcheck = false;
        worldPosition = Vector3.zero;
        rb = this.gameObject.GetComponent<Rigidbody2D>();


    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            if (!timepause)
            {
                Time.timeScale = timescale;
                timepause = true;
            }

            else if (timepause)
            {
                timepause = false;
                Time.timeScale = 1f;
                Debug.Log(Time.deltaTime);
                Debug.Log("Time scale" + Time.timeScale);
                worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                rb.velocity = (new Vector2(worldPosition.x, worldPosition.y) - new Vector2(this.transform.position.x, this.transform.position.y)) * Speed;
                rbcheck = false;

            }
        }

        if (!rbcheck)
        {
            if (Vector2.Distance(new Vector2(this.transform.position.x, this.transform.position.y), new Vector2(worldPosition.x, worldPosition.y)) <= 0.5f)
            {
                rb.velocity = Vector2.zero;
                rbcheck = true;
            }
        }



    }




}
