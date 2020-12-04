using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using ScriptableObjectArchitecture;

public class BlockDisplay : MonoBehaviour
{
    public BlockList blockList;
    public List<Vector2> points = new List<Vector2>();
    private Vector3 center;

    [SerializeField] private float pieceDistScale;
    private int preloadPieces = 9;
    private List<GameObject> pieces = new List<GameObject>();
    public GameObject piece;
    public PieceDataList dataList;
    private int chosenColor;

    public int activeChild;
    // Start is called before the first frame update

    private void Awake()
    {
        LoadData(13);
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
            pieces[i].transform.position = (Vector2)gameObject.transform.position + pieceDistScale * points[i];
            pieces[i].GetComponent<PieceDisplay>().LoadData(dataList.pieceDataList[chosenColor]);
            pieces[i].SetActive(true);
        }
        gameObject.transform.localScale = new Vector2(0.3f, 0.3f);
        foreach (Transform child in transform)
        {
            if(child.gameObject.activeSelf)
            {
                activeChild++;
            }
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < points.Count; i++)
        {
            pieces[i].transform.position = Vector2.zero;
            pieces[i].SetActive(false);
        }
        gameObject.transform.localScale = new Vector2(1f, 1f);
        points.Clear();
        activeChild = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 sumVector = Vector3.zero;
        foreach (Transform child in gameObject.transform)
        {
            sumVector += child.position;
        }
        center = sumVector / gameObject.transform.childCount;        
    }

    public void LoadData(int data)
    {
        for (int i = 0; i < blockList.blockDatas[data].points.Count; i++)
        {
            points.Add(blockList.blockDatas[data].points[i]);
        }
        chosenColor = Random.Range(0, dataList.pieceDataList.Count);
    }
}
