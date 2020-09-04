using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KenosDetector : MonoBehaviour
{
    [SerializeField] private CircleCollider2D detector;
    [SerializeField] private WorldShift playerShift;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private GameObject indicator;
    [SerializeField] private GameObject indicatorZone;
    [SerializeField] private GameObject effect;
    [SerializeField] private GameObject effectZone;

    private List<GameObject> enemyList = new List<GameObject>();
    private List<GameObject> indicatorList = new List<GameObject>();

    private List<GameObject> platformList = new List<GameObject>();
    private List<GameObject> effectList = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {
        if (playerShift == null) {
            playerShift = gameObject.GetComponent<WorldShift>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject enemy in enemyList) { //for each indicator in for enemies
            Vector2 direction = enemy.transform.position - transform.position; //calculate vector between this and enemy
            int index = enemyList.IndexOf(enemy);
            if (index>=0) {
                indicatorList[index].transform.up = direction;// rotate each indicator in indicatorlist to point to enemy
                Color tmp = indicatorList[index].GetComponentInChildren<SpriteRenderer>().color;
                tmp.a = 1-(direction.magnitude/detector.radius);//set tranparency proportional to distance.
                indicatorList[index].GetComponentInChildren<SpriteRenderer>().color = tmp;
            }
        }
        
        float closestDistance = detector.radius;
        foreach (GameObject platform in platformList) {//for each platform in platformlist
            Vector2 distanceVector = platform.transform.position - transform.position;
            if (distanceVector.magnitude < closestDistance) {
                closestDistance = distanceVector.magnitude; //get the distance to closest kenos platform
            }
            //position corresponding effect in effectList
        }
        // if (playerSprite != null) { //change player sprite color
        //     Color tmp2 = playerSprite.color;
        //     float green = ((165f/255f) + ((90f/255f) * (closestDistance/detector.radius)));
        //     float blue = ((240f/255f) + ((17f/255f) * (closestDistance/detector.radius)));
        //     // Debug.Log("G: " + green + " B: " + blue);
        //     tmp2 = new Color(1, green, blue);
        //     playerSprite.color = tmp2;
        // }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "Enemy" && playerShift.w_Type && !enemyList.Contains(other.gameObject)) { //if enemy is detected and not in enemy list already
            enemyList.Add(other.gameObject);//add enemy to enemylist
            int index = enemyList.IndexOf(other.gameObject);
            indicatorList.Insert(index, Instantiate(indicator,indicatorZone.transform)); //create indicator in indicatorlist
            indicatorList[index].active = true;
            // Debug.Log("Enemy added at index " + index);
        }
        else if (other.gameObject.tag == "Enemy" && !playerShift.w_Type && enemyList.Contains(other.gameObject)) { //if you shift remove those indicators
            int index = enemyList.IndexOf(other.gameObject); //get index to remove object
            if (index>=0) {
                GameObject indicatorToRemove = indicatorList[index];
                enemyList.Remove(other.gameObject);//remove enemy from enemylist
                indicatorList.Remove(indicatorToRemove); //remove paired enemy
                Destroy(indicatorToRemove); //destroy the removed indicator
                // Debug.Log("Enemy removed at index " + index);
            }
        }
        if (other.gameObject.layer == 11 && other.gameObject.tag != "Enemy" && playerShift.w_Type && !platformList.Contains(other.gameObject)) { //if kenos platform is detected
            platformList.Add(other.gameObject);//add platform to platformlist
            //add effectobject
            Debug.Log("Platform added");
        }
        else if (other.gameObject.layer == 11 && other.gameObject.tag != "Enemy" && !playerShift.w_Type && platformList.Contains(other.gameObject)) { //if kenos platform is detected
            platformList.Remove(other.gameObject);//remove platform from platformlist
            //remove effect object
            Debug.Log("Platform removed");
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Enemy" && enemyList.Contains(other.gameObject)) { //if enemy is detected 
            int index = enemyList.IndexOf(other.gameObject); //get index to remove object
            if (index>=0) {
                GameObject indicatorToRemove = indicatorList[index];
                enemyList.Remove(other.gameObject);//remove enemy from enemylist
                indicatorList.Remove(indicatorToRemove); //remove paired enemy
                Destroy(indicatorToRemove); //destroy the removed indicator
                // Debug.Log("Enemy removed at index " + index);
            }
        }
        if (other.gameObject.layer == 11 && other.gameObject.tag != "Enemy" && platformList.Contains(other.gameObject)) { //if kenos platform is detected
            platformList.Remove(other.gameObject);//remove platform from platformlist
            //remove paired effectobject
            Debug.Log("Platform removed");
        }
    }    

}
