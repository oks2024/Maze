using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Ziggyware;
using Microsoft.Xna.Framework.Graphics;

namespace Maze.Renderer.Effects
{
    class CombineFinalRenderer
    {
        private Game1 mGame;

        private int mBackBufferWidth;
        private int mBackBufferHeight;
        private Vector2 mHalfPixel;

        private QuadRenderComponent mQuadRenderer;

        private Effect mCombineFinalEffect;

        public CombineFinalRenderer(Game1 pGame)
        {
            this.mGame = pGame;
        }

        public void LoadContent()
        {
            mBackBufferHeight = mGame.GraphicsDevice.PresentationParameters.BackBufferHeight;
            mBackBufferWidth = mGame.GraphicsDevice.PresentationParameters.BackBufferWidth;

            mHalfPixel = new Vector2(0.5f / (float)mBackBufferWidth, 0.5f / (float) mBackBufferHeight);

            mCombineFinalEffect = mGame.Content.Load<Effect>(@"Shaders\Effects\CombineFinal\CombineFinal");

            mQuadRenderer = new QuadRenderComponent(mGame);
            mGame.Components.Add(mQuadRenderer);

        }

        public void Render(RenderTarget2D pCombineFinalRT, RenderTarget2D pDiffuseRT, RenderTarget2D pLightRT)
        {
            //mGame.GraphicsDevice.SetRenderTarget(pCombineFinalRT);

            mCombineFinalEffect.Parameters["AlbedoMap"].SetValue(pDiffuseRT);
            mCombineFinalEffect.Parameters["LightMap"].SetValue(pLightRT);
            mCombineFinalEffect.Parameters["halfPixel"].SetValue(mHalfPixel);

            mCombineFinalEffect.Techniques[0].Passes[0].Apply();

            mQuadRenderer.Render(Vector2.One * -1, Vector2.One);

            //mGame.GraphicsDevice.SetRenderTarget(null);
        }
    }
}
