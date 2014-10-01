using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace ExtremeFireFighter
{
    class SpawnerEnemy : Enemy
    {
        public SpawnerEnemy(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed,
            int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, millisecondsPerFrame)
        {
            //base health for testing out projectile damage
            health_ = 1;
        }

        public override void changeDirection(Vector2 playerLocation, Vector2 spriteLocation)
        {
            //add random movement
            base.changeDirection(playerLocation, spriteLocation);
        }

        public virtual Boolean canSpawn(GameTime gameTime)
        {
            return false;
        }
    }
}
