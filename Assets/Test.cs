using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public BlockData blockList;
    public List<Vector2> points = new List<Vector2>();
    public GameObject piece;
    public PieceData pieceData;
    public Vector3 center;
    // Start is called before the first frame update
    void Start()
    {
        points = blockList.points;
        for (int i = 0; i < points.Count; i++)
        {
            GameObject newPiece = Instantiate(piece, new Vector2(points[i].x, points[i].y), Quaternion.identity);
            newPiece.GetComponent<PieceDisplay>().LoadData(pieceData);
            newPiece.transform.parent = gameObject.transform;
        }
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
}
