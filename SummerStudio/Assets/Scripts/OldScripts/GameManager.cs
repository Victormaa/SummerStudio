using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    No,
    Prepare,
    Ongoing,
    End
};

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Control control;

    public State _state = State.No;

    private int shotChance = 0;

    public int _shotChance
    {
        get
        {
            return shotChance;
        }
    }

    private float levelTime = 0; // time in seconds

    public float _levelTime
    {
        get
        {
            return levelTime;
        }
    }

    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if(instance == null)
            {
                new GameManager();
            }
            return instance;
        }
    }

    //levels contain each levels statistic;
    public List<Level> levels = new List<Level>();

    public int CurrentLN = 0;

    private Level CurrentLevel;

    public Level _currentLevel
    {
        get
        {
            return CurrentLevel;
        }
    }

    private void Awake()
    {
        if(instance != null)
        {
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        CurrentLevel = levels[0];
        ChangeState(State.Prepare);
    }

    // Update is called once per frame
    void Update()
    {
        
        switch (_state){
            case State.Prepare:

                break;
            case State.Ongoing:
                levelTime -= Time.deltaTime;

                if (levelTime <= 0)
                    ChangeState(State.End);

                break;
            case State.End:

                break;
            default:

                break;
        }
    }

    private void Shot()
    {
        shotChance -= 1;
    }

    public void ToGameMode()
    {
        ChangeState(State.Ongoing);
    }

    //all change state should go through this function
    public void ChangeState(State _newstate)
    {
        //Todo: to make things work more powerful
        //you should set up a event system here
        //to make each time a change state happens 
        //an onexit or an onenter would happens.

        //this is OnExit()
        switch (_state)
        {
            case State.Ongoing:
                control.canotShot();
                break;
            case State.Prepare:

                control.InvokeRepeating("InvokeGen", 1.5f,1.8f);

                break;
            case State.End:

                break;
            default:
                break;
        }

        //change state
        _state = _newstate;

        // this is OnEnter()
        switch (_state)
        {
            case State.Ongoing:
                control.canShot();
                break;
            case State.Prepare:
                levelTime = CurrentLevel.time;
                shotChance = CurrentLevel.shotChance;
                break;
            case State.End:
                control.CancelInvoke();
                break;
            default:
                break;
        }
    }
}
