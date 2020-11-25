using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDisplay : MonoBehaviour
{
    public BlockList blockList;
    public int chosenBlockData;
    public List<Vector2> points = new List<Vector2>();

    public GameObject piece;
    public List<PieceData> pieceData = new List<PieceData>();
    public int chosenColor;
    // Start is called before the first frame update
    void Awake()
    {
        chosenBlockData = Random.Range(0, blockList.blockDatas.Count);
        LoadData(blockList.blockDatas[chosenBlockData]);
        chosenColor = Random.Range(0, pieceData.Count);

        for (int i = 0; i < points.Count; i++)
        {
            GameObject newPiece = Instantiate(piece, new Vector2(points[i].x, points[i].y), Quaternion.identity);
            newPiece.GetComponent<PieceDisplay>().LoadData(pieceData[chosenColor]);            
            newPiece.transform.parent = gameObject.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(new Vector3(0, 0, 3 * Time.deltaTime));
    }

    public void LoadData(BlockData data)
    {
        points = blockList.blockDatas[chosenBlockData].points;
    }    
}
