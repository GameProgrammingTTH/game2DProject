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
    class loadPicture
    {

        Texture2D pic1;
        Texture2D pic2;
        Texture2D pic3;
        Texture2D pic4;
        string nameStr1, nameStr2, nameStr3, nameStr4;
        public void Initial(string name1, string name2, string name3, string name4)
        {
            nameStr1 = name1;
            nameStr2 = name2;
            nameStr3 = name3;
            nameStr4 = name4;

        }

        public void LoadContent(ContentManager Content)
        {
            pic1 = Content.Load<Texture2D>(nameStr1);
            pic2 = Content.Load<Texture2D>(nameStr2);
            pic3 = Content.Load<Texture2D>(nameStr3);
            pic4 = Content.Load<Texture2D>(nameStr4);
        }
        public void Update(GameTime gameTime)
        { }
        public void Draw(SpriteBatch sp)
        {
            sp.Draw(pic1, new Vector2(200,50), Color.White);
            sp.Draw(pic2, new Vector2(450, 50), Color.White);
            sp.Draw(pic3, new Vector2(200, 250), Color.White);
            sp.Draw(pic4, new Vector2(450, 250), Color.White);

        }

    }
}
