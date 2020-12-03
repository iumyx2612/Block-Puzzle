using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Piece Data")]
public class PieceDataList : ScriptableObject
{
    public List<PieceData> pieceDataList = new List<PieceData>();
}
