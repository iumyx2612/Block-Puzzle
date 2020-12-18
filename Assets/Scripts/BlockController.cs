using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace myengine.BlockPuzzle
{
    public class BlockController : MonoBehaviour
    {
        public GameEvent rotate;

        public BlockList blockList;
        public SpawnPoint spawnPoint;
        [SerializeField] private int amountToPool;
        public GameObjectCollection blocks;
        public GameObject block;
        [SerializeField] private GameObject blockContainer;
        private int randomBlock;

        public GameEvent nextpuzzle;
        public IntVariable activeBlocks;

        public GameObjectCollection remainBlocks;

        private void OnEnable()
        {
            rotate.AddListener(Rotate);
            nextpuzzle.AddListener(LoadBlockList);
        }
        private void OnDisable()
        {
            rotate.RemoveListener(Rotate);
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
                randomBlock = Random.Range(0, blockList.blockDatas.Count);
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
                    randomBlock = Random.Range(0, blockList.blockDatas.Count);
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

        public void Rotate()
        {
            foreach (GameObject block in blocks)
            {
                if (block.activeSelf)
                {
                    block.transform.Rotate(0, 0, 90);
                    for (int i = 0; i < block.GetComponent<BlockDisplay>().points.Count; i++)
                    {
                        block.GetComponent<BlockDisplay>().points[i] =
                            new Vector2(-block.GetComponent<BlockDisplay>().points[i].y,
                            block.GetComponent<BlockDisplay>().points[i].x);
                    }
                }
            }
        }
    }
}