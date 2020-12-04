using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    public GameEvent rotate;
    public GameEvent nextpuzzle;
    public BlockList blockList;
    public SpawnPoint spawnPoint;
    [SerializeField] private int amountToPool;
    private List<GameObject> blocks = new List<GameObject>();
    public GameObject block;
    [SerializeField] private GameObject blockContainer;
    private int randomBlock;

    
    private void OnEnable()
    {
        rotate.AddListener(Rotate);
        nextpuzzle.AddListener(LoadBlockList);
    }
    private void OnDisable()
    {
        rotate.RemoveListener(Rotate);
        nextpuzzle.RemoveListener(LoadBlockList);
    }

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
            blocks[i].transform.parent = blockContainer.transform;
            blocks[i].GetComponent<BlockDrag>().oldPos = blocks[i].transform.position;
            //foreach (Transform child in blocks[i].transform)
            //{
            //    child.GetComponent<BlockDrag>().oldPos = blocks[i].transform.position;
            //}
        }
    }

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
                blocks[i].transform.parent = blockContainer.transform;
                blocks[i].GetComponent<BlockDrag>().oldPos = blocks[i].transform.position;
                //foreach (Transform child in blocks[i].transform)
                //{
                //    child.GetComponent<BlockDrag>().oldPos = blocks[i].transform.position;
                //}
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
