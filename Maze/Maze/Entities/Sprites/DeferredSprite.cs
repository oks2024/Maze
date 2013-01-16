using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Maze.Entities.Materials;
using Maze.Entities.Textures;

namespace Maze.Entities.Sprites
{
    class DeferredSprite
    {
        private Vector3 mPosition;

        private int mMaterialId;

        public DeferredSprite(string pDiffuse, string pNormal, string pSpecular)
        {
            mMaterialId = MaterialManager.Load(TextureManager.Load(pDiffuse), TextureManager.Load(pNormal), TextureManager.Load(pSpecular));
        }

        public DeferredSprite(string pDiffuse, string pNormal, string pSpecular, Vector3 pPosition)
        {
            mMaterialId = MaterialManager.Load(TextureManager.Load(pDiffuse), TextureManager.Load(pNormal), TextureManager.Load(pSpecular));
            this.mPosition = pPosition;
        }

        #region Getters/Setters
        public Vector3 Position
        {
            get { return mPosition; }
            set { mPosition = value; }
        }

        public int MaterialId
        {
            get { return mMaterialId; }
            set { mMaterialId = value; }
        }
        #endregion

        #region Override operators
        public static bool operator >(DeferredSprite pSprite1, DeferredSprite pSprite2)
        {
            if (pSprite1.MaterialId == pSprite2.MaterialId)
            {
                return pSprite1.Position.Z > pSprite2.Position.Z;
            }

            return pSprite1.MaterialId > pSprite2.MaterialId;
        }

        public static bool operator <(DeferredSprite pSprite1, DeferredSprite pSprite2)
        {
            if (pSprite1.MaterialId == pSprite2.MaterialId)
            {
                return pSprite1.Position.Z < pSprite2.Position.Z;
            }

            return pSprite1.MaterialId < pSprite2.MaterialId;
        }

        public static bool operator >=(DeferredSprite pSprite1, DeferredSprite pSprite2)
        {
            if (pSprite1.MaterialId == pSprite2.MaterialId)
            {
                return pSprite1.Position.Z >= pSprite2.Position.Z;
            }

            return pSprite1.MaterialId >= pSprite2.MaterialId;
        }

        public static bool operator <=(DeferredSprite pSprite1, DeferredSprite pSprite2)
        {
            if (pSprite1.MaterialId == pSprite2.MaterialId)
            {
                return pSprite1.Position.Z <= pSprite2.Position.Z;
            }

            return pSprite1.MaterialId <= pSprite2.MaterialId;
        }

        #endregion
    }
}
