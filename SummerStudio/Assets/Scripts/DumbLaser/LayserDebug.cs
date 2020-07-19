using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LayserDebug : MonoBehaviour
{
    
    public TextMeshPro textmesh;
    public LaserEye laserEye;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (laserEye.mLaserState)
        {
            case LaserState.Hide:
                textmesh.text = "Hide";
                break;
            case LaserState.Sleeping:
                textmesh.text = "Sleeping";
                break;
            case LaserState.Alerting:
                textmesh.text = "Alerting";
                break;
            case LaserState.Aiming:
                textmesh.text = "Aiming";
                break;
            case LaserState.Locked:
                textmesh.text = "Locked";
                break;
            case LaserState.Shoot:
                textmesh.text = "Shoot";
                break;
            default:
                break;
        }
    }
}
