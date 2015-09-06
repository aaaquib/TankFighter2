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
    public class Bullet
    {

        public Texture2D bullet;
        public Boolean explosion;
        public Vector2 currentTankSpeed;
        public Vector2 posInScreen;
        public Rectangle recBullet;
        public float direcction;
        public Int32 fireTime;
        public Int32 exploteTime;
        
        public Bullet( int range)
        {
            fireTime = range;
            exploteTime = 0;
            explosion = false;
            recBullet = new Rectangle(0, 0, 2, 2);
            // TODO: Construct any child components here
        }

        public void LoadContent(GraphicsDevice graphics, ContentManager Content)
        {
            if (fireTime > 0)
            {
                bullet = Content.Load<Texture2D>("Tank_Shell_Crop");
            }
            
        }
        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public void Initialize()
        {
            // TODO: Add your initialization code here

           // base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            // TODO: Add your update code here
            
            //base.Update(gameTime);
        }
    }
}
