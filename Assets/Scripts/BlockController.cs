using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    public BlockList blockList;
    private BlockDisplay bDisplay;
    public GameObject block;
    private GameObject emptyGO;
    private int randomBlock;

    private void Awake()
    {
        bDisplay = FindObjectOfType<BlockDisplay>();
        emptyGO = new GameObject();
        emptyGO.name = "Block Container";
    }

    public void NextBlock()
    {
        randomBlock = Random.Range(0, blockList.blockDatas.Count);
        bDisplay.chosenBlockData = randomBlock;
        bDisplay.LoadData(blockList.blockDatas[bDisplay.chosenBlockData]);
        for (int i = 0; i < block.transform.childCount; i++)
        {
            block.transform.GetChild(i).transform.position = new Vector2(bDisplay.points[i].x, bDisplay.points[i].y);
        }
    }

    public void Rotate()
    {
        emptyGO.transform.position = bDisplay.center;
        block.transform.parent = emptyGO.transform;
        emptyGO.transform.Rotate(0, 0, 90);
    }
}
