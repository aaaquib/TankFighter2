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
    public class PlayerOne
    {
        public Tank currTank;
        public LightTank lightTank;
        public HeavyTank heavyTank;
        public RangedTank rangedTank;
        public GamePadState oldStateG;
        public Int32 health;
        Boolean canfire;
        public Int32 actualTank;

        public Int32 powerupID;
        Int32 powerupCounter;

        Texture2D powerupArmor;
        Texture2D powerupFirepower;
        Texture2D powerupRange;
        Texture2D powerupSpeed;

        Texture2D garage;

        // Added for Garage Menu
        Texture2D lightTankInfo, heavyTankInfo, rangeTankInfo;
        public int info = 1, currentTank;
        public bool inGarage = false;
        public KeyboardState oldState;
        Rectangle garageInfo;

        //HUD texture
        Texture2D hudBackground;
        Texture2D hudHeat;
        Texture2D hudHealth;
        Texture2D hudFire;
        //Rectangle backgroundBar, heatBar;
        //End of HUD Texture

        //public static Camera2d cam;

        Rectangle recGarage;
        public PlayerOne()
        {
            // TODO: Construct any child components here
            canfire = true;
            actualTank = 1;

            health = 3;
            lightTank = new LightTank( 20, 75, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2 - 10);
            heavyTank = new HeavyTank( 20, 75, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2 - 10);
            rangedTank = new RangedTank( 40, 75, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2 - 10);

            powerupID = 0;
            powerupCounter = 60;
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public void Initialize()
        {
            // TODO: Add your initialization code here

            //cam = new Camera2d();
            //cam.Pos = new Vector2(75, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2 - 10);

            currTank = lightTank;

            lightTank.Initialize();
            heavyTank.Initialize();
            rangedTank.Initialize();
            //base.Initialize();
        }

        public Vector2 getPosition()
        {
            if (actualTank == 1)
                return lightTank.screenposChassis;
            else if (actualTank == 2)
                return heavyTank.screenposChassis;
            else
                return rangedTank.screenposChassis;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public void LoadContent(GraphicsDevice graphics, ContentManager Content)
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            recGarage = new Rectangle(5, graphics.Viewport.Height / 2 - 25, 50, 100);

            garage = Content.Load<Texture2D>("garage");
            hudBackground = Content.Load<Texture2D>("Background");
            hudHeat = Content.Load<Texture2D>("heat");
            hudHealth = Content.Load<Texture2D>("health");
            hudFire = Content.Load<Texture2D>("reload");
            garageInfo = new Rectangle(55, graphics.Viewport.Height / 2 - 25, 200, 200);
            lightTankInfo = Content.Load<Texture2D>("Garage_Info_Light");
            heavyTankInfo = Content.Load<Texture2D>("Garage_Info_Heavy");
            rangeTankInfo = Content.Load<Texture2D>("Garage_Info_Ranged");

            powerupArmor = Content.Load<Texture2D>("Armor");
            powerupFirepower = Content.Load<Texture2D>("FirePower");
            powerupRange = Content.Load<Texture2D>("Range");
            powerupSpeed = Content.Load<Texture2D>("Speed");

            lightTank.LoadContent(graphics,Content);
            heavyTank.LoadContent(graphics,Content);
            rangedTank.LoadContent(graphics,Content);

            // TODO: use this.Content to load your game content here
        }
        //Added for garage Menu
        public void garageEntered(int actualTank)
        {
            inGarage = true;
            
            GamePadState newStateG = GamePad.GetState(PlayerIndex.One);
            KeyboardState newState = Keyboard.GetState();
            if (newState.IsKeyDown(Main.keyMap.getKey(1, 7)) && oldState != newState || GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed && oldStateG != newStateG)
            {
                if (info != 3)
                    info++;
                else
                    info = 1;

            }
            if (newState.IsKeyDown(Main.keyMap.getKey(1, 8)) && oldState != newState || GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed && oldStateG != newStateG)
            {
                if (info != 1)
                    info--;
                else
                    info = 3;
            }


            if (Keyboard.GetState().IsKeyDown(Main.keyMap.getKey(1, 9)) || GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed && oldStateG != newStateG)
            {
                this.actualTank = info;
                //if (info == 1) currTank = lightTank;
                //else if (info == 2) currTank = heavyTank;
                //else currTank = rangedTank;
            }
            oldState = newState;
            oldStateG = newStateG;

        }



        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {

            // TODO: Add your update code here
            // TODO: Add your update logic here


            if (powerupCounter == 0) powerupCounter = 60;
            switch (actualTank)
            {
                case 1:
                    if (Keyboard.GetState().IsKeyDown(Main.keyMap.getKey(1, 4)) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X < 0)
                    {
                        lightTank.RotateTurretLeft();
                    }

                    if (Keyboard.GetState().IsKeyDown(Main.keyMap.getKey(1, 5)) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X > 0)
                    {
                        lightTank.RotateTurretRight();
                    }

                    if (Keyboard.GetState().IsKeyDown(Main.keyMap.getKey(1, 2)) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0)
                    {

                        lightTank.RotateChassisLeft();
                    }

                    if (Keyboard.GetState().IsKeyDown(Main.keyMap.getKey(1, 3)) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0)
                    {
                        lightTank.RotateChassisRight();
                    }
                    if (Keyboard.GetState().IsKeyDown(Main.keyMap.getKey(1, 0)) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0)
                    {

                        lightTank.engineHeat -= 5;
                        lightTank.MoveChassis();
                        lightTank.engineHeat--;
                        if (lightTank.engineHeat <= 0)
                        {
                            lightTank.currentTankSpeed.X = 0;
                            lightTank.currentTankSpeed.Y = 0;
                        }

                        
                    }

                    else if (Keyboard.GetState().IsKeyDown(Main.keyMap.getKey(1, 1)) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < 0)
                    {

                        lightTank.engineHeat -= 5;
                        lightTank.MoveChassisReverse();
                        lightTank.engineHeat--;
                        if (lightTank.engineHeat <= 0)
                        {
                            lightTank.currentTankSpeed.X = 0;
                            lightTank.currentTankSpeed.Y = 0;
                        }

                    }
                    else if (Keyboard.GetState().IsKeyUp(Main.keyMap.getKey(1, 1)) && Keyboard.GetState().IsKeyUp(Main.keyMap.getKey(1, 0)))
                    {

                        lightTank.engineHeat++;
                    }

                    if (Keyboard.GetState().IsKeyDown(Main.keyMap.getKey(1, 6)) && canfire || GamePad.GetState(PlayerIndex.One).Triggers.Right == 1.0f)
                    {
                        GamePad.SetVibration(PlayerIndex.One, 0.5f, 0.5f);
                        lightTank.fire();
                        GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
                        canfire = false;
                    }
                    if (Keyboard.GetState().IsKeyUp(Main.keyMap.getKey(1, 6)))
                    {
                        canfire = true;
                    }

                    //Added for Garage Menu
                    if (lightTank.checkTankIntersection(recGarage))
                    {
                        garageEntered(actualTank);
                    }
                    else
                        inGarage = false;


                    

                    break;

                case 2:

                    if (Keyboard.GetState().IsKeyDown(Main.keyMap.getKey(1, 4)) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X < 0)
                    {
                        heavyTank.RotateTurretLeft();
                    }

                    if (Keyboard.GetState().IsKeyDown(Main.keyMap.getKey(1, 5)) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X > 0)
                    {
                        heavyTank.RotateTurretRight();
                    }

                    if (Keyboard.GetState().IsKeyDown(Main.keyMap.getKey(1, 2)) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0)
                    {
                        heavyTank.RotateChassisLeft();
                    }

                    if (Keyboard.GetState().IsKeyDown(Main.keyMap.getKey(1, 3)) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0)
                    {
                        heavyTank.RotateChassisRight();
                    }
                    if (Keyboard.GetState().IsKeyDown(Main.keyMap.getKey(1, 0)) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0)
                    {

                        heavyTank.engineHeat -= 5;
                        heavyTank.MoveChassis();
                        heavyTank.engineHeat--;
                        if (heavyTank.engineHeat <= 0)
                        {
                            heavyTank.currentTankSpeed.X = 0;
                            heavyTank.currentTankSpeed.Y = 0;
                        }

                    }

                    else if (Keyboard.GetState().IsKeyDown(Main.keyMap.getKey(1, 1)) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < 0)
                    {

                        heavyTank.engineHeat -= 5;
                        heavyTank.MoveChassisReverse();
                        heavyTank.engineHeat--;
                        if (heavyTank.engineHeat <= 0)
                        {
                            heavyTank.currentTankSpeed.X = 0;
                            heavyTank.currentTankSpeed.Y = 0;
                        }

                    }
                    else if (Keyboard.GetState().IsKeyUp(Main.keyMap.getKey(1, 1)) && Keyboard.GetState().IsKeyUp(Main.keyMap.getKey(1, 0)))
                    {

                        heavyTank.engineHeat++;
                    }

                    if (Keyboard.GetState().IsKeyDown(Main.keyMap.getKey(1, 6)) && canfire || GamePad.GetState(PlayerIndex.One).Triggers.Right == 1.0f)
                    {
                        GamePad.SetVibration(PlayerIndex.One, 0.5f, 0.5f);
                        heavyTank.fire();
                        GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);

                        canfire = false;
                    }
                    if (Keyboard.GetState().IsKeyUp(Main.keyMap.getKey(1, 6)))
                    {
                        canfire = true;
                    }

                    //Added for Garage Menu
                    if (heavyTank.checkTankIntersection(recGarage))
                    {
                        garageEntered(actualTank);
                    }
                    else
                        inGarage = false;
                    break;

                case 3:

                    if (Keyboard.GetState().IsKeyDown(Main.keyMap.getKey(1, 4)) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X < 0)
                    {
                        rangedTank.RotateTurretLeft();
                    }

                    if (Keyboard.GetState().IsKeyDown(Main.keyMap.getKey(1, 5)) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X > 0)
                    {
                        rangedTank.RotateTurretRight();
                    }
                    if (rangedTank.isPacked && rangedTank.wantMove)
                    {
                        if (Keyboard.GetState().IsKeyDown(Main.keyMap.getKey(1, 2)) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0)
                        {
                            rangedTank.RotateChassisLeft();
                        }

                        if (Keyboard.GetState().IsKeyDown(Main.keyMap.getKey(1, 3)) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0)
                        {
                            rangedTank.RotateChassisRight();
                        }



                        if (Keyboard.GetState().IsKeyDown(Main.keyMap.getKey(1, 0)) && rangedTank.engineHeat > 0 || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0)
                        {
                            rangedTank.engineHeat -= 5;
                            rangedTank.MoveChassis();
                            rangedTank.engineHeat--;
                        }
                        else if (Keyboard.GetState().IsKeyDown(Main.keyMap.getKey(1, 1)) && rangedTank.engineHeat > 0 || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < 0)
                        {
                            rangedTank.engineHeat -= 5;
                            rangedTank.MoveChassisReverse();
                            rangedTank.engineHeat--;
                        }
                        else if (Keyboard.GetState().IsKeyUp(Main.keyMap.getKey(1, 1)) && Keyboard.GetState().IsKeyUp(Main.keyMap.getKey(1, 0)))
                        {
                            rangedTank.engineHeat++;
                        }
                    }
                    else
                    {
                        if (Keyboard.GetState().IsKeyDown(Main.keyMap.getKey(1, 2)) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X < 0)
                        {
                            
                            if (rangedTank.firingCooldown == rangedTank.firingCooldownMax)
                            {
                                if (!rangedTank.isPacked)
                                {
                                    rangedTank.firingCooldown = 0;
                                    rangedTank.isPacked = true;
                                }
                                else
                                {
                                    rangedTank.RotateChassisLeft();
                                   
                                }
                            }
                        }

                        if (Keyboard.GetState().IsKeyDown(Main.keyMap.getKey(1, 3)) || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X > 0)
                        {
                            
                            if (rangedTank.firingCooldown == rangedTank.firingCooldownMax)
                            {
                                if (!rangedTank.isPacked)
                                {
                                    rangedTank.firingCooldown = 0;
                                    rangedTank.isPacked = true;
                                }
                                else
                                {
                                    rangedTank.RotateChassisRight();
                                }
                            
                            
                            }
                        }

                        if (Keyboard.GetState().IsKeyDown(Main.keyMap.getKey(1, 0)) && rangedTank.engineHeat > 0 || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0)
                        {
                            
                            if (rangedTank.firingCooldown == rangedTank.firingCooldownMax)
                            {
                                if (!rangedTank.isPacked)
                                {
                                    rangedTank.firingCooldown = 0;
                                    rangedTank.isPacked = true;
                                }
                                else
                                {
                                    rangedTank.engineHeat -= 5;
                                    rangedTank.MoveChassis();
                                    rangedTank.engineHeat--;
                                }
                            
                            
                            }
                        }
                        else if (Keyboard.GetState().IsKeyDown(Main.keyMap.getKey(1, 1)) && rangedTank.engineHeat > 0 || GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < 0)
                        {
                            
                            if (rangedTank.firingCooldown == rangedTank.firingCooldownMax)
                            {
                                if (!rangedTank.isPacked)
                                {
                                    rangedTank.firingCooldown = 0;
                                    rangedTank.isPacked = true;
                                }
                                else
                                {
                                    rangedTank.engineHeat -= 5;
                                    rangedTank.MoveChassisReverse();
                                    rangedTank.engineHeat--;
                                }
                            
                            
                            }
                        }
                        else if (Keyboard.GetState().IsKeyUp(Main.keyMap.getKey(1, 1)) && Keyboard.GetState().IsKeyUp(Main.keyMap.getKey(1, 0)))
                        {
                            rangedTank.engineHeat++;
                        }
                    }
                    if (Keyboard.GetState().IsKeyDown(Main.keyMap.getKey(1, 6)) && canfire || GamePad.GetState(PlayerIndex.One).Triggers.Right == 1.0f)
                    {
                        GamePad.SetVibration(PlayerIndex.One, 0.5f, 0.5f);
                        rangedTank.fire();
                        GamePad.SetVibration(PlayerIndex.One, 0.0f, 0.0f);
                        canfire = false;
                    }
                    if (Keyboard.GetState().IsKeyUp(Main.keyMap.getKey(1, 6)))
                    {
                        canfire = true;
                    }
                    // Added for garage Menu
                    if (rangedTank.checkTankIntersection(recGarage))
                    {
                        garageEntered(actualTank);
                    }
                    else
                        inGarage = false;
                   
                    break;
            }


            
            lightTank.Update(gameTime);
            heavyTank.Update(gameTime);
            rangedTank.Update(gameTime);
            //base.Update(gameTime);
        }


        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(garage, recGarage, Color.White);
            //Added for garage Menu
            if (inGarage)
            {
                switch (info)
                {
                    case 1:

                        spriteBatch.Draw(lightTankInfo, garageInfo, Color.White);
                        break;
                    case 2:
                        spriteBatch.Draw(heavyTankInfo, garageInfo, Color.White);
                        break;
                    case 3:
                        spriteBatch.Draw(rangeTankInfo, garageInfo, Color.White);
                        break;
                }
            }
            switch (actualTank)
            {

                case 1:
                    lightTank.Draw(gameTime, spriteBatch);

                    int heat = 36 * lightTank.engineHeat / lightTank.maxHeat;
                    int lightTankHealth = 36 * health / 3;
                    int cooldown = 24 * lightTank.firingCooldown / lightTank.firingCooldownMax;
                    if (lightTank.engineHeat < lightTank.maxHeat)
                    {

                        spriteBatch.Draw(hudBackground, new Rectangle((int)lightTank.screenposChassis.X + 25, (int)lightTank.screenposChassis.Y, 5, 36), Color.White);
                        spriteBatch.Draw(hudHeat, new Rectangle((int)lightTank.screenposChassis.X + 25, (int)lightTank.screenposChassis.Y, 5, heat), Color.White);
                        //spriteBatch.End();
                    }
                    if (health < 3)
                    {
                        spriteBatch.Draw(hudBackground, new Rectangle((int)lightTank.screenposChassis.X + 18, (int)lightTank.screenposChassis.Y, 5, 36), Color.White);
                        spriteBatch.Draw(hudHealth, new Rectangle((int)lightTank.screenposChassis.X + 18, (int)lightTank.screenposChassis.Y, 5, lightTankHealth), Color.White);

                    }
                    if (lightTank.firingCooldown < lightTank.firingCooldownMax)
                    {

                        spriteBatch.Draw(hudBackground, new Rectangle((int)lightTank.screenposChassis.X - 20, (int)lightTank.screenposChassis.Y, 5, 24), Color.White);
                        spriteBatch.Draw(hudFire, new Rectangle((int)lightTank.screenposChassis.X - 20, (int)lightTank.screenposChassis.Y, 5, cooldown), Color.White);
                    }
                    switch (powerupID)
                    {

                        case 0: break;
                        case 1: spriteBatch.Draw(powerupRange, new Rectangle((int)lightTank.screenposChassis.X - 20, (int)lightTank.screenposChassis.Y, 24, 24), Color.White);
                            powerupCounter--;
                            if (powerupCounter == 0) powerupID = 0;
                            break;

                        case 2: spriteBatch.Draw(powerupArmor, new Rectangle((int)lightTank.screenposChassis.X - 20, (int)lightTank.screenposChassis.Y, 24, 24), Color.White);
                            powerupCounter--;
                            if (powerupCounter == 0) powerupID = 0;
                            break;

                        case 3: spriteBatch.Draw(powerupFirepower, new Rectangle((int)lightTank.screenposChassis.X - 20, (int)lightTank.screenposChassis.Y, 24, 24), Color.White);
                            powerupCounter--;
                            if (powerupCounter == 0) powerupID = 0;
                            break;



                        case 4: spriteBatch.Draw(powerupSpeed, new Rectangle((int)lightTank.screenposChassis.X - 20, (int)lightTank.screenposChassis.Y, 24, 24), Color.White);
                            powerupCounter--;
                            if (powerupCounter == 0) powerupID = 0;
                            break;
                    }
                    break;
                case 2:
                    heavyTank.Draw(gameTime, spriteBatch);
                    heat = 36 * heavyTank.engineHeat / heavyTank.maxHeat;
                    int heavyTankHealth = 36 * health / 3;
                    int heavycooldown = 24 * heavyTank.firingCooldown / heavyTank.firingCooldownMax;
                    
                    //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                    if (heavyTank.engineHeat < heavyTank.maxHeat)
                    {
                        spriteBatch.Draw(hudBackground, new Rectangle((int)heavyTank.screenposChassis.X + 25, (int)heavyTank.screenposChassis.Y, 5, 36), Color.White);
                        spriteBatch.Draw(hudHeat, new Rectangle((int)heavyTank.screenposChassis.X + 25, (int)heavyTank.screenposChassis.Y, 5, heat), Color.White);
                        //spriteBatch.End();
                    }
                    if (heavyTank.firingCooldown < heavyTank.firingCooldownMax)
                    {
                        spriteBatch.Draw(hudBackground, new Rectangle((int)heavyTank.screenposChassis.X - 20, (int)heavyTank.screenposChassis.Y, 5, 24), Color.White);
                        spriteBatch.Draw(hudFire, new Rectangle((int)heavyTank.screenposChassis.X - 20, (int)heavyTank.screenposChassis.Y, 5, heavycooldown), Color.White);
                    }
                    if (health < 3)
                    {
                        spriteBatch.Draw(hudBackground, new Rectangle((int)heavyTank.screenposChassis.X + 18, (int)heavyTank.screenposChassis.Y, 5, 36), Color.White);
                        spriteBatch.Draw(hudHealth, new Rectangle((int)heavyTank.screenposChassis.X + 18, (int)heavyTank.screenposChassis.Y, 5, heavyTankHealth), Color.White);
                    }

                    switch (powerupID)
                    {
                        case 0: break;
                        case 1: spriteBatch.Draw(powerupRange, new Rectangle((int)heavyTank.screenposChassis.X - 20, (int)heavyTank.screenposChassis.Y, 24, 24), Color.White);
                            powerupCounter--;
                            if (powerupCounter == 0) powerupID = 0;
                            break;
                        case 2: spriteBatch.Draw(powerupArmor, new Rectangle((int)heavyTank.screenposChassis.X - 20, (int)heavyTank.screenposChassis.Y, 24, 24), Color.White);
                            powerupCounter--;
                            if (powerupCounter == 0) powerupID = 0;
                            break;
                        case 3: spriteBatch.Draw(powerupFirepower, new Rectangle((int)heavyTank.screenposChassis.X - 20, (int)heavyTank.screenposChassis.Y, 24, 24), Color.White);

                            powerupCounter--;
                            if (powerupCounter == 0) powerupID = 0;
                            break;
                        case 4: spriteBatch.Draw(powerupSpeed, new Rectangle((int)heavyTank.screenposChassis.X - 20, (int)heavyTank.screenposChassis.Y, 24, 24), Color.White);
                            powerupCounter--;
                            if (powerupCounter == 0) powerupID = 0;
                            break;

                    }

                    break;
                case 3:
                    rangedTank.Draw(gameTime, spriteBatch);
                    heat = 36 * rangedTank.engineHeat / rangedTank.maxHeat;
                    int rangedTankHealth = 36 * health / 3;
                    int rangedcooldown = 24 * rangedTank.firingCooldown / rangedTank.firingCooldownMax;
                    
                    //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
                    if (rangedTank.engineHeat < rangedTank.maxHeat)
                    {
                        spriteBatch.Draw(hudBackground, new Rectangle((int)rangedTank.screenposChassis.X + 25, (int)rangedTank.screenposChassis.Y, 5, 36), Color.White);
                        spriteBatch.Draw(hudHeat, new Rectangle((int)rangedTank.screenposChassis.X + 25, (int)rangedTank.screenposChassis.Y, 5, heat), Color.White);
                        //spriteBatch.End();
                    }
                    if (rangedTank.firingCooldown < rangedTank.firingCooldownMax)
                    {
                        spriteBatch.Draw(hudBackground, new Rectangle((int)rangedTank.screenposChassis.X - 20, (int)rangedTank.screenposChassis.Y, 5, 24), Color.White);
                        spriteBatch.Draw(hudFire, new Rectangle((int)rangedTank.screenposChassis.X - 20, (int)rangedTank.screenposChassis.Y, 5, rangedcooldown), Color.White);
                    }
                    if (health < 3)
                    {
                        spriteBatch.Draw(hudBackground, new Rectangle((int)rangedTank.screenposChassis.X + 18, (int)rangedTank.screenposChassis.Y, 5, 36), Color.White);
                        spriteBatch.Draw(hudHealth, new Rectangle((int)rangedTank.screenposChassis.X + 18, (int)rangedTank.screenposChassis.Y, 5, rangedTankHealth), Color.White);
                    }

                    switch (powerupID)
                    {
                        case 0: break;
                        case 1: spriteBatch.Draw(powerupRange, new Rectangle((int)rangedTank.screenposChassis.X - 20, (int)rangedTank.screenposChassis.Y, 24, 24), Color.White);
                            powerupCounter--;
                            if (powerupCounter == 0) powerupID = 0;
                            break;
                        case 2: spriteBatch.Draw(powerupArmor, new Rectangle((int)rangedTank.screenposChassis.X - 20, (int)rangedTank.screenposChassis.Y, 24, 24), Color.White);
                            powerupCounter--;
                            if (powerupCounter == 0) powerupID = 0;
                            break;
                        case 3: spriteBatch.Draw(powerupFirepower, new Rectangle((int)rangedTank.screenposChassis.X - 20, (int)rangedTank.screenposChassis.Y, 24, 24), Color.White);

                            powerupCounter--;
                            if (powerupCounter == 0) powerupID = 0;
                            break;
                        case 4: spriteBatch.Draw(powerupSpeed, new Rectangle((int)rangedTank.screenposChassis.X - 20, (int)rangedTank.screenposChassis.Y, 24, 24), Color.White);
                            powerupCounter--;
                            if (powerupCounter == 0) powerupID = 0;
                            break;

                    }

                    break;
            }


        }

    }
}
