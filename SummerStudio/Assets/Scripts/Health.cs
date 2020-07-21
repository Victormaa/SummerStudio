using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int max_health;
    public int current_health;
    public bool state; //true = invulnerable state, false = vulnerable state
    public float invulnerability_cd;
    public Image[] hearts;
    public GameObject game_over;

    private void Start()
    {
        game_over.SetActive(false);

        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            HideHealth();
        }

    }

    public void TakeDamage()
    {
        if(current_health <= 1)
        {
            current_health--;
            hearts[current_health].enabled = false;
            game_over.SetActive(true); //display game over
        }
        else
        {
            current_health--; //subtract from health
            state = true; //set invulnerability
            //play invulnerable anim
            Invoke("ResetState", invulnerability_cd); //disable invulnerability
            hearts[current_health].enabled = false; //hide heart
        }
    }

    public void DisplayHealth()
    {
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
        }
    }

    public void TakeHealth(GameObject obj)
    {
        current_health++; //add to health
        hearts[current_health - 1].enabled = true; //show heart
        Destroy(obj);
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
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Health" && current_health < max_health)
        {
            TakeHealth(collision.gameObject);
        }
    }
}
