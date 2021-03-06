﻿#region Using statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using System.IO;
#endregion

//Names: Ryan Bell

namespace SwagSword
{
    /// <summary>
    /// The purpose of the MapManager
    /// </summary>
    public class MapManager : Manager
    {
        #region Fields
        int tileSize;                                               //The width and height in pixels of each tile                                                      
        int mapWidth;                                               //The width in number of tiles of the map
        int mapHeight;                                              //The height in number of tiles of the map
        int resWidth;                                               //The width of the map in pixels
        int resHeight;                                              //The hieght of the map in pixels
        int strongholdWidth;                                        //The width of a stronghold in pixels
        int radius;                                                 //The radius of the path circle surrounding each of the strongholds
        Point rightCenterPoint;                                     //The center points of each of the bases/strongholds
        Point leftCenterPoint;
        Point upperCenterPoint;
        Point lowerCenterPoint;
        Texture2D map;                                              //A texture2d which ends up holding the base map
        GraphicsDevice graphicsDevice;                              //Will hold information about the graphics device that the system is using
        Stronghold leftStronghold;                                  //Reference to the four strongholds of the game
        Stronghold rightStronghold;
        Stronghold topStronghold;
        Stronghold lowerStronghold;
        List<Stronghold> strongholds;                               //A list to store each stronghold object for easy iteration and retrieval
        List<Rectangle> tents;                                      //A list to hold all of the tent objects for drawing
        List<Rectangle> centerpieces;                               //This will hold rectangles that estimate the bounds for the main paths for bounds checking
        #endregion

        #region Properties                                         
        public int TileSize { get { return tileSize; } }            //All of the get and set properites related to the map
        public int MapWidth { get { return mapWidth; } }            //The center points have setter properties becasue
        public int MapHeight { get { return mapHeight; } }          //their value will be set with the mapMaker
        public int ResWidth { get { return resWidth; } }
        public int ResHeight { get { return resHeight; } }
        public Point RightCenterPoint { get { return rightCenterPoint; } set { rightCenterPoint = value; } }
        public Point LeftCenterPoint { get { return leftCenterPoint; } set { leftCenterPoint = value; } }
        public Point UpperCenterPoint { get { return upperCenterPoint; } set { upperCenterPoint = value; } }
        public Point LowerCenterPoint { get { return lowerCenterPoint; } set { lowerCenterPoint = value; } }
        public int Radius { get { return radius; } }
        public List<Stronghold> Strongholds { get { return strongholds; } }
        #endregion

        #region ConstructInit
        /// <summary>
        /// Instantiate the tents and centerpieces lists becasue they are populated within this class
        /// </summary>
        /// <param name="mainMan">A reference to the main manager</param>
        public MapManager(Game1 mainMan)
            : base(mainMan)
        {
            tents = new List<Rectangle>();
            centerpieces = new List<Rectangle>();
        }

        /// <summary>
        /// Set the values of various map size variables in preparation for the map creation
        /// It also sets the reference for the graphics device
        /// </summary>
        public override void Init()
        {
            tileSize = 64;
            resWidth = 5248;
            resHeight = 5248;
            mapWidth = resWidth / tileSize;
            mapHeight = resHeight / tileSize;

            graphicsDevice = mainMan.GraphicsDevice;
        }
        #endregion

