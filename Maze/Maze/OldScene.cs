using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Ziggyware;

namespace Maze
{
    public class OldScene:DrawableGameComponent
    {
        private Game1 game;
        private SpriteBatch spriteBatch;

        private AreaLight light1;
        private Texture2D mBackground;

        QuadRenderComponent quadRender;

        private RenderTarget2D screenShadows;
        private ShadowMapResolver shadowMapResolver;

        public OldScene(Game1 game)
            : base(game)
        {
            this.game = game;
            quadRender = new QuadRenderComponent(game);
            game.Components.Add(quadRender);
        }

        public override void Initialize()
        {
            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            mBackground = game.Content.Load<Texture2D>("tile");
            base.Initialize();
        }

        protected override void LoadContent()
        {
            shadowMapResolver = new ShadowMapResolver(GraphicsDevice, quadRender, ShadowMapSize.Size256, ShadowMapSize.Size1024);
            shadowMapResolver.LoadContent(game.Content);
            light1 = new AreaLight(GraphicsDevice, ShadowMapSize.Size512);
            screenShadows = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {

            //first light area
            light1.LightPosition = new Vector2(200, 200);
            light1.BeginDrawingShadowCasters();
            //DrawCasters(lightArea1);
            light1.EndDrawingShadowCasters();
            shadowMapResolver.ResolveShadows(light1.RenderTarget, light1.RenderTarget, new Vector2(200, 200));

            GraphicsDevice.SetRenderTarget(screenShadows);
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
            spriteBatch.Draw(light1.RenderTarget, light1.LightPosition - light1.LightAreaSize * 0.5f, Color.White);
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);


            GraphicsDevice.Clear(Color.Black);

            DrawBackground();

            BlendState blendState = new BlendState();
            blendState.ColorSourceBlend = Blend.DestinationColor;
            blendState.ColorDestinationBlend = Blend.SourceColor;

            spriteBatch.Begin(SpriteSortMode.Immediate, blendState);
            spriteBatch.Draw(screenShadows, Vector2.Zero, Color.White);
            spriteBatch.End();

            
            base.Draw(gameTime);
        }

        private void DrawBackground()
        {
            Rectangle source = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.LinearWrap, DepthStencilState.Default, RasterizerState.CullNone);
            spriteBatch.Draw(mBackground, Vector2.Zero, source, Color.White, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
            spriteBatch.End();
        }

    }
}
