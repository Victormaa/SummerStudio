using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private CharacterController2D m_controller2D;

    [Range(5,50)]public float runSpeed = 40f;

    bool crouch = false;

    bool jump = false;

    float horizontalMove = 0;
    // Start is called before the first frame update
    void Start()
    {
        if (this.GetComponent<CharacterController2D>())
        {
            m_controller2D = this.GetComponent<CharacterController2D>();
        }
        else
        {
            Debug.LogError("Role needs a rigb2d");
        }

        m_controller2D.OnCrouchEvent.AddListener(onCrouch);
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Crouch"))
            crouch = true;

            

        if (Input.GetButtonUp("Crouch"))
            crouch = false;

        if (Input.GetButtonDown("Jump"))
            jump = true;
            
    }

    private void FixedUpdate()
    {
        m_controller2D.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
        jump = false;
    }

    public void onCrouch(bool crouch)
    {
            Debug.Log(crouch);
    }
}
