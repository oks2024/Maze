using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Maze.Entities.Textures;

namespace Maze.Entities.Datas
{
    class InstanciatedData<T>
    {
        protected int mCount;
        protected T mData;
        protected string mPath;

        internal InstanciatedData(string pPath, T pData)
        {
            mData = pData;
            mPath = pPath;
            mCount = 0;
        }

        internal void IncreaseCount()
        {
            mCount++;
        }

        public void Unload()
        {
            mCount--;
            if (mCount <= 0)
            {
                TextureManager.UnLoad(mPath);
            }
        }


        #region Getter/Setters
        public T Data
        {
            get
            {
                return mData;
            }
        }

        public string Path
        {
            get
            {
                return mPath;
            }
        }
        #endregion
    }
}
