using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Timers;
using System.Diagnostics;

namespace ExtremeFireFighter
{

    class Pyro : ShootingEnemy
    {
        int timeBetweenAttacks = 0;
        Stopwatch time = new Stopwatch();
        int movementCount = 0;
        Random numberGenerator = new Random();

        public Pyro(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed,
            int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, millisecondsPerFrame)
        {
            health_ = 5;
            Projectile_Damage = 1;
            time.Start();
        }

        public override bool canFireProjectile(GameTime gameTime)
        {
            timeBetweenAttacks += gameTime.ElapsedGameTime.Milliseconds;
            if (numberGenerator.Next(0, 20) == 0 && timeBetweenAttacks >= 750)
            {
                timeBetweenAttacks = 0;
                return true;
            }
            else
                return false;
        }

        public override void changeDirection(Vector2 playerLocation, Vector2 spriteLocation)
        {

            if (movementCount == 30)
            {
                int direction = (int)(numberGenerator.Next(0, 4) + spriteLocation.X) % 4;
                System.Diagnostics.Debug.WriteLine(direction);

                this.setDirection(movementDirections[direction]);
                movementCount = 0;
            }
            else
                movementCount++;
        }
    }
}
