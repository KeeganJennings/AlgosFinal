using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGeneration
{
    class MazeGenerator
    {
        int mazeSize;
        MazeCube[,] mazeLayout;
        List<MazeCube> openList;
        List<MazeCube> closedList;
        MazeCube target, start, current;
        int gScore;

        MazeGenerator(int size)
        {
            mazeSize = size;
            mazeLayout = new MazeCube[mazeSize, mazeSize];
            openList = new List<MazeCube>();
            closedList = new List<MazeCube>();
            gScore = 0;
        }

        public void MazeGenerationLoop()
        {
            for(int i = 0; i < mazeSize; i++)
            {
                for(int j = 0; j < mazeSize; j++)
                {
                    MazeCube cube = new MazeCube();
                    cube.xPos = i;
                    cube.yPos = j;
                    mazeLayout[i, j] = cube;
                }
            }

            CreateObstaclesAndGoals();
        }

        public void CreateObstaclesAndGoals()
        {
            Random rnd = new Random();
            int rndX, rndY;

            rndY = rnd.Next(mazeSize);

            mazeLayout[0, rndY].isStart = true;

            rndY = rnd.Next(mazeSize);

            mazeLayout[mazeSize, rndY].isEnd = true;

            for (int i = 0; i < mazeSize; i++)
            {
                rndX = rnd.Next(mazeSize);
                rndY = rnd.Next(mazeSize);

                if(mazeLayout[rndX, rndY].isEnd != true || mazeLayout[rndX, rndY].isStart != true)
                {
                    mazeLayout[rndX, rndY].isObstacle = true;
                }
                else
                {
                    i--;
                }
            }

            AStarAlgo();
        }

        public void OpenCorrectPath()
        {

        }

        //AStar
        public int ComputeHScore(int x, int y, int targetX, int targetY)
        {
            return Math.Abs(targetX - x) + Math.Abs(targetY - y);
        }

        public void AStarAlgo()
        {
            openList.Add(start);

            while(openList.Count > 0)
            {
                var lowest = openList.Min(l => l.fCost);
                current = openList.First(l => l.fCost == lowest);

                // add the current square to the closed list
                closedList.Add(current);

                // remove it from the open list
                openList.Remove(current);

                // if we added the destination to the closed list, we've found a path
                if (closedList.FirstOrDefault(l => l.xPos == target.xPos && l.yPos == target.yPos) != null)
                    break;

                var adjacentSquares = GetWalkableAdjacentSquares(current.xPos, current.yPos, mazeLayout);
                gScore++;

                foreach (var adjacentSquare in adjacentSquares)
                {
                    // if this adjacent square is already in the closed list, ignore it
                    if (closedList.FirstOrDefault(l => l.xPos == adjacentSquare.xPos
                            && l.yPos == adjacentSquare.yPos) != null)
                        continue;

                    // if it's not in the open list...
                    if (openList.FirstOrDefault(l => l.xPos == adjacentSquare.xPos
                            && l.yPos == adjacentSquare.yPos) == null)
                    {
                        // compute its score, set the parent
                        adjacentSquare.gCost = gScore;
                        adjacentSquare.hCost = ComputeHScore(adjacentSquare.xPos,
                            adjacentSquare.yPos, target.xPos, target.yPos);
                        adjacentSquare.fCost = adjacentSquare.gCost + adjacentSquare.hCost;
                        adjacentSquare.parent = current;

                        // and add it to the open list
                        openList.Insert(0, adjacentSquare);
                    }
                    else
                    {
                        // test if using the current G score makes the adjacent square's F score
                        // lower, if yes update the parent because it means it's a better path
                        if (gScore + adjacentSquare.hCost < adjacentSquare.fCost)
                        {
                            adjacentSquare.gCost = gScore;
                            adjacentSquare.fCost = adjacentSquare.gCost + adjacentSquare.hCost;
                            adjacentSquare.parent = current;
                        }
                    }
                }
            }
        }

        static List<MazeCube> GetWalkableAdjacentSquares(int x, int y, MazeCube[,] map)
        {
            var proposedLocations = new List<MazeCube>()
            {
                new MazeCube { xPos = x, yPos = y - 1 },
                new MazeCube { xPos = x, yPos = y + 1 },
                new MazeCube { xPos = x - 1, yPos = y },
                new MazeCube { xPos = x + 1, yPos = y },
            };

            return proposedLocations.Where(
                l => map[l.xPos, l.yPos] == null || map[l.xPos, l.yPos] == null).ToList();
        }
    }
}
