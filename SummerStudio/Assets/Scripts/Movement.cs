using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // public Collider2D bottonCollider2D; //dunno what this is for so it's disabled for now
    private CharacterController2D m_controller2D;
    [SerializeField] Canvas pauseMenu;
    public bool characterControlEnabled = true; //disable when game is paused or if player dies/completes level
    private float timeScaleAtPause = 1;
    [Range(5,50)]public float runSpeed = 40f;
    bool crouch = false;
    bool jump = false;
    float horizontalMove = 0;

    public Texture2D CursorTex;
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

        Cursor.SetCursor(CursorTex, Vector2.zero, CursorMode.ForceSoftware);
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("escape") || Input.GetKeyDown("p")) { //if player presses button to pause game
            //toggle pause menu
            if (!characterControlEnabled) {
                resumeGame();
            }
            else {
                pauseGame();
            }
        }
        
        if (characterControlEnabled) {
            horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

            if (Input.GetButtonDown("Crouch"))
                crouch = true;

            if (Input.GetButtonUp("Crouch"))
                crouch = false;

            if (Input.GetButtonDown("Jump"))
                jump = true;

            if(this.GetComponent<CharacterController2D>().IsStuck())
            {
                Debug.Log("Character Registers as Stuck");
                // this.gameObject.transform.SetY(0.15f);
            }
        }
    }

    private void FixedUpdate()
    {
        if (characterControlEnabled) {
            m_controller2D.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
            jump = false;
        }
    }

    public void onCrouch(bool crouch)
    {
        Debug.Log(crouch);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Physical")
            Debug.Log(collision.gameObject.name);
    }

    public void pauseGame() {
        characterControlEnabled = false; //toggle player control
        if (pauseMenu != null) {
            pauseMenu.gameObject.SetActive(true);
        }
        timeScaleAtPause = Time.timeScale;
        Time.timeScale = 0;
        Debug.Log("Game Paused.");
    }

    public void resumeGame() {
        characterControlEnabled = true; //toggle player control
        if (pauseMenu != null) {
            pauseMenu.gameObject.SetActive(false);
        }
        Time.timeScale = timeScaleAtPause;
        Debug.Log("Game Unpaused.");
    }
}

public static class TransformExtentions
{
    public static void SetX(this Transform transform, float x)
    {
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
    public static void SetY(this Transform transform, float y)
    {
        transform.position = new Vector3(transform.position.x, y, transform.position.z);
    }
    public static void SetZ(this Transform transform, float z)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, z);
    }
}