using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class BlockDrag : MonoBehaviour
{
    public BoolReference check;
    private bool onPoint = false;
    public Vector3 oldPos;
    public bool isActive = false;

    private void Start()
    {
        
    }

    private void Drag()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(touch.position);
            switch(touch.phase)
            {
                case TouchPhase.Began:
                    if(gameObject.GetComponent<BoxCollider2D>() == Physics2D.OverlapPoint(touchPos))
                    {
                        onPoint = true;
                        isActive = true;
                    }
                    break;
                case TouchPhase.Moved:
                    if(gameObject.GetComponent<BoxCollider2D>() == Physics2D.OverlapPoint(touchPos) 
                        && onPoint)
                    {
                        transform.position = touchPos;
                    }
                    break;
                case TouchPhase.Ended:
                    onPoint = false;
                    isActive = false;
                    break;
            }
        }
    }

    public void Check()
    {
        
    }

    private void Update()
    {
        Drag();
        //if(Input.GetMouseButtonUp(0))
        //{
        //    if(!check)
        //    {
        //        transform.position = oldPos;
        //        isActive = false;
        //    }
        //    else
        //    {
        //        isActive = true;
        //    }
        //}
    }
}
