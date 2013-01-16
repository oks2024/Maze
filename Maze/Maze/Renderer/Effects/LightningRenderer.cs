using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Ziggyware;
using Microsoft.Xna.Framework.Graphics;
using DeferredRendering.Entities.Cameras;

namespace Maze.Renderer.Effects
{
    class LightningRenderer
    {
        private Game1 mGame;

        private int mBackBufferWidth;
        private int mBackBufferHeight;
        private Vector2 mHalfPixel;

        private QuadRenderComponent mQuadRenderer;

        private Effect mDirectionalLightEffect;
        private Effect mPointLightEffect;

        private Model mSphereModel;

        public LightningRenderer(Game1 pGame)
        {
            mGame = pGame;
        }

        public void LoadContent()
        {
            mBackBufferHeight = mGame.GraphicsDevice.PresentationParameters.BackBufferHeight;
            mBackBufferWidth = mGame.GraphicsDevice.PresentationParameters.BackBufferWidth;

            mHalfPixel = new Vector2(0.5f / (float)mBackBufferWidth, 0.5f / (float)mBackBufferHeight);

            mDirectionalLightEffect = mGame.Content.Load<Effect>(@"Shaders\Effects\Lightning\DirectionalLight");
            mPointLightEffect = mGame.Content.Load<Effect>(@"Shaders\Effects\Lightning\PointLight");

            mSphereModel = mGame.Content.Load<Model>(@"Models\sphere");

            mQuadRenderer = new QuadRenderComponent(mGame);
            mGame.Components.Add(mQuadRenderer);

        }

        public void Render(RenderTarget2D pLightRT, RenderTarget2D pAlbedoRT, RenderTarget2D pNormalRT, RenderTarget2D pDepthRT, Camera pCamera)
        {
            mGame.GraphicsDevice.SetRenderTarget(pLightRT);

            mGame.GraphicsDevice.Clear(Color.Transparent);

            mGame.GraphicsDevice.BlendState = BlendState.AlphaBlend;
            mGame.GraphicsDevice.DepthStencilState = DepthStencilState.None;

            mDirectionalLightEffect.Parameters["AlbedoMap"].SetValue(pAlbedoRT);
            mDirectionalLightEffect.Parameters["NormalMap"].SetValue(pNormalRT);
            mDirectionalLightEffect.Parameters["DepthMap"].SetValue(pDepthRT);
            mDirectionalLightEffect.Parameters["LightDirection"].SetValue(new Vector3(0.5f, -0.3f, 0.0f));
            mDirectionalLightEffect.Parameters["Color"].SetValue(new Vector3(0.9f, 0.5f, 0.3f));
            mDirectionalLightEffect.Parameters["CameraPosition"].SetValue(pCamera.Position);
            mDirectionalLightEffect.Parameters["InvertViewProjection"].SetValue(Matrix.Invert(pCamera.View * pCamera.Projection));
            mDirectionalLightEffect.Parameters["HalfPixel"].SetValue(mHalfPixel);

            mDirectionalLightEffect.Techniques[0].Passes[0].Apply();
            mQuadRenderer.Render(Vector2.One * -1, Vector2.One);

            DrawPointLight(pAlbedoRT, pNormalRT, pDepthRT, pCamera, pCamera.Position - Vector3.UnitZ * 150, Color.Blue, 100, 1);

            mGame.GraphicsDevice.BlendState = BlendState.Opaque;
            //mGame.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            mGame.GraphicsDevice.SetRenderTarget(null);
        }

        private void DrawPointLight(RenderTarget2D pAlbedoRT, RenderTarget2D pNormalRT, RenderTarget2D pDepthRT, Camera pCamera, Vector3 pPosition, Color pColor, int pRadius, int pIntensity)
        {
            // Set-up light buffer render targets.
            mPointLightEffect.Parameters["AlbedoMap"].SetValue(pAlbedoRT);
            mPointLightEffect.Parameters["NormalMap"].SetValue(pNormalRT);
            mPointLightEffect.Parameters["DepthMap"].SetValue(pDepthRT);

            // Compute the light world matrix.
            // Scale according to the radius and translate it to light position.
            Matrix sphereToWorldMatrix = Matrix.CreateScale(pRadius) * Matrix.CreateTranslation(pPosition);

            mPointLightEffect.Parameters["World"].SetValue(sphereToWorldMatrix);
            mPointLightEffect.Parameters["View"].SetValue(pCamera.View);
            mPointLightEffect.Parameters["Projection"].SetValue(pCamera.Projection);

            mPointLightEffect.Parameters["LightPosition"].SetValue(pPosition);

            mPointLightEffect.Parameters["Color"].SetValue(pColor.ToVector3());
            mPointLightEffect.Parameters["LightIntensity"].SetValue(pIntensity);
            mPointLightEffect.Parameters["LightRadius"].SetValue(pRadius);

            // Parameters for specular computations.
            mPointLightEffect.Parameters["CameraPosition"].SetValue(pCamera.Position);
            mPointLightEffect.Parameters["InvertViewProjection"].SetValue(Matrix.Invert(pCamera.View * pCamera.Projection));

            // Size of a half pixel, for texture alignement.
            mPointLightEffect.Parameters["HalfPixel"].SetValue(mHalfPixel);

            // Calculate the distance between the camera and the light center.
            float cameraToCenter = Vector3.Distance(pCamera.Position, pPosition);

            // If we are inside the volume, draw the sphere's inside face.
            if (cameraToCenter < pRadius)
                mGame.GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;
            else
                mGame.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

            mPointLightEffect.Techniques[0].Passes[0].Apply();

            foreach (ModelMesh mesh in mSphereModel.Meshes)
            {
                foreach (ModelMeshPart meshPart in mesh.MeshParts)
                {
                    mGame.GraphicsDevice.Indices = meshPart.IndexBuffer;
                    mGame.GraphicsDevice.SetVertexBuffer(meshPart.VertexBuffer);

                    mGame.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, meshPart.NumVertices, meshPart.StartIndex, meshPart.PrimitiveCount);
                }
            }

            mGame.GraphicsDevice.RasterizerState = RasterizerState.CullCounterClockwise;

        }
    }
}
