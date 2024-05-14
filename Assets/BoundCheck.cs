using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoundCheck : MonoBehaviour
{
    public GameObject mark;
    void Start()
    {
       
    }
    void Mark()
    {
        for (int x = 0; x < Utils.boundMap.GetLength(0); x++)
        {
            for (int y = 0; y < Utils.boundMap.GetLength(1); y++)
            {
                if(!Utils.boundMap[x, y]) Instantiate(mark,new Vector3(x - (Utils.mapX/2),1,y * -1 + (Utils.mapZ/2)),Quaternion.identity);    
            }
        }
    }
        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.M)) Mark();
            //Debug.Log(Utils.mapX);
            //Debug.Log($"{transform.position} -> {Utils.boundMap[Mathf.RoundToInt(transform.position.x + (Utils.mapX/2)), Mathf.RoundToInt(transform.position.z * -1 + (Utils.mapZ/2))]}");
        }
    }
