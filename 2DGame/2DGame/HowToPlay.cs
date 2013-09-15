using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _2DGame
{
    class HowToPlay
    {
        SpriteFont font;
        String gameTitle = "Project Game 2D";
        public void LoadContent(ContentManager Content)
        {
            font = Content.Load<SpriteFont>("Fonts\\Arial");
        }
        public void Update(GameTime gameTime)
        {
            
        }
        public void Draw(SpriteBatch sp)
        {
            
            sp.DrawString(font, gameTitle, new Vector2(300, 200), Color.White);
            
        }
    }
}
