using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace ExtremeFireFighter
{
    class TheSource : Enemy
    {
        int attackCoolDown = 750;
        int count = 0;
        Random numberGen = new Random();

        public TheSource(Texture2D textureImage, Vector2 position, Point frameSize,
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed,
            int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, millisecondsPerFrame)
        {
            //base health for testing out projectile damage
            health_ = 100;
        }

        public String chooseAttack(GameTime gameTime, int allyCount)
        {
            attackCoolDown += gameTime.ElapsedGameTime.Milliseconds;
            if (attackCoolDown >= 750)
            {
                if (numberGen.Next(0, 7) == 0)
                {
                    attackCoolDown = 0;
                    if (numberGen.Next(0, 11) <= 6)
                        return "spawnSmoke";

                    else
                        return "spawnPyro";
                }
                else
                {
                    attackCoolDown = 0;
                    if ((numberGen.Next(0, 11) > 7) || count == 4)
                    {
                        count = 0;
                        return "multiDirectional";
                    }
                    else
                    {
                        count++;
                        return "trishot";
                    }
                }
            }
            else
                return "";
        }
    }
}
