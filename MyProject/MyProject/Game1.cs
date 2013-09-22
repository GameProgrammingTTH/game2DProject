using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace MyProject
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        enum GameState
        {
            StartMenu,
            Loading,
            HowToPlay,
            Playing,
            Paused,
            NextLevel,
            GameOver
        }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D resumeButton,pauseButton,howToPlayButton,exitButton,startButton;
        
        Texture2D loadingScreen, nextLevelScreen, gameOverScreen;
        Texture2D gotoMainMenu;
        Vector2 startButtonPosition;
        Vector2 howToPlayPosition;
        Vector2 exitButtonPosition;
        Vector2 resumeButtonPosition;
        Vector2 gotoMainMenuPosition;

        int Score = 0, level = 1;

        SpriteFont fontScore, fontLevel;
        
        GameState gameState;
        Thread backgroundThread, nextLevelThread;
        bool isLoading = false;
        Level1 lv1;

        MouseState mouseState;
        MouseState previousMouseState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            //enable the mousepointer
            IsMouseVisible = true;
            this.Window.Title = "English Game 2d";
            graphics.PreferredBackBufferWidth = 900;
            graphics.PreferredBackBufferHeight = 680;
            graphics.ApplyChanges();
            
            //set the gamestate to start menu
            gameState = GameState.StartMenu;

            //get the mouse state
            mouseState = Mouse.GetState();
            previousMouseState = mouseState;

            
            

            lv1 = new Level1();

            base.Initialize();
        }

        
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            //score and level
            fontScore = Content.Load<SpriteFont>("Arial");
            fontLevel = Content.Load<SpriteFont>("Arial");
            //load the buttonimages into the content pipeline
            startButton = Content.Load<Texture2D>("start");
            exitButton = Content.Load<Texture2D>("exit");
            howToPlayButton = Content.Load<Texture2D>("howToPlay");
            ////set the position of the buttons
            startButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - (startButton.Width / 2),
                                               (GraphicsDevice.Viewport.Height / 2) - (startButton.Height / 2) - 100);
            howToPlayPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - (howToPlayButton.Width / 2),
                                               (GraphicsDevice.Viewport.Height / 2) - (howToPlayButton.Height / 2));
            exitButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - (exitButton.Width / 2),
                                               (GraphicsDevice.Viewport.Height / 2) - (exitButton.Height / 2) + 100);

            Console.WriteLine(startButton.Name);
            Debug.WriteLine(startButton.Name);

            //load the loading screen
            loadingScreen = Content.Load<Texture2D>("loading");
            gameOverScreen = Content.Load<Texture2D>("gameOver");
            nextLevelScreen = Content.Load<Texture2D>("nextLevel");
            lv1.LoadContent(Content);
            
        }

        
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            //load the game when needed
            if (gameState == GameState.Loading && !isLoading) //isLoading bool is to prevent the LoadGame method from being called 60 times a seconds
            {
                //set backgroundthread
                backgroundThread = new Thread(LoadGame);
                isLoading = true;

                //start backgroundthread
                backgroundThread.Start();
            }
            if (gameState == GameState.NextLevel)
            {
                nextLevelThread = new Thread(NextLevelFunc);
                nextLevelThread.Start();
                
            }

            //move the orb if the game is in progress
            if (gameState == GameState.Playing)
            {
                lv1.Update(gameTime);
            }

            //wait for mouseclick
            mouseState = Mouse.GetState();
            if (previousMouseState.LeftButton == ButtonState.Pressed &&
                mouseState.LeftButton == ButtonState.Released)
            {
                MouseClicked(mouseState.X, mouseState.Y);
            }

            previousMouseState = mouseState;

            if (gameState == GameState.Playing && isLoading)
            {
                LoadGame();
                isLoading = false;
            }
            base.Update(gameTime);
        }

       
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            //draw the start menu
            if (gameState == GameState.StartMenu)
            {
                spriteBatch.Draw(startButton, startButtonPosition, Color.White);
                spriteBatch.Draw(howToPlayButton, howToPlayPosition, Color.White);
                spriteBatch.Draw(exitButton, exitButtonPosition, Color.White);
            }

            //show the loading screen when needed
            if (gameState == GameState.Loading)
            {
                spriteBatch.Draw(loadingScreen, new Vector2((GraphicsDevice.Viewport.Width / 2) - (loadingScreen.Width / 2), (GraphicsDevice.Viewport.Height / 2) - (loadingScreen.Height / 2)), Color.YellowGreen);
                
            }
            //show next level screen

            if (gameState == GameState.NextLevel)
            {
                spriteBatch.Draw(nextLevelScreen, Vector2.Zero, Color.White);
            }
            //show gameOver screen
            if (gameState == GameState.GameOver)
            {
                spriteBatch.Draw(gameOverScreen, Vector2.Zero, Color.White);
                spriteBatch.Draw(gotoMainMenu, new Vector2(380,500), Color.White);
            }
            //draw the the game when playing
            if (gameState == GameState.Playing)
            {
                if (level == 1)
                {

                    lv1.Draw(spriteBatch);
                    if (lv1.life == 0)
                    {
                        gameState = GameState.GameOver;
                        lv1.life = 3;
                    }
                    if(lv1.score != 0)
                    {
                        Score += lv1.score;
                        lv1.score = 0;
                        // goto next lv
                        level++;
                        gameState = GameState.NextLevel;
                    }
                }

                
                //menu top
                spriteBatch.Draw(pauseButton, new Vector2(0, 0), Color.White);
                spriteBatch.DrawString(fontScore, "Score:  " + Score, new Vector2(750, 5), Color.Blue);
                spriteBatch.DrawString(fontLevel, "Level:  " + level, new Vector2(750, 50), Color.Red);
                
                
            }

            //draw the pause screen
            if (gameState == GameState.Paused)
            {
                spriteBatch.Draw(gotoMainMenu, gotoMainMenuPosition, Color.White);
                spriteBatch.Draw(resumeButton, resumeButtonPosition, Color.White);
                spriteBatch.Draw(exitButton, exitButtonPosition, Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        void MouseClicked(int x, int y)
        {
            //creates a rectangle of 10x10 around the place where the mouse was clicked
            Rectangle mouseClickRect = new Rectangle(x, y, 10, 10);

            //check the startmenu
            if (gameState == GameState.StartMenu)
            {
                Rectangle startButtonRect = new Rectangle((int)startButtonPosition.X, (int)startButtonPosition.Y, 163, 40);
                Rectangle exitButtonRect = new Rectangle((int)exitButtonPosition.X, (int)exitButtonPosition.Y, 163, 40);
                Rectangle howToPlayButtonRect = new Rectangle((int)howToPlayPosition.X, (int)howToPlayPosition.Y, 163, 40);

                if (mouseClickRect.Intersects(startButtonRect)) //player clicked start button
                {
                    gameState = GameState.Loading;
                    isLoading = false;
                }
                else if (mouseClickRect.Intersects(exitButtonRect)) //player clicked exit button
                {
                    Exit();
                }
                else if (mouseClickRect.Intersects(howToPlayButtonRect)) // player clicked howtoPlay button
                { 
                
                }
            }

            //check the pausebutton
            if (gameState == GameState.Playing)
            {
                Rectangle pauseButtonRect = new Rectangle(0, 0, 163, 40);

                if (mouseClickRect.Intersects(pauseButtonRect))
                {
                    gameState = GameState.Paused;
                }
            }
            // check the game over
            if (gameState == GameState.GameOver)
            { 
                Rectangle gotoMainmenuRect = new Rectangle(380,500, 163, 40);
                if (mouseClickRect.Intersects(gotoMainmenuRect))
                {
                    gameState = GameState.StartMenu;
                    Score = 0;
                    level = 1;
                    
                    clearAllLv();
                }
            }
            //check the resumebutton
            if (gameState == GameState.Paused)
            {
                Rectangle gotoMainmenuRect = new Rectangle((int)gotoMainMenuPosition.X, (int)gotoMainMenuPosition.Y, 163, 40);
                Rectangle resumeButtonRect = new Rectangle((int)resumeButtonPosition.X, (int)resumeButtonPosition.Y, 163, 40);
                Rectangle exitButtonRect = new Rectangle((int)exitButtonPosition.X, (int)exitButtonPosition.Y, 163, 40);
                if (mouseClickRect.Intersects(resumeButtonRect))
                {
                    gameState = GameState.Playing;
                }
                else if (mouseClickRect.Intersects(exitButtonRect))
                {
                    this.Exit();
                }
                
                else if (mouseClickRect.Intersects(gotoMainmenuRect))
                {
                    gameState = GameState.StartMenu;
                    Score = 0;
                    level = 1;
                    clearAllLv();
                }
            }
        }

        void LoadGame()
        {
            //load the game images into the content pipeline

            gotoMainMenu = Content.Load<Texture2D>("menu");
            pauseButton = Content.Load<Texture2D>("pause");
            resumeButton = Content.Load<Texture2D>("resume");
            gotoMainMenuPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - (gotoMainMenu.Width / 2),
                                               (GraphicsDevice.Viewport.Height / 2) - (gotoMainMenu.Height / 2) - 100);
            resumeButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - (resumeButton.Width / 2),
                                               (GraphicsDevice.Viewport.Height / 2) - (resumeButton.Height / 2));
            exitButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - (resumeButton.Width / 2),
                                               (GraphicsDevice.Viewport.Height / 2) - (resumeButton.Height / 2) + 100); 

            
            //since this will go to fast for this demo's purpose, wait for 3 seconds
            Thread.Sleep(3000);

            //start playing
            gameState = GameState.Playing;
            isLoading = false;
        }
        public void clearAllLv()
        {
            lv1.clear();
        }
        public void NextLevelFunc()
        {
            Thread.Sleep(3000);
            gameState = GameState.Playing;
        }
    }
}
