﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


//Names: Nelson Scott, Peter Lockhart, Ryan Bell

namespace SwagSword
{
    /// <summary>
    /// Draw Manager should call draw on all game objects, and holds textures
    /// </summary>
    public class DrawManager:Manager
    {
        #region Fields
        //Character texture lists
        private List<Texture2D> goodGuyTextures;

        //Screens Texture
        private Texture2D titleImage;
        private Texture2D gameOverImage;
        private Texture2D pauseImage;
        private Texture2D swordStatScreen;
        private Texture2D loadingScreen;

        //UI Textures (buttons... what not)
        private Texture2D pointerTexture;
        private Texture2D resumeTexture;
        private Texture2D statsTexture;
        private Texture2D exitTexture;
        private Texture2D winTexture;

        //Weapon Textures (Create a dictionary)
        private Texture2D swordTexture;
        private Texture2D whipTexture;
        private Texture2D pateTexture;

        //Map Textures
        private Texture2D pathwayTexture;
        private Texture2D sandyTexture;
        private Texture2D grassTexture;
        private Texture2D stronghold;

        //Fonts
        private SpriteFont healthFont;
        private SpriteFont statFont;

        //Dictionary of faction sprites
        private Dictionary<Faction, Texture2D> spriteDict;

        private Camera camera;
        #endregion

        #region Properties
        //Character texture lists
        public List<Texture2D> GoodGuyTextures { get { return goodGuyTextures; } }

        //Screen Textures
        public Texture2D TitleImage { get { return titleImage; } set { titleImage = value; } }
        public Texture2D GameOverImage { get { return gameOverImage; } set { gameOverImage = value; } }
        public Texture2D PauseImage { get { return pauseImage; } set { pauseImage = value; } }
        public Texture2D SwordStatScreen { get { return swordStatScreen; } set { swordStatScreen = value; } }
        public Texture2D LoadingScreen { get { return loadingScreen; } set { loadingScreen = value; } }

        //UI Textures
        public Texture2D PointerTexture { get { return pointerTexture; } set { pointerTexture = value; } }
        public Texture2D ResumeTexture { get { return resumeTexture; } set { resumeTexture = value; } }
        public Texture2D StatsTexture { get { return statsTexture; } set { statsTexture = value; } }
        public Texture2D ExitTexture { get { return exitTexture; } set { exitTexture = value; } }
        public Texture2D WinTexture { get { return winTexture; } set { winTexture = value; } }

        //Weapon textures
        public Texture2D SwordTexture { get { return swordTexture; } set { swordTexture = value; } }
        public Texture2D WhipTexture { get { return whipTexture; } set { whipTexture = value; } }
        public Texture2D PateTexture { get { return pateTexture; } set { pateTexture = value; } }

        //Map textures
        public Texture2D PathwayTexture { get { return pathwayTexture; } set { pathwayTexture = value; } }
        public Texture2D SandyTexture { get { return sandyTexture; } set { sandyTexture = value; } }
        public Texture2D GrassTexture { get { return grassTexture; } set { grassTexture = value; } }        
        public Texture2D Stronghold { get { return stronghold; } set { stronghold = value; } }

        //SpriteFonts
        public SpriteFont HealthFont { get { return healthFont; } set { healthFont = value; } }
        public SpriteFont StatFont { get { return statFont; } set { statFont = value; } }

        //Sprite Dictionary
        public Dictionary<Faction, Texture2D> SpriteDict { get { return spriteDict; } set { spriteDict = value; } }

        //Camera
        public Camera Camera { get { return camera; } }
        #endregion


        public DrawManager(Game1 mainMan):base(mainMan)
        {

        }

        //Init
        public override void Init()
        {
            //Init Texture lists
            goodGuyTextures = new List<Texture2D>();
            //Sprite dictionary
            spriteDict = new Dictionary<Faction, Texture2D>();
        }

        public override void Update()
        {
            //Updates the camera position every frame
                camera.Update();

        }

        public void ActivateCamera()
        {
            Viewport viewport = new Viewport(0, 0, mainMan.WindowWidth, mainMan.WindowHeight);
            camera = new Camera(viewport, mainMan);
        }

        /// <summary>
        /// Used to call draw on all objects and determine draw order
        /// </summary>
        public void Draw(SpriteBatch spritebatch, GameTime gameTime)
        {
            spritebatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.getTransformation());
            //The game states will determine what is being drawn for now.
            if(mainMan.UIMan.State == GameState.title)
            {

            }

            if(mainMan.UIMan.State == GameState.pause)
            {

            }
            
            if(mainMan.UIMan.State == GameState.game)
            {
                mainMan.GameMan.MapMan.Draw(spritebatch);
                //Draw all characters
                foreach (Character character in mainMan.GameMan.Characters)
                {
                    character.Draw(spritebatch, gameTime);
                }

                //Draw player
                foreach (Player player in mainMan.GameMan.Players)
                {
                    player.Draw(spritebatch);
                }
            }
            if(mainMan.UIMan.State == GameState.gameOver)
            {
                //For the game over/when character dies.
            }
            if(mainMan.UIMan.State == GameState.exit)
            {
                mainMan.Exit();
            }
            mainMan.UIMan.Screens.Peek().Draw(spritebatch);


            //Draw the pointer
            spritebatch.Draw(pointerTexture, mainMan.InputMan.PointerPosition,
                new Rectangle(0, 0, pointerTexture.Width, pointerTexture.Height), Color.White, 0f,
                new Vector2(pointerTexture.Width / 2, pointerTexture.Height / 2), 1.0f, SpriteEffects.None, 1);

            spritebatch.End();
        }
    }
}
