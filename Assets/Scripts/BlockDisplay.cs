using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using ScriptableObjectArchitecture;
using myengine.BlockPuzzle;

namespace myengine.BlockPuzzle
{
    public class BlockDisplay : MonoBehaviour
    {
        public BlockList blockList;
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

        public List<Vector2> fakePoints = new List<Vector2>();
        // Start is called before the first frame update

        private void Awake()
        {
            //LoadData(11);
            for (int i = 0; i < preloadPieces; i++)
            {
                GameObject newPiece = Instantiate(piece, transform);
                newPiece.SetActive(false);
                pieces.Add(newPiece);
            }
        }
        void OnEnable()
        {
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
            //foreach (Vector2 point in points)
            //{
            //    fakePoints.Add(point);
            //}
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
            //fakePoints.Clear();
            offset = Vector2.zero;
            possibleRotation = 0;
            activeChild = 0;
            transform.rotation = Quaternion.identity;
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void LoadData(int data)
        {
            for (int i = 0; i < blockList.blockDatas[data].points.Count; i++)
            {
                points.Add(blockList.blockDatas[data].points[i]);
            }
            possibleRotation = blockList.blockDatas[data].possibleRotation;
            offset = blockList.blockDatas[data].offset;
            chosenColor = Random.Range(1, dataList.pieceDataList.Count);
        }        
    }

}
