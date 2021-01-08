using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using ScriptableObjectArchitecture;
using myengine.BlockPuzzle;
using UnityEngine.UI;
using DG.Tweening;

namespace myengine.BlockPuzzle
{
    public class BlockDisplay : MonoBehaviour
    {
        public BlockList chosenGroup;
        public List<Vector2> points = new List<Vector2>();
        public Vector2 offset;
        public int possibleRotation;

        [SerializeField] private float pieceDistScale;
        private int preloadPieces = 9;
        private List<GameObject> pieces = new List<GameObject>();
        public GameObject piece;
        public PieceDataList dataList;
        private int chosenColor;

        public int activeChild;

        public GameObject rotateBtn;
        public BoolVariable enableRotate;
        public IntVariable rotateCounter;
        private List<Vector2> initialPoints = new List<Vector2>();
        [HideInInspector] public int rotateTimes = 0;
        [SerializeField] private GameEvent rotateBtnClicked;
        private bool isDisabled = false;

        public bool state;

        // Start is called before the first frame update

        private void Awake()
        {
            //LoadData(22);
            for (int i = 0; i < preloadPieces; i++)
            {
                GameObject newPiece = Instantiate(piece, transform);
                newPiece.SetActive(false);
                pieces.Add(newPiece);
            }
            RotateBtnOnOff();
        }
        void OnEnable()
        {
            RotateBtnOnOff();
            for (int i = 0; i < points.Count; i++)
            {
                pieces[i].transform.localPosition = pieceDistScale * (points[i] + offset);
                pieces[i].GetComponent<PieceDisplay>().LoadData(dataList.pieceDataList[chosenColor]);
                pieces[i].SetActive(true);
            }
            gameObject.transform.localScale = new Vector2(0.5f, 0.5f);
            foreach (Transform child in transform)
            {
                if (child.gameObject.activeSelf)
                {
                    activeChild++;
                }
            }
            //if(enableRotate && possibleRotation <= 1)
            //{
            //    isDisabled = true;
            //    foreach (GameObject piece in pieces)
            //    {
            //        if (piece.activeSelf)
            //        {
            //            piece.GetComponent<PieceDisplay>().LoadFakeData(dataList.pieceDataList[0]);
            //        }
            //    }
            //}
            enableRotate.AddListener(RotateBtnOnOff);
            rotateBtnClicked.AddListener(ToggleDisable);
        }

        public void RotateBtnOnOff()
        {
            if(enableRotate)
            {
                if (possibleRotation > 1)
                {
                    rotateBtn.SetActive(true);
                }
            }
            else
            {
                rotateBtn.SetActive(false);
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < points.Count; i++)
            {
                pieces[i].transform.localPosition = Vector2.zero;
                pieces[i].SetActive(false);
            }
            gameObject.transform.localScale = new Vector2(1f, 1f);
            gameObject.transform.position = gameObject.GetComponent<BlockDrag>().oldPos;
            points.Clear();
            initialPoints.Clear();
            offset = Vector2.zero;
            possibleRotation = 0;
            activeChild = 0;
            transform.rotation = Quaternion.identity;
            enableRotate.RemoveListener(RotateBtnOnOff);
            rotateBtnClicked.RemoveListener(ToggleDisable);
            rotateTimes = 0;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void LoadData(int data)
        {
            for (int i = 0; i < chosenGroup.blockDatas[data].points.Count; i++)
            {
                points.Add(chosenGroup.blockDatas[data].points[i]);
                initialPoints.Add(chosenGroup.blockDatas[data].points[i]);
            }
            possibleRotation = chosenGroup.blockDatas[data].possibleRotation;
            offset = chosenGroup.blockDatas[data].offset;
            chosenColor = Random.Range(1, dataList.pieceDataList.Count);
        }

        public void Rotate()
        {
            if(possibleRotation > 1)
            {
                for (int i = 0; i < points.Count; i++)
                {
                    points[i] = new Vector2(-points[i].y, points[i].x);
                }
                transform.Rotate(0, 0, 90);
            }
            int point_to_check = 0;
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i] != initialPoints[i])
                {
                    point_to_check++;
                }
                if(point_to_check == points.Count - 1)
                {
                    rotateTimes = 1;
                }
            }
            point_to_check = 0;
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i] == initialPoints[i])
                {
                    point_to_check++;
                }
                if(point_to_check == points.Count)
                {
                    rotateTimes = 0;
                }
            }
        }

        public void ToggleDisable()
        {
            //if (possibleRotation <= 1 && !isDisabled)
            //{
            //    isDisabled = true;
            //    foreach (GameObject piece in pieces)
            //    {
            //        if (piece.activeSelf)
            //        {
            //            piece.GetComponent<PieceDisplay>().LoadFakeData(dataList.pieceDataList[0]);
            //        }
            //    }
            //}
            //else
            //{
            //    isDisabled = false;
            //    foreach (GameObject piece in pieces)
            //    {
            //        if (piece.activeSelf)
            //        {
            //            piece.GetComponent<PieceDisplay>().LoadFakeData(dataList.pieceDataList[chosenColor]);
            //        }
            //    }
            //}
            if (!enableRotate)
            {
                for (int i = 0; i < points.Count; i++)
                {
                    points[i] = initialPoints[i];
                }
                transform.rotation = Quaternion.identity;
                rotateTimes = 0;
            }
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i] != initialPoints[i])
                {
                    rotateTimes = 1;
                }
            }
        }
        
        public void StateToggle()
        {
            if(!state)
            {
                for (int i = 0; i < points.Count; i++)
                {
                    pieces[i].GetComponent<PieceDisplay>().LoadFakeData(dataList.pieceDataList[0]);
                }
            }
            else
            {
                for (int i = 0; i < points.Count; i++)
                {
                    pieces[i].GetComponent<PieceDisplay>().LoadFakeData(dataList.pieceDataList[chosenColor]);
                }
            }
        }

    }

}
