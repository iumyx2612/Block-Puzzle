using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace myengine.BlockPuzzle
{
    public class TileObject : MonoBehaviour
    {
        public PieceDisplay model;
        public PieceData _data;
        public PieceData fakeData;
        public Vector2 position;
        public GameObject flashbang;
        

        public void addPieceData(PieceData data)
        {
            model.LoadData(data);
            model.gameObject.SetActive(data != null);
            Color tmp = model.gameObject.GetComponent<SpriteRenderer>().color;
            tmp.a = 1f;
            model.gameObject.GetComponent<SpriteRenderer>().color = tmp;
            _data = data;
        }

        public void Hovering(PieceData data)
        {
            model.LoadData(data);
            fakeData = data;
            model.gameObject.SetActive(data != null);
            Color tmp = model.gameObject.GetComponent<SpriteRenderer>().color;
            tmp.a = 0.6f;
            model.gameObject.GetComponent<SpriteRenderer>().color = tmp;
        }

        public void UnHover()
        {
            fakeData = null;
            model.gameObject.SetActive(false);
            flashbang.SetActive(false);
        }

        public void Finish()
        {
            model.gameObject.SetActive(false);
            _data = null;
            fakeData = null;
            flashbang.SetActive(false);
        }

        public void Flash()
        {
            if(!flashbang.activeSelf)
            {
                flashbang.SetActive(true);
            }
        }

        public void UnFlash()
        {
            if(flashbang.activeSelf)
            {
                flashbang.SetActive(false);
            }
        }

        public bool isEmptyHover()
        {
            if(fakeData == null)
            {
                return true;
            }
            return false;
        }

        public bool isEmpty()
        {
            if (_data == null)
            {
                return true;
            }
            return false;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
