using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;


namespace ExtremeFireFighter
{
    class EvilCatLady : SpawnerEnemy
    {
        int timeBetweenSpawn = 4000;

        int movementDirection, movementCount = 0;
        Random numberGen = new Random();
        Vector2[] movementDirections = new Vector2[4] { new Vector2(-1, 0), new Vector2(0, -1), new Vector2(1, 0), new Vector2(0, 1) };

        public EvilCatLady(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed,
            int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, millisecondsPerFrame)
        {
            health_ = 5;
            movementDirection = numberGen.Next(0, 4);
            this.setDirection(movementDirections[movementDirection]);
        }

        public override void changeDirection(Vector2 playerLocation, Vector2 spriteLocation)
        {
            if (movementCount == 60)
            {
                if (movementDirection == 3)
                    movementDirection = 0;
                else
                    movementDirection++;

                this.setDirection(movementDirections[movementDirection]);
                movementCount = 0;
            }
            else
                movementCount++;
        }

        public override Boolean canSpawn(GameTime gameTime)
        {
            timeBetweenSpawn += gameTime.ElapsedGameTime.Milliseconds;
            if (numberGen.Next(0, 20) == 0 && timeBetweenSpawn >= 5000)
            {
                timeBetweenSpawn = 0;
                return true;
            }
            else
                return false;
        }

    }
}
