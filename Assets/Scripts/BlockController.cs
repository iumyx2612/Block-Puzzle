using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    public BlockList blockList;
    public SpawnPoint spawnPoint;
    [SerializeField] private int amountToPool;
    private List<GameObject> blocks = new List<GameObject>();
    public GameObject block;
    private int randomBlock;    

    private void Awake()
    {
        for (int i = 0; i < amountToPool / spawnPoint.spawnPoints.Count; i++)
        {
            for (int j = 0; j < spawnPoint.spawnPoints.Count; j++)
            {
                GameObject newBlock = Instantiate(block, new Vector3(0, 0, 0), Quaternion.identity);
                newBlock.SetActive(false);
                blocks.Add(newBlock);
            }
        }
        for (int i = 0; i < 3; i++)
        {
            randomBlock = Random.Range(0, blockList.blockDatas.Count);
            blocks[i].GetComponent<BlockDisplay>().LoadData(randomBlock);
            blocks[i].SetActive(true);
            blocks[i].transform.position = spawnPoint.spawnPoints[i];
        }
    }

    //public List<GameObject> GetPooledObjects()
    //{
    //    List<GameObject> pooledBlocks = new List<GameObject>();
    //    for (int i = 0; i < blocks.Count; i++)
    //    {
    //        if(!blocks[i].activeSelf)
    //        {
    //            pooledBlocks.Add(blocks[i]);
    //        }
    //    }
    //    if(pooledBlocks.Count == 0)
    //    {
    //        for (int i = 0; i < spawnPoints.spawnPoints.Count; i++)
    //        {
    //            GameObject newBlock = Instantiate(block, spawnPoints.spawnPoints[i], Quaternion.identity);
    //            blocks.Add(newBlock);
    //            pooledBlocks.Add(newBlock);
    //            newBlock.SetActive(false);
    //        }
    //    }
    //    return pooledBlocks;
    //}

    public void LoadBlockList()
    {
        for (int i = 0; i < 6; i++)
        {
            if(blocks[i].activeSelf)
            {
                blocks[i].SetActive(false);
            }
            else
            {
                randomBlock = Random.Range(0, blockList.blockDatas.Count);
                blocks[i].GetComponent<BlockDisplay>().LoadData(randomBlock);
                blocks[i].SetActive(true);
                blocks[i].transform.position = spawnPoint.spawnPoints[i % 3];
            }
        }
    }

    public void Rotate()
    {
        foreach (GameObject block in blocks)
        {
            if(block.activeSelf)
            {
                block.transform.Rotate(0, 0, 90);
            }
        }
    }


}
