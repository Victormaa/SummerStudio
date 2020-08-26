using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//I saw bug inside this document couple times. it seems like the array here is out of.

public class Health : MonoBehaviour
{
    public int max_health;
    public int current_health;
    public bool state; //true = invulnerable state, false = vulnerable state
    public float invulnerability_cd;
    public Image[] hearts;
    public Image[] broken_hearts;
    public Image[] bg_hearts;
    public GameObject game_over;
    [SerializeField] private AudioClip healthpackSound;
    [SerializeField] private AudioClip getHurtSound;
    private bool isColliding = false;

    [Range(0, 1000)] [SerializeField] private float knockbackStrength = 500f;
    [Range(0, 2)] [SerializeField] private float knockbackDuration = 1f;
    [SerializeField] public Movement movement;
    [SerializeField] private Rigidbody2D playerRigidbody2D;

    [Header("Events")]
    [Space]
    public static UnityEvent onTakenDamage;

    private void Awake()
    {
        if (onTakenDamage == null)
            onTakenDamage = new UnityEvent();
    }

    private void Start()
    {
        game_over.SetActive(false);

        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            HideHealth();
        }
        onTakenDamage.AddListener(TakeDamage);
    }

    public void TakeDamage()
    {
        
        if (current_health <= 1)
        {
            current_health--;

            hearts[current_health].enabled = false; //  goes wrong when current health lower than 0;

            game_over.SetActive(true); //display game over
        }
        else
        {
            current_health--; //subtract from health
            state = true; //set invulnerability
            if (getHurtSound != null) {
                AudioSource.PlayClipAtPoint(getHurtSound, transform.position);
            }
            //play invulnerable anim
            Invoke("ResetState", invulnerability_cd); //disable invulnerability
            hearts[current_health].enabled = false; //hide heart
        }
    }

    public void DisplayHealth()
    {
        for (int i = 0; i < max_health; i++)
        {
            broken_hearts[i].enabled = true;
            bg_hearts[i].enabled = true;
        }

        for (int i = 0; i < current_health; i++)
        {
            hearts[i].enabled = true;
        }
    }

    public void HideHealth()
    {
        for (int i = 0; i < max_health; i++)
        {
            hearts[i].enabled = false;
            broken_hearts[i].enabled = false;
            bg_hearts[i].enabled = false;
        }
    }

    public void TakeHealth()
    {
        current_health++; //add to health
        hearts[current_health - 1].enabled = true; //show heart
        if (healthpackSound != null) {
            AudioSource.PlayClipAtPoint(healthpackSound, transform.position);
        }
    }

    public int ReturnHealth()
    {
        return current_health;
    }

    void ResetState()
    {
        state = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy" && state != true)
        {
            TakeDamage();
            Knockback(collision.gameObject);
        }
    }

    private void CheckFirstCollision()
    {
        if(isColliding == false)
        {
            isColliding = true;
        }
        else
        {
            isColliding = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        CheckFirstCollision();
        if(collision.gameObject.tag == "Health" && current_health < max_health && isColliding == true)
        {
            Destroy(collision.gameObject);
            TakeHealth();
        }
        else if(collision.gameObject.tag == "Enemy" && state != true && gameObject.tag != "Kenos Collider")
        {
            TakeDamage();
            Knockback(collision.gameObject);
        }
    }

    

    public void Knockback(GameObject enemy)
    {
        if (movement != null) {
            movement.KBDisableControl(); //disable player controls for knockbackDuration
            var vel = playerRigidbody2D.velocity;
            Debug.Log(vel.x);
            Vector2 knockbackDirection = new Vector2(-1f * vel.x / 7.5f, 1f); //calculate knockback direction
            knockbackDirection.Normalize();
            knockbackDirection = knockbackDirection * (knockbackStrength);
            // Debug.Log(knockbackStrength + " strength, " + knockbackDuration + " duration, " + knockbackDirection.x + " " + knockbackDirection.y);
            if (playerRigidbody2D != null) {
                Vector3 vel2 = playerRigidbody2D.velocity;
                vel2.x = 0f;
			    vel2.y = 0f;
                playerRigidbody2D.velocity = vel2;
                playerRigidbody2D.AddForce(knockbackDirection); //add impulse force of knockback Strength away from enemy hit
            }
            movement.Invoke("KBEnableControl", knockbackDuration);
        }
    }
}
