using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace MyProject
{
    class Level1
    {
        Rectangle alphabet;
        Rectangle answer;
        Texture2D texAlphabet;
        Texture2D texAnwser;
        Texture2D texBg;

        public void LoadContent(ContentManager Content)
        {
            alphabet = new Rectangle(10, 650, 880, 20);
            answer = new Rectangle(10, 550, 800, 20);
            texAlphabet = Content.Load<Texture2D>("solidred");
            texAnwser = Content.Load<Texture2D>("solidred");
            texBg = Content.Load<Texture2D>("bglv1");
        }

        public void Update()
        {
        }

        public void Draw(SpriteBatch sp)
        {
            sp.Draw(texBg, Vector2.Zero, Color.White);
            sp.Draw(texAlphabet, alphabet, Color.White);
            sp.Draw(texAnwser, answer, Color.White);
        }
    }
}
