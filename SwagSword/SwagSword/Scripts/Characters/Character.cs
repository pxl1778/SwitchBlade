﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

//Names: Nelson Scott

namespace SwagSword
{
    //Enum for factions
    public enum Faction
    {
        Tribal,
        Good,
        Rich,
        Thief
    }

    public enum AnimationState
    {
        FaceDown,
        FaceUp,
        FaceLeft,
        FaceRight,
        MoveDown,
        MoveUp,
        MoveLeft,
        MoveRight
    }

    public enum CharacterState
    {
        Spawn,
        Switch,
        Active,
        Hurt,
        Dead
    }

    /// <summary>
    /// The base class for every type of Character
    /// </summary>
    public class Character
    {
        #region Fields
        //Used to access Game
        private Game1 mainMan;

        //Main
        private Texture2D texture;
        private Rectangle rectangle;
        private Vector2 position;
        private Vector2 center; //The center point to be used for rotation
        private SpriteEffects spriteEffect; //Used for flipping the sprite
        private Color color;
        private Color flashColor;
        private Color normalColor;
        private Faction type;
        private AnimationState animationState;
        private bool isControlled;
        private CharacterState characterState;
        private float stateTimer;

        //Animation
        private int frameWidth;
        private int frameHeight;
        private int frameX;
        private int frameY;
        private int totalFrames;
        private float frameLength; //The length of time a frame is displayed for
        private float frameTime; //How long a frame has been displayed so far

        //Weapon + Abilities
        private Weapon weapon;

        //Stats
        private int health;
        private int maxHealth;
        private int damage;
        private float strength;
        private float movementSpeed;
        private float attackSpeedMin; //In degrees
        private float attackSpeedMax;

        //Physics
        private float velocityX;
        private float velocityY;
        private float drag;
        private float direction; //Think of it as the forward angle
        #endregion

        #region Properties
        //Main
        public Texture2D Texture { get { return texture; } set { texture = value; } }
        public Rectangle Rectangle { get { return rectangle; } set { rectangle = value; } }
        public Rectangle HitBox { get { return new Rectangle((int)position.X - frameWidth / 2, (int)position.Y - frameWidth / 2, frameWidth, frameHeight); } }
        public float X 
        { 
            get { return position.X; } 
            set 
            {
                position.X = value;
                if (position.X < 0)
                    position.X = 0;
                if (position.X > mainMan.MapWidth)
                    position.X = mainMan.MapWidth;

            } 
        }
        public float Y 
        { 
            get { return position.Y; } 
            set 
            {
                position.Y = value;
                if (position.Y < 0)
                    position.Y = 0;
                if (position.Y > mainMan.MapHeight)
                    position.Y = mainMan.MapHeight;
            } 
        }
        public SpriteEffects SpriteEffect { get { return spriteEffect; } set { spriteEffect = value; } }
        public Color Color { get { return color; } set { color = value; } }
        public Color FlashColor { get { return flashColor; } set { flashColor = value; } }
        public Color NormalColor { get { return normalColor; } set { normalColor = value; } }
        public Faction Type { get { return type; } set { type = value; } }
        public AnimationState AnimationState { get { return animationState; } set { animationState = value; } }
        public CharacterState CharacterState { get { return characterState; } set { characterState = value; } }
        public float StateTimer { get { return stateTimer; } set { stateTimer = value; } }
        public bool IsControlled { get { return isControlled; } set { isControlled = value; } }

        //Weapon + Abilities
        public Weapon Weapon { get { return weapon; } }

        //Stats
        public int Health { get { return health; } set { health = value; } }
        public int MaxHealth { get { return maxHealth; } }
        public float Strength { get { return strength; } }
        public int Damage { get { return damage; } }
        public float MovementSpeed { get { return movementSpeed; } }
        public float AttackSpeedMin { get { return attackSpeedMin; } }
        public float AttackSpeedMax { get { return attackSpeedMax; } }

