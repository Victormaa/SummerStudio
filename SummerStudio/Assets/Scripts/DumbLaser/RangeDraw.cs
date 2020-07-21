using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeDraw : MonoBehaviour
{
    [Range(0.1f, 100f)]
    public float radius = 1.0f;

    [Range(3, 256)]
    public int numSegments = 128;

    // Start is called before the first frame update
    void Start()
    {
        DoRenderer();
    }

    public void DoRenderer()
    {
        LineRenderer lineRenderer = gameObject.GetComponent<LineRenderer>();

        Color c1 = new Color(0.5f, 0.5f, 0.5f, 1);

        //lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        
        lineRenderer.startColor = c1;
        lineRenderer.endColor = c1;
        lineRenderer.startWidth = 0.03f;
        lineRenderer.endWidth = 0.03f;
        lineRenderer.positionCount = numSegments + 1;
        lineRenderer.useWorldSpace = false;

        float deltaTheta = (float)(1.0f * Mathf.PI) / numSegments;
        float theta = 0.5f * Mathf.PI;

        for (int i = 0; i < numSegments + 1; i++)
        {
            float y = radius * Mathf.Cos(theta);
            float x = radius * Mathf.Sin(theta);
            Vector2 pos = new Vector2(x, y);
            lineRenderer.SetPosition(i, pos);
            theta += deltaTheta;
        }
    }
}
