using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TileBoard : MonoBehaviour
{
   // public GameManager gameManager;
    public Tile TileFrefab;
    public TileState[] tileStates;
    private TileGid gid;
    private List<Tile> tiles;
    private bool waiting;

    private Vector2 touchStartPosition;
    private Vector2 touchEndPosition;

    public  Transform buttonCreate;

    private void Awake()
    {
        gid = GetComponentInChildren<TileGid>();
        tiles = new List<Tile>(16);
    }

    private void Start()
    {

    }

    public void ClearBoard()
    {
        foreach (var cell in gid.cells)
        {
            cell.tile = null;
        }
        foreach (var tile in tiles)
        {
            Destroy(tile.gameObject);
        }
        tiles.Clear();
    }

    public void CreateTile()
    {
        Tile tile = Instantiate(TileFrefab, gid.transform);
       
        tile.SetState(tileStates[0], 1);
        tile.Spawn(gid.getRandomEmptyCell(), buttonCreate);
        tiles.Add(tile);
    }

    private void Update()
    {
        //if (waiting == false)
        //{

        //    if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        //    {

        //        touchStartPosition = Input.GetTouch(0).position;
        //    }


        //    if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        //    {

        //        touchEndPosition = Input.GetTouch(0).position;


        //        float swipeDistanceX = touchEndPosition.x - touchStartPosition.x;
        //        float swipeDistanceY = touchEndPosition.y - touchStartPosition.y;


        //        if (swipeDistanceY > 0 && Mathf.Abs(swipeDistanceY) > Mathf.Abs(swipeDistanceX))
        //        {

        //            MoveTiles(Vector2Int.up, 0, 1, 1, 1);
        //        }
        //        else if (swipeDistanceY < 0 && Mathf.Abs(swipeDistanceY) > Mathf.Abs(swipeDistanceX))
        //        {
        //            MoveTiles(Vector2Int.down, 0, 1, gid.height - 2, -1);
        //        }
        //        else if (swipeDistanceX < 0 && Mathf.Abs(swipeDistanceX) > Mathf.Abs(swipeDistanceY))
        //        {
        //            MoveTiles(Vector2Int.left, 1, 1, 0, 1);
        //        }
        //        else if (swipeDistanceX > 0 && Mathf.Abs(swipeDistanceX) > Mathf.Abs(swipeDistanceY))
        //        {
        //            MoveTiles(Vector2Int.right, gid.width - 2, -1, 0, 1);
        //        }
        //    }
        //}

        if (waiting == false)
        {
            //if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            //{
            //    MoveTiles(Vector2Int.up, 0, 1, 1, 1);
            //}
            //else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            //{
            //    MoveTiles(Vector2Int.down, 0, 1, gid.height - 2, -1);
            //}
            //else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            //{
            //    MoveTiles(Vector2Int.left, 1, 1, 0, 1);
            //}
            //else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            //{
            //    MoveTiles(Vector2Int.right, gid.width - 2, -1, 0, 1);
            //}
        }

    }

    private void MoveTiles(Vector2Int direction, int startX, int incrementX, int startY, int incrementY)
    {
        bool changed = false;
        for (int x = startX; x >= 0 && x < gid.width; x += incrementX)
        {
            for (int y = startY; y >= 0 && y < gid.height; y += incrementY)
            {
                TileCell cell = gid.GetCell(x, y);
                if (cell.occupied)
                {
                    changed |= MoveTile(cell.tile, direction);

                }
            }
        }

        if (changed)
        {
            StartCoroutine(WaitingForChange());
        }
    }

    private bool MoveTile(Tile tile, Vector2Int direction)
    {
        TileCell newCell = null;
        TileCell adjacent = gid.GetAdjacentCell(tile.cell, direction);

        while (adjacent != null)
        {
            if (adjacent.occupied)
            {
                if (CanMerge(tile, adjacent.tile))
                {
                    Merge(tile, adjacent.tile);
                    return true;
                }
                break;
            }
            newCell = adjacent;
            adjacent = gid.GetAdjacentCell(adjacent, direction);
        }

        if (newCell != null)
        {
            tile.MoveTo(newCell);
            return true;
        }

        return false;
    }
    private void Merge(Tile a, Tile b)
    {
        tiles.Remove(a);
        a.Merge(b.cell);

        int index = Mathf.Clamp(IndexOf(b.state) + 1, 0, tileStates.Length - 1);
        int number = b.number * 2;

        b.SetState(tileStates[index], number);
    }

    private int IndexOf(TileState state)
    {
        for (int i = 0; i < tileStates.Length; i++)
        {
            if (state == tileStates[i])
            {
                return i;
            }
        }

        return -1;
    }
    private bool CanMerge(Tile a, Tile b)
    {
        return a.number == b.number && !b.locked;
    }

    private IEnumerator WaitingForChange()
    {
        waiting = true;
        yield return new WaitForSeconds(0.1f);
        waiting = false;
        //TODO: create new Tile
        //ToDO: check for game over
        foreach (var tile in tiles)
        {
            tile.locked = false;
        }

        if (tiles.Count != gid.size)
        {
            CreateTile();
        }
        if (ChekForGameOVer())
        {
           // gameManager.Gameover();
        }


    }

    private bool ChekForGameOVer()
    {
        if (tiles.Count != gid.size)
        {
            return false;
        }

        foreach (var tile in tiles)
        {
            TileCell up = gid.GetAdjacentCell(tile.cell, Vector2Int.up);
            TileCell down = gid.GetAdjacentCell(tile.cell, Vector2Int.down);
            TileCell left = gid.GetAdjacentCell(tile.cell, Vector2Int.left);
            TileCell right = gid.GetAdjacentCell(tile.cell, Vector2Int.right);

            if (up != null && CanMerge(tile, up.tile))
            {
                return false;
            }
            if (down != null && CanMerge(tile, down.tile))
            {
                return false;
            }
            if (left != null && CanMerge(tile, left.tile))
            {
                return false;
            }
            if (right != null && CanMerge(tile, right.tile))
            {
                return false;
            }
        }
        return true;
    }
}
