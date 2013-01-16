using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Maze.Entities.Textures;

namespace Maze.Entities.Materials
{
    class MaterialManager
    {

        //private List<DeferredMaterial> mMaterials;
        private Dictionary<int, DeferredMaterial> mMaterials;

        #region Singleton
        private static MaterialManager mInstance = null;

        private static MaterialManager Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new MaterialManager();
                }
                return mInstance;
            }
        }
        #endregion

        // Constructor
        private MaterialManager()
        {
            mMaterials = new Dictionary<int, DeferredMaterial>();

        }

        public static int Load(InstanciatedTexture pDiffuse, InstanciatedTexture pNormal, InstanciatedTexture pSpecular)
        {
            
            int i = 0;
            bool found = false;

            while (!found && (i < Instance.mMaterials.Count))
            {
                if ((Instance.mMaterials[i].Diffuse == pDiffuse) && (Instance.mMaterials[i].Normal == pNormal) && (Instance.mMaterials[i].Specular == pSpecular))
                {
                    found = true;
                }
                else
                    i++;
            } 

            if (found)
            {
                Instance.mMaterials[i].IncreaseCounter();
                return Instance.mMaterials[i].Id;
            }
            else
            {
                int id;
                if (Instance.mMaterials.Count == 0)
                    id = 0;
                else
                    id = Instance.mMaterials[Instance.mMaterials.Count - 1].Id + 1;

                DeferredMaterial newMaterial = new DeferredMaterial(id, pDiffuse, pNormal, pSpecular);
                Instance.mMaterials.Add(id, newMaterial);
                return id;
            }
        }

        public static void Unload(int id)
        {
            if (Instance.mMaterials.ContainsKey(id))
            {
                Instance.mMaterials.Remove(id);
            }
        }

        public static void GetTextures(int pId, out Texture2D pDiffuse, out Texture2D pNormal, out Texture2D pSpecular)
        {
            DeferredMaterial material = Instance.mMaterials[pId];
            pDiffuse = material.Diffuse.Data;
            pNormal = material.Normal.Data;
            pSpecular = material.Specular.Data;
        }

    }
}