        //Physics
        public float VelocityX { get { return velocityX; } set { velocityX = value; } }
        public float VelocityY { get { return velocityY; } set { velocityY = value; } }
        public float Drag { get { return drag; } }
        public float Direction { get { return direction; } set { direction = value; } }
        #endregion

        public Character(int x, int y, Texture2D texture, Game1 mainMan)
        {
            //Manager
            this.mainMan = mainMan;

            //Set texture
            this.texture = texture;

            //Set position
            rectangle = new Rectangle(0, 0, 66, 66);
            position = new Vector2(x, y);
            center = new Vector2(66.0f / 2f, 66.0f / 2f);

            //Set weapon, abilites
            weapon = new Weapon(this, mainMan);
        }

        public virtual void Init()
        {
            //Init Stats
            isControlled = false;
            SwitchState(CharacterState.Spawn);
            Color = normalColor;
            InitStats();

            //Init Animation
            frameX = 0;
            frameY = 0;
            frameWidth = 66;
            frameHeight = 66;
            totalFrames = 4;
            frameLength = 0.1f;
            frameTime = 0.0f;
            animationState = AnimationState.FaceRight;

            //Init physics
            velocityX = 0;
            velocityY = 0;
            drag = movementSpeed; //Drag should never go above this
            direction = 0f;
        }

        void InitStats()
        {
            //Will init all stats based on a config file
            //Example Health stats
            health = 100;
            maxHealth = 100;
            strength = 30f;
            damage = 25;
            attackSpeedMin = 10.0f;
            attackSpeedMax = 20.0f;
            movementSpeed = 5.0f;
        }

        /// <summary>
        /// The main update method
        /// </summary>
        public virtual void Update()
        {
            //Swap between character states
            switch (characterState)
            {
                case CharacterState.Spawn:
                    if (stateTimer % 0.2f < 0.1f)
                    {
                        FlashColors();
                    }

                    UpdatePhysics();

                    stateTimer -= (float)mainMan.GameTime.ElapsedGameTime.TotalSeconds;
                    if (stateTimer <= 0f)
                    {
                        SwitchState(CharacterState.Active);
                    }
                    break;

                case CharacterState.Switch:
                    if (stateTimer % 0.2f < 0.1f)
                    {
                        FlashColors();
                    }

                    UpdatePhysics();

                    stateTimer -= (float)mainMan.GameTime.ElapsedGameTime.TotalSeconds;
                    if (stateTimer <= 0f)
                    {
                        SwitchState(CharacterState.Active);
                    }
                    break;

                case CharacterState.Active:
                    UpdatePhysics();
                    break;

                case CharacterState.Hurt:
                    if (stateTimer % 0.2f < 0.1f)
                    {
                        FlashColors();
                    }

                    UpdatePhysics();

                    stateTimer -= (float)mainMan.GameTime.ElapsedGameTime.TotalSeconds;
                    if (stateTimer <= 0f)
                    {
                        SwitchState(CharacterState.Active);
                    }
                    break;

                case CharacterState.Dead:
                    if (color.A - (byte)mainMan.GameTime.ElapsedGameTime.TotalMilliseconds <= 0)
                    {
                        Kill();
                    }
                    else
                    {
                        color.A -= (byte)mainMan.GameTime.ElapsedGameTime.TotalMilliseconds;
                    }
                    break;
            }

            weapon.Update();
        }

        /// <summary>
        /// Switches the characterState and updates stateTimer
        /// </summary>
        public void SwitchState(CharacterState state)
        {
            characterState = state;
            color = normalColor;
            switch (characterState)
            {
                case CharacterState.Spawn:
                    stateTimer = 1.0f;
                    flashColor = Color.GreenYellow;
                    break;

                case CharacterState.Switch:
                    stateTimer = 1.0f;
                    flashColor = Color.Gold;
                    break;

                case CharacterState.Active:
                    stateTimer = 0f;
                    break;

                case CharacterState.Hurt:
                    stateTimer = 0.2f;
                    flashColor = Color.Red;
                    break;

                case CharacterState.Dead:
                    
                    break;
            }
        }

