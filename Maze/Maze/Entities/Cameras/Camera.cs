using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace DeferredRendering.Entities.Cameras
{
    public class Camera : Microsoft.Xna.Framework.GameComponent
    {
        private bool isPlayer;

        private float cameraArc = -30;

        private float mSpeed = 0.2f;

        public float CameraArc
        {
            get { return cameraArc; }
            set { cameraArc = value; }
        }

        private float cameraRotation = 0;

        public float CameraRotation
        {
            get { return cameraRotation; }
            set { cameraRotation = value; }
        }

        private float cameraDistance = 1000;

        public float CameraDistance
        {
            get { return cameraDistance; }
            set { cameraDistance = value; }
        }
        private Matrix view;
        private Matrix projection;

        private Vector3 position;

        public Vector3 Position
        {
            get { return position; }
        }

        private float nearPlaneDistance = 1;
        public float NearPlaneDistance
        {
            get { return nearPlaneDistance; }
            set { nearPlaneDistance = value; }
        }

        private float farPlaneDistance = 3000;
        public float FarPlaneDistance
        {
            get { return farPlaneDistance; }
            set { farPlaneDistance = value; }
        }


        public Matrix View
        {
            get
            {

                return view;
            }
            set
            {
                view = value;
            }
        }

        public Matrix Projection
        {
            get
            {


                return projection;
            }
            set
            {
                projection = value;
            }
        }

        KeyboardState currentKeyboardState = new KeyboardState();
        GamePadState currentGamePadState = new GamePadState();

        public Camera(Game game, bool isPlayer)
            : base(game)
        {
            // TODO: Construct any child components here
            this.isPlayer = isPlayer;
            position = new Vector3(0);
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            base.Initialize();
        }


        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            if (isPlayer)
            {
                currentKeyboardState = Keyboard.GetState();
                currentGamePadState = GamePad.GetState(PlayerIndex.One);

                // TODO: Add your update code here

                float time = (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                // Check for input to rotate the camera up and down around the model.
                if (currentKeyboardState.IsKeyDown(Keys.Up) ||
                    currentKeyboardState.IsKeyDown(Keys.Z))
                {
                    position.Y += time * mSpeed;
                    cameraArc += time * 0.1f;
                }

                if (currentKeyboardState.IsKeyDown(Keys.Down) ||
                    currentKeyboardState.IsKeyDown(Keys.S))
                {
                    position.Y -= time * mSpeed;
                    cameraArc -= time * 0.1f;
                }

                cameraArc += currentGamePadState.ThumbSticks.Right.Y * time * 0.05f;

                // Limit the arc movement.
                if (cameraArc > 90.0f)
                    cameraArc = 90.0f;
                else if (cameraArc < -90.0f)
                    cameraArc = -90.0f;

                // Check for input to rotate the camera around the model.
                if (currentKeyboardState.IsKeyDown(Keys.Right) ||
                    currentKeyboardState.IsKeyDown(Keys.D))
                {
                    position.X += time * mSpeed;
                    cameraRotation += time * 0.1f;
                }

                if (currentKeyboardState.IsKeyDown(Keys.Left) ||
                    currentKeyboardState.IsKeyDown(Keys.Q))
                {
                    position.X -= time * mSpeed;
                    cameraRotation -= time * 0.1f;
                }

                cameraRotation += currentGamePadState.ThumbSticks.Right.X * time * 0.05f;

                // Check for input to zoom camera in and out.
                if (currentKeyboardState.IsKeyDown(Keys.A))
                    cameraDistance += time * 0.25f;

                if (currentKeyboardState.IsKeyDown(Keys.E))
                    cameraDistance -= time * 0.25f;

                cameraDistance += currentGamePadState.Triggers.Left * time * 0.25f;
                cameraDistance -= currentGamePadState.Triggers.Right * time * 0.25f;

                // Limit the arc movement.
                if (cameraDistance > 11900.0f)
                    cameraDistance = 11900.0f;
                else if (cameraDistance < 10.0f)
                    cameraDistance = 10.0f;

                if (currentGamePadState.Buttons.RightStick == ButtonState.Pressed ||
                    currentKeyboardState.IsKeyDown(Keys.R))
                {
                    cameraArc = -30;
                    cameraRotation = 0;
                    cameraDistance = 100;
                }

                view = Matrix.CreateTranslation(0, -10, 0) *
                          Matrix.CreateRotationY(MathHelper.ToRadians(cameraRotation)) *
                          Matrix.CreateRotationX(MathHelper.ToRadians(cameraArc)) *
                          Matrix.CreateLookAt(new Vector3(0, 0, -cameraDistance),
                                              new Vector3(0, 0, 0), Vector3.Up);

                view = Matrix.CreateLookAt(new Vector3(position.X, position.Y, 200), new Vector3(position.X, position.Y, 0), Vector3.Up);

                position = Vector3.Transform(Vector3.Zero, Matrix.Invert(view));

                float aspectRatio = (float)Game.Window.ClientBounds.Width /
                                    (float)Game.Window.ClientBounds.Height;
                projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                                                                        aspectRatio,
                                                                        nearPlaneDistance,
                                                                        farPlaneDistance);

                projection = Matrix.CreateOrthographic(1280, 720, 1, 300);

            }
            base.Update(gameTime);
        }
    }
}
