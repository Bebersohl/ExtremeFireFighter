using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//Dee, Dillon
//Ebersohl, Brandon
//Gordon, Madeline
//Larson, Alex
//Section 1

namespace ExtremeFireFighter
{
    abstract class Sprite
    {
        // Stuff needed to draw the sprite
        Texture2D textureImage;
        protected Point frameSize;
        Point currentFrame;
        Point sheetSize;

        // Collision data
        int collisionOffset;

        // Framerate stuff
        int timeSinceLastFrame = 0;
        int millisecondsPerFrame;
        const int defaultMillisecondsPerFrame = 16;

        // Movement data
        protected Vector2 speed;
        protected Vector2 position;

        // Abstract definition of direction property
        public abstract Vector2 direction
        {
            get;
        }

        public Vector2 getPosition()
        {
            return position;
        }

        public void setPosition(int x, int y)
        {
            position.X = x;
            position.Y = y;
        }

        public void setDirection(Vector2 direction)
        {
            this.speed = direction;
        }

        public Point Current_Frame
        {
            get { return currentFrame; }
            set { currentFrame = value; }
        }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize,
    int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed)
            : this(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, defaultMillisecondsPerFrame)
        {
        }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed,
            int millisecondsPerFrame)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.frameSize = frameSize;
            this.collisionOffset = collisionOffset;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.speed = speed;
            this.millisecondsPerFrame = millisecondsPerFrame;
        }

        public virtual void Update(GameTime gameTime, Rectangle clientBounds)
        {
            if (this is FireFighter && (direction.X == 0 && direction.Y == 0))
            {
                currentFrame.X = 0;
            }
            else
            {
                // Update frame if time to do so based on framerate
                timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
                if (timeSinceLastFrame > millisecondsPerFrame)
                {
                    // Increment to next frame
                    timeSinceLastFrame = 0;
                    ++currentFrame.X;
                    if (currentFrame.X >= sheetSize.X)
                    {
                        currentFrame.X = 0;
                    }
                }
            }

        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Draw the sprite
            spriteBatch.Draw(textureImage,
                position,
                new Rectangle(currentFrame.X * frameSize.X,
                    currentFrame.Y * frameSize.Y,
                    frameSize.X, frameSize.Y),
                Color.White, 0, Vector2.Zero,
                1f, SpriteEffects.None, 0);
        }

        // Gets the collision rect based on position, framesize and collision offset
        public Rectangle collisionRect
        {
            get
            {
                return new Rectangle(
                    (int)position.X + collisionOffset,
                    (int)position.Y + collisionOffset,
                    frameSize.X,
                    frameSize.Y);
            }
        }

        

        

    }
}
