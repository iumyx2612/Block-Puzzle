using myengine.BlockPuzzle;
using ScriptableObjectArchitecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        private void OnEnable()
        {
            dragListen.AddListener(Check);
            placeListen.AddListener(Place);
        }

        private void OnDisable()
        {
            dragListen.RemoveListener(Check);
            placeListen.RemoveListener(Place);
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

        public void Check(BlockDrag drag)
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
                        for (int j = 1; j < drag.gameObject.GetComponent<BlockDisplay>().points.Count; j++)
                        {
                            Vector2 tempPoint = drag.gameObject.GetComponent<BlockDisplay>().points[j];
                            Vector2 pointToCheck = tileList[i].position + tempPoint;
                            TileObject tile = tim(pointToCheck);
                            if (tile != null && !tileToCheck.Contains(tile))
                            {
                                tileToCheck.Add(tile);
                            }
                        }
                        if (tileToCheck.Count > 0)
                        {
                            int checkAmount = 0;
                            foreach (TileObject tile in tileToCheck)
                            {
                                if (tile.isEmpty())
                                {
                                    checkAmount++;
                                }
                                if (checkAmount >= drag.gameObject.transform.GetComponent<BlockDisplay>().activeChild)
                                {
                                    drag.gameObject.GetComponent<BlockDrag>().check = true;
                                    drag.gameObject.GetComponent<BlockDrag>().hovering = true;
                                }
                            }
                            #region kiểm tra nếu đang hover thì sẽ xuất hiện bóng của block và nếu ở trạng thái có thể hoàn thiện thì phát sáng
                            if (drag.gameObject.GetComponent<BlockDrag>().hovering)
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
                            #endregion
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
            if (drag.gameObject.GetComponent<BlockDrag>().check)
            {
                foreach (TileObject tile in tileToCheck)
                {
                    tile.addPieceData(drag.transform.GetChild(0).GetComponent<PieceDisplay>().data);
                }
            }
            foreach (TileObject tile in tileToCheck)
            {
                CheckCompletion(tile);
                if (rowComplete)
                {
                    foreach (TileObject _tile in rowTiles)
                    {
                        _tile.Finish();
                    }
                    rowComplete = false;
                }
                if (colComplete)
                {
                    foreach (TileObject _tile in colTiles)
                    {
                        _tile.Finish();
                    }
                    colComplete = false;
                }
            }
            tileToCheck.Clear();
            activeBlocks.Value--;
            if (activeBlocks.Value <= 0)
            {
                loadNextBlocks.Raise();
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
    }
}
