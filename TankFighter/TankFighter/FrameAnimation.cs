
using Microsoft.Xna.Framework.Graphics;

namespace TankFighter
{
    class FrameAnimation : SpriteManager
    {
        public FrameAnimation(Texture2D texture, int frames, float scale)
            : base(texture, frames, scale)
        {
        }

        public void SetFrame(int frame)
        {
            frameIndex = frame;
        }
        public int getFrame()
        {
            return frameIndex;
        }
    }
}