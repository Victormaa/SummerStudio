using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{

    #region The Dictionary _Ostrich contains all the Ostrich
    #pragma warning disable 0649
    [SerializeField]
    private GameObject RedO;
    [SerializeField]
    private GameObject BlueO;
    [SerializeField]
    private GameObject GreenO;
    [SerializeField]
    private GameObject YellowO;
    #pragma warning restore 0649

    private Dictionary<int, GameObject> _Ostrich = new Dictionary<int, GameObject>();
    //private int upperBound = 155;
    //private int lowerBound = -233;
    //private int leftBound = -735;
    //private int rightBound = 735;
    #endregion
    public float _timer = 0.0f;

    public delegate void Click();

    static public Click takephoto;

    public GameObject viewfinder;

    private bool overButton = false;

    private void Awake()
    {
        _Ostrich.Add(0, RedO);
        _Ostrich.Add(1, BlueO);
        _Ostrich.Add(2, GreenO);
        _Ostrich.Add(3, YellowO);
    }

    void Start()
    {
        //InvokeRepeating("Generate", 2.0f, 5.0f);
        canotShot();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0)
        {
            Touch touch0 = Input.GetTouch(0);

            if (touch0.phase == TouchPhase.Ended && !overButton)
            {
                _timer = 0.0f;
                Vector2 pivot = touch0.position;
                Vector3 _position = Camera.main.ScreenToWorldPoint(pivot);
                _position.z = 0;

                takephoto = null;
                viewfinder.transform.position = _position;
                viewfinder.SetActive(true);

                ContactFilter2D contactFilter2D = new ContactFilter2D();

                Collider2D[] allOverlappingColliders = new Collider2D[12];

                int numOfoverlap = viewfinder.GetComponent<Collider2D>().OverlapCollider(contactFilter2D, allOverlappingColliders);

                for(int i = 0; i < numOfoverlap; i++)
                {
                    Debug.Log(allOverlappingColliders[i].gameObject.name);
                }
                
                takephoto += disappear;
            }
        }

        if (takephoto != null)
        {
            _timer += Time.deltaTime;
            if (_timer > 2.0f)
            {
                takephoto.Invoke();
                takephoto = null;
            }
        }
    }

    void disappear()
    {
        viewfinder.SetActive(false);
    }

    public void canotShot()
    {
        overButton = true;
    }

    public void canShot()
    {
        overButton = false;
    }

    public void Generate()
    {
        // in this function I need to decide how many times to generate a Ostrich
        // it should be 2-3 times. each Ostrich should be set up a inteval between
        // them.
        int times = Random.Range(2, 4);
        //Debug.Log("Generate " + times + " times Ostrich");

        for(int i = 0; i < times; i++)
        {
            float interval = Random.Range(0.3f, 0.8f);

            int Ocolor = Random.Range(0, 4);

            //Debug.Log("interval time is " + interval + "seconds");

            StartCoroutine(OstrichGen(interval, Ocolor));
        }
    }

    private IEnumerator OstrichGen(float interval, int Ocolor)
    {

        yield return new WaitForSeconds(interval);
        int _x = Random.Range(0, 2);
        float _y = Random.Range(0.0f, 1.0f);
        int flipAngle = 0;

        _y = (1 - _y) * 1348.0f + 100.0f;
        _x = (_x == 0) ? 50 : 2174;
        flipAngle = (_x == 50) ? 0 : 180;

        GameObject b = Instantiate(_Ostrich[Ocolor], new Vector3(_x, _y, 0), Quaternion.identity *
            Quaternion.Euler(0, flipAngle, 0));

        // make sure the overlap order will shows up as from low to high
        b.GetComponent<SpriteRenderer>().sortingOrder = 1448 - (int)_y;


        Vector3 worldPoisition = Camera.main.ScreenToWorldPoint(b.transform.position);
        worldPoisition.z = 0;
        b.transform.position = worldPoisition;


        if (_x == 50)
        {
            Vector3 destination = Camera.main.ScreenToWorldPoint(new Vector3(_x + Screen.width + 10, _y, -Camera.main.transform.position.z));
            LeanTween.move(b, destination, 4.0f);
        }
        else
        {
            Vector3 destination = Camera.main.ScreenToWorldPoint(new Vector3(_x - Screen.width - 10, _y, -Camera.main.transform.position.z));
            LeanTween.move(b, destination, 4.0f);
        }
    }

    public void InvokeGen()
    {
        float generateTime = Random.Range(2.5f, 4.0f);

        Invoke("Generate", generateTime);
    }
}
