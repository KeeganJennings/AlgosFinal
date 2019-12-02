using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeGeneration
{
    enum SidesOpen { TopRightOpen, TopLeftOpen, TopBottomOpen, RightLeftOpen, BottomRightOpen, BottomLeftOpen, AllOpen, AllClosed }
    class MazeCube
    {
        public SidesOpen sidesOpen;
        public bool isObstacle, isEnd, isStart;
        public int xPos, yPos, fCost, gCost, hCost;
        public MazeCube parent;

        public MazeCube()
        {
            isObstacle = false;
            isEnd = false;
            isStart = false;
            sidesOpen = SidesOpen.AllClosed;
        }
    }
}
