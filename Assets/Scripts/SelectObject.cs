using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SelectObject : MonoBehaviour
{

    Ray ray;
    RaycastHit hit;
    Vector3 cast;
    public GameObject senseRadiusDrawer;
    private SenseRender sr;
    void Start()
    {
        sr = senseRadiusDrawer.GetComponent<SenseRender>();
    }
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
         if (Input.GetMouseButtonDown(0))
            {
                senseRadiusDrawer.transform.SetParent(null);
                sr.DrawCircle(1, 1);
                senseRadiusDrawer.transform.localPosition = Vector3.zero;
                senseRadiusDrawer.transform.rotation = Quaternion.identity;
            }
        if (Physics.Raycast(ray, out hit, 100f, 1 << 8, QueryTriggerInteraction.Ignore))
        {
            if (Input.GetMouseButtonDown(0) && hit.transform.CompareTag("Creature"))
            {
                //hit.transform.GetComponent<Creature>().isSelected = true;
                //senseRadiusDrawer.transform.position = transform.position;
                senseRadiusDrawer.transform.parent = hit.transform;
                cast = hit.transform.position;
                sr.DrawCircle(100, hit.transform.GetComponent<Creature>().senseRadius);
                senseRadiusDrawer.transform.localPosition = Vector3.zero;
                senseRadiusDrawer.transform.rotation = Quaternion.identity;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position;
#if UNITY_EDITOR
        Handles.color = Color.red;
        Handles.DrawLine(Input.mousePosition, cast);
#endif

    }
}
