using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text LevelTimer;

    public Text LeftShotChance;

    public GameObject RulesPanel;

    public Text Rule;

    public GameObject EndPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       LevelTimer.text = GameManager.Instance._levelTime.ToString("n1");
       LeftShotChance.text = GameManager.Instance._shotChance.ToString();
       Rule.text = "You got " + GameManager.Instance._currentLevel.time + " seconds to shot " + GameManager.Instance._currentLevel.targetnums + " " + GameManager.Instance._currentLevel.color + " ostrichs. ";

       if (GameManager.Instance._state == State.Prepare)
           EndPanel.SetActive(false);
       if (GameManager.Instance._state == State.Ongoing)
       {
           RulesPanel.SetActive(false);
           EndPanel.SetActive(false);
       }
       if (GameManager.Instance._state == State.End)
           EndPanel.SetActive(true);
    }
}
