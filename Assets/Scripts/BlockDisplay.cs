using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using ScriptableObjectArchitecture;
using myengine.BlockPuzzle;
using UnityEngine.UI;

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
        public GameEvent rotate;
        private List<Vector2> initialPoints = new List<Vector2>();
        private int rotateTimes = 0;
        [SerializeField] private GameEvent rotateBtnClicked;
        private bool isDisabled = false;

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
            enableRotate.AddListener(RotateBtnOnOff);
            rotateBtnClicked.AddListener(Disable);
        }

        public void RotateBtnOnOff()
        {
            if(enableRotate)
            {
                rotateBtn.SetActive(true);
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
            rotateBtnClicked.RemoveListener(Disable);
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
                rotateCounter.Value--;
                rotateTimes++;
                for (int i = 0; i < points.Count; i++)
                {
                    points[i] = new Vector2(-points[i].y, points[i].x);
                }
                transform.Rotate(0, 0, 90);
                int num_of_points_to_check = 0;
                for (int i = 0; i < points.Count; i++)
                {
                    if (points[i] == initialPoints[i])
                    {
                        num_of_points_to_check++;
                    }
                }
                if (num_of_points_to_check == points.Count)
                {
                    rotateCounter.Value += rotateTimes;
                }
            }            
        }

        public void Disable()
        {
            if(!isDisabled)
            {
                isDisabled = true;
                if (possibleRotation <= 1)
                {
                    foreach (GameObject piece in pieces)
                    {
                        if(piece.activeSelf)
                        {
                            piece.GetComponent<PieceDisplay>().LoadFakeData(dataList.pieceDataList[0]);
                        }
                    }
                }
            }
            else
            {
                isDisabled = false;
                foreach (GameObject piece in pieces)
                {
                    if (piece.activeSelf)
                    {
                        piece.GetComponent<PieceDisplay>().LoadFakeData(dataList.pieceDataList[chosenColor]);
                    }
                }
            }
        }
    }

}
