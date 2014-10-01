using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;
using System.Timers;

//Dee, Dillon
//Ebersohl, Brandon
//Gordon, Madeline
//Larson, Alex
//Section 1

namespace ExtremeFireFighter
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class SpriteManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        int levelClear = 1;
        Random random = new Random();
        Random numberGen = new Random();
        SpriteFont font;
        bool bossFight = false;

        Vector2[] multiDirecitonal = new Vector2[] { new Vector2(0, 0), new Vector2(1100, 800), new Vector2(1100, 0), new Vector2(0, 800),
                                                     new Vector2(450,0), new Vector2(450,800), new Vector2(0,450), new Vector2(1100,450)};

        int[] fiftyW = { 0, 50, 100, 150, 200, 250, 300, 350, 400, 450, 500, 550, 600, 650, 700, 750, 800, 850, 900, 950, 1000, 1050 };
        int[] fiftyT = { 90, 140, 190, 240, 290, 340, 390, 440, 490, 540, 590, 640, 690, 740};

        //SpriteBatch for drawing
        SpriteBatch spriteBatch;

        //A sprite for the player and a list of automated sprites
        FireFighter player;
        PlayerCrossHair crossHair;


        //Enemy test, test2,test3;

        List<Enemy> enemyList = new List<Enemy>();
        List<Sprite> projectileList = new List<Sprite>();
        List<Enemy> spawnList = new List<Enemy>();

        Timer timer = new Timer(1);
        bool allowFire = true;
        bool weaponChanged = true;

        public SpriteManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
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

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            timer.Start();
            timer.Elapsed += new ElapsedEventHandler(LaunchTimer);
            font = Game.Content.Load<SpriteFont>("myFont");

            //Load the player sprite
            player = new FireFighter(
                Game.Content.Load<Texture2D>(@"Images/EF_playerSpriteSheet"),
                Vector2.Zero, new Point(50, 50), 0, new Point(0, 3),
                new Point(4, 1), new Vector2(3, 3), 200);
            player.setPosition(524, 1050);

            crossHair = new PlayerCrossHair(
                Game.Content.Load<Texture2D>(@"Images/CrossHairImage"),
                Vector2.Zero, new Point(75, 75), 0, new Point(0, 0),
                new Point(1, 1), new Vector2(6, 6));
            crossHair.setPosition(513, 363);

            generateFloor();



            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            
            //Check for current weapon and then call
            if (weaponChanged == true)
            {
                //Weapon as parameter
                ChangeWeapon();
                weaponChanged = false;
            }


            List<Int32> projectileRemoveList = new List<Int32>();
            List<Int32> enemyRemoveList = new List<Int32>();
            
            int projectileCount = 0;
            int enemyCount = 0;

            // Update player
            player.Update(gameTime, Game.Window.ClientBounds);
            crossHair.Update(gameTime, Game.Window.ClientBounds);

            // Update all sprites
            foreach (Enemy s in enemyList)
            {


                if (s is ChaserEnemy && s.collisionRect.Intersects(player.collisionRect))
                {

                        player.applyDamage(((ChaserEnemy)s).applyCollisionDamage());
                        enemyRemoveList.Add(enemyCount);
                }
                else if (s is Fire && s.collisionRect.Intersects(player.collisionRect))
                {
                    player.applyDamage(2);
                    enemyRemoveList.Add(enemyCount);
                }
                else if (s is TheSource != true)
                {
                    if (s is ShootingEnemy)
                    {
                        if (((ShootingEnemy)s).canFireProjectile(gameTime))
                            createEnemyProjectile("", s.getPosition());
                    }
                    else if (s is SpawnerEnemy)
                    {
                        if (((SpawnerEnemy)s).canSpawn(gameTime) && s is EvilCatLady)
                            spawnEnemy("evilcat", s.getPosition());
                        else if (((SpawnerEnemy)s).canSpawn(gameTime) && s is Fire)
                        {
                            spawnEnemy("fire", s.getPosition());
                            Vector2 nextSpawnLocation;
                            switch (numberGen.Next(0, 4))
                            {
                                case 0:
                                    nextSpawnLocation.X = s.getPosition().X + 50;
                                    nextSpawnLocation.Y = s.getPosition().Y;
                                    spawnEnemy("fire", nextSpawnLocation);
                                    break;
                                case 1:
                                    nextSpawnLocation.X = s.getPosition().X - 50;
                                    nextSpawnLocation.Y = s.getPosition().Y;
                                    spawnEnemy("fire", nextSpawnLocation);
                                    break;
                                case 2:
                                    nextSpawnLocation.X = s.getPosition().X;
                                    nextSpawnLocation.Y = s.getPosition().Y + 50;
                                    spawnEnemy("fire", nextSpawnLocation);
                                    break;
                                case 3:
                                    nextSpawnLocation.X = s.getPosition().X;
                                    nextSpawnLocation.Y = s.getPosition().Y - 50;
                                    spawnEnemy("fire", nextSpawnLocation);
                                    break;

                            }
                        }
                    }
                }
                else if(s is TheSource)
                    {
                        switch (((TheSource)s).chooseAttack(gameTime, enemyList.Count))
                        {
                            case "trishot":
                                Vector2 targetDirectionUP = player.getPosition();
                                Vector2 targetDirectionDown = player.getPosition();

                                createSourceProjectile(player.getPosition(), new Vector2(s.getPosition().X - 70, s.getPosition().Y), 5);

                                targetDirectionUP = Vector2.Add(targetDirectionUP, player.direction * 25);
                                createSourceProjectile(targetDirectionUP, new Vector2(s.getPosition().X - 70, s.getPosition().Y), 5);

                                targetDirectionDown = Vector2.Subtract(targetDirectionDown, player.direction * 25);
                                createSourceProjectile(targetDirectionDown, new Vector2(s.getPosition().X - 70, s.getPosition().Y), 5);
                                break;
                            case "spawnSmoke":
                                spawnEnemy("smoke", new Vector2(s.getPosition().X - 70, s.getPosition().Y));
                                break;
                            case "spawnPyro":
                                switch (numberGen.Next(0, 4))
                                {
                                    case 0:
                                        spawnEnemy("pyro", new Vector2(50, 50));
                                        break;
                                    case 1:
                                        spawnEnemy("pyro", new Vector2(50, 750));
                                        break;
                                    case 2:
                                        spawnEnemy("pyro", new Vector2(1050, 50));
                                        break;
                                    case 3:
                                        spawnEnemy("pyro", new Vector2(1050, 750));
                                        break;
                                }
                                break;
                            case "multiDirectional":
                                foreach (Vector2 Y in multiDirecitonal)
                                    createSourceProjectile(Y, new Vector2(s.getPosition().X - 70, s.getPosition().Y), 5);
                                break;

                        }
                    }

                s.changeDirection(player.getPosition(), s.getPosition());
                

                enemyCount++;
                s.Update(gameTime, Game.Window.ClientBounds);

            }


                for (int k = enemyRemoveList.Count; k > 0; k--)
                    enemyList.RemoveAt(enemyRemoveList.ElementAt(k - 1));
            enemyRemoveList.Clear();
            enemyCount = 0;


            foreach (Enemy x in spawnList)
                enemyList.Add(x);

            spawnList.Clear();
            
            

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (allowFire == true)
                {
                    createProjectile("", new Vector2(1, 1));
                    timer.Stop();
                    timer.Start();
                }
            }

            foreach (Projectile p in projectileList)
            {
                foreach (Enemy s in enemyList)
                {
                    if (p.Player_Projectile && p.collisionRect.Intersects(s.collisionRect) && !enemyRemoveList.Contains(enemyCount))
                    {
                        s.applyDamage(p.Projectile_Damage);
                        if (s.health_ <= 0)
                        {
                            enemyRemoveList.Add(enemyCount);
                            projectileRemoveList.Add(projectileCount);
                        }
                        else
                            projectileRemoveList.Add(projectileCount);
                    }
                    enemyCount++;
                }

                if (p.Player_Projectile == false && p.collisionRect.Intersects(player.collisionRect))
                {
                    player.applyDamage(p.Projectile_Damage);
                    projectileRemoveList.Add(projectileCount);
                }


                //Instead of < 0 we need to use projectile size * -1 (ex projectile size of 75 would be a x < -75)
                if (p.getPosition().X > 1100 || p.getPosition().X < 0)
                {
                    projectileRemoveList.Add(projectileCount);
                }
                else if (p.getPosition().Y > 900 || p.getPosition().Y < 0)
                {
                    projectileRemoveList.Add(projectileCount);
                }

                enemyCount = 0;
                projectileCount++;

                p.Update(gameTime, Game.Window.ClientBounds);

                //add section to see if projectile collids with enemies
            }


            try
            {
                for (int k = projectileRemoveList.Count; k > 0; k--)
                    projectileList.RemoveAt(projectileRemoveList.ElementAt(k - 1));
            }
            catch
            {

            }

            for (int k = enemyRemoveList.Count; k > 0; k--)
                enemyList.RemoveAt(enemyRemoveList.ElementAt(k - 1));

            

            if (player.health_ <= 0)
                Game.Exit();
            
            //Player in next level door
            if (player.getPosition().X >= 515 && player.getPosition().X <= 560 && player.getPosition().Y <= 47)
            {
                bool nullList = false;
                if (enemyList.Count == 0)
                {
                    levelClear += 1;
                    player.setPosition(524, 1050);
                    generateFloor();
                    //clear level. Add 1 to floor. generate new floor.
                }
                else
                {
                    foreach (Enemy e in enemyList)
                    {
                        if (e == null)
                        {
                            nullList = true;
                        }
                        else
                        {
                            nullList = false;
                            break;
                        }
                    }

                    if (nullList == true)
                    {
                        Game.Exit();
                    }
                }
            }


            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);

            string floorString = "Floor: " + levelClear;
            spriteBatch.DrawString(font, floorString,
                new Vector2(15, 10), Color.Red);

            string healthString = "HP: " + player.health_;
            spriteBatch.DrawString(font, healthString,
                new Vector2(15, 30), Color.Red);



            foreach (Sprite p in projectileList)
                p.Draw(gameTime, spriteBatch);


            foreach (Enemy s in enemyList)
            {
                if (s.GetType() != typeof(TheSource))
                {
                    s.Draw(gameTime, spriteBatch);
                }
            }


            foreach (Enemy s in enemyList)
            {
                if (s.GetType() == typeof(TheSource))
                {
                    s.Draw(gameTime, spriteBatch);
                }
            }


            // Draw the player
            player.Draw(gameTime, spriteBatch);
            crossHair.Draw(gameTime, spriteBatch);
            

            if (bossFight == true)
            {
                bool bossDestroyed = true;

                foreach (Enemy s in enemyList)
                {
                    if (s.GetType() == typeof(TheSource))
                    {
                        bossDestroyed = false;
                        break;
                    }
                }

                if (bossDestroyed == true)
                {
                    Game1.currentGameState = Game1.GameState.Continue;
                    bossFight = false;
                }
            }


            
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void LaunchTimer(object source, ElapsedEventArgs e)
        {
            timer.Stop();
            timer = new Timer(1000);
            timer.Elapsed += new ElapsedEventHandler(EnableFire);
            timer.Start();
        }

        public void EnableFire(object source, ElapsedEventArgs e)
        {
            allowFire = true;
        }

        public void ChangeWeapon()
        {
            //Timer length from parameter weapon type
            timer.Stop();
            timer = new Timer(350);
            timer.Elapsed += new ElapsedEventHandler(EnableFire);
            timer.Start();
        }

        public void createProjectile(String projectileType, Vector2 position)
        {
            allowFire = false;


            Vector2 velocity = Vector2.Subtract(new Vector2(crossHair.getPosition().X + 38, crossHair.getPosition().Y + 38), new Vector2(player.getPosition().X + 25, player.getPosition().Y + 25));


            velocity.Normalize();

            //implement projectile type later
            projectileList.Add(new Projectile(
                Game.Content.Load<Texture2D>(@"Images/WaterBall_Small"),
                new Vector2(player.getPosition().X + 20, player.getPosition().Y + 25), new Point(15, 15), 0, new Point(0, 0),
                new Point(4, 1), velocity * 5, 200, true,2));
        }

        public void createEnemyProjectile(String projectileType, Vector2 position)
        {
            Vector2 velocity = Vector2.Subtract(player.getPosition(), position);
            velocity.Normalize();
            projectileList.Add(new Projectile(
                Game.Content.Load<Texture2D>(@"Images/FireBall_Small"),
                position, new Point(15, 15), 0, new Point(0, 0),
                new Point(2, 1), velocity * 5, 200, false,1));
        }

        public void createSourceProjectile(Vector2 target, Vector2 position, byte speed)
        {
            Vector2 midSpawn = position;
            midSpawn.X += 150;
            midSpawn.Y += 150;

            Vector2 velocity = Vector2.Subtract(target, midSpawn);
            velocity.Normalize();
            projectileList.Add(new Projectile(
                Game.Content.Load<Texture2D>(@"Images/FireBall_Small"),
                midSpawn, new Point(15, 15), 0, new Point(0, 0),
                new Point(2, 1), velocity * speed, 200, false, 1));
        }

        public void spawnEnemy(String enemyType, Vector2 position)
        {
            
            switch(enemyType)
            {
                case "evilcat":
                    spawnList.Add(new EvilCat(Game.Content.Load<Texture2D>(@"Images/cat_sprite"),
                        position, new Point(60, 40), 0, new Point(0, 0),
                        new Point(2, 1), new Vector2(6, 6), 200));
                    break;
                case "smoke":
                    Vector2 midSpawn = position;
                    midSpawn.X += 150;
                    midSpawn.Y += 150;
                    spawnList.Add(new EvilSmoke(Game.Content.Load<Texture2D>(@"Images/smoke_Sprite"),
                        midSpawn, new Point(60, 60), 0, new Point(0, 0),
                        new Point(2, 1), new Vector2(6, 6), 200));
                    break;
                case "pyro":
                    spawnList.Add(new Pyro(Game.Content.Load<Texture2D>(@"Images/pyro_sprite"),
                        position, new Point(60, 60), 0, new Point(0, 0),
                        new Point(2, 1), new Vector2(6, 6), 200));
                    break; 
                
                case "fire":
                    Boolean allowFireSpawn = true;
                    foreach (Enemy s in enemyList)
                    {
                        if (s.getPosition() != position && position.Y >= 90 && position.Y <= 740 && position.X >= 0 && position.X <= 1050)
                            continue;
                        else
                        {
                            allowFireSpawn = false;
                            break;
                        }

                    }
                    if(allowFireSpawn)
                        spawnList.Add(new Fire(Game.Content.Load<Texture2D>(@"Images/fire2"),
                        position, new Point(50, 50), 0, new Point(0, 0),
                        new Point(4, 1), new Vector2(4, 4), 200));
                    break;
            }
        }

        public void generateFloor()
        {
            int topProb = levelClear;
            //Limit liklihood to 99%
            if (levelClear >= 99)
            {
                topProb = 297;
            }


            projectileList.Clear();

            //If boss spawns
            if (random.Next(1, 101) <= topProb / 3)
            {
                bossFight = true;


                Enemy fire = new Fire(
                    //Sprite
                                Game.Content.Load<Texture2D>(@"Images/fire2"),
                    //Location
                                new Vector2(fiftyW[9], fiftyT[4]),
                    //Frame size
                                new Point(50, 50),
                    //Collision Offset
                                0,
                    //Current frame
                                new Point(0, 0),
                    //Sheet size
                                new Point(4, 1),
                    //Speed
                                new Vector2(0, 0),
                    //Millisecond per frame
                                200);
                enemyList.Add(fire);

                fire = new Fire(
                    //Sprite
                                Game.Content.Load<Texture2D>(@"Images/fire2"),
                    //Location
                                new Vector2(fiftyW[9], fiftyT[8]),
                    //Frame size
                                new Point(50, 50),
                    //Collision Offset
                                0,
                    //Current frame
                                new Point(0, 0),
                    //Sheet size
                                new Point(4, 1),
                    //Speed
                                new Vector2(0, 0),
                    //Millisecond per frame
                                200);
                enemyList.Add(fire);

                fire = new Fire(
                    //Sprite
                                Game.Content.Load<Texture2D>(@"Images/fire2"),
                    //Location
                                new Vector2(fiftyW[12], fiftyT[8]),
                    //Frame size
                                new Point(50, 50),
                    //Collision Offset
                                0,
                    //Current frame
                                new Point(0, 0),
                    //Sheet size
                                new Point(4, 1),
                    //Speed
                                new Vector2(0, 0),
                    //Millisecond per frame
                                200);
                enemyList.Add(fire);

                fire = new Fire(
                    //Sprite
                                Game.Content.Load<Texture2D>(@"Images/fire2"),
                    //Location
                                new Vector2(fiftyW[12], fiftyT[4]),
                    //Frame size
                                new Point(50, 50),
                    //Collision Offset
                                0,
                    //Current frame
                                new Point(0, 0),
                    //Sheet size
                                new Point(4, 1),
                    //Speed
                                new Vector2(0, 0),
                    //Millisecond per frame
                                200);
                enemyList.Add(fire);


                TheSource source = new TheSource(
                Game.Content.Load<Texture2D>(@"Images/source_sprite"),
                new Vector2(462, 256), new Point(175, 287), 0, new Point(0, 0),
                new Point(2, 1), new Vector2(0, 0), 200);

                enemyList.Add(source);
            }
            else if (levelClear == 1)
            {
                for (int i = 0; i < 2; i++)
                {
                    int xcoord = 0;
                    int ycoord = 0;

                    do
                    {
                        xcoord = fiftyW[random.Next(0, fiftyW.Count())];
                        ycoord = fiftyT[random.Next(0, fiftyT.Count())];
                    } while (xcoord > 299 && xcoord < 749 && ycoord > 599); 

                    Enemy fire = new Fire(
                        //Sprite
                            Game.Content.Load<Texture2D>(@"Images/fire2"),
                        //Location
                            new Vector2(xcoord, ycoord),
                        //Frame size
                            new Point(50, 50),
                        //Collision Offset
                            0,
                        //Current frame
                            new Point(0, 0),
                        //Sheet size
                            new Point(4, 1),
                        //Speed
                            new Vector2(0, 0),
                        //Millisecond per frame
                            200);

                    enemyList.Add(fire);
                }
            }
            //Spawn standard enemies
            else
            {
                //Permanent Spawn of fire (every floor)
                if (levelClear == levelClear)
                {
                    int xcoord = 0;
                    int ycoord = 0;

                    do
                    {
                        xcoord = fiftyW[random.Next(0, fiftyW.Count())];
                        ycoord = fiftyT[random.Next(0, fiftyT.Count())];
                    } while (xcoord > 299 && xcoord < 749 && ycoord > 599); 

                    //Randomize stats based on level
                    //Limit location to a radius from user
                    Enemy fire = new Fire(
                        //Sprite
                        Game.Content.Load<Texture2D>(@"Images/fire2"),
                        //Location
                        new Vector2(xcoord, ycoord),
                        //Frame size
                        new Point(50, 50),
                        //Collision Offset
                        0,
                        //Current frame
                        new Point(0, 0),
                        //Sheet size
                        new Point(4, 1),
                        //Speed
                        new Vector2(0, 0),
                        //Millisecond per frame
                        200);

                    enemyList.Add(fire);
                }


                //Probably of smoke enemy spawning
                if (random.Next(1, 101) <= (levelClear * 2) && levelClear > 3)
                {
                    //Number of enemies that can spawn
                    //random.Next(1, max-1 for given floor) --- add +1 to achieve target max
                    int currentRandom = random.Next(1, (int)(2.0 * levelClear / 50) + 2 + 1);
                    for (int i = 0; i < currentRandom; i++)
                    {
                        int xcoord = 0;
                        int ycoord = 0;

                        do
                        {
                            xcoord = random.Next(0, 1099);
                            ycoord = random.Next(0, 799);
                        } while (xcoord > 324 && xcoord < 774 && ycoord > 600);

                        //Randomize stats based on level
                        //Limit location to a radius from user
                        Enemy smoke = new EvilSmoke(
                            //Sprite
                            Game.Content.Load<Texture2D>(@"Images/smoke_sprite"),
                            //Location
                            new Vector2(random.Next(0, 1099), random.Next(0, 799)),
                            //Frame size
                            new Point(60, 60),
                            //Collision Offset
                            0,
                            //Current frame
                            new Point(0, 0),
                            //Sheet size
                            new Point(2, 1),
                            //Speed
                            new Vector2(0, 0),
                            //Millisecond per frame
                            200);

                        enemyList.Add(smoke);
                    }
                }

                //Spawn enemies; add to array


                //Probably of fire enemy spawning
                if (random.Next(1, 101) <= ((double)30 * (double)levelClear / (double)100) + 60)
                {
                    //Number of enemies that can spawn
                    //random.Next(1, max for given floor)
                    int currentRandom = random.Next(1, (int)(6 * levelClear / 80) + 2 + 1);
                    System.Diagnostics.Debug.WriteLine(currentRandom);
                    for (int i = 0; i < currentRandom; i++)
                    {
                        int xcoord = 0;
                        int ycoord = 0;

                        do
                        {
                            xcoord = fiftyW[random.Next(0, fiftyW.Count())];
                            ycoord = fiftyT[random.Next(0, fiftyT.Count())];
                        } while (xcoord > 299 && xcoord < 749 && ycoord > 599); 

                        //Randomize stats based on level
                        //Limit location to a radius from user
                        Enemy fire = new Fire(
                            //Sprite
                            Game.Content.Load<Texture2D>(@"Images/fire2"),
                            //Location
                            new Vector2(xcoord, ycoord),
                            //Frame size
                            new Point(50, 50),
                            //Collision Offset
                            0,
                            //Current frame
                            new Point(0, 0),
                            //Sheet size
                            new Point(4, 1),
                            //Speed
                            new Vector2(0, 0),
                            //Millisecond per frame
                            200);

                        enemyList.Add(fire);
                    }

                    //Spawn enemies; add to array
                }


                //Probably of debris spawning
                if (random.Next(1, 11) <= 15)
                {
                    //Number of enemies that can spawn
                    //random.Next(1, max for given floor)
                    //for (int i = 0; i < random.Next(1, 10); i++)
                    //{
                    //    //Randomize stats based on level
                    //    //Limit location to a radius from user
                    //    Enemy smoke = new EvilSmoke(
                    //        //Sprite
                    //        Game.Content.Load<Texture2D>(@"Images/enemy"),
                    //        //Location
                    //        new Vector2(random.Next(0, 1099), random.Next(0, 799)),
                    //        //Frame size
                    //        new Point(75, 75),
                    //        //Collision Offset
                    //        10,
                    //        //Current frame
                    //        new Point(0, 0),
                    //        //Sheet size
                    //        new Point(2, 1),
                    //        //Speed
                    //        new Vector2(0, 0),
                    //        //Millisecond per frame
                    //        200);

                    //    enemyList.Add(smoke);
                    //}

                    //Spawn enemies; add to array
                }


                //Probably of cat lady enemy spawning
                //Cats need to be consequential of cat lady
                if (random.Next(1, 101) <= levelClear && levelClear > 10)
                {


                    //Number of enemies that can spawn
                    //random.Next(1, max for given floor)
                    int currentRandom = random.Next(1, (int)(3 * levelClear / 100) + 1);
                    for (int i = 0; i < currentRandom; i++)
                    {
                        int xcoord = 0;
                        int ycoord = 0;

                        do
                        {
                            xcoord = random.Next(0, 1099);
                            ycoord = random.Next(0, 799);
                        } while (xcoord > 324 && xcoord < 774 && ycoord > 600); 

                        //Randomize stats based on level
                        //Limit location to a radius from user
                        SpawnerEnemy catlady = new EvilCatLady(
                            //Sprite
                            Game.Content.Load<Texture2D>(@"Images/cat_lady_sprite"),
                            //Location
                            new Vector2(xcoord, ycoord),
                            //Frame size
                            new Point(60, 60),
                            //Collision Offset
                            0,
                            //Current frame
                            new Point(0, 0),
                            //Sheet size
                            new Point(2, 1),
                            //Speed
                            new Vector2(0, 0),
                            //Millisecond per frame
                            200);

                        enemyList.Add(catlady);
                    }

                    //Spawn enemies; add to array
                }


                //Probably of pyro enemy spawning
                if (random.Next(1, 101) <= levelClear && levelClear > 5)
                {
                    //Number of enemies that can spawn
                    //random.Next(1, max for given floor)
                    int currentRandom = random.Next(1, (int)(7 * levelClear / 75) + 3);
                    for (int i = 0; i < currentRandom; i++)
                    {
                        int xcoord = 0;
                        int ycoord = 0;

                        do
                        {
                            xcoord = random.Next(0, 1099);
                            ycoord = random.Next(0, 799);
                        } while (xcoord > 324 && xcoord < 774 && ycoord > 600); 

                        //Randomize stats based on level
                        //Limit location to a radius from user
                        ShootingEnemy pyro = new Pyro(
                            //Sprite
                            Game.Content.Load<Texture2D>(@"Images/pyro_sprite"),
                            //Location
                            new Vector2(xcoord, ycoord),
                            //Frame size
                            new Point(60, 60),
                            //Collision Offset
                            0,
                            //Current frame
                            new Point(0, 0),
                            //Sheet size
                            new Point(2, 1),
                            //Speed
                            new Vector2(0, 0),
                            //Millisecond per frame
                            200);

                        enemyList.Add(pyro);
                    }

                    //Spawn enemies; add to array
                }
            }
        }

    }
}
