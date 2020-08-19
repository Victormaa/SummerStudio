using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Exit : MonoBehaviour
{
    //if player hits end of level trigger and has all machine parts
    //play open door animation
    //at end of animation remove box collider from door

    //once player hits next level box disable their control (keep them moving right) and move to the next level 
    
    private int collected_parts;
    private int required_parts;
    public GameObject endDoor;
    public GameObject level_complete;
    [SerializeField] private int nextScene = 0;
    [SerializeField] private Movement movement;
    [SerializeField] private AudioSource physical_bgm;
    [SerializeField] private AudioSource kenos_bgm;
    [SerializeField] private AudioSource victory_bgm;
    public float musicVolume = 1f;

    private void Start()
    {
        level_complete.SetActive(false);
        if (movement == null) {
            movement = (Movement) GameObject.FindObjectOfType (typeof(Movement));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Exit")
        {
            collected_parts = this.gameObject.GetComponent<Collectables>().machine_parts;
            required_parts = this.gameObject.GetComponent<Collectables>().required_machine_parts;
            Debug.Log(collected_parts);

            if(collected_parts >= required_parts && level_complete.activeSelf == false)
            {
                if (endDoor != null) {
                    endDoor.GetComponent<Animator>().SetTrigger("OpenDoor");
                }
                physical_bgm.Stop();
                kenos_bgm.Stop();
                victory_bgm.volume = musicVolume;
                victory_bgm.Play();
                level_complete.SetActive(true);
            }
        }

        else if (collision.gameObject.tag == "NextLevelTrigger") {
            movement.characterControlEnabled = false;//disable player movement
            StartCoroutine (LoadSceneAfterDelay(2f)); //wait for 3 seconds, then load next scene
        }
    }

    IEnumerator LoadSceneAfterDelay(float seconds) {
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup-startTime < seconds) {
            yield return null;
        }
        SceneManager.LoadScene(nextScene);// move to next scene
    }

}
