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
    class Fire : SpawnerEnemy
    {
        int timeBetweenSpawn = 0;
        Random numberGen = new Random();

        public Fire(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed,
            int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, millisecondsPerFrame)
        {
            health_ = 4;
        }

        public override Boolean canSpawn(GameTime gameTime)
        {
            timeBetweenSpawn += gameTime.ElapsedGameTime.Milliseconds;
            if (timeBetweenSpawn >= 7000 && numberGen.Next(0,16) == 0)
            {
                timeBetweenSpawn = 0;
                return true;
            }
            else
                return false;
        }
        public override void changeDirection(Vector2 playerLocation, Vector2 spriteLocation)
        {
            this.setDirection(new Vector2(0,0));
        }
    }
}
