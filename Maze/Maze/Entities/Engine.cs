using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maze.Entities.Scenes;

namespace Maze.Entities
{

    class Engine
    {
        private Game1 mGame;
        private Scene mCurrentScene;

        #region Singleton
        private static Engine mInstance = null;

        public static Engine Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new Engine();
                }
                return mInstance;
            }
        }
        #endregion

        private Engine()
        {
        }

        #region Getters/Setters
        public Game1 Game
        {
            get { return mGame; }
            set { mGame = value; }
        }

        public Scene CurrentScene
        {
            get { return mCurrentScene; }
            set { mCurrentScene = value; }
        }
        #endregion
    }
}
