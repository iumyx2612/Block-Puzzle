using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDisplay : MonoBehaviour
{
    public BlockList blockList;
    public List<Vector2> points = new List<Vector2>();
    public Vector3 center;

    private int preloadPieces = 9;
    public List<GameObject> pieces = new List<GameObject>();
    public GameObject piece;
    public List<PieceData> pieceData = new List<PieceData>();
    private int chosenColor;
    // Start is called before the first frame update

    private void Awake()
    {
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
            pieces[i].transform.position = points[i];
            pieces[i].GetComponent<PieceDisplay>().LoadData(pieceData[chosenColor]);
            pieces[i].SetActive(true);
        }
    }

    private void OnDisable()
    {
        //points.Clear();
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
        points = blockList.blockDatas[data].points;
        Debug.Log(points.Count);
        chosenColor = Random.Range(0, pieceData.Count);
    }    
}
