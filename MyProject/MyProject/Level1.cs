using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    class Level1
    {

        
        //UI letterHandler
        Texture2D aButton,lButton,iButton,sButton,tButton,eButton,nButton;
        Vector2 aButtonPosition, lButtonPosition, iButtonPosition, sButtonPosition, tButtonPosition, eButtonPosition, nButtonPosition;
       
        //submit and clear button
        Texture2D submitButton, clearButton;
        Vector2 submitButtonPosition, clearButtonPosition;

        //font
        SpriteFont lettersText, answerText, msgText, AnnouText, lifeText;

        
        Texture2D texBg;
        loadPicture loadPicLevel1 = new loadPicture();
        int xPosition = 150, yPosition = 600;
        
        int xAnswerPosition = 150, yAnswerPosition = 500;
        // toado
        List<Vector2> toadoLetters = new List<Vector2>();
        //Thread gotoNextLevelThread;

        //double timeToNextLevel = 0;
        
        string keyword = "listen";
        string answerKey = "";

        public int score = 0, life = 3;
        string msg = "";
        int count = 0;
        MouseState mouseState;
        MouseState previousMouseState;
        public void Initialize()
        {
            mouseState = Mouse.GetState();
            previousMouseState = mouseState;
            life = 3;
            
            
        }
        
        
        public void LoadContent(ContentManager Content)
        {
            //loadText
            lettersText = Content.Load<SpriteFont>("Arial");
            answerText = Content.Load<SpriteFont>("Arial");
            AnnouText = Content.Load<SpriteFont>("smallArial");
            lifeText = Content.Load<SpriteFont>("Arial");
            msgText = Content.Load<SpriteFont>("smallArial");

            texBg = Content.Load<Texture2D>("bglv1");
            submitButton = Content.Load<Texture2D>("submit");
            clearButton = Content.Load<Texture2D>("clear");
            submitButtonPosition = new Vector2(700,510);
            clearButtonPosition = new Vector2(800, 510);
            // load pics
            loadPicLevel1 = new loadPicture();
            loadPicLevel1.Initial("level1p1", "level1p2", "level1p3", "level1p4");
            loadPicLevel1.LoadContent(Content);
            // Load letters
            toadoLetters.Add(new Vector2(xPosition, yPosition)); xPosition += 70;
            toadoLetters.Add(new Vector2(xPosition, yPosition)); xPosition += 70;
            toadoLetters.Add(new Vector2(xPosition, yPosition)); xPosition += 70;
            toadoLetters.Add(new Vector2(xPosition, yPosition)); xPosition += 70;
            toadoLetters.Add(new Vector2(xPosition, yPosition)); xPosition += 70;
            toadoLetters.Add(new Vector2(xPosition, yPosition)); xPosition += 70;
            toadoLetters.Add(new Vector2(xPosition, yPosition)); xPosition += 70;


            //--
            tButton = Content.Load<Texture2D>("letters/t");
            lButton = Content.Load<Texture2D>("letters/l");
            aButton = Content.Load<Texture2D>("letters/a");
            iButton = Content.Load<Texture2D>("letters/i");
            sButton = Content.Load<Texture2D>("letters/s");
            nButton = Content.Load<Texture2D>("letters/n");
            eButton = Content.Load<Texture2D>("letters/e");

            setLetterPosition();
            
            
        }

        public void Update(GameTime gameTime)
        {

            //wait for mouseclick
            mouseState = Mouse.GetState();
            if (previousMouseState.LeftButton == ButtonState.Pressed &&
                mouseState.LeftButton == ButtonState.Released)
            {
                MouseClicked(mouseState.X, mouseState.Y);
            }
            previousMouseState = mouseState;
            
        }

        void MouseClicked(int x, int y)
        {
            Rectangle mouseClickRect = new Rectangle(x, y, 10, 10);

            //control button
            Rectangle submitButtonRect = new Rectangle((int)submitButtonPosition.X, (int)submitButtonPosition.Y, submitButton.Width, submitButton.Height);
            Rectangle clearButtonRect = new Rectangle((int)clearButtonPosition.X, (int)clearButtonPosition.Y, clearButton.Width, clearButton.Height);
            // Rect letters
            Rectangle aButtonRect = new Rectangle((int)aButtonPosition.X, (int)aButtonPosition.Y, aButton.Width, aButton.Height);
            Rectangle lButtonRect = new Rectangle((int)lButtonPosition.X, (int)lButtonPosition.Y, lButton.Width, lButton.Height);
            Rectangle iButtonRect = new Rectangle((int)iButtonPosition.X, (int)iButtonPosition.Y, iButton.Width, iButton.Height);
            Rectangle sButtonRect = new Rectangle((int)sButtonPosition.X, (int)sButtonPosition.Y, sButton.Width, sButton.Height);
            Rectangle tButtonRect = new Rectangle((int)tButtonPosition.X, (int)tButtonPosition.Y, tButton.Width, tButton.Height);
            Rectangle eButtonRect = new Rectangle((int)eButtonPosition.X, (int)eButtonPosition.Y, eButton.Width, eButton.Height);
            Rectangle nButtonRect = new Rectangle((int)nButtonPosition.X, (int)nButtonPosition.Y, nButton.Width, nButton.Height);
            // Block rect
            Rectangle blockRect =  new Rectangle(100,500,600,70);

            if (!mouseClickRect.Intersects(blockRect))
            {

                if (mouseClickRect.Intersects(submitButtonRect)) //player clicked submit button
                {
                    // check keyword and answer
                    if (count == 6)
                    {
                        xAnswerPosition = 150;
                        count = 0;
                        if (String.Compare(keyword, answerKey) == 0)
                        {
                            
                            msg = "Congratulation!";
                            score = 60;
                            answerKey = "";
                            setLetterPosition();

                        }
                        else
                        {
                            msg = "Wrong answer!";
                            answerKey = "";
                            life--;
                            setLetterPosition();
                        }
                    }
                }
                if (mouseClickRect.Intersects(clearButtonRect)) // player clicked clear button
                {

                    clear();

                }
                if (count < 6)
                {

                    if (mouseClickRect.Intersects(aButtonRect))
                    {
                        //draw a letter and add it into answerkey
                        answerKey = String.Concat(answerKey, "a");
                        aButtonPosition.X = xAnswerPosition;
                        aButtonPosition.Y = yAnswerPosition;

                        xAnswerPosition = xAnswerPosition + 70;
                        count++;
                        setDefaultMsg();
                    }
                    if (mouseClickRect.Intersects(lButtonRect))
                    {
                        answerKey = String.Concat(answerKey, "l");
                        lButtonPosition.X = xAnswerPosition;
                        lButtonPosition.Y = yAnswerPosition;
                        setDefaultMsg();
                        xAnswerPosition = xAnswerPosition + 70;
                        count++;
                    }
                    if (mouseClickRect.Intersects(iButtonRect))
                    {
                        answerKey = String.Concat(answerKey, "i");
                        iButtonPosition.X = xAnswerPosition;
                        iButtonPosition.Y = yAnswerPosition;
                        count++;
                        setDefaultMsg();
                        xAnswerPosition = xAnswerPosition + 70;
                    }
                    if (mouseClickRect.Intersects(sButtonRect))
                    {
                        answerKey = String.Concat(answerKey, "s");
                        sButtonPosition.X = xAnswerPosition;
                        sButtonPosition.Y = yAnswerPosition;
                        count++;
                        setDefaultMsg();
                        xAnswerPosition = xAnswerPosition + 70;
                    }
                    if (mouseClickRect.Intersects(tButtonRect))
                    {
                        answerKey = String.Concat(answerKey, "t");
                        tButtonPosition.X = xAnswerPosition;
                        tButtonPosition.Y = yAnswerPosition;
                        count++;
                        setDefaultMsg();
                        xAnswerPosition = xAnswerPosition + 70;
                    }
                    if (mouseClickRect.Intersects(eButtonRect))
                    {
                        answerKey = String.Concat(answerKey, "e");
                        eButtonPosition.X = xAnswerPosition;
                        eButtonPosition.Y = yAnswerPosition;
                        count++;
                        setDefaultMsg();
                        xAnswerPosition = xAnswerPosition + 70;
                    }
                    if (mouseClickRect.Intersects(nButtonRect))
                    {
                        answerKey = String.Concat(answerKey, "n");
                        nButtonPosition.X = xAnswerPosition;
                        nButtonPosition.Y = yAnswerPosition;
                        count++;
                        setDefaultMsg();
                        xAnswerPosition = xAnswerPosition + 70;
                    }
                }
            }
        }

        public void setDefaultMsg()
        {
            msg = "";
        }
        public void Draw(SpriteBatch sp)
        {
            
            
            sp.Draw(texBg, Vector2.Zero, Color.White);
            sp.Draw(submitButton, submitButtonPosition, Color.White);
            sp.Draw(clearButton, clearButtonPosition, Color.White);
            loadPicLevel1.Draw(sp);
            //draw letters
            sp.Draw(aButton, aButtonPosition, Color.White);
            sp.Draw(lButton, lButtonPosition, Color.White);
            sp.Draw(tButton, tButtonPosition, Color.White);
            sp.Draw(nButton, nButtonPosition, Color.White);
            sp.Draw(iButton, iButtonPosition, Color.White);
            sp.Draw(eButton, eButtonPosition, Color.White);
            sp.Draw(sButton, sButtonPosition, Color.White);
            //draw text
            sp.DrawString(answerText, "Answer:", new Vector2(20, 510), Color.Red);
            sp.DrawString(lettersText, "Letters:", new Vector2(20, 610), Color.Red);
            sp.DrawString(AnnouText, "Hint: 6 letters", new Vector2(200, 450), Color.Blue);
            sp.DrawString(lifeText, "Life:   " + life, new Vector2(750, 100), Color.Red);
            sp.DrawString(msgText, msg, new Vector2(750, 610), Color.Green);
                
            
        }
        public void setLetterPosition()
        {
            tButtonPosition = new Vector2(toadoLetters[0].X, toadoLetters[0].Y);
            lButtonPosition = new Vector2(toadoLetters[1].X, toadoLetters[1].Y);
            aButtonPosition = new Vector2(toadoLetters[2].X, toadoLetters[2].Y);
            iButtonPosition = new Vector2(toadoLetters[3].X, toadoLetters[3].Y);
            sButtonPosition = new Vector2(toadoLetters[4].X, toadoLetters[4].Y);
            nButtonPosition = new Vector2(toadoLetters[5].X, toadoLetters[5].Y);
            eButtonPosition = new Vector2(toadoLetters[6].X, toadoLetters[6].Y);
        }
        public void clear()
        {
            count = 0;
            
            msg = "";
            answerKey = "";
            xAnswerPosition = 150;
            setLetterPosition();
        }
        
        
    }
}