        /// <summary>
        /// Swaps fast between the normal color and the flash color
        /// </summary>
        public void FlashColors()
        {
            if (color == normalColor)
            {
                color = flashColor;
            }
            else
            {
                color = normalColor;
            }
        }

        /// <summary>
        /// Each character will handle it's own physics
        /// </summary>
        public void UpdatePhysics()
        {
            //Update position based on velocity X, Y
            X += velocityX;
            Y += velocityY;

            //Add natural drag to movement speed
            if (velocityX != 0)
            {
                if (velocityX > 0)
                {
                    velocityX -= drag;
                    if (velocityX < 0)
                    {
                        velocityX = 0;
                    }
                }
                else
                {
                    velocityX += drag;
                    if (velocityX > 0)
                    {
                        velocityX = 0;
                    }
                }
            }
            if (velocityY != 0)
            {
                if (velocityY > 0)
                {
                    velocityY -= drag;
                    if (velocityY < 0)
                    {
                        velocityY = 0;
                    }
                }
                else
                {
                    velocityY += drag;
                    if (velocityY > 0)
                    {
                        velocityY = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Starts a new animation by setting frameY and reseting the frame timer
        /// </summary>
        /// <param name="anim"></param>
        public void StartAnimation(AnimationState anim)
        {
            switch (anim)
            {
                case AnimationState.MoveDown:
                    frameY = 1;
                    break;
                case AnimationState.MoveUp:
                    frameY = 4;
                    break;
                case AnimationState.MoveLeft:
                    frameY = 3;
                    break;
                case AnimationState.MoveRight:
                    frameY = 2;
                    break;
            }

            animationState = anim;
            frameX = 0;
            frameTime = 0.0f;
        }


        /// <summary>
        /// Used to damage character, knock them back
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="force"></param>
        public void TakeHit(int damage, float force, float hitDirection)
        {
            //Take down health;
            health -= damage;
            if (health < 0)
            {
                SwitchState(CharacterState.Dead);
                health = 0;
            }
            else
            {
                SwitchState(CharacterState.Hurt);
            }

            velocityX += force * (float)Math.Cos((90f - hitDirection) * Math.PI / 180f);
            velocityY += force * (float)Math.Sin((90f - hitDirection) * Math.PI / 180f);
        }

        /// <summary>
        /// Kills this character
        /// </summary>
        public void Kill()
        {
            if (isControlled)
            {

            }
            else
            {
                
            }
            mainMan.GameMan.Characters.Remove(this);
        }

        /// <summary>
        /// Each character will be responsible for drawing themselves
        /// </summary>
        /// <param name="spritebatch"></param>
        public void Draw(SpriteBatch spritebatch, GameTime gameTime)
        {
            switch (animationState)
            {
                case AnimationState.FaceDown:
                    frameX = 0;
                    frameY = 0;
                    break;
                case AnimationState.FaceUp:
                    frameX = 3;
                    frameY = 0;
                    break;
                case AnimationState.FaceLeft:
                    frameX = 2;
                    frameY = 0;
                    break;
                case AnimationState.FaceRight:
                    frameX = 1;
                    frameY = 0;
                    break;

                default:
                    frameTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    while (frameTime > frameLength)
                    {
                        if (frameX < totalFrames - 1)
                        {
                            frameX++;
                        }
                        else
                        {
                            frameX = 0;
                        }
                        frameTime = 0f;
                    }
                    break;
            }

            rectangle = new Rectangle(frameX * frameWidth, frameY * frameHeight, frameWidth, frameHeight);

            spritebatch.Draw(texture, position, rectangle, color, 0f, center, 1.0f, spriteEffect, 1);

            weapon.Draw(spritebatch);
        }
    }
}
