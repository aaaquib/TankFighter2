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
    public class Radar
    {
        private Texture2D PlayerOneImage;
        private Texture2D PlayerTwoImage;
        private Texture2D RadarImage;

        // Local coords of the radar image's center, used to offset image when being drawn
        private Vector2 RadarImageCenter;

        // Distance that the radar can "see"
        private Vector2 RadarRange;

        // Radius of radar circle on the screen
        private Rectangle RadarScreen = new Rectangle(350,500,100,100);

        // This is the center position of the radar hud on the screen. 
        static Vector2 RadarCenterPos = new Vector2(400, 550);

        Vector2 playerOneRelative;
        Vector2 playerTwoRelative;

        public Radar(ContentManager Content, string playerOneDotPath, string playerTwoDotPath, string radarImagePath)
        {
            PlayerOneImage = Content.Load<Texture2D>(playerOneDotPath);
            PlayerTwoImage = Content.Load<Texture2D>(playerTwoDotPath);
            RadarImage = Content.Load<Texture2D>(radarImagePath);

            RadarImageCenter = new Vector2(RadarImage.Width / 2.0f, RadarImage.Height / 2.0f);

        }

        public void Draw(SpriteBatch spriteBatch, Vector2 PlayerOnePos, Vector2 PlayerTwoPos)
        {
            // The last parameter of the color determines how transparent the radar circle will be
            spriteBatch.Draw(RadarImage, RadarScreen, new Color(0,0,0,85));

            // If enemy is in range
            
                playerOneRelative.X = RadarScreen.X + PlayerOnePos.X * RadarScreen.Width/Main.getWorldWidth();
                playerOneRelative.Y = RadarScreen.Y + PlayerOnePos.Y * RadarScreen.Height / Main.getWorldHeight();
                playerTwoRelative.X = RadarScreen.X + PlayerTwoPos.X * RadarScreen.Width / Main.getWorldWidth();
                playerTwoRelative.Y = RadarScreen.Y + PlayerTwoPos.Y * RadarScreen.Height / Main.getWorldHeight();
                //float distance = diffVect.Length();

                //// Check if enemy is within RadarRange
                //if (distance < RadarRange)
                //{
                //    // Scale the distance from world coords to radar coords
                //    diffVect *= RadarScreenRadius / RadarRange;

                //    // Offset coords from radar's center
                //    diffVect += RadarCenterPos;

                //    // Draw enemy dot on radar
            //spriteBatch.Draw(PlayerTwoImage, diffVect, Color.White);
                //}
            

            // Draw player's dot last
                spriteBatch.Draw(PlayerOneImage, playerOneRelative, Color.White);
                spriteBatch.Draw(PlayerTwoImage, playerTwoRelative, Color.White);
        }
    }
}
