using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ziggyware;
using DeferredRendering.Entities.Cameras;
using Maze.Entities.Sprites;
using Maze.Entities;
using Maze.Entities.Materials;

namespace Maze.Renderer.Effects
{
    class GBufferRenderer
    {
        private Game1 mGame;

        private Effect mClearBufferEffect;
        private Effect mRenderBufferEffect;

        private int mBackBufferHeight;
        private int mBackBufferWidth;

        private Vector2 mHalfPixel;

        private QuadRenderComponent mQuadRenderer;

        private Model mGround;

        public GBufferRenderer(Game1 pGame)
        {
            this.mGame = pGame;
        }

        public void LoadContent()
        {
            mBackBufferHeight = mGame.GraphicsDevice.PresentationParameters.BackBufferHeight;
            mBackBufferWidth = mGame.GraphicsDevice.PresentationParameters.BackBufferWidth;

            mHalfPixel.X = 0.5f / (float)mGame.GraphicsDevice.PresentationParameters.BackBufferWidth;
            mHalfPixel.Y = 0.5f / (float)mGame.GraphicsDevice.PresentationParameters.BackBufferHeight;

            mClearBufferEffect = mGame.Content.Load<Effect>(@"Shaders\Effects\GBuffer\ClearGBuffer");
            mRenderBufferEffect = mGame.Content.Load<Effect>(@"Shaders\Effects\GBuffer\RenderGBuffer");

            mQuadRenderer = new QuadRenderComponent(mGame);
            mGame.Components.Add(mQuadRenderer);

            mGround = mGame.Content.Load<Model>(@"Models\sprite200");

            //mGroundDiffuse = mGame.Content.Load<Texture2D>(@"Models\ground_diffuse");
            //mGroundNormal = mGame.Content.Load<Texture2D>(@"Models\ground_normal");
            //mGroundSpecular = mGame.Content.Load<Texture2D>(@"Models\ground_specular");
        }

        public void Render(RenderTarget2D pAlbedoRT, RenderTarget2D pNormalRT, RenderTarget2D pDepthRT, Camera pCamera)
        {
            mGame.GraphicsDevice.SetRenderTargets(pAlbedoRT, pNormalRT, pDepthRT);

            // Clear GBuffer.
            mClearBufferEffect.Techniques[0].Passes[0].Apply();
            mQuadRenderer.Render(Vector2.One * -1, Vector2.One);

            foreach (DeferredSprite sprite in Engine.Instance.CurrentScene.Sprites)
            {
                mRenderBufferEffect.Techniques[0].Passes[0].Apply();

                Texture2D diffuse;
                Texture2D normal;
                Texture2D specular;

                MaterialManager.GetTextures(sprite.MaterialId, out diffuse, out normal, out specular);

                mRenderBufferEffect.Parameters["DiffuseMap"].SetValue(diffuse);
                mRenderBufferEffect.Parameters["NormalMap"].SetValue(normal);
                mRenderBufferEffect.Parameters["SpecularMap"].SetValue(specular);

                mRenderBufferEffect.Parameters["World"].SetValue(Matrix.CreateTranslation(sprite.Position));
                mRenderBufferEffect.Parameters["View"].SetValue(pCamera.View);
                mRenderBufferEffect.Parameters["Projection"].SetValue(pCamera.Projection);

                // Set Render States.
                mGame.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                mGame.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;
                mGame.GraphicsDevice.BlendState = BlendState.Opaque;

                foreach (ModelMesh mesh in mGround.Meshes)
                {
                    foreach (ModelMeshPart meshPart in mesh.MeshParts)
                    {

                        //meshPart.VertexBuffer.VertexDeclaration
                        mGame.GraphicsDevice.Indices = meshPart.IndexBuffer;
                        mGame.GraphicsDevice.SetVertexBuffer(meshPart.VertexBuffer);


                        mGame.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, meshPart.VertexOffset, 0, meshPart.NumVertices, meshPart.StartIndex, meshPart.PrimitiveCount);
                    }
                }
            }

            // Resolve Render Targets.
            mGame.GraphicsDevice.SetRenderTargets(null);
        }

    }
}
