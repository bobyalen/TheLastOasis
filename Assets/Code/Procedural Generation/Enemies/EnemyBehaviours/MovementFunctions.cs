using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public static class MovementFunctions
{
    public static Vector2 FollowPlayer(float speed, Vector2 currentPos)
    {
        //PlayerController pc = Singleton.Instance.PlayerController;
        Vector2 playerPos = PlayerController.Instance.transform.position;

        Vector2 Direction = (playerPos - currentPos).normalized;
        return Direction * speed;
    }

    public static Vector2 RunFromPlayer(float speed, Vector2 currentPos)
    {
        Vector2 playerPos = PlayerController.Instance.transform.position;
        Vector2 dir = (currentPos - playerPos).normalized;
        return dir * speed;
    }

    public static GridCell GetCell(Vector2 pos, GridCell[,] grid)
    {
        pos.x = Mathf.RoundToInt(pos.x);
        pos.y = Mathf.RoundToInt(pos.y);
        for(int i = 0; i < grid.GetLength(0);i++)
        {
            for(int j = 0; j < grid.GetLength(1); j++)
            {
                if (grid[i, j].pos == pos)
                    return grid[i, j];
            }
        }
        return null;
    }
    public static List<GridCell> GetPath(Vector2 currPos, Vector2 destPos, GridCell[,] grid)
    {
        List<GridCell> list = new List<GridCell>();
        int rows = grid.GetLength(0);
        int columns = grid.GetLength(1);

        GridCell[,] newGrid = new GridCell[rows, columns];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                newGrid[i, j] = new GridCell();
                newGrid[i, j].f = float.MaxValue;
                newGrid[i, j].g = float.MaxValue;
                newGrid[i, j].h = float.MaxValue;
                newGrid[i, j].x = grid[i, j].x;
                newGrid[i, j].y = grid[i, j].y;
                newGrid[i, j].parent = null;
                newGrid[i, j].pos = grid[i, j].pos;
                newGrid[i, j].isObstacle = grid[i, j].isObstacle;
            }
        }
        GridCell playerCell = GetCell(destPos,
            newGrid);
        if(playerCell == null)
        {
            Debug.LogError("Couldn't find player on grid!");
            return null;
        }
        GridCell startCell = GetCell(currPos, newGrid);
        if(startCell == null)
        {
            Debug.LogError("Couldn't find enemy on grid!");
            return null;
        }
        if(startCell.x == playerCell.x && startCell.y == playerCell.y)
        {
            Debug.Log("Player and Enemy in the same cell!");
            return null;
        }
        var activeTiles = new List<GridCell>();
        var visitedTiles = new List<GridCell>();

        startCell = GetCell(currPos, newGrid);
        startCell.f = startCell.g = startCell.h = 0.0f;
        playerCell = GetCell(playerCell.pos, newGrid);

        activeTiles.Add(startCell);
        while(activeTiles.Count> 0)
        {
            GridCell tile = null;
            float minF = float.MaxValue;
            foreach(GridCell cell in activeTiles)
            {
                if(cell.f < minF)
                {
                    minF = cell.f;
                    tile = cell;
                }
            }
            if(tile != null)
                activeTiles.Remove(tile);

            visitedTiles.Add(tile);
            List<GridCell> neighbours = GetCellNeighbours(tile, newGrid);
            for (int i = 0; i < neighbours.Count; i++)
            {
                float f = -1.0f, g = -1.0f, h = -1.0f;
                if (!neighbours[i].isObstacle)
                {
                    if (neighbours[i].x == playerCell.x && neighbours[i].y == playerCell.y)
                    {
                        playerCell.parent = newGrid[tile.y, tile.x];
                        Stack<GridCell> stackPath = new Stack<GridCell>();
                        GridCell currCell = playerCell;
                        while(currCell.parent != null)
                        {
                            stackPath.Push(currCell);
                            currCell = currCell.parent;
                        }
                        //stackPath.Push(currCell);
                        while(stackPath.Count > 0)
                        {
                            list.Add(stackPath.Pop());
                        }
                        return list;
                    }
                    else if (!visitedTiles.Any(x=> x.x == neighbours[i].x && x.y == neighbours[i].y))
                    {
                        g = tile.g + 1;
                        h = Mathf.Abs(neighbours[i].x - playerCell.x) + Mathf.Abs(neighbours[i].y - playerCell.y);
                        f = g + h;
                        if (neighbours[i].f == float.MaxValue ||
                            neighbours[i].f > f)
                        {
                            int x = neighbours[i].x;
                            int y = neighbours[i].y;
                            newGrid[y,x].f = f;
                            newGrid[y,x].g = g;
                            newGrid[y,x].h = h;
                            newGrid[y,x].parent = newGrid[tile.y, tile.x];
                            activeTiles.Add(newGrid[y,x]);
                        }
                    }
                }
            }

        }

        return list;
    }



    static List<GridCell> GetCellNeighbours(GridCell cell, GridCell[,] grid)
    {
        List<GridCell> neighbours = new List<GridCell>();
        if (cell.x != 0)
        {
            neighbours.Add(grid[cell.y, cell.x - 1]);
            if(cell.y != 0)
            {
                if (!grid[cell.y - 1, cell.x].isObstacle && !grid[cell.y, cell.x - 1].isObstacle)
                    neighbours.Add(grid[cell.y - 1, cell.x - 1]);
            }
            if(cell.y != grid.GetLength(0) - 1)
            {

                if (!grid[cell.y + 1, cell.x].isObstacle && !grid[cell.y, cell.x - 1].isObstacle)
                    neighbours.Add(grid[cell.y + 1, cell.x - 1]);
            }
        }
        if (cell.x != grid.GetLength(1) - 1)
        {
            neighbours.Add(grid[cell.y, cell.x + 1]);
            if (cell.y != 0)
            {
                if (!grid[cell.y - 1, cell.x].isObstacle && !grid[cell.y, cell.x + 1].isObstacle)
                    neighbours.Add(grid[cell.y - 1, cell.x + 1]);
            }
            if (cell.y != grid.GetLength(0) - 1)
            {

                if (!grid[cell.y + 1, cell.x].isObstacle && !grid[cell.y, cell.x + 1].isObstacle)
                    neighbours.Add(grid[cell.y + 1, cell.x + 1]);
            }
        }
        if (cell.y != 0)
        {
            neighbours.Add(grid[cell.y - 1, cell.x]);
        }
        if (cell.y != grid.GetLength(0) - 1)
            neighbours.Add(grid[cell.y + 1, cell.x]);
        return neighbours;
    }

    static public bool IsAtDestination(GridCell cell, Vector2 pos)
    {
        Vector2 cellPos = cell.pos;

        return (cellPos - pos).magnitude < 0.1f;
    }

    static public Vector2 MoveTowards(GridCell cell, float speed, Vector2 pos)
    {
        Vector2 cellPos = cell.pos;
        return speed * (cellPos - pos).normalized;
    }

    static private bool IsTileValid(int x, int y, GridCell[,] grid)
    {
        if (x < 0 || x >= grid.GetLength(1) || y < 0 || y >= grid.GetLength(0))
            return false;
        return !grid[y, x].isObstacle;
    }

    static public int GetDegreesOfFreedom(Vector2 startPos, Vector2 destPos, GridCell[,] grid)
    {
        var startCell = GetCell(startPos, grid);
        if(startCell == null)
        {
            Debug.LogError("Couldn't find starting positions for degrees of freedom!");
            return -1;
        }
        var destCell = GetCell(destPos, grid);
        if(destCell == null)
        {
            Debug.LogError("Couldn't find destination positions for degrees of freedom!");
            return -1;
        }
        if(startCell.x == destCell.x)
        {
            int sign = (int)Mathf.Sign(destCell.y - startCell.y);
            return IsTileValid(startCell.x, startCell.y + sign, grid) ? 3 : 0;
        }
        else if (startCell.y == destCell.y)
        {
            int sign = (int)Mathf.Sign(destCell.x - startCell.x);
            return IsTileValid(startCell.x + sign, startCell.y, grid) ? 3 : 0;
        }
        else
        {
            int signX = (int)Mathf.Sign(destCell.x - startCell.x);
            int signY = (int)Mathf.Sign(destCell.y - startCell.y);

            int degOfFreedom = 0;
            if (IsTileValid(startCell.x + signX, startCell.y, grid))
                degOfFreedom++;
            if (IsTileValid(startCell.x, startCell.y + signY, grid))
                degOfFreedom++;
            if (IsTileValid(startCell.x + signX, startCell.y + signY, grid))
                degOfFreedom++;
            return degOfFreedom;
        }
    }

    static Vector2 CheckDampening(Vector2 dampening)
    {
        //function that makes the boids avoid each other by
        //'steering clear' on top of 'slowing down'
        if (dampening == Vector2.zero)
            return dampening;
        if(dampening.x == 0)
        {
            dampening.x = dampening.y / 3.0f;
            dampening.y = 2.0f * dampening.y / 3.0f;
            return dampening;
        }
        if(dampening.y == 0)
        {
            dampening.y = dampening.x / 3.0f;
            dampening.x = 2.0f * dampening.x / 3.0f;
            return dampening;
        }
        if(Mathf.Abs(dampening.x / dampening.y) > 10.0f)
        {
            dampening.y = Mathf.Sign(dampening.y) * Mathf.Abs(dampening.x / 5.0f);
            return dampening;
        }
        if(Mathf.Abs(dampening.y / dampening.x) > 10.0f)
        {
            dampening.x = Mathf.Sign(dampening.x) * Mathf.Abs(dampening.y / 5.0f);
            return dampening;
        }
        return dampening;

        

    }

    public static Vector2 GetBoidAvoidanceFactor(Vector2 pos, RoomScript room)
    {
        var validEnemies = room.enemies.Where(x => ((Vector2)x.transform.position - pos).magnitude <= 1.0f);
        Vector2 dampening = Vector2.zero;
        float avoidanceFactor = 0.05f;
        foreach(EnemyBase enemy in validEnemies)
        {
            var diff = (Vector2)enemy.transform.position - pos;
            dampening -= diff * diff.magnitude;
        }
        dampening = CheckDampening(dampening);
        return dampening.normalized * avoidanceFactor;
    }

    public static Vector2 FollowPlayer(float speed, Vector2 pos, RoomScript room, List<GridCell> path, ref GridCell nextCell)
    {
        Vector2 playerPos = PlayerController.Instance.transform.position;
        path = MovementFunctions.GetPath(pos, playerPos, room.pathFindingGrid);
        int degOfFreedom = MovementFunctions.GetDegreesOfFreedom(pos,
            playerPos,
            room.pathFindingGrid);
        Vector2 boidDampening = MovementFunctions.GetBoidAvoidanceFactor(pos, room) * speed;
        if (degOfFreedom == 3)
        {
            return MovementFunctions.FollowPlayer(speed, pos) + boidDampening;
        }
        else if (path != null && path.Count > 0)
        {
            if (nextCell == null || !MovementFunctions.IsAtDestination(nextCell, path[0].pos))
            {
                nextCell = path[0];
                path.RemoveAt(0);
            }
            if (MovementFunctions.IsAtDestination(nextCell, pos))
            {
                nextCell = path[0];
                path.RemoveAt(0);
            }
            return MovementFunctions.MoveTowards(nextCell, speed, pos) + boidDampening;
        }
        return MovementFunctions.FollowPlayer(speed, pos) + boidDampening;
    }
    
    public static Vector2 Skitter(float speed, Vector2 pos, RoomScript room, List<GridCell> path, ref GridCell nextCell, ref GridCell destCell)
    {
        GridCell currCell = GetCell(pos, room.pathFindingGrid);
        int x = currCell.x, y = currCell.y;
        int i = 0;
        if(destCell == null)
        {
            do
            {
                x = Random.Range(currCell.x - 3, currCell.x + 3);
                y = Random.Range(currCell.y - 3, currCell.y + 3);
                i++;

            } while (!IsTileValid(x, y, room.pathFindingGrid) || (x == currCell.x && y == currCell.y));
            if (i >= 1000)
            {
                Debug.LogError("Couldn't find random cell within range!");
                return Random.insideUnitCircle * speed;
            }
            destCell = room.pathFindingGrid[y, x];
            path = GetPath(pos, destCell.pos, room.pathFindingGrid);
            if(path.Count > 0)
            {
                nextCell = path[0];
                path.RemoveAt(0);
            }
        }
        if(path.Count > 0)
        {
            if (nextCell == null || !IsAtDestination(nextCell, path[0].pos) || IsAtDestination(nextCell,pos))
            {
                nextCell = path[0];
                path.RemoveAt(0);

            }
            return MoveTowards(nextCell, speed, pos);
        }
        else
        {
            if(nextCell == null || IsAtDestination(nextCell, pos))
            {
                destCell = null;
                return Vector2.zero;
            }
            return MoveTowards(nextCell, speed, pos);
        }

        return Random.insideUnitCircle;
    }
}
