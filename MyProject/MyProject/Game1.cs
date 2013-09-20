using System;
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
            Paused
        }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D startButton;
        Texture2D exitButton;
        Texture2D howToPlayButton;
        Texture2D pauseButton;
        Texture2D resumeButton;
        Texture2D loadingScreen;
        Texture2D gotoMainMenu;
        Vector2 startButtonPosition;
        Vector2 howToPlayPosition;
        Vector2 exitButtonPosition;
        Vector2 resumeButtonPosition;
        Vector2 gotoMainMenuPosition;

        GameState gameState;
        Thread backgroundThread;
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

            //load the loading screen
            loadingScreen = Content.Load<Texture2D>("loading");

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

            //move the orb if the game is in progress
            if (gameState == GameState.Playing)
            {
                
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

            //draw the the game when playing
            if (gameState == GameState.Playing)
            {
                lv1.Draw(spriteBatch);   
                //pause button
                spriteBatch.Draw(pauseButton, new Vector2(0, 0), Color.White);
             
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
    }
}
