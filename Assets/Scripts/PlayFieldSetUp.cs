﻿using myengine.BlockPuzzle;
using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace myengine.BlockPuzzle
{
    public class PlayFieldSetUp : MonoBehaviour
    {
        [SerializeField] private float xDist;
        [SerializeField] private float yDist;
        [SerializeField] private GameObject squareField;
        private List<TileObject> tileList = new List<TileObject>();
        public float xSquareField;
        public float ySquareField;
        public GameObject greenArea;
        [SerializeField] private Vector2 position;
        [SerializeField] private Vector2 basePosition;
        [SerializeField] private float pieceDistScale;

        public BlockDragGameEvent dragListen;
        public BlockDragGameEvent placeListen;

        private List<TileObject> tileToCheck = new List<TileObject>();
        private GameObject oldTile;
        private GameObject curTile;

        public IntVariable activeBlocks;
        public GameEvent loadNextBlocks;

        private List<TileObject> rowTiles = new List<TileObject>();
        private List<TileObject> colTiles = new List<TileObject>();
        private bool rowComplete = false;
        private bool colComplete = false;

        private List<TileObject> hoverRowTiles = new List<TileObject>();
        private List<TileObject> hoverColTiles = new List<TileObject>();
        private bool hoverRowComplete = false;
        private bool hoverColComplete = false;

        public GameObjectCollection remainBlocks;
        public BoolVariable isGameOver;
        private List<TileObject> remainingTile = new List<TileObject>();
        public PieceDataList PieceDataList;
        public GameEvent gameOverEvent;

        private int num_of_row_col;
        private readonly int[] scoreSet = new int[]
        {
            10, 30, 60, 100, 200
        };
        public Text curScoreText;
        public IntVariable curScore;

        public GameEvent test;

        private void OnEnable()
        {
            dragListen.AddListener(DragCheck);
            test.AddListener(Test);
            placeListen.AddListener(Place);
        }

        private void OnDisable()
        {
            dragListen.RemoveListener(DragCheck);
            placeListen.RemoveListener(Place);
            isGameOver.Value = false;
            test.RemoveListener(Test);
            curScore.Value = 0;
        }

        public TileObject tim(Vector2 pos)
        {
            for (int i = 0; i < tileList.Count; ++i)
            {
                if (tileList[i].position == pos)
                {
                    return tileList[i];
                }
            }
            return null;
        }

        public bool Check(List<Vector2> points, TileObject tile)
        {
            List<Vector2> temp_points = new List<Vector2>();
            foreach (Vector2 point in points)
            {
                temp_points.Add(point);
            }
            for (int i = 1; i < points.Count; i++)
            {
                Vector2 pointToCheck = tile.position + temp_points[i];
                TileObject tempTile = tim(pointToCheck);
                if(tempTile != null && !tileToCheck.Contains(tempTile))
                {
                    tileToCheck.Add(tempTile);
                }
                else
                {
                    return false;
                }
            }
            if (tileToCheck.Count > 0)
            {
                int checkAmount = 0;
                foreach (TileObject _tile in tileToCheck)
                {
                    if (_tile.isEmpty())
                    {
                        checkAmount++;
                    }
                    if (checkAmount >= points.Count)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void DragCheck(BlockDrag drag)
        {
            GameObject basePiece = drag.gameObject.transform.GetChild(0).gameObject;
            if(gameObject.GetComponent<BoxCollider2D>().bounds.Contains(basePiece.transform.position))
            {
                for (int i = 0; i < tileList.Count; i++)
                {
                    GameObject ground = tileList[i].gameObject;
                    if (ground.GetComponent<BoxCollider2D>().bounds.Contains(basePiece.transform.position))
                    {
                        curTile = ground;
                        if (oldTile != curTile)
                        {
                            foreach (TileObject tile in tileToCheck)
                            {
                                if (tile.isEmpty())
                                {
                                    tile.UnHover();
                                }
                            }
                            foreach (TileObject tile in hoverColTiles)
                            {
                                tile.UnFlash();
                            }
                            foreach (TileObject tile in hoverRowTiles)
                            {
                                tile.UnFlash();
                            }
                            oldTile = curTile;
                            tileToCheck.Clear(); drag.gameObject.GetComponent<BlockDrag>().check = false;
                            drag.gameObject.GetComponent<BlockDrag>().hovering = false;
                            hoverColComplete = false;
                            hoverRowComplete = false;
                            hoverRowTiles.Clear();
                            hoverColTiles.Clear();
                        }
                        if (!tileToCheck.Contains(tileList[i]))
                        {
                            tileToCheck.Add(tileList[i]);
                        }
                        if(Check(drag.gameObject.GetComponent<BlockDisplay>().points, tileList[i]))
                        {
                            drag.check = true;
                            drag.hovering = true;
                            if(drag.hovering)
                            {
                                foreach (TileObject tile in tileToCheck)
                                {
                                    tile.Hovering(basePiece.GetComponent<PieceDisplay>().data);
                                }
                                foreach (TileObject tile in tileToCheck)
                                {
                                    HoverCompletionCheck(tile);
                                }
                                if (hoverRowComplete)
                                {
                                    foreach (TileObject tile in hoverRowTiles)
                                    {
                                        tile.Flash();
                                    }
                                }
                                if (hoverColComplete)
                                {
                                    foreach (TileObject tile in hoverColTiles)
                                    {
                                        tile.Flash();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (TileObject tile in tileToCheck)
                {
                    if (tile.isEmpty())
                    {
                        tile.UnHover();
                    }
                }
                foreach (TileObject tile in hoverColTiles)
                {
                    tile.UnFlash();
                }
                foreach (TileObject tile in hoverRowTiles)
                {
                    tile.UnFlash();
                }
                oldTile = curTile;
                tileToCheck.Clear(); drag.gameObject.GetComponent<BlockDrag>().check = false;
                drag.gameObject.GetComponent<BlockDrag>().hovering = false;
                hoverColComplete = false;
                hoverRowComplete = false;
                hoverRowTiles.Clear();
                hoverColTiles.Clear();
            }
            
        }

        public void Place(BlockDrag drag)
        {
            if (drag.check)
            {
                foreach (TileObject tile in tileToCheck)
                {
                    tile.AddPieceData(drag.transform.GetChild(0).GetComponent<PieceDisplay>().data);
                    remainingTile.Remove(tile);
                }
            }
            int oldRowCount = 0;
            int curRowCount = 0;
            int oldColCount = 0;
            int curColCount = 0;
            foreach (TileObject tile in tileToCheck)
            {
                CheckCompletion(tile);
                if (rowComplete)
                {
                    curRowCount = rowTiles.Count;
                    int ran_num = Random.Range(0, 2);
                    foreach (TileObject tileObject in rowTiles)
                    {
                        if(ran_num == 0)
                        {
                            tileObject.SpinnyFinish((int)tileObject.position.x);
                        }
                        else
                        {
                            tileObject.DragFinish((int)tileObject.position.x);
                        }
                    }
                    if(oldRowCount != curRowCount)
                    {
                        oldRowCount = curRowCount;
                        num_of_row_col += 1;
                    }
                    rowComplete = false;
                }
                if (colComplete)
                {
                    curColCount = colTiles.Count;
                    int ran_num = Random.Range(0, 2);
                    foreach (TileObject tileObject in colTiles)
                    {
                        if(ran_num == 0)
                        {
                            tileObject.SpinnyFinish(-(int)tileObject.position.y);
                        }
                        else
                        {
                            tileObject.DragFinish(-(int)tileObject.position.y);
                        }
                    }
                    if (oldColCount != curColCount)
                    {
                        num_of_row_col += 1;
                    }
                    colComplete = false;
                }
            }
            ScoreCalculation(drag.gameObject.GetComponent<BlockDisplay>().activeChild);
            if (num_of_row_col > 0)
            {
                ScoreCalculation(scoreSet[num_of_row_col - 1]);
            }
            rowTiles.Clear();
            colTiles.Clear();
            tileToCheck.Clear();
            activeBlocks.Value--;
            remainBlocks.Remove(drag.gameObject);
            if (activeBlocks.Value <= 0)
            {
                loadNextBlocks.Raise();
            }
            isGameOver.Value = EndGameCheck();
            tileToCheck.Clear();
            if(isGameOver)
            {
                Freeze();
                StartCoroutine(WaitTillGameOver());
            }
        }

        public void CheckCompletion(TileObject tile)
        {
            int _x = (int)tile.position.x;
            int _y = (int)tile.position.y;
            int rowCount = 0;
            int colCount = 0;
            #region check row
            for (int i = 1; i <= 8; i++)
            {
                if (!tileList[-8 * _y + i - 1].isEmpty())
                {
                    rowCount++;
                    rowTiles.Add(tileList[-8 * _y + i - 1]);
                }
                else
                {
                    rowTiles.Clear();
                    break;
                }
            }
            if (rowCount == 8)
            {
                rowComplete = true;
            }
            #endregion
            #region check col
            for (int i = 0; i <= 7; i++)
            {
                if (!tileList[i * 8 + _x].isEmpty())
                {
                    colCount++;
                    colTiles.Add(tileList[i * 8 + _x]);
                }
                else
                {
                    colTiles.Clear();
                    break;
                }
            }
            if (colCount == 8)
            {
                colComplete = true;
            }
            #endregion
        }

        public void HoverCompletionCheck(TileObject tile)
        {
            List<TileObject> tempRowTile = new List<TileObject>();
            List<TileObject> tempColTile = new List<TileObject>();
            int _x = (int)tile.position.x;
            int _y = (int)tile.position.y;
            #region check row
            for (int i = 1; i <= 8; i++)
            {
                if (!tileList[-8 * _y + i - 1].isEmptyHover())
                {
                    if(!tempRowTile.Contains(tileList[-8 * _y + i - 1]))
                    {
                        tempRowTile.Add(tileList[-8 * _y + i - 1]);
                    }
                }
                else
                {
                    tempRowTile.Clear();
                    break;
                }
            }
            if (tempRowTile.Count == 8)
            {
                for (int i = 0; i < tempRowTile.Count; i++)
                {
                    if (!hoverRowTiles.Contains(tempRowTile[i]))
                    {
                        hoverRowTiles.Add(tempRowTile[i]);
                    }
                }
            }
            #endregion
            #region check col
            for (int i = 0; i <= 7; i++)
            {
                if (!tileList[i * 8 + _x].isEmptyHover())
                {
                    if(!tempColTile.Contains(tileList[i * 8 + _x]))
                    {
                        tempColTile.Add(tileList[i * 8 + _x]);
                    }
                }
                else
                {
                    tempColTile.Clear();
                    break;
                }
            }
            if (tempColTile.Count == 8)
            {
                for (int i = 0; i < tempColTile.Count; i++)
                {
                    if(!hoverColTiles.Contains(tempColTile[i]))
                    {
                        hoverColTiles.Add(tempColTile[i]);
                    }
                }
            }
            #endregion
            if(hoverColTiles.Count > 0)
            {
                hoverColComplete = true;
            }
            if(hoverRowTiles.Count > 0)
            {
                hoverRowComplete = true;
            }
        }

        public void RemainingTiles()
        {
            for (int i = 0; i < tileList.Count; i++)
            {
                if(tileList[i].isEmpty())
                {
                    if(!remainingTile.Contains(tileList[i]))
                    {
                        remainingTile.Add(tileList[i]);
                    }
                }
            }
        }

        public List<Vector2> FakeRotate(List<Vector2> points)
        {
            List<Vector2> fakePoints = new List<Vector2>();
            for (int i = 0; i < points.Count; i++)
            {
                fakePoints.Add(new Vector2(-points[i].y, points[i].x));
            }
            return fakePoints;
        }

        public bool EndGameCheck()
        {
            RemainingTiles();
            foreach (GameObject block in remainBlocks)
            {
                if (remainingTile.Count < block.GetComponent<BlockDisplay>().activeChild)
                {
                    return true;
                }
                List<Vector2> possible_points_of_block = new List<Vector2>();
                for (int i = 0; i < remainingTile.Count; i++)
                {
                    List<Vector2> fakePoints = block.GetComponent<BlockDisplay>().points;
                    for (int j = 0; j < block.GetComponent<BlockDisplay>().possibleRotation; j++)
                    {
                        tileToCheck.Clear();
                        tileToCheck.Add(remainingTile[i]);
                        if(j > 0)
                        {
                            if(Check(fakePoints, remainingTile[i]))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (Check(block.GetComponent<BlockDisplay>().points, remainingTile[i]))
                            {
                                return false;
                            }
                        }
                        fakePoints = FakeRotate(fakePoints);
                    }
                }
            }
            return true;
        }

        public void Freeze()
        {
            for (int i = 0; i < tileList.Count; i++)
            {
                if(!tileList[i].isEmpty())
                {
                    tileList[i].Freeze();
                }
            }
        }

        public void ScoreCalculation(int score)
        {
            curScore.Value += score;
            curScoreText.text = curScore.ToString();
        }

        void Start()
        {
            Vector2 topLeft = new Vector2(greenArea.transform.position.x - greenArea.GetComponent<SpriteRenderer>().size.x / 2,
                greenArea.transform.position.y + greenArea.GetComponent<SpriteRenderer>().size.y / 2);
            xSquareField = squareField.GetComponent<BoxCollider2D>().size.x / 2;
            ySquareField = squareField.GetComponent<BoxCollider2D>().size.y / 2;
            basePosition = topLeft + new Vector2(xSquareField + 0.1f, -ySquareField - 0.1f);
            position = basePosition;
            int square_number = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    GameObject newSquare = Instantiate(squareField, transform);
                    square_number++;
                    newSquare.name = "Square" + square_number;
                    TileObject temp = newSquare.GetComponent<TileObject>();
                    tileList.Add(temp);
                    newSquare.transform.position = position;
                    temp.position = new Vector2(j % 8, -i);
                    position.x += xDist;
                }
                position.y -= yDist;
                position.x = basePosition.x;
            }
        }

        void Update()
        {

        }        

        public void Test()
        {
            List<int> testnumbers = new List<int>()
            {
                1, 2, 3, 4, 6, 8, 9, 10, 11, 13, 15, 16, 17, 18, 19, 20, 22, 24, 25, 25, 27, 29, 31, 32,
                33, 34, 35, 36, 38, 40, 41, 42, 43, 45, 47, 49, 50, 51, 52, 54, 57, 58, 59
            };
            foreach (int num in testnumbers)
            {
                tileList[num - 1].AddPieceData(PieceDataList.pieceDataList[1]);
            }
        }
        IEnumerator WaitTillGameOver()
        { 
            yield return new WaitForSeconds(1f);
            gameOverEvent.Raise();
        }
    }
}
