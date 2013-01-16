using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Maze.Entities.Textures
{
    class TextureManager
    {

        private Dictionary<string, InstanciatedTexture> mTextures;

        #region Singleton
        private static TextureManager mInstance = null;

        private static TextureManager Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new TextureManager();
                }
                return mInstance;
            }
        }
        #endregion

        private TextureManager()
        {
            mTextures = new Dictionary<string, InstanciatedTexture>();
        }

        static public InstanciatedTexture Load(string pPath)
        {
            if (!Instance.mTextures.ContainsKey(pPath))
            {
                Instance.mTextures.Add(pPath, new InstanciatedTexture(pPath, Engine.Instance.Game.Content.Load<Texture2D>(pPath)));    
            }

            Instance.mTextures[pPath].IncreaseCount();

            return Instance.mTextures[pPath];
        }

        static public void UnLoad(string pPath)
        {
            if (Instance.mTextures.ContainsKey(pPath))
            {
                Instance.mTextures.Remove(pPath);
            }
        }
    }
}
