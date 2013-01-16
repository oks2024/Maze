using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Maze.Entities.Datas;

namespace Maze.Entities.Textures
{
    class InstanciatedTexture: InstanciatedData<Texture2D>
    {
        public Texture2D texture
        {
            get
            {
                return mData;
            }
        }

        public InstanciatedTexture(string pPath, Texture2D pData)
            : base(pPath, pData)
        {
        }

        #region Override operators
        public static bool operator ==(InstanciatedTexture pTex1, InstanciatedTexture pTex2)
        {
            return String.Equals(pTex1.Path, pTex2.Path);
        }

        public static bool operator !=(InstanciatedTexture pTex1, InstanciatedTexture pTex2)
        {
            return !(pTex1 == pTex2);
        }
        #endregion

        #region Override
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }
}
