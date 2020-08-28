using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KenosDetector : MonoBehaviour
{
    [SerializeField] private WorldShift playerShift;
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
            indicatorList[index].transform.up = direction;// rotate each indicator in indicatorlist to point to enemy
            Color tmp = indicatorList[index].GetComponentInChildren<SpriteRenderer>().color;
            tmp.a = 1-(direction.magnitude/4f);//set tranparency proportional to distance.
            indicatorList[index].GetComponentInChildren<SpriteRenderer>().color = tmp;

        }
        

        //for each platform in platformlist
            //position corresponding effect in effectList
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "Enemy" && playerShift.w_Type && !enemyList.Contains(other.gameObject)) { //if enemy is detected and not in enemy list already
            enemyList.Add(other.gameObject);//add enemy to enemylist
            int index = enemyList.IndexOf(other.gameObject);
            indicatorList.Insert(index, Instantiate(indicator,indicatorZone.transform)); //create indicator in indicatorlist
            indicatorList[index].active = true;
            // Debug.Log("Enemy added at index " + index);
        }
        if (other.gameObject.layer == 11 && playerShift.w_Type) { //if kenos platform is detected
            platformList.Add(other.gameObject);//add platform to platformlist
            //add effectobject
            // Debug.Log("Platform added");
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Enemy") { //if enemy is detected 
            int index = enemyList.IndexOf(other.gameObject); //get index to remove object
            GameObject indicatorToRemove = indicatorList[index];
            enemyList.Remove(other.gameObject);//remove enemy from enemylist
            indicatorList.Remove(indicatorToRemove); //remove paired enemy
            Destroy(indicatorToRemove); //destroy the removed indicator
            // Debug.Log("Enemy removed at index " + index);
        }
        if (other.gameObject.layer == 11) { //if kenos platform is detected
            platformList.Remove(other.gameObject);//remove platform from platformlist
            //remove paired effectobject
            // Debug.Log("Platform removed");
        }
    }    

}
