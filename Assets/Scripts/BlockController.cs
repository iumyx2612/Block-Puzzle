using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace myengine.BlockPuzzle
{
    public class BlockController : MonoBehaviour
    {
        //public BlockList blockList;
        [SerializeField] private List<BlockList> blockLists = new List<BlockList>();
        [SerializeField] private SpawnPoint spawnPoint;
        [SerializeField] private int amountToPool;
        public GameObjectCollection blocks;
        public GameObject block;
        [SerializeField] private GameObject blockContainer;
        private int randomBlock;

        [SerializeField] private GameEvent nextpuzzle;
        [SerializeField] private IntVariable activeBlocks;
        [SerializeField] private IntVariable curScore;
        private List<BlockList> spawnAbleGroup = new List<BlockList>();
        private BlockList chosenGroup;

        [SerializeField] private GameObjectCollection remainBlocks;
        [SerializeField] private BoolVariable isGameOver;

        private void OnEnable()
        {
            nextpuzzle.AddListener(LoadBlockList);
        }
        private void OnDisable()
        {
            nextpuzzle.RemoveListener(LoadBlockList);
            activeBlocks.Value = 0;
            blocks.Clear();
            remainBlocks.Clear();
        }

        private void Awake()
        {
            for (int i = 0; i < amountToPool / spawnPoint.spawnPoints.Count; i++)
            {
                for (int j = 0; j < spawnPoint.spawnPoints.Count; j++)
                {
                    GameObject newBlock = Instantiate(block, spawnPoint.spawnPoints[j], Quaternion.identity) as GameObject;
                    newBlock.GetComponent<BlockDrag>().oldPos = newBlock.transform.position;
                    newBlock.SetActive(false);
                    blocks.Add(newBlock);
                }
            }
            for (int i = 0; i < 3; i++)
            {
                float randNum = Random.Range(0f, 0.85f);
                float percentage = 0;
                for (int j = 0; j < blockLists.Count; j++)
                {
                    percentage += blockLists[j].percentage;
                    if (percentage >= randNum)
                    {
                        chosenGroup = blockLists[j];
                        break;
                    }
                }
                //#region testcase
                //if (i == 0)
                //{
                //    randomBlock = 22;
                //}
                //else
                //{
                //    randomBlock = Random.Range(12, 13);
                //}
                //#endregion
                randomBlock = Random.Range(0, chosenGroup.blockDatas.Count);
                blocks[i].GetComponent<BlockDisplay>().chosenGroup = chosenGroup;
                blocks[i].GetComponent<BlockDisplay>().LoadData(randomBlock);
                blocks[i].SetActive(true);
                blocks[i].transform.position = spawnPoint.spawnPoints[i];
                blocks[i].transform.parent = blockContainer.transform;
                //blocks[i].GetComponent<BlockDrag>().oldPos = blocks[i].transform.position;
                activeBlocks.Value++;
                remainBlocks.Add(blocks[i]);
            }
        }

        public void LoadBlockList()
        {
            for (int i = 0; i < 6; i++)
            {
                if (!blocks[i].activeSelf)
                {
                    for (int j = 0; j < blockLists.Count; j++)
                    {
                        if(curScore > blockLists[j].startingScore && !spawnAbleGroup.Contains(blockLists[j]))
                        {
                            spawnAbleGroup.Add(blockLists[j]);
                        }
                    }
                    float upperRange = 0f;
                    for (int j = 0; j < spawnAbleGroup.Count; j++)
                    {
                        upperRange += spawnAbleGroup[j].percentage;
                    }
                    float randNum = Random.Range(0, upperRange);
                    float percentage = 0;
                    for (int j = 0; j < blockLists.Count; j++)
                    {
                        percentage += blockLists[j].percentage;
                        if (percentage >= randNum)
                        {
                            chosenGroup = blockLists[j];
                            break;
                        }
                    }
                    randomBlock = Random.Range(0, chosenGroup.blockDatas.Count);
                    blocks[i].GetComponent<BlockDisplay>().chosenGroup = chosenGroup;
                    blocks[i].GetComponent<BlockDisplay>().LoadData(randomBlock);
                    blocks[i].SetActive(true);
                    remainBlocks.Add(blocks[i]);
                    activeBlocks.Value++;
                    //blocks[i].transform.position = spawnPoint.spawnPoints[i % 3];
                    if (blocks[i].transform.parent == null)
                    {
                        blocks[i].transform.parent = blockContainer.transform;
                    }
                    //blocks[i].GetComponent<BlockDrag>().oldPos = blocks[i].transform.position;
                }
                else
                {
                    blocks[i].SetActive(false);
                    remainBlocks.Remove(blocks[i]);
                }
            }
        }                
    }
}