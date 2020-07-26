using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuNavigation : MonoBehaviour
{
    // public LoadScene sceneLoader;
    public AudioClip menuNavSound;
    public AudioClip menuSelectSound;
    public AudioSource menuSounds;
    [SerializeField] [Range(0, 1)] float volume = 1f;
    [SerializeField] [Range(0, 1)] float selectVolume = 1f;

    private bool controlEnabled = true;
    private Color32 selectedColor = new Color32(0, 180, 0, 255);
    private Color32 unselectedColor = new Color32(255, 255, 255, 255);
    private Color32 clickedColor = new Color32(0, 255, 0, 255);

    // public const int POINTERXPOS = 400;
    public Image pointer;

    public Text[] options;

    private int selectedOption;

    // Use this for initialization
    void Start () {
        selectedOption = 0;
        foreach (Text option in options) {
            option.color = unselectedColor;
        }
        options[0].color = selectedColor;
        pointer.transform.position = new Vector3(pointer.transform.position.x, options[0].transform.position.y);
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.DownArrow) && controlEnabled == true/*|| Controller input*/)
        { //Input telling it to go up or down.
            selectedOption += 1;
            if (selectedOption >= options.Length) //If at end of list go back to top
            {
                selectedOption = 0;
            }

            foreach (Text option in options) {
                option.color = unselectedColor;
            }
            options[selectedOption].color = selectedColor;
            pointer.transform.position = new Vector3(pointer.transform.position.x, options[selectedOption].transform.position.y);
            menuSounds.PlayOneShot(menuNavSound, volume);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && controlEnabled == true/*|| Controller input*/)
        { //Input telling it to go up or down.
            selectedOption -= 1;
            if (selectedOption < 0) //If at end of list go back to top
            {
                selectedOption = options.Length-1;
            }

            foreach (Text option in options) {
                option.color = unselectedColor;
            }
            options[selectedOption].color = selectedColor;
            pointer.transform.position = new Vector3(pointer.transform.position.x, options[selectedOption].transform.position.y);
            menuSounds.PlayOneShot(menuNavSound, volume);
        }

        if (controlEnabled == true && (Input.GetKeyDown(KeyCode.Space) ||  Input.GetKeyDown("joystick button 0"))){
            controlEnabled = false;
            // Debug.Log("Picked: " + selectedOption); //For testing as the switch statment does nothing right now.
            menuSounds.PlayOneShot(menuSelectSound, selectVolume);
            options[selectedOption].color = clickedColor;
            StartCoroutine(MenuOptionSceneChange());
            
        }
    }
    IEnumerator MenuOptionSceneChange() {
            yield return new WaitForSeconds(0.3f);
            // if (options[selectedOption].tag == "StartOption") {
            //     sceneLoader.SceneLoader(6);
            // }
            // else if (options[selectedOption].tag == "MainMenuOption") {
            //     sceneLoader.SceneLoader(0);
            // }
            // else if (options[selectedOption].tag == "QuitOption") {
            //     sceneLoader.doExitGame();
            // }
            // else if (options[selectedOption].tag == "SettingsOption") {
            //     sceneLoader.SceneLoader(7);
            // }
            // else if (options[selectedOption].tag == "CreditsOption") {
            //     sceneLoader.SceneLoader(8);
            // }
            controlEnabled = true;
            options[selectedOption].color = selectedColor;
        }
}
