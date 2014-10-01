using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace ExtremeFireFighter
{
    class FireFighter : Sprite
    {
        private int health;


        public int health_
        {
            get { return health; }
            set { health = value; }
        }

        public void applyDamage(int damage)
        {
            health -= damage;
        }

        public override Vector2 direction
        {
            get
            {
                Vector2 inputDirection = Vector2.Zero;

                if (Keyboard.GetState().IsKeyDown(Keys.A))
                {
                    inputDirection.X -= 1;
                    if (Current_Frame.Y != 0 && !(Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.S)))
                        Current_Frame = new Point(0, 0);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                    inputDirection.X += 1;
                    if (Current_Frame.Y != 2 && !(Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.S)))
                        Current_Frame = new Point(0, 2);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    inputDirection.Y -= 1;
                    if (Current_Frame.Y != 3)
                        Current_Frame = new Point(0, 3);
                }
                if (Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    inputDirection.Y += 1;
                    if (Current_Frame.Y != 1)
                        Current_Frame = new Point(0, 1);
                }

                return inputDirection * speed;
            }
        }

        public FireFighter(Texture2D textureImage, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize,
            Vector2 speed, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, millisecondsPerFrame)
        {
            health = 20;
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            // Move the sprite based on direction
            position += direction;

            // If sprite is off the screen, move it back within the game window
            if (position.X < 0)
                position.X = 0;
            if (position.Y < 47)
                position.Y = 47;
            if ((position.X >= 230 && position.X <= 275) || (position.X >= 780 && position.X <= 825))
            {
                if (position.Y < 57)
                {
                    position.Y = 57;
                }
            }
            if (position.X > clientBounds.Width - frameSize.X)
                position.X = clientBounds.Width - frameSize.X;
            if (position.Y > clientBounds.Height - frameSize.Y)
                position.Y = clientBounds.Height - frameSize.Y;

            base.Update(gameTime, clientBounds);
        }

    }
}
