using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int max_health;
    public int current_health;
    public bool state; //true = invulnerable state, false = vulnerable state
    public float invulnerability_cd;

    public Image[] hearts;

    public void TakeDamage()
    {
        if(current_health == 1)
        {
            current_health--;
            //display game over
            //
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

    public void TakeHealth()
    {
        if(current_health == max_health)
        {
            //do not collide with health
        }
        else
        {
            current_health++; //add to health
            hearts[current_health - 1].enabled = true; //show heart
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
        }
        else if(collision.gameObject.tag == "Health")
        {
            TakeHealth();
        }
    }
}
