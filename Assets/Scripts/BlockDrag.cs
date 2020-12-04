using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;

public class BlockDrag : MonoBehaviour
{
    private bool onPoint = false;
    public Vector3 oldPos;
    private Vector2 defaultScaleSize = new Vector2(0.65f, 0.65f);
    [SerializeField] Vector2 dragScaleSize = new Vector2(1f, 1f);
    public BlockDragGameEvent drag;
    public bool check = false;

    private void Start()
    {
        
    }
    private void Drag()
    {
        
        TouchPhase touch = InputManager.Touch;
        if (touch.touchStarted)
        {
            switch (touch.touchPhase)
            {                
                case UnityEngine.TouchPhase.Began:
                    if (gameObject.GetComponent<BoxCollider2D>().bounds.Contains(touch.pos))
                    {
                        onPoint = true;
                        gameObject.transform.localScale = dragScaleSize;
                    }
                    break;
                case UnityEngine.TouchPhase.Moved:
                    if (gameObject.GetComponent<BoxCollider2D>().bounds.Contains(touch.pos) && onPoint)
                    {
                        gameObject.transform.localScale = dragScaleSize;
                        transform.position = touch.pos;
                        drag.Raise(this);
                    }
                    if(check)
                    {
                        Destroy(gameObject);
                    }
                    break;
                case UnityEngine.TouchPhase.Ended:
                    onPoint = false;
                    gameObject.transform.localScale = defaultScaleSize;
                    break;
            }
        }
    }
    

    private void Update()
    {
        Drag();
    }
}
