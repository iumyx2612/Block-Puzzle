using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchPhase
{
    public bool touchStarted = false;
    public Vector2 pos;
    public UnityEngine.TouchPhase touchPhase;
}
public static class InputManager
{
    public static TouchPhase Touch
    {
        get
        {
            TouchPhase touch  = new TouchPhase();
#if UNITY_ANDROID || UNITY_IOS
            touch.touchStarted = Input.touchCount > 0;
            if(Input.touchCount > 0)
            {
                touch.touchPhase = Input.GetTouch(0).phase;
                touch.pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            }
            
#endif
#if UNITY_EDITOR
            touch.touchStarted = Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0);
            if (Input.GetMouseButtonDown(0))
            {
                touch.touchPhase = UnityEngine.TouchPhase.Began;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                touch.touchPhase = UnityEngine.TouchPhase.Ended;
            }
            else if (Input.GetMouseButton(0))
            {
                touch.touchPhase = UnityEngine.TouchPhase.Moved;
            }
             
            touch.pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
#endif
            return touch;
        }
    }
}
