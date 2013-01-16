using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Maze.Renderer.Effects;
using DeferredRendering.Entities.Cameras;


namespace Maze.Renderer
{
    /// <summary>
    /// This is the main renderer. It will draw the scene, calling all the current effects, and will output the final render target.
    /// </summary>
    public class DeferredRenderer : DrawableGameComponent
    {
        private Game1 mGame;

        // Renderers.
        private GBufferRenderer mGBufferRenderer;
        private LightningRenderer mLightningRenderer;
        private CombineFinalRenderer mConbineFinalRenderer;

        // Render Targets.
        private RenderTarget2D mAlbedoRT;
        private RenderTarget2D mNormalRT;
        private RenderTarget2D mDepthRT;
        private RenderTarget2D mLightRT;
        private RenderTarget2D mCombineFinalRT;

        // Cameras.
        private Camera mCamera;

        // Debug RT.
        private SpriteBatch mSpriteBatch;


        public DeferredRenderer(Game1 pGame)
            : base(pGame)
        {
            this.mGame = pGame;

            mGBufferRenderer = new GBufferRenderer(mGame);
            mLightningRenderer = new LightningRenderer(mGame);
            mConbineFinalRenderer = new CombineFinalRenderer(mGame);
        }



        public override void Initialize()
        {

            mCamera = new Camera(mGame, true);

            Game.Components.Add(mCamera);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //Get the sizes of the backbuffer, in order to have matching render targets.
            int backBufferWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
            int backBufferHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;


            mAlbedoRT = new RenderTarget2D(GraphicsDevice, backBufferWidth, backBufferHeight, false, SurfaceFormat.Color, DepthFormat.Depth24);
            mNormalRT = new RenderTarget2D(GraphicsDevice, backBufferWidth, backBufferHeight, false, SurfaceFormat.Color, DepthFormat.None);
            mDepthRT = new RenderTarget2D(GraphicsDevice, backBufferWidth, backBufferHeight, false, SurfaceFormat.Single, DepthFormat.None);
            mLightRT = new RenderTarget2D(GraphicsDevice, backBufferWidth, backBufferHeight, false, SurfaceFormat.Color, DepthFormat.None);
            mCombineFinalRT = new RenderTarget2D(GraphicsDevice, backBufferWidth, backBufferHeight, false, SurfaceFormat.Color, DepthFormat.None);

            mSpriteBatch = new SpriteBatch(Game.GraphicsDevice);

            mGBufferRenderer.LoadContent();
            mLightningRenderer.LoadContent();
            mConbineFinalRenderer.LoadContent();
            

            base.LoadContent();
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            mGBufferRenderer.Render(mAlbedoRT, mNormalRT, mDepthRT, mCamera);
            
            mLightningRenderer.Render(mLightRT, mAlbedoRT, mNormalRT, mDepthRT, mCamera);

            mConbineFinalRenderer.Render(mCombineFinalRT, mAlbedoRT, mLightRT);

            

            int halfWidth = GraphicsDevice.Viewport.Width / 8;
            int halfHeight = GraphicsDevice.Viewport.Height / 8;

            
            mSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            mSpriteBatch.Draw(mAlbedoRT, new Rectangle(0, 0, halfWidth, halfHeight), Color.White);
            mSpriteBatch.Draw(mNormalRT, new Rectangle(halfWidth, 0, halfWidth, halfHeight), Color.White);
            mSpriteBatch.Draw(mDepthRT, new Rectangle(0, halfHeight, halfWidth, halfHeight), Color.White);
            mSpriteBatch.Draw(mLightRT, new Rectangle(halfWidth, halfHeight, halfWidth, halfHeight), Color.White);
            mSpriteBatch.End();
            
            
            base.Draw(gameTime);
        }
    }
}
