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
    public class Tank
    {
        public Tank()
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public void Initialize()
        {
            // TODO: Add your initialization code here

            //base.Initialize();
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



        //public void CheckForWallCollision(int dimensions)
        //{
        //    if (Position.X >= dimensions)
        //    {
        //        Velocity.X *= -1;
        //        Position.X = dimensions;
        //    }
        //    else if (Position.X <= -dimensions)
        //    {
        //        Velocity.X *= -1;
        //        Position.X = -dimensions;
        //    }

        //    if (Position.Y >= dimensions)
        //    {
        //        Velocity.Y *= -1;
        //        Position.Y = dimensions;
        //    }
        //    else if (Position.Y <= -dimensions)
        //    {
        //        Velocity.Y *= -1;
        //        Position.Y = -dimensions;
        //    }
        //}
    }
}
