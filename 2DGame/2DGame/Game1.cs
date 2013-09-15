using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace _2DGame
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        // change mouse
        Texture2D cursorTex;
        Vector2 cursorPos;
        HowToPlay howToPlay;
        enum GameState
        { 
            MainMenu,
            HowToPlay,
            Playing,
            
        }

        GameState CurrentGameState = GameState.MainMenu;
        // screen adjust
        int screenWidth = 800, screenHeight = 600;
        // some button for main menu
        cButton btnHowToPlay;
        cButton btnPlay;
        cButton btnExit;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            //this.IsMouseVisible = false;
            howToPlay = new HowToPlay();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            // screen stuff
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            //IsMouseVisible = true;
            cursorTex = Content.Load<Texture2D>("images\\cursors\\cursor1");
            howToPlay.LoadContent(Content);
            btnPlay = new cButton(Content.Load<Texture2D>("images\\button\\play"), graphics.GraphicsDevice);
            btnPlay.setPosition(new Vector2(350,200));
            btnHowToPlay = new cButton(Content.Load<Texture2D>("images\\button\\howToPlay"), graphics.GraphicsDevice);
            btnHowToPlay.setPosition(new Vector2(350, 300));
            btnExit = new cButton(Content.Load<Texture2D>("images\\button\\exit"), graphics.GraphicsDevice);
            btnExit.setPosition(new Vector2(350, 400));

        }

        protected override void UnloadContent()
        {
            
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            MouseState mouse = Mouse.GetState();
            cursorPos = new Vector2(mouse.X - 12, mouse.Y);
            switch (CurrentGameState)
            { 
                case GameState.MainMenu:
                    if (btnPlay.isClicked == true) CurrentGameState = GameState.Playing;
                    if (btnHowToPlay.isClicked == true) CurrentGameState = GameState.HowToPlay;
                    if (btnExit.isClicked == true) this.Exit();
                    btnPlay.Update(mouse);
                    btnHowToPlay.Update(mouse);
                    btnExit.Update(mouse);
                    break;
                case GameState.Playing:
                    break;
                case GameState.HowToPlay:

                    break;
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            
            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    spriteBatch.Draw(Content.Load<Texture2D>("images\\background\\bgMainMenu"), new Rectangle(0,0,screenWidth,screenHeight), Color.White);
                    btnPlay.Draw(spriteBatch);
                    btnHowToPlay.Draw(spriteBatch);
                    btnExit.Draw(spriteBatch);
                    break;
                case GameState.Playing:
                    break;
                case GameState.HowToPlay:
                    howToPlay.Draw(spriteBatch);
                    break;
            }
            spriteBatch.Draw(cursorTex, cursorPos, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
