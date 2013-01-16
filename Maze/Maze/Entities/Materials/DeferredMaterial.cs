using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Maze.Entities.Textures;

namespace Maze.Entities.Materials
{
    class DeferredMaterial
    {
        private int mId;

        private InstanciatedTexture mDiffuse;
        private InstanciatedTexture mNormal;
        private InstanciatedTexture mSpecular;

        private int mCount;

        internal DeferredMaterial(int pId, InstanciatedTexture pDiffuse, InstanciatedTexture pNormal, InstanciatedTexture pSpecular)
        {
            this.Id = pId;
            this.Diffuse = pDiffuse;
            this.Normal = pNormal;
            this.Specular = pSpecular;
        }

        internal void IncreaseCounter()
        {
            mCount++;
        }

        internal void Unload()
        {
            mCount--;
            if (mCount <= 0)
            {
                mDiffuse.Unload();
                mNormal.Unload();
                mSpecular.Unload();
                MaterialManager.Unload(Id);
            }
        }

        #region Getters/Setters

        public int Id
        {
            get { return mId; }
            set { mId = value; }
        }

        public InstanciatedTexture Diffuse
        {
            get { return mDiffuse; }
            set { mDiffuse = value; }
        }

        public InstanciatedTexture Normal
        {
            get { return mNormal; }
            set { mNormal = value; }
        }

        public InstanciatedTexture Specular
        {
            get { return mSpecular; }
            set { mSpecular = value; }
        }
        #endregion

        #region Override operators
        public static bool operator ==(DeferredMaterial pMat1, DeferredMaterial pMat2)
        {
            if (pMat1.Diffuse != pMat2.Diffuse)
                return false;
            if (pMat1.Normal != pMat2.Normal)
                return false;
            if (pMat1.Specular != pMat2.Specular)
                return false;

            return true;
        }

        public static bool operator !=(DeferredMaterial pMat1, DeferredMaterial pMat2)
        {
            return !(pMat1 == pMat2);
        }
        #endregion

        #region Override
        public override bool Equals(object obj)
        {
            return this == (DeferredMaterial)obj;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }
}
