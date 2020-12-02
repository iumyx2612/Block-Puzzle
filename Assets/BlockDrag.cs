using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDrag : MonoBehaviour
{
    public bool check;
    private Vector3 oldPos;

    private void Awake()
    {
        oldPos = transform.parent.position;
    }

    private void OnMouseDrag()
    {
        Vector3 newPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, +10f));
        transform.parent.position = newPos;
    }

    private void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            if(!check)
            {
                Debug.Log("Not check");
                transform.parent.position = oldPos;
            }
        }
    }
}
