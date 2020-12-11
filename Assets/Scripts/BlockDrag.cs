using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableObjectArchitecture;
using DG.Tweening;

namespace myengine.BlockPuzzle
{
    public class BlockDrag : MonoBehaviour
    {
        public BlockList blockList;
        private bool onPoint = false;
        public Vector3 oldPos;
        private Vector2 defaultScaleSize = new Vector2(0.5f, 0.5f);
        [SerializeField] Vector2 dragScaleSize = new Vector2(1f, 1f);

        public bool isSelecting = false;

        public BlockDragGameEvent drag;
        public BlockDragGameEvent place;
        public Vector2 curPos;
        public Vector2 lastPos;
        public bool hovering = false;
        public bool check = false;

        public BlockDragGameEvent completeCheck;

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
                            isSelecting = true;
                            onPoint = true;
                            gameObject.transform.localScale = dragScaleSize;
                        }
                        break;
                    case UnityEngine.TouchPhase.Moved:
                        if(!isSelecting)
                        {
                            break;
                        }
                        if (gameObject.GetComponent<BoxCollider2D>().bounds.Contains(touch.pos) && onPoint)
                        {
                            gameObject.transform.localScale = dragScaleSize;
                            transform.position = touch.pos;
                            curPos = transform.position;
                            drag.Raise(this);
                        }
                        break;
                    case UnityEngine.TouchPhase.Ended:
                        if (!isSelecting)
                        {
                            break;
                        }
                        if (!check)
                        {
                            transform.DOMove(oldPos, 0.2f);
                            //transform.position = oldPos;
                        }
                        else
                        {
                            //gameObject.SetActive(false);
                            gameObject.transform.position = new Vector2(999, 999);
                            place.Raise(this);
                        }
                        isSelecting = false;
                        hovering = false;
                        onPoint = false;
                        gameObject.transform.localScale = defaultScaleSize;
                        check = false;
                        break;
                }
            }
        }


        private void Update()
        {
            Drag();
        }
    }
}
