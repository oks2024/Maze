using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maze.Entities.Sprites;
using Microsoft.Xna.Framework;
using Maze.Gameplay.Maze;

namespace Maze.Entities.Scenes
{
    class Scene
    {
        private List<DeferredSprite> mSprites;

        internal List<DeferredSprite> Sprites
        {
            get { return mSprites; }
            set { mSprites = value; }
        }

        public Scene()
        {
            mSprites = new List<DeferredSprite>();
            Add(new DeferredSprite(@"Models\ground_diffuse", @"Models\ground_normal", @"Models\ground_specular", new Vector3(0, 0, 0)));
            //Add(new DeferredSprite(@"Models\ground_diffuse", @"Models\ground_normal", @"Models\ground_specular", new Vector3(100, 0, 100)));
        }

        public void Add(DeferredSprite pSprite)
        {
            mSprites.Add(pSprite);
        }

        public void BuildFromMaze(MazeStructure pMaze)
        {
            for (int y = 0; y < pMaze.YSize; y++)
            {
                for (int x = 0; x < pMaze.XSize; x++)
                {
                    if (pMaze.MazeData[y][x].IsFloor)
                    {
                        Add(new DeferredSprite(@"Models\ground_diffuse", @"Models\ground_normal", @"Models\ground_specular", new Vector3(x * 200 , y * 200, 0)));
                    }
                }
            }
        }
    }
}
