using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ExtremeFireFighter
{

    class ChaserEnemy : Enemy
    {
        int collisionDamageValue = 0;

        public int Collision_Damage
        {
            get { return collisionDamageValue; }
            set { collisionDamageValue = value; }
        }

        public ChaserEnemy(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed,
            int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, millisecondsPerFrame)
        { }

        public override void changeDirection(Vector2 playerLocation, Vector2 spriteLocation)
        {

            Vector2 newDirection = Vector2.Subtract(playerLocation, spriteLocation);
            newDirection.Normalize();
            this.setDirection(newDirection);
        }

        public virtual int applyCollisionDamage()
        {
            return Collision_Damage;
        }


    }
}
