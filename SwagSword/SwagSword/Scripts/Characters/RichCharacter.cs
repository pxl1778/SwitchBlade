﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SwagSword
{
    class RichCharacter : Character
    {
        public RichCharacter(int x, int y, Texture2D texture, Game1 mainMan): base(x, y, texture, mainMan)
        {
            //Set config file here
            Init();
        }

        public override void Init()
        {
            Type = Faction.Rich;
            NormalColor = Color.SteelBlue;

            base.Init();
        }

        public override void Update()
        {
            base.Update();
        }
    }
}