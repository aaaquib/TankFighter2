using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TankFighter
{
    public abstract class SpriteManager
    {
        protected Texture2D texture;
        public Vector2 position = Vector2.Zero;
        public Color color = Color.White;
        public Vector2 origin;
        public float rotation = 0f;
        public float scale = .35f;
        public SpriteEffects spriteEffect;
        protected Rectangle[,] rectangles;
        protected int frameIndex = 0, frames = 0;

        public SpriteManager(Texture2D texture, int frames, float scale)
        {
            this.texture = texture;
            this.frames = frames;
            this.scale = scale;
            int width = 64;
            int height = 64;
            rectangles = new Rectangle[4, 4];
            for (int i = 0; i < 4; i++)
            {

                for (int j = 0; j < 4; j++)
                {
                    rectangles[i, j] = new Rectangle(j * width, height * i, width, height);
                }


            }

        }
        public void Draw(SpriteBatch spriteBatch, int frame)
        {

            frameIndex = (frame) / 4;
            spriteBatch.Draw(texture, position, rectangles[frameIndex, frame - (frameIndex * 4)], color, rotation, origin, scale, spriteEffect, 0f);


        }
    }
}
