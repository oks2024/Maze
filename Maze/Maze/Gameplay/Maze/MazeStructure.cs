using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Maze.Gameplay.Maze
{
    public class MazeStructure
    {
        private List<List<MazeElement>> mMazeData;

        private Random random;
        private int mXSize;
        private int mYSize;


        
        public MazeStructure()
        {
            random = new Random();
            Create(150, 100);
        }

        public void Create(int pSizeX, int pSizeY)
        {

            mXSize = pSizeX;
            mYSize = pSizeY;

            mMazeData = new List<List<MazeElement>>();

            

            for (int x = 0; x < pSizeY; x++)
            {
                mMazeData.Add(new List<MazeElement>());
                for (int y = 0; y < pSizeX; y++)
                {
                    mMazeData[x].Add(new MazeElement());
                }
            }

            List<Vector2> positionSeen = new List<Vector2>();
            Vector2 position = new Vector2(pSizeY / 2, pSizeX / 2);
            positionSeen.Add(position);

            while (positionSeen.Count > 0)
            {
                

                //Vector2 currentPosition = positionSeen[0];
                int m = 0;
                int _m = positionSeen.Count;
                while (m < _m)
                {
                    Vector2 currentPosition = positionSeen[m];
                    bool removePosition = false;

                    switch (random.Next(4))
                    {
                        case 0:
                            currentPosition.Y -= 2.0f;
                            if (currentPosition.Y < 0 || mMazeData[(int)currentPosition.X][(int)currentPosition.Y].IsFloor)
                            {
                                removePosition = true;
                                break;
                            }
                            mMazeData[(int)currentPosition.X][(int)currentPosition.Y + 1].IsFloor = true;
                            break;

                        case 1:
                            currentPosition.Y += 2.0f;
                            if (currentPosition.Y >= pSizeX || mMazeData[(int)currentPosition.X][(int)currentPosition.Y].IsFloor)
                            {
                                removePosition = true;
                                break;
                            }
                            mMazeData[(int)currentPosition.X][(int)currentPosition.Y - 1].IsFloor = true;
                            break;

                        case 2:
                            currentPosition.X -= 2.0f;
                            if (currentPosition.X < 0|| mMazeData[(int)currentPosition.X][(int)currentPosition.Y].IsFloor)
                            {
                                removePosition = true;
                                break;
                            }
                            mMazeData[(int)currentPosition.X + 1][(int)currentPosition.Y].IsFloor = true;
                            break;

                        case 3:
                            currentPosition.X += 2.0f;
                            if (currentPosition.X >= pSizeY || mMazeData[(int)currentPosition.X][(int)currentPosition.Y].IsFloor)
                            {
                                removePosition = true;
                                break;
                            }
                            mMazeData[(int)currentPosition.X - 1][(int)currentPosition.Y].IsFloor = true;
                            break;

                    }
                    if (removePosition)
                    {
                        positionSeen.RemoveAt(m);
                        _m--;
                    }

                    else
                    {
                        positionSeen.Add(new Vector2((int)currentPosition.X, (int)currentPosition.Y));
                        positionSeen.Add(new Vector2((int)currentPosition.X, (int)currentPosition.Y));

                        mMazeData[(int)currentPosition.X][(int)currentPosition.Y].IsFloor = true;
                        ++m;
                    }
                }

            }



            
            for (int x = 0; x < pSizeY; x++)
            {
                for (int y = 0; y < pSizeX; y++)
                {
                    if (mMazeData[x][y].IsFloor)
                        Console.Write('.');
                    else
                        Console.Write('#');
                    //Console.Write(mMazeData[x][y].IsFloor);
                }
                Console.Write('\n');
            }
        }

        #region Getters/Setters
        public int XSize
        {
            get { return mXSize; }
        }

        public int YSize
        {
            get { return mYSize; }
        }

        internal List<List<MazeElement>> MazeData
        {
            get { return mMazeData; }
            set { mMazeData = value; }
        }
        #endregion
    }
}
