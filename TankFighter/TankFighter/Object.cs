using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace TankFighter
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Object : Microsoft.Xna.Framework.GameComponent
    {
        public bool destructible,solid;
        //public bool exists;
        Vector2 position;
        SpriteBatch spriteBatch;
        public Rectangle rectangle;
        public int identifier;
        Texture2D objectTexture;
        public bool visible;

        public string type;


        public Object(Game game, int ID,string _type, bool vis, int positionX, int positionY)
            : base(game)
        {
            // TODO: Construct any child components here
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            position.X = positionX;
            position.Y = positionY;
            visible = vis;
            type = _type;
            identifier = ID;
            switch (type)
            {
                case "tree": objectTexture = Game.Content.Load<Texture2D>("tree");
                    solid = true;
                    destructible = true;
                    rectangle = new Rectangle(positionX, positionY, 20, 20);
                    break;
                case "rock": objectTexture = Game.Content.Load<Texture2D>("rock");
                    solid = true;
                    destructible = false;
                    rectangle = new Rectangle(positionX, positionY, 36, 36);
                    break;
                case "crates": objectTexture = Game.Content.Load<Texture2D>("Wood_Crate_img");
                    solid = true;
                    destructible = true;
                    rectangle = new Rectangle(positionX, positionY, 36, 36);
                    break;
                case "powerup_range": objectTexture = Game.Content.Load<Texture2D>("Range");
                    solid = true;
                    destructible = false;
                    rectangle = new Rectangle(positionX, positionY, 36, 36);
                    break;
                case "powerup_speed": objectTexture = Game.Content.Load<Texture2D>("Speed");
                    solid = true;
                    destructible = false;
                    rectangle = new Rectangle(positionX, positionY, 36, 36);
                    break;
                case "powerup_firepower": objectTexture = Game.Content.Load<Texture2D>("FirePower");
                    solid = true;
                    destructible = false;
                    rectangle = new Rectangle(positionX, positionY, 36, 36);
                    break;
                case "powerup_armor": objectTexture = Game.Content.Load<Texture2D>("Armor");
                    solid = true;
                    destructible = false;
                    rectangle = new Rectangle(positionX, positionY,36, 36);
                    break;
                case "rock1": objectTexture = Game.Content.Load<Texture2D>("rock1");
                    solid = true;
                    destructible = false;
                    rectangle = new Rectangle(positionX, positionY, 24, 24); break;
                case "rock2": objectTexture = Game.Content.Load<Texture2D>("rock2");
                    solid = true;
                    destructible = false;

                    rectangle = new Rectangle(positionX, positionY, 30, 30); break;

                case "rock3": objectTexture = Game.Content.Load<Texture2D>("rock3");
                    solid = true;
                    destructible = false;
                    rectangle = new Rectangle(positionX, positionY, 36, 36); break;
                case "tree1": objectTexture = Game.Content.Load<Texture2D>("tree1");
                    solid = true;
                    destructible = true;
                    rectangle = new Rectangle(positionX, positionY, 36, 36); break;
                case "tree2": objectTexture = Game.Content.Load<Texture2D>("tree_1_crop");
                    solid = destructible = true;
                    rectangle = new Rectangle(positionX, positionY, 50, 50); break;
                case "tree3": objectTexture = Game.Content.Load<Texture2D>("tree_2_crop");
                    solid = destructible = true;
                    rectangle = new Rectangle(positionX, positionY, 40, 40); break;
                case "bush1": objectTexture = Game.Content.Load<Texture2D>("Grass_Bush_Crop");
                    solid = destructible = true;
                    rectangle = new Rectangle(positionX, positionY, 40, 40); break;

            }
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here
            base.Initialize();
        }



        Boolean checkBulletCollision(Rectangle boxRectangle)
        {
            if (rectangle.Intersects(boxRectangle))
            {
                return true;
            }
            return false;
        }
       

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
           

            base.Update(gameTime);
        }

        public void Draw(GameTime gametime, SpriteBatch sb)
        {
            if (visible == true)
            {
                //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                sb.Draw(objectTexture, rectangle, Color.White);
                //spriteBatch.End();
            }
        }
    }
}
