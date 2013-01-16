using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Maze.Gameplay.Maze
{
    class MazeElement
    {
        private bool mIsWall = false;
        private bool mIsFloor = false;

        public MazeElement()
        {
            this.mIsFloor = false;
        }

        #region Getters/Setters
        public bool IsFloor
        {
            get { return mIsFloor; }
            set { mIsFloor = value; }
        }

        public bool IsWall
        {
            get { return mIsWall; }
            set { mIsWall = value; }
        }
        #endregion
    }
}
