using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDrag : MonoBehaviour
{
    public bool check;
    public Vector3 oldPos;
    public bool isActive = false;

    private void Start()
    {
        
    }

    private void OnMouseDrag()
    {
        Vector3 newPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, +10f));
        transform.parent.position = newPos;
        isActive = true;
    }

    private void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            if(!check)
            {
                transform.parent.position = oldPos;
                isActive = false;
            }
            else
            {
                isActive = true;
            }
        }
    }
}