        //called after the textures are loaded to avoid null pointer exception
        public void Startup()
        {
            //The stongholds are set to be five times the size of a tile and the radius of the surrounding circle is set.
            strongholdWidth = tileSize * 5;
            radius = 560;
            //A new MapMaker object is created with the tilesize, radius, width and height values passed in along with
            //references to the graphics device and the main manager
            MapMaker mapMaker = new MapMaker(radius, tileSize, resWidth, resHeight, graphicsDevice, mainMan);
            //The main method of the mapMaker object is called which returns a Texture2d. This is captured as the static map
            map = mapMaker.MakeMap();
            
            //Strongholds are set with a texture2d and a containing rectangle (sized so that they are centered at the respective center points). 
            //A reference to the main manager is set as well as which faction the strongholds belong to which is used to determine capturing. 
            leftStronghold = new Stronghold(mainMan.DrawMan.LeftStronghold, new Rectangle(radius - strongholdWidth / 2, resHeight / 2 - strongholdWidth / 2, strongholdWidth, strongholdWidth), mainMan, Faction.Good);
            rightStronghold = new Stronghold(mainMan.DrawMan.RightStronghold, new Rectangle(resWidth - radius - strongholdWidth / 2, resHeight / 2 - strongholdWidth / 2, strongholdWidth, strongholdWidth), mainMan, Faction.Tribal);
            topStronghold = new Stronghold(mainMan.DrawMan.TopStronghold, new Rectangle(resWidth / 2 - strongholdWidth / 2, radius - strongholdWidth / 2, strongholdWidth, strongholdWidth), mainMan, Faction.Rich);
            lowerStronghold = new Stronghold(mainMan.DrawMan.LowerStronghold, new Rectangle(resWidth / 2 - strongholdWidth / 2, resHeight - radius - strongholdWidth / 2, strongholdWidth, strongholdWidth), mainMan, Faction.Thief);

            //The collision boxes for the strongholds are set based on the strongholds' defining rectangles
            mainMan.GameMan.LeftStrong = leftStronghold.Rect;
            mainMan.GameMan.RightStrong = rightStronghold.Rect;
            mainMan.GameMan.TopStrong = topStronghold.Rect;
            mainMan.GameMan.LowerStrong = lowerStronghold.Rect;

            //The 4 strongholds are added to the list of strongholds
            strongholds = new List<Stronghold>();
            strongholds.Add(leftStronghold);
            strongholds.Add(rightStronghold);
            strongholds.Add(topStronghold);
            strongholds.Add(lowerStronghold);

            //four tents are placed around every stronghold
            foreach(Stronghold s in strongholds)
            {
                tents.Add(new Rectangle(s.Rect.Center.X - 328, s.Rect.Center.Y + 200, 128, 128));
                tents.Add(new Rectangle(s.Rect.Center.X - 328, s.Rect.Center.Y - 328, 128, 128));
                tents.Add(new Rectangle(s.Rect.Center.X + 200, s.Rect.Center.Y + 200, 128, 128));
                tents.Add(new Rectangle(s.Rect.Center.X + 200, s.Rect.Center.Y - 328, 128, 128));
            }
            //The four center pieces are added for the strongholds
            centerpieces.Add(new Rectangle(leftStronghold.Rect.Center.X - 500, leftStronghold.Rect.Y + 60, 160, 160));
            centerpieces.Add(new Rectangle(rightStronghold.Rect.Center.X + 400, rightStronghold.Rect.Y + 60, 160, 160));
            centerpieces.Add(new Rectangle(topStronghold.Rect.Center.X - 80, topStronghold.Rect.Center.Y - 500, 160, 160));
            centerpieces.Add(new Rectangle(lowerStronghold.Rect.Center.X - 80, lowerStronghold.Rect.Center.Y + 400, 160, 160));
            return;
        }

        
        public void Draw(SpriteBatch spriteBatch)
        {
            //draw the static map with no tansparency (color white)
            spriteBatch.Draw(map, new Rectangle(0, 0, resWidth, resHeight), Color.White);
            //draw the strongholds
            foreach(Stronghold s in strongholds)
            {
                s.Draw(spriteBatch);
            }
            //Draw each of the centerpieces with 0 transparency
            spriteBatch.Draw(mainMan.DrawMan.BanditCenterpiece, centerpieces[3], Color.White);
            spriteBatch.Draw(mainMan.DrawMan.GoodGuyCenterpiece, centerpieces[0], Color.White);
            spriteBatch.Draw(mainMan.DrawMan.RichCenterpiece, centerpieces[2], Color.White);
            spriteBatch.Draw(mainMan.DrawMan.TribalCenterpiece, centerpieces[1], Color.White);
            //loop through all of the tents by faction and draw them
            for(int i = 0; i < 4; i ++)
                {spriteBatch.Draw(mainMan.DrawMan.GoodGuyTents, tents[i], Color.White);}
            for(int i = 4; i < 8; i ++)
                {spriteBatch.Draw(mainMan.DrawMan.TribalTents, tents[i], Color.White);}
            for(int i = 8; i < 12; i ++)
                {spriteBatch.Draw(mainMan.DrawMan.RichTents, tents[i], Color.White);}
            for(int i = 12; i < 16; i ++)
                {spriteBatch.Draw(mainMan.DrawMan.BanditTents, tents[i], Color.White);}
        }
        /// <summary>
        /// Method to calculate distance between two points in terms of pixels
        /// </summary>
        /// <param name="a">The first point</param>
        /// <param name="b">The second point</param>
        /// <returns></returns>
        public double CalcDistance(Point a, Point b)
        {
            return Math.Sqrt(Math.Pow((a.X - b.X), 2) + Math.Pow((a.Y - b.Y), 2));
        }
        /// <summary>
        /// A method to calculate the distance between two points, in pixels, using x and y coordinates
        /// </summary>
        /// <param name="aX">The x coord of the first point</param>
        /// <param name="aY">The y coord of the first point</param>
        /// <param name="bX">The x coord of the second point</param>
        /// <param name="bY">The y coord of the second point</param>
        /// <returns></returns>
        public double CalcDistance(int aX, int aY, int bX, int bY)
        {
            return Math.Sqrt(Math.Pow((aX - bX), 2) + Math.Pow((aY - bY), 2));
        }


    }
}