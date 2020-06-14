using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewFinder : MonoBehaviour
{
    private IEnumerator coroutine;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        coroutine = disappear();
        StartCoroutine(coroutine);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator disappear()
    {
        yield return new WaitForSeconds(2.0f);
        this.gameObject.SetActive(false);
    }

}
