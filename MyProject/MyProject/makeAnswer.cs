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

namespace MyProject
{
    class makeAnswer
    {
        string solution, userAnswer;
        int count = 0;

        public void Initialize()
        {
            solution = "";
            userAnswer = "";
        }
        public void setSolution(string sol) 
        {
            solution = sol;
        }

        public void addLetter(string letter)
        {
            userAnswer = String.Concat(userAnswer,letter);
        }

        public void clearAnswer()
        {
            userAnswer = "";
        }

        public int compare()
        {
            return String.Compare(solution, userAnswer);
        }
        public void LoadContent(ContentManager Content)
        { }
        public void Update(GameTime gameTime)
        { }
        public void Draw(SpriteBatch sp)
        { }
    }
}
