﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SwagSword
{
    class WinScreen : UIScreen
    {
        public WinScreen(Game1 mainMan):
            base(mainMan)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(mainMan.DrawMan.WinTexture, new Rectangle(leftX + 200, topY + 300, 500, 500), Color.White);
            spriteBatch.DrawString(mainMan.DrawMan.HealthFont, "YOU WIN", new Vector2(leftX + mainMan.WindowHalfWidth - 40, topY + 100), Color.Yellow);
        }
    }
}