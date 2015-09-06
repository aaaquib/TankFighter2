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
    public class LightTank : Tank
    {
        public int firePower;
        public Rectangle recChassis;
        Vector2 originChassis;
        Vector2 originTurret;
        public Vector2 currentTankSpeed;
        Vector2 bulletSpeed;
        public Vector2 screenposChassis;
        Vector2 screenposTurret;
        GraphicsDevice actual;
        List<Bullet> bullets;
        Texture2D chassis;
        Texture2D turret;

        public Boolean dontMove;
        public Int32 engineHeat;
        public Int32 maxHeat;
        //int positionX;
        //int positionY;

        SpriteBatch spriteBatch;

        public float chassisDirection; //If the Chassis or Turret is facing right, the value is 0
        public float turretDirection;
        float elapsed;

        public float speedFactor = -2;
        float bulletspeedFactor = -21;
        public int firingCooldown, firingCooldownMax;
        //Must be negative or else will move in opposite direction
        ////Tank Attributes in-game
        //int speedDefault;
        //int armorDefault;
        //int firepowerDefault;
        public int rangeDefault;
        //int speedCurrent;
        //int armorCurrent;
        //int firepowerCurrent;
        //int rangeCurrent;



        //Sound
        public SoundEffect movement;
        public SoundEffect firing;
        public SoundEffect explosion;
        public SoundEffect turretTurn;

        FrameAnimation explosions;
        Texture2D shellExplosion;
        private float timeElapsed;
        int frame = 0;
        bool isExploding = false;
        bool forward;

        public LightTank(int range, int xPos,int yPos)
        {
            firePower = 1;
            screenposChassis.X = xPos;
            screenposChassis.Y = yPos;
            firingCooldownMax = 75;
            firingCooldown = firingCooldownMax;
            bullets = new List<Bullet>();
            maxHeat = 1000;
            engineHeat = maxHeat ;
            recChassis = new Rectangle(0, 0, 35, 65);
            rangeDefault = range;
            dontMove = false;

            // TODO: Construct any child components here
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

        public void LoadContent(GraphicsDevice graphics, ContentManager Content)
        {
            spriteBatch = new SpriteBatch(graphics);
            actual = graphics;
            chassis = Content.Load<Texture2D>("tank");
            turret = Content.Load<Texture2D>("toMove");
            screenposTurret.X = screenposChassis.X;
            screenposTurret.Y = screenposChassis.Y;
            recChassis = new Rectangle((int)screenposChassis.X, (int)screenposChassis.Y, 36, 60);
            originTurret.X = turret.Width / 2 +7;
            originTurret.Y = turret.Height / 2 + 20;
            originChassis.X = turret.Width / 2;
            originChassis.Y = turret.Height / 2;

            firing = Content.Load<SoundEffect>("Bullet_Explosion");
            explosion = Content.Load<SoundEffect>("Bullet_Fire");
            movement = Content.Load<SoundEffect>("Tank_Movement");
            turretTurn = Content.Load<SoundEffect>("turret");
            foreach (Bullet bullet in bullets)
            {
                bullet.LoadContent(graphics,GameOptions.m_content);
            }
            shellExplosion = Content.Load<Texture2D>("Shell_Explosion_Sprite_Sheet");
            explosions = new FrameAnimation(shellExplosion, 16, 1.0f);
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds; 
            
            currentTankSpeed = new Vector2(speedFactor * ((float)Math.Cos(chassisDirection + MathHelper.PiOver2)), speedFactor * ((float)Math.Sin(chassisDirection + MathHelper.PiOver2)));
            bulletSpeed = new Vector2(bulletspeedFactor * ((float)Math.Cos(turretDirection + MathHelper.PiOver2)), (bulletspeedFactor) * ((float)Math.Sin(turretDirection + MathHelper.PiOver2)));
            elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            foreach (Bullet bullet in bullets)
            {
                if (!bullet.explosion)
                {
                    bullet.Update(gameTime);
                }
            }
            //recChassis.Location = new Point((int)screenposChassis.X-18, (int)screenposChassis.Y - 20);
            if (engineHeat < maxHeat) engineHeat += 5; //Cool down engine
            if (engineHeat > maxHeat) engineHeat = maxHeat;
            if (engineHeat < 0)
            {
                engineHeat = 0;
                currentTankSpeed.X = 0;
                currentTankSpeed.Y = 0;
            }
            if (firingCooldown < firingCooldownMax) firingCooldown++;
            //base.Update(gameTime);
        }
        
        public void fire()
        {
            if (firingCooldown == firingCooldownMax)
            {
                firing.Play();
                Bullet toFire = new Bullet(this.rangeDefault);
                toFire.LoadContent(actual,GameOptions.m_content);
                toFire.posInScreen = screenposChassis;
                toFire.direcction = turretDirection;
                toFire.currentTankSpeed = bulletSpeed;
                toFire.recBullet.Location = new Point((int)screenposChassis.X, (int)screenposChassis.Y);
                bullets.Add(toFire);
                firingCooldown = 0;
            }
        }



        public void RotateTurretLeft()
        {
            turretTurn.Play();
            turretDirection -= 2 * elapsed;
            float circle = MathHelper.Pi * 4;
            turretDirection = turretDirection % circle;
        }

        public void RotateTurretRight()
        {
            turretTurn.Play();
            turretDirection += 2* elapsed;
            float circle = MathHelper.Pi * 4;
            turretDirection = turretDirection % circle;
        }

        public void RotateChassisLeft()
        {
            chassisDirection -= 2 * elapsed;
            float circle = MathHelper.Pi * 4;
            chassisDirection = chassisDirection % circle;
        }

        public void RotateChassisRight()
        {
            chassisDirection += 2 * elapsed;
            float circle = MathHelper.Pi * 4;
            chassisDirection = chassisDirection % circle;
        }

        public void MoveChassis()
        {
            forward = true;
            movement.Play();
            engineHeat -= 2;
            if (!BoundaryCollision()){
            if (!dontMove)
            {
                screenposChassis += currentTankSpeed;
                screenposTurret += currentTankSpeed;
            }
            else
            {
                screenposChassis -= currentTankSpeed;
                screenposTurret -= currentTankSpeed;
                //dontMove = false;
            }

            recChassis = new Rectangle((int)screenposChassis.X, (int)screenposChassis.Y, 36, 60);}
            
        }

        public void MoveChassisReverse()
        {
            forward = false;
            movement.Play();
            engineHeat -= 2;
            if (!BoundaryCollision())
            {
                if (!dontMove)
                {
                    screenposChassis -= currentTankSpeed;
                    screenposTurret -= currentTankSpeed;

                }
                else
                {
                    screenposChassis += currentTankSpeed;
                    screenposTurret += currentTankSpeed;
                    //dontMove = false;
                }

                recChassis = new Rectangle((int)screenposChassis.X, (int)screenposChassis.Y, 36, 60);
            }
        }

        public bool BoundaryCollision()
        {
            if (forward)
            {
                if ((screenposChassis.X + currentTankSpeed.X - chassis.Width) > 0 && (screenposChassis.X + currentTankSpeed.X + chassis.Width) < Main.getWorldWidth())
                    if ((screenposChassis.Y + currentTankSpeed.Y - chassis.Height) > 0 && (screenposChassis.Y + currentTankSpeed.Y + chassis.Height) < Main.getWorldHeight())
                        return false;
            }
            else
            {
                if ((screenposChassis.X - currentTankSpeed.X - chassis.Width) > 0 && (screenposChassis.X - currentTankSpeed.X + chassis.Width) < Main.getWorldWidth())
                    if ((screenposChassis.Y - currentTankSpeed.Y - chassis.Height) > 0 && (screenposChassis.Y - currentTankSpeed.Y + chassis.Height) < Main.getWorldHeight())
                        return false;
            }
            return true;
        }

        //bool checkTankCollision(

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
            //spriteBatch.Begin();
            

            if (chassis != null && turret != null)
            {
                
                //spriteBatch.Draw(chassis, screenposChassis, null, Color.White, chassisDirection, originTurret, 1f, SpriteEffects.None, 0f);
                spriteBatch.Draw(chassis, recChassis, null, Color.White, chassisDirection, originTurret, SpriteEffects.None, 0f);
                
                foreach (Bullet bullet in bullets)
            {
                if (bullet.fireTime > 5 && !bullet.explosion)
                {
                    spriteBatch.Draw(bullet.bullet, bullet.posInScreen, null, Color.CornflowerBlue, turretDirection, originChassis, 0.4f, SpriteEffects.None, 0f);
                    if (!bullet.explosion)
                    {
                        bullet.posInScreen += bullet.currentTankSpeed;
                        bullet.recBullet.Location = new Point((int)bullet.posInScreen.X, (int)bullet.posInScreen.Y);

                    }

                    bullet.fireTime--;
                }
                else if (bullet.explosion && bullet.fireTime > 0)
                {
                    spriteBatch.Draw(bullet.bullet, bullet.posInScreen, null, Color.CornflowerBlue, turretDirection, originChassis, 0.4f, SpriteEffects.None, 0f);
                    isExploding = true;
                    explosions.position.X = bullet.posInScreen.X - 32;
                    explosions.position.Y = bullet.posInScreen.Y - 32;
                    bullet.fireTime--;
                }
            }

                if (isExploding)
                {
                    if (timeElapsed > 0.04f)
                    {
                        frame++;
                        timeElapsed = 0;
                    }
                    explosions.Draw(spriteBatch, frame);
                    if (frame == 15)
                    {
                        frame = 0;
                        isExploding = false;
                    }
                }

                spriteBatch.Draw(turret, screenposTurret, null, Color.White, turretDirection, originChassis, 1f, SpriteEffects.None, 0f);
            }
          
            //spriteBatch.End();
        }

        internal Boolean checkBulletIntersection(Rectangle boxRectangle)
        {
            foreach (Bullet a in bullets)
            {
                if (a.recBullet.Intersects(boxRectangle) && !a.explosion)
                {
                    boxRectangle.Location = new Point(0, 0);
                    if (a.fireTime > 5)
                    {
                        a.fireTime = 5;
                    }
                    a.explosion = true;
                    a.bullet = GameOptions.m_content.Load<Texture2D>("explosion");
                    explosion.Play();
                    return true;
                }
            }
            return false;
        }
        internal Boolean checkTankIntersection(Rectangle boxRectangle)
        {
            if (recChassis.Intersects(boxRectangle))
            {
                dontMove = true;
                return true;
            }
            else
            {
                dontMove = false;
                return false;
            }
        }
    }
}
