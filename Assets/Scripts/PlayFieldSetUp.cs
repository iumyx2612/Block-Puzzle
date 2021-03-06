﻿using myengine.BlockPuzzle;
using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

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
        [SerializeField] private GameObject greenArea;
        [SerializeField] private Vector2 position;
        [SerializeField] private Vector2 basePosition;

        [SerializeField] private BlockDragGameEvent dragListen;
        [SerializeField] private BlockDragGameEvent placeListen;

        private List<TileObject> tileToCheck = new List<TileObject>();
        private GameObject oldTile;
        private GameObject curTile;

        [SerializeField] private IntVariable activeBlocks;
        [SerializeField] private GameEvent loadNextBlocks;

        private List<TileObject> rowTiles = new List<TileObject>();
        private List<TileObject> colTiles = new List<TileObject>();
        private bool rowComplete = false;
        private bool colComplete = false;

        private List<TileObject> hoverRowTiles = new List<TileObject>();
        private List<TileObject> hoverColTiles = new List<TileObject>();
        private bool hoverRowComplete = false;
        private bool hoverColComplete = false;

        [SerializeField] private GameObjectCollection remainBlocks;
        [SerializeField] private BoolVariable isGameOver;
        private List<TileObject> remainingTile = new List<TileObject>();
        private List<TileObject> usedTiles = new List<TileObject>();
        [SerializeField] private PieceDataList PieceDataList;
        [SerializeField] private GameEvent gameOverEvent;
        [SerializeField] private BoolVariable isShopNeed;
        [SerializeField] private GameEvent ShopNeed;

        private int num_of_row_col;
        private readonly int[] scoreSet = new int[]
        {
            10, 30, 60, 100, 200
        };
        [SerializeField] private Text curScoreText;
        public IntVariable curScore;
        [SerializeField] private IntGameEvent scorePop;
        ///*[SerializeField] */private TextMeshPro scorePopText;
        [SerializeField] private TextMeshPro[] scorePopContainer;
        private Vector2 scorePopPos;

        public Text highScoreText;
        public IntVariable highScore;

        public GameEvent welldoneGameEvent;

        [SerializeField] private GameEvent rotate;
        [SerializeField] private IntVariable rotateCounter;
        [SerializeField] private BoolVariable enableRotate;
        [SerializeField] private GameEvent rotateBtnClick;

        public GameEvent test;

        private void OnEnable()
        {
            dragListen.AddListener(DragCheck);
            test.AddListener(Test);
            placeListen.AddListener(Place);
            highScore.Value = PlayerPrefs.GetInt("HighScore");
            highScoreText.text = highScore.Value.ToString();
            scorePop.AddListener(ScorePop);
        }

        private void OnDisable()
        {
            dragListen.RemoveListener(DragCheck);
            placeListen.RemoveListener(Place);
            isGameOver.Value = false;
            test.RemoveListener(Test);
            scorePop.RemoveListener(ScorePop);
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
            GameObject basePiece = drag.gameObject.transform.GetChild(1).gameObject;
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
                AudioManager._instance.Play("Place");
                foreach (TileObject tile in tileToCheck)
                {
                    tile.AddPieceData(drag.transform.GetChild(1).GetComponent<PieceDisplay>().data);
                    remainingTile.Remove(tile);
                }
                rotateCounter.Value -= drag.gameObject.GetComponent<BlockDisplay>().rotateTimes;
                rotate.Raise();
                if(rotateCounter.Value <= 0)
                {
                    enableRotate.Value = false;
                    rotateBtnClick.Raise();
                }
                scorePopPos = drag.endPos;
            }
            foreach (TileObject tile in tileToCheck)
            {
                CheckCompletion(tile);
            }
            num_of_row_col = (colTiles.Count + rowTiles.Count) / 8;
            if (rowComplete)
            {
                if(num_of_row_col == 1)
                {
                    AudioManager._instance.Play("One");
                    foreach (TileObject tileObject in rowTiles)
                    {
                        tileObject.DragFinish((int)tileObject.position.x);
                    }
                }
                if(num_of_row_col >= 2)
                {
                    AudioManager._instance.Play("Multiple");
                    foreach (TileObject tileObject in rowTiles)
                    {
                        tileObject.SpinnyFinish((int)tileObject.position.x);
                    }
                }
            }
            if (colComplete)
            {
                if (num_of_row_col == 1)
                {
                    AudioManager._instance.Play("One");
                    foreach (TileObject tileObject in colTiles)
                    {
                        tileObject.DragFinish(-(int)tileObject.position.y);
                    }
                }
                if (num_of_row_col >= 2)
                {
                    AudioManager._instance.Play("Multiple");
                    foreach (TileObject tileObject in colTiles)
                    {
                        tileObject.SpinnyFinish(-(int)tileObject.position.y);
                    }
                }
            }
            ScoreCalculation(drag.gameObject.GetComponent<BlockDisplay>().activeChild - 1);
            if(!rowComplete && !colComplete)
            {
                scorePop.Raise(drag.gameObject.GetComponent<BlockDisplay>().activeChild - 1);
            }
            if (num_of_row_col > 0)
            {
                ScoreCalculation(scoreSet[num_of_row_col - 1]);               
                scorePop.Raise(scoreSet[num_of_row_col - 1] + drag.gameObject.GetComponent<BlockDisplay>().activeChild - 1);
                if (num_of_row_col >= 3)
                {
                    welldoneGameEvent.Raise();
                }
            }
            num_of_row_col = 0;
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
            rowComplete = false;
            colComplete = false;
            tileToCheck.Clear();
            if(isGameOver)
            {
                Freeze();
                AudioManager._instance.Play("Lose");
                //foreach (GameObject block in remainBlocks)
                //{
                //    block.GetComponent<BlockDrag>().enabled = false;
                //}
                StartCoroutine(WaitTillGameOver());
            }
        }

        public void CheckCompletion(TileObject tile)
        {
            List<TileObject> tempRowTile = new List<TileObject>();
            List<TileObject> tempColTile = new List<TileObject>();
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
                    tempRowTile.Add(tileList[-8 * _y + i - 1]);
                }
                else
                {
                    tempRowTile.Clear();
                    break;
                }
            }
            if (rowCount == 8)
            {
                rowComplete = true;
                for (int i = 0; i < 8; i++)
                {
                    if(!rowTiles.Contains(tempRowTile[i]))
                    {
                        rowTiles.Add(tempRowTile[i]);
                    }
                }
            }
            #endregion
            #region check col
            for (int i = 0; i <= 7; i++)
            {
                if (!tileList[i * 8 + _x].isEmpty())
                {
                    colCount++;
                    tempColTile.Add(tileList[i * 8 + _x]);
                }
                else
                {
                    tempColTile.Clear();
                    break;
                }
            }
            if (colCount == 8)
            {
                colComplete = true;
                for (int i = 0; i < 8; i++)
                {
                    if(!colTiles.Contains(tempColTile[i]))
                    {
                        colTiles.Add(tempColTile[i]);
                    }
                }
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
            usedTiles.Clear();
            for (int i = 0; i < tileList.Count; i++)
            {
                if(tileList[i].isEmpty())
                {
                    if(!remainingTile.Contains(tileList[i]))
                    {
                        remainingTile.Add(tileList[i]);
                    }
                }
                else
                {
                    if(!usedTiles.Contains(tileList[i]))
                    {
                        usedTiles.Add(tileList[i]);
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
            int shopNeedCount = 0;
            RemainingTiles();
            foreach (GameObject block in remainBlocks)
            {
                //if (remainingTile.Count < block.GetComponent<BlockDisplay>().activeChild)
                //{
                //    return true;
                //}
                List<Vector2> possible_points_of_block = new List<Vector2>();
                for (int i = 0; i < remainingTile.Count; i++)
                {
                    tileToCheck.Clear();
                    tileToCheck.Add(remainingTile[i]);
                    if (Check(block.GetComponent<BlockDisplay>().points, remainingTile[i]))
                    {
                        if(!block.GetComponent<BlockDisplay>().state)
                        {
                            block.GetComponent<BlockDisplay>().state = true;
                            block.GetComponent<BlockDisplay>().StateToggle();
                        }
                        return false;
                    }
                }
                block.GetComponent<BlockDisplay>().state = false;
                block.GetComponent<BlockDisplay>().StateToggle();
                bool _break = false;
                for (int i = 0; i < remainingTile.Count; i++)
                {
                    if(_break)
                    {
                        break;
                    }
                    List<Vector2> fakePoints = block.GetComponent<BlockDisplay>().points;
                    for (int j = 1; j < block.GetComponent<BlockDisplay>().possibleRotation; j++)
                    {
                        fakePoints = FakeRotate(fakePoints);
                        tileToCheck.Clear();
                        tileToCheck.Add(remainingTile[i]);
                        if(Check(fakePoints, remainingTile[i]))
                        {
                            shopNeedCount++;
                            _break = true;
                            break;
                        }
                    }
                }
                if (shopNeedCount >= remainBlocks.Count)
                {
                    if ((rowComplete || colComplete))
                    {
                        isShopNeed.Value = true;
                        StartCoroutine(WaitTillShopNeeded());
                        return false;
                    }
                    else
                    {
                        isShopNeed.Value = true;
                        ShopNeed.Raise();
                        return false;
                    }
                }

            }
            return true;
        }

        public void Freeze()
        {
            for (int i = 0; i < usedTiles.Count; i++)
            {
                float timer = 1f / usedTiles.Count;
                usedTiles[i].Freeze(timer * i);
            }
        }

        public void ScoreCalculation(int score)
        {
            curScore.Value += score;
            HighScoreCalculation();
            curScoreText.text = curScore.ToString();
        }
        
        public void ScorePop(int score)
        {
            for (int i = 0; i < scorePopContainer.Length; i++)
            {
                if(!scorePopContainer[i].gameObject.activeSelf)
                {
                    TextMeshPro scorePopText;
                    scorePopText = scorePopContainer[i];
                    scorePopText.text = "+" + score.ToString();
                    scorePopText.transform.position = scorePopPos;
                    scorePopText.gameObject.SetActive(true);
                    Sequence scorePopTextSeq = DOTween.Sequence();
                    Tween tween1 = scorePopText.transform.DOLocalMove(scorePopPos + new Vector2(0, 1f), 0.5f);
                    Tween tween2 = scorePopText.DOFade(0f, 0.5f);
                    scorePopTextSeq.Append(tween1).Append(tween2);
                    scorePopTextSeq.AppendCallback(delegate
                    {
                        scorePopText.gameObject.SetActive(false);
                        //scorePopText.transform.localScale = new Vector2(0f, 0f);
                        Color _base = scorePopText.color;
                        _base.a = 1;
                        scorePopText.color = _base;
                        scorePopText.transform.localPosition = transform.position;
                    });
                    break;
                }                
            }            
        }

        public void HighScoreCalculation()
        {
            if (curScore.Value > highScore.Value)
            {
                highScore.Value = curScore.Value;
                highScoreText.text = highScore.Value.ToString();
                PlayerPrefs.SetInt("HighScore", highScore.Value);
            }
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
            AudioManager._instance.Play("Theme");
        }

        public void Test()
        {
            List<int> testnumbers = new List<int>()
            {
                1,3,5,8,10,11,12,14,16,17,19,21,23,24,26,28,30,32,
                33,34,35,37,39,42,43,44,45,46,50,52,53,54,55,57,58,59,60,61,62
            };
            foreach (int num in testnumbers)
            {
                tileList[num - 1].AddPieceData(PieceDataList.pieceDataList[1]);
            }
        }
        IEnumerator WaitTillGameOver()
        { 
            yield return new WaitForSeconds(1.5f);
            gameOverEvent.Raise();
        }

        IEnumerator WaitTillShopNeeded()
        {
            yield return new WaitForSeconds(0.5f);
            ShopNeed.Raise();
        }
    }
}
