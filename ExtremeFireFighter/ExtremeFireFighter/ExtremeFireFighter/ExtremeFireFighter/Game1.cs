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

namespace ExtremeFireFighter
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteManager spriteManager;
        SpriteFont font;
        KeyboardState oldState;
        MouseState mouseState;
        MouseState previousMouseState;
        Texture2D backGround;
        Rectangle mainFrame;

        private Texture2D startButton;
        private Texture2D continueButton;
        private Texture2D exitButton;
        private Texture2D resumeButton;
        private Texture2D logo;
        private Texture2D logo2;

        private Vector2 startButtonPosition;
        private Vector2 continueButtonPosition;
        private Vector2 exitButtonPosition;
        private Vector2 resumeButtonPosition;
        private Vector2 logoPosition;

        //Game States
        public enum GameState { Start, Paused, Playing, Continue };
        public static GameState currentGameState = GameState.Start;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 1100;
        }

        protected override void Initialize()
        {


            //Components.Add(spriteManager);

            //whether update method is called
            //spriteManager.Enabled = false;
            oldState = Keyboard.GetState();
            mouseState = Mouse.GetState();
            previousMouseState = mouseState;

            //logo
            logoPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 250, 50);

            //button positions
            startButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 300);
            continueButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 300);
            exitButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 350);
            resumeButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 300);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //load font
            font = Content.Load<SpriteFont>("myFont");

            graphics.ApplyChanges();
            IsMouseVisible = true;
            //load logo
            logo = Content.Load<Texture2D>(@"Images/logo");
            logo2 = Content.Load<Texture2D>(@"Images/logo_finish");

            //load buttons
            startButton = Content.Load<Texture2D>(@"Images/start");
            continueButton = Content.Load<Texture2D>(@"Images/continue");
            exitButton = Content.Load<Texture2D>(@"Images/exit");
            resumeButton = Content.Load<Texture2D>(@"Images/resume");

            backGround = Content.Load<Texture2D>(@"Images/room_empty");
            mainFrame = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            var newState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            //certain activities for each gamestate
            switch (currentGameState)
            {
                case GameState.Start:
                    if (previousMouseState.LeftButton == ButtonState.Pressed &&
                        mouseState.LeftButton == ButtonState.Released)
                    {
                        MouseClick(mouseState.X, mouseState.Y);
                    }

                    previousMouseState = mouseState;

                    break;
                case GameState.Continue:
                    spriteManager.Visible = false;
                    spriteManager.Enabled = false;

                    if (previousMouseState.LeftButton == ButtonState.Pressed &&
                        mouseState.LeftButton == ButtonState.Released)
                    {
                        MouseClick(mouseState.X, mouseState.Y);
                    }

                    previousMouseState = mouseState;

                    break;
                case GameState.Paused:

                    //for  button
                    if (previousMouseState.LeftButton == ButtonState.Pressed &&
                        mouseState.LeftButton == ButtonState.Released)
                    {
                        MouseClick(mouseState.X, mouseState.Y);
                    }

                    previousMouseState = mouseState;
                    break;

                case GameState.Playing:
                    if (newState.IsKeyDown(Keys.Escape) && !oldState.IsKeyDown(Keys.Escape))
                    {
                        //player pressed key
                        currentGameState = GameState.Paused;
                        spriteManager.Visible = false;
                        spriteManager.Enabled = false;


                    }
                    else if (newState.IsKeyDown(Keys.Escape) && oldState.IsKeyDown(Keys.Escape))
                    {
                        //player is holding key
                    }
                    else if (!newState.IsKeyDown(Keys.Escape) && oldState.IsKeyDown(Keys.Escape))
                    {
                        //player let key go
                        currentGameState = GameState.Paused;
                        spriteManager.Visible = false;
                        spriteManager.Enabled = false;
                    }
                    break;
            }

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            switch (currentGameState)
            {
                case GameState.Start:

                    GraphicsDevice.Clear(Color.White);
                    //Begin Game
                    spriteBatch.Begin();
                    spriteBatch.Draw(backGround, mainFrame, Color.Black);
                    spriteBatch.Draw(logo, logoPosition, Color.White);
                    spriteBatch.Draw(startButton, startButtonPosition, Color.White);
                    spriteBatch.Draw(exitButton, exitButtonPosition, Color.White);

                    spriteBatch.End();
                    break;

                case GameState.Continue:

                    GraphicsDevice.Clear(Color.White);
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                    IsMouseVisible = true;
                    spriteBatch.Draw(backGround, mainFrame, Color.Black);
                    spriteBatch.Draw(logo2, logoPosition, Color.White);
                    //draw resume button
                    spriteBatch.Draw(continueButton, continueButtonPosition, Color.White);
                    //draw exit button
                    spriteBatch.Draw(exitButton, exitButtonPosition, Color.White);

                    spriteBatch.End();
                    break;

                case GameState.Playing:
                    GraphicsDevice.Clear(Color.White);
                    spriteBatch.Begin();
                    spriteBatch.Draw(backGround, mainFrame, Color.White);
                    spriteManager.Visible = true;
                    spriteManager.Enabled = true;
                    IsMouseVisible = false;

                    spriteBatch.End();
                    break;

                case GameState.Paused:
                    GraphicsDevice.Clear(Color.White);
                    spriteBatch.Begin();
                    IsMouseVisible = true;
                    spriteBatch.Draw(backGround, mainFrame, Color.White);
                    //draw resume button
                    spriteBatch.Draw(resumeButton, resumeButtonPosition, Color.White);
                    //draw exit button
                    spriteBatch.Draw(exitButton, exitButtonPosition, Color.White);

                    spriteBatch.End();
                    break;
            }

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }


        void MouseClick(int x, int y)
        {
            //10 x 10 rectangle for mouse
            Rectangle mouseRect = new Rectangle(x, y, 10, 10);

            Rectangle exitButtonRectangle = new Rectangle((int)exitButtonPosition.X,
                    (int)exitButtonPosition.Y, 100, 25);

            if (mouseRect.Intersects(exitButtonRectangle))
            {
                this.Exit();
            }

            if (currentGameState == GameState.Start)
            {
                Rectangle startButtonRectangle = new Rectangle((int)startButtonPosition.X,
                    (int)startButtonPosition.Y, 100, 25);

                if (mouseRect.Intersects(startButtonRectangle))
                {
                    spriteManager = new SpriteManager(this);
                    currentGameState = GameState.Playing;
                }

                if (!Components.Contains(spriteManager))
                {
                    try
                    {
                        Components.Add(spriteManager);
                    }
                    catch (Exception exception) { }
                 }
           }

                else if (currentGameState == GameState.Continue)
                {
                    Rectangle continueButtonRectangle = new Rectangle((int)continueButtonPosition.X,
                        (int)continueButtonPosition.Y, 100, 25);

                    if (mouseRect.Intersects(continueButtonRectangle))
                    {
                        currentGameState = GameState.Playing;
                    }


                }
                else if (currentGameState == GameState.Paused)
                {
                    Rectangle resumeButtonRectangle = new Rectangle((int)resumeButtonPosition.X,
                       (int)resumeButtonPosition.Y, 100, 25);
                    if (mouseRect.Intersects(resumeButtonRectangle))
                    {
                        currentGameState = GameState.Playing;
                    }
                }
            }
        }
    }
