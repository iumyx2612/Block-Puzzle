using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    //public BlockList blockList;
    //public SpawnPoint spawnPoints;
    //[SerializeField] private GameObject pool;
    //[SerializeField] private int amountToPool;
    //public List<GameObject> blocks = new List<GameObject>();
    public GameObject block;
    private int randomBlock;

    public GameObject blockContainer;

    private void Awake()
    {
        //for (int i = 0; i < amountToPool / spawnPoints.spawnPoints.Count; i++)
        //{
        //    for (int j = 0; j < spawnPoints.spawnPoints.Count; j++)
        //    {
        //        GameObject newBlock = Instantiate(block, new Vector3(0, 0, 0), Quaternion.identity);
        //        newBlock.SetActive(false);
        //        blocks.Add(newBlock);
        //    }
        //}
        //for (int i = 0; i < 3; i++)
        //{
        //    randomBlock = Random.Range(0, blockList.blockDatas.Count);
        //    blocks[i].GetComponent<BlockDisplay>().LoadData(randomBlock);
        //    blocks[i].SetActive(true);
        //    blocks[i].transform.position = spawnPoints.spawnPoints[i];
        //}
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

    //public void LoadBlockList()
    //{
    //    for (int i = 0; i < 6; i++)
    //    {
    //        int count = 0;
    //        if(count == 3)
    //        {
    //            break;
    //        }
    //        if(blocks[i].activeSelf)
    //        {
    //            blocks[i].SetActive(false);
    //            Debug.Log("Deact");
    //        }
    //        else
    //        {
    //            randomBlock = Random.Range(0, blockList.blockDatas.Count);
    //            blocks[i].GetComponent<BlockDisplay>().LoadData(randomBlock);
    //            blocks[i].SetActive(true);
    //            blocks[i].transform.position = spawnPoints.spawnPoints[i % 3];
    //            //blocks[i].GetComponent<BlockDisplay>().LoadData(randomBlock);
    //            //Debug.Log(randomBlock);
    //            //blocks[i].SetActive(true);
    //            count++;
    //        }
    //    }
    //}

    public void Rotate()
    {
        blockContainer.transform.position = block.GetComponent<Test>().center;
        block.transform.parent = blockContainer.transform;
        blockContainer.transform.Rotate(0, 0, 90);
    }


}
