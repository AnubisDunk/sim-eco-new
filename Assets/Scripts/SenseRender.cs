using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SenseRender : MonoBehaviour
{

    //public int steps = 50;
    //public float radius;
    LineRenderer cRenderer;

    public void DrawCircle(int steps,float radius)
    {
        cRenderer = GetComponent<LineRenderer>();
        cRenderer.positionCount = (steps + 1);
        cRenderer.useWorldSpace = false;
        CreatePoints(steps,radius);
    }

    void CreatePoints(int steps,float radius)
    {
        float x;
        float y;

        float angle = 20f;

        for (int i = 0; i < (steps + 1); i++)
        {
            x = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            y = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;

            cRenderer.SetPosition(i, new Vector3(x, 0, y));

            angle += (360f / steps);
        }
    }
}

