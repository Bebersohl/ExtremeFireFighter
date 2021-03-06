﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ExtremeFireFighter
{
    class EvilCat : ChaserEnemy
    {
        //have it use the specific image for evilcat
        public EvilCat(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed,
            int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, millisecondsPerFrame)
        {
            //when an enemy of this type is created these are there stats
            health_ = 2;
            Collision_Damage = 5;
            
        }

        public override void changeDirection(Vector2 playerLocation, Vector2 spriteLocation)
        {
            Vector2 newDirection = Vector2.Subtract(playerLocation, spriteLocation);
            newDirection.Normalize();
            this.setDirection(newDirection * 5);
        }


    }
}
