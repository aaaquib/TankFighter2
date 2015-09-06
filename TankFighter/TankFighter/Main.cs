using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MapLibrary;
namespace TankFighter
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Main : Microsoft.Xna.Framework.GameComponent
    {
        int mapChoose = 1;
        int firstMap = 1;
        GraphicsDevice m_graphics_device;
        SpriteBatch m_spriteBatch;
        PlayerOne playerOne;
        PlayerTwo playerTwo;

        List<Object> environmentObjects;
        int ObjectCount = 0;
        List<MapItem> mapItems;
        List<TankOrigins> tankOrigins;
        List<MapProperties> mapProperties;

        //background
        Texture2D world;
        static Rectangle worldRectangle;
        Texture2D instruction;
        
        public int mapCounter = 0;
        SoundEffect powerup;
        SoundEffect treeCrushing;
        SoundEffect crateCrushing;
        SoundEffect click;
        Song bgMusic;
        MediaLibrary mediaLibrary;
        //PlayerAI playerAi;
        float x, y;

         public static bool isVictoryOne,isVictoryTwo;

        //Main Menu /////////////////////////////////////////////////////
        Boolean isMenu, isInstruction, isSettings, isKey, isLoading = false;
        //Texture2D menu_43, menu_169, menuBox, arrow;
        Rectangle menuRectangle43, menuRectangle169, instructionRectangle, instructionRectangle2, instructionRectangle3, settingRec, menuBoxRec, arrowRec, rangeRec, powerRec, speedRec, armorRec, keyRec;
        float aspectRatio, width, height;
        Color color;
        SpriteFont font;
        int arrowX = 555, arrowY = 40, selection = 0,  players = 2, map = 1, settingSelection = 0, keySelection = 0;
        public KeyboardState oldState;
        public static KeyBoardMap keyMap;
        GamePadState oldStateG;
        //Added///////////////////////////////////////////////////////

        Effect damageFX;
        float percent1, percent2;

        public static Camera2d cam_p1;
        public static Camera2d cam_p2;

        Viewport view1;
        Viewport view2;
        Viewport view3;

        Radar radar;

        //static Main instance = null;
        private static ContentManager m_content;
        Game m_game;

        //public static Main Instance(ContentManager content)
        //{
        //    instance = new Main();
        //    m_content = content;
        //    //instance.setContentManger(content);
        //    //instance.setWindowLimits(WindowWidth, WindowHeight);

        //    //cam = new Camera2d();
        //    //cam.Pos = new Vector2(GameOptions.getWidth() / 2, GameOptions.getHeight() / 2);

        //    return instance;
        //}

        public Main(Game1 game) : base(game)
        {
            m_game = game;
            isVictoryOne = false;
            isVictoryTwo = false;
            playerOne = new PlayerOne();
            playerTwo = new PlayerTwo();
            
            m_graphics_device = game.GraphicsDevice;
            m_content = game.Content;
            //m_content.RootDirectory = "Content";
            //width = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            //height = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            worldRectangle = new Rectangle(0, 0, 2048, 1539);
            //playerAi = new PlayerAI(this);
            //Main Menu///////////////////////////////////////////////
            keyMap = new KeyBoardMap();            
            aspectRatio = width / height;
            isMenu = false;
            isInstruction = false;
            isSettings = false;
            menuRectangle43 = new Rectangle(0, 0, 800, 550);
            menuRectangle169 = new Rectangle(0, 0, 800, 500);
            menuRectangle43 = new Rectangle(0, 0, (int)width, (int)height);
            menuRectangle169 = new Rectangle(0, 0, (int)width, (int)height);
            menuBoxRec = new Rectangle(550, 10, 240, 120);
            arrowRec = new Rectangle(arrowX, arrowY, 12, 12);
            color = new Color(0, 0, 0, 85);
            instructionRectangle = new Rectangle(150, 150, 600, 350);
            instructionRectangle2 = new Rectangle(300, 140, 240, 320);
            instructionRectangle3 = new Rectangle(50, 140, 240, 320);
            settingRec = new Rectangle(550, 140, 240, 320);
            keyRec = new Rectangle(50, 140, 480, 320);
            rangeRec = new Rectangle(180, 300, 25, 25);
            powerRec = new Rectangle(180, 325, 25, 25);
            armorRec = new Rectangle(180, 350, 25, 25);
            speedRec = new Rectangle(180, 375, 25, 25);
            //Added///////////////////////////////////////////////////
            mediaLibrary = new MediaLibrary();

            GameOptions.setContent(m_content);
        }

        public static int getWorldWidth()
        {
            return worldRectangle.Width;
        }

        public static int getWorldHeight()
        {
            return worldRectangle.Height;
        }

        public void LoadMapData()
        {
            //Load Map data onto separate variables
            XmlDocument reader = new XmlDocument();
            //reader.Load("MapObj.xml");
            //reader.Load("level1.xml");
            switch (mapCounter)
            {
                case 0: reader.Load("level1.xml"); break;
                case 1: reader.Load("level2.xml"); break;
                case 2: reader.Load("level3.xml"); break;
                case 3: reader.Load("level4.xml"); break;
                case 4: reader.Load("level5.xml"); break;
                case 5: reader.Load("MapObj.xml"); break;

            }
            //from the read in - create an XmlNodeList 
            XmlNodeList allNodes = reader.ChildNodes;
            foreach (XmlNode rootNode in allNodes)
            {
                if (rootNode.Name == "OBJ")
                {
                    // make another XmlNodeList - this time pulling all the childnodes nested in <HUDSettings>
                    XmlNodeList rootChildren = rootNode.ChildNodes;
                    foreach (XmlNode node in rootChildren)
                    {
                        if (node.Name == "envObj")
                        {
                            mapItems.Add(new MapItem(node));
                            ObjectCount++;
                        }

                        if (node.Name == "mapProperty")
                        {
                            mapProperties.Add(new MapProperties(node));
                        }

                        if (node.Name == "playerPosition")
                        {
                            tankOrigins.Add(new TankOrigins(node));
                        }

                    }
                }
            }

        }

        public void CreateMapObjects()
        {
            int counter = 0;
            //Create map objects here.
            foreach (MapItem mapItem in mapItems)
            {
                environmentObjects.Add(new Object(Game, mapItem.ID, mapItem.type, mapItem.visible, mapItem.X, mapItem.Y));
                counter++;
            }
            foreach (MapItem mapItem in mapItems)
            {
                environmentObjects.Add(new Object(Game, mapItem.ID, mapItem.type, mapItem.visible, 2*mapItem.X, 2*mapItem.Y));
                counter++;
            }
            environmentObjects.ToArray();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization logic here
            mapItems = new List<MapItem>();
            environmentObjects = new List<Object>();
            tankOrigins = new List<TankOrigins>();
            mapProperties = new List<MapProperties>();
            LoadMapData();
            percent1 = 0;
            percent2 = 0;
            CreateMapObjects();
            foreach (Object k in environmentObjects)
            {
                k.Initialize();
            }
            playerOne.Initialize();
            playerTwo.Initialize();
            //playerAi.Initialize();

            cam_p1 = new Camera2d();
            cam_p2 = new Camera2d();

            view1 = new Viewport(0, 0, GameOptions.getWidth() / 2 - 2, GameOptions.getHeight());
            view2 = new Viewport(GameOptions.getWidth() / 2 + 2, 0, GameOptions.getWidth() / 2 - 2, GameOptions.getHeight());
            view3 = new Viewport(0, 0, GameOptions.getWidth(), GameOptions.getHeight());

            cam_p1.Pos = playerOne.lightTank.screenposChassis;
            cam_p2.Pos = playerTwo.lightTank.screenposChassis;

            //base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        public void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            m_spriteBatch = new SpriteBatch(m_graphics_device);
            playerOne.LoadContent(m_graphics_device,m_content);
            playerTwo.LoadContent(m_graphics_device, m_content);
            radar = new Radar(m_content, "PlayerOneSmall", "PlayerTwoSmall", "Rectangle");
            //playerAi.LoadContent(graphics);
            world = m_content.Load<Texture2D>("desert");
            worldRectangle = new Rectangle(0, 0, world.Width, world.Height);
            //menu_43 = this.Content.Load<Texture2D>("Menu_BG");
            //menu_169 = this.Content.Load<Texture2D>("Menu_BG2");
            font = m_content.Load<SpriteFont>("spriteFont");
            damageFX = m_content.Load<Effect>("Damage");
            //menuBox = Content.Load<Texture2D>("Rectangle");
            //arrow = Content.Load<Texture2D>("arrow");
            //instruction = m_content.Load<Texture2D>("TFinstructionscreen");
            
            powerup = m_content.Load<SoundEffect>("PowerUp");
            treeCrushing = m_content.Load<SoundEffect>("tree");
            crateCrushing = m_content.Load<SoundEffect>("wood_crate");
            //click = m_content.Load<SoundEffect>("Menu_Click");
            bgMusic = m_content.Load<Song>("Game_Music1");
            MediaPlayer.Play(bgMusic);
            MediaPlayer.IsRepeating = true;
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        //public void newGame()
        //{
        //    isVictoryOne = false;
        //    isVictoryTwo = false;
        //    playerOne = new PlayerOne();
        //    playerTwo = new PlayerTwo();
        //    backGroundRectangle = new Rectangle(0, 0, 2048, 1539);
        //}

        //KeyMapper///////////////////////////////////////////
        public Keys checkKeys()
        {

            if (Keyboard.GetState().IsKeyDown(Keys.D1))
                return Keys.D1;
            if (Keyboard.GetState().IsKeyDown(Keys.D2))
                return Keys.D2;
            if (Keyboard.GetState().IsKeyDown(Keys.D3))
                return Keys.D3;
            if (Keyboard.GetState().IsKeyDown(Keys.D4))
                return Keys.D4;
            if (Keyboard.GetState().IsKeyDown(Keys.D5))
                return Keys.D5;
            if (Keyboard.GetState().IsKeyDown(Keys.D6))
                return Keys.D6;
            if (Keyboard.GetState().IsKeyDown(Keys.D7))
                return Keys.D7;
            if (Keyboard.GetState().IsKeyDown(Keys.D8))
                return Keys.D8;
            if (Keyboard.GetState().IsKeyDown(Keys.D9))
                return Keys.D9;
            if (Keyboard.GetState().IsKeyDown(Keys.D0))
                return Keys.D0;
            if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
                return Keys.OemMinus;
            if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
                return Keys.OemPlus;
            if (Keyboard.GetState().IsKeyDown(Keys.OemTilde))
                return Keys.OemTilde;
            if (Keyboard.GetState().IsKeyDown(Keys.Tab))
                return Keys.Tab;
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
                return Keys.Q;
            if (Keyboard.GetState().IsKeyDown(Keys.W))
                return Keys.W;
            if (Keyboard.GetState().IsKeyDown(Keys.E))
                return Keys.E;
            if (Keyboard.GetState().IsKeyDown(Keys.R))
                return Keys.R;
            if (Keyboard.GetState().IsKeyDown(Keys.T))
                return Keys.T;
            if (Keyboard.GetState().IsKeyDown(Keys.Y))
                return Keys.Y;
            if (Keyboard.GetState().IsKeyDown(Keys.U))
                return Keys.U;
            if (Keyboard.GetState().IsKeyDown(Keys.I))
                return Keys.I;
            if (Keyboard.GetState().IsKeyDown(Keys.O))
                return Keys.O;
            if (Keyboard.GetState().IsKeyDown(Keys.P))
                return Keys.P;
            if (Keyboard.GetState().IsKeyDown(Keys.Oem8))
                return Keys.Oem8;
            if (Keyboard.GetState().IsKeyDown(Keys.OemAuto))
                return Keys.OemAuto;
            if (Keyboard.GetState().IsKeyDown(Keys.OemBackslash))
                return Keys.OemBackslash;
            if (Keyboard.GetState().IsKeyDown(Keys.OemClear))
                return Keys.OemClear;
            if (Keyboard.GetState().IsKeyDown(Keys.OemCloseBrackets))
                return Keys.OemCloseBrackets;
            if (Keyboard.GetState().IsKeyDown(Keys.OemComma))
                return Keys.OemComma;
            if (Keyboard.GetState().IsKeyDown(Keys.OemCopy))
                return Keys.OemCopy;
            if (Keyboard.GetState().IsKeyDown(Keys.OemEnlW))
                return Keys.OemEnlW;
            if (Keyboard.GetState().IsKeyDown(Keys.OemOpenBrackets))
                return Keys.OemOpenBrackets;
            if (Keyboard.GetState().IsKeyDown(Keys.OemPeriod))
                return Keys.OemPeriod;
            if (Keyboard.GetState().IsKeyDown(Keys.OemPipe))
                return Keys.OemPipe;
            if (Keyboard.GetState().IsKeyDown(Keys.OemQuestion))
                return Keys.OemQuestion;
            if (Keyboard.GetState().IsKeyDown(Keys.OemQuotes))
                return Keys.OemQuotes;
            if (Keyboard.GetState().IsKeyDown(Keys.OemSemicolon))
                return Keys.OemSemicolon;
            if (Keyboard.GetState().IsKeyDown(Keys.CapsLock))
                return Keys.CapsLock;
            if (Keyboard.GetState().IsKeyDown(Keys.A))
                return Keys.A;
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                return Keys.S;
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                return Keys.D;
            if (Keyboard.GetState().IsKeyDown(Keys.F))
                return Keys.F;
            if (Keyboard.GetState().IsKeyDown(Keys.G))
                return Keys.G;
            if (Keyboard.GetState().IsKeyDown(Keys.H))
                return Keys.H;
            if (Keyboard.GetState().IsKeyDown(Keys.J))
                return Keys.J;
            if (Keyboard.GetState().IsKeyDown(Keys.K))
                return Keys.K;
            if (Keyboard.GetState().IsKeyDown(Keys.L))
                return Keys.L;
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                return Keys.Enter;
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                return Keys.LeftShift;
            if (Keyboard.GetState().IsKeyDown(Keys.LeftControl))
                return Keys.LeftControl;
            if (Keyboard.GetState().IsKeyDown(Keys.LeftAlt))
                return Keys.LeftAlt;
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                return Keys.Left;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                return Keys.Right;
            if (Keyboard.GetState().IsKeyDown(Keys.RightAlt))
                return Keys.RightAlt;
            if (Keyboard.GetState().IsKeyDown(Keys.RightControl))
                return Keys.RightControl;
            if (Keyboard.GetState().IsKeyDown(Keys.RightShift))
                return Keys.RightShift;
            if (Keyboard.GetState().IsKeyDown(Keys.Z))
                return Keys.Z;
            if (Keyboard.GetState().IsKeyDown(Keys.X))
                return Keys.X;
            if (Keyboard.GetState().IsKeyDown(Keys.C))
                return Keys.C;
            if (Keyboard.GetState().IsKeyDown(Keys.V))
                return Keys.V;
            if (Keyboard.GetState().IsKeyDown(Keys.B))
                return Keys.B;
            if (Keyboard.GetState().IsKeyDown(Keys.N))
                return Keys.N;
            if (Keyboard.GetState().IsKeyDown(Keys.M))
                return Keys.M;
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                return Keys.Up;
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                return Keys.Down;
            if (Keyboard.GetState().IsKeyDown(Keys.Delete))
                return Keys.Delete;
            if (Keyboard.GetState().IsKeyDown(Keys.PageDown))
                return Keys.PageDown;
            if (Keyboard.GetState().IsKeyDown(Keys.PageUp))
                return Keys.PageUp;
            if (Keyboard.GetState().IsKeyDown(Keys.Insert))
                return Keys.Insert;
            if (Keyboard.GetState().IsKeyDown(Keys.Back))
                return Keys.End;
            if (Keyboard.GetState().IsKeyDown(Keys.Home))
                return Keys.Home;
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
                return Keys.Space;
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad0))
                return Keys.NumPad0;
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad1))
                return Keys.NumPad1;
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad2))
                return Keys.NumPad2;
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad3))
                return Keys.NumPad3;
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad4))
                return Keys.NumPad4;
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad5))
                return Keys.NumPad5;
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad6))
                return Keys.NumPad6;
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad7))
                return Keys.NumPad7;
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad8))
                return Keys.NumPad8;
            if (Keyboard.GetState().IsKeyDown(Keys.NumPad9))
                return Keys.NumPad9;

            else
                return Keys.None;


        }
        //Added///////////////////////////////////////////////

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            //if (isMenu)
            //{
            //    MediaPlayer.Resume();
            //    if (mapChoose != firstMap)
            //    {
            //        mapCounter = map - 1;
            //        mapItems = new List<MapItem>();
            //        environmentObjects = new List<Object>();
            //        tankOrigins = new List<TankOrigins>();
            //        mapProperties = new List<MapProperties>();
            //        LoadMapData();
            //        CreateMapObjects();
            //        foreach (Object k in environmentObjects)
            //        {
            //            k.Initialize();
            //        }
            //        firstMap = mapChoose;
            //        isLoading = false;
            //    }
            //    mapChoose= map;
               
            //}
            if (playerOne.actualTank == 1)
            {
                cam_p1.Move(playerOne.lightTank.screenposChassis);
            }
            else if (playerOne.actualTank == 2)
            {
                cam_p1.Move(playerOne.heavyTank.screenposChassis);
            }
            else if (playerOne.actualTank == 3)
            {
                cam_p1.Move(playerOne.rangedTank.screenposChassis);
            }
            if (playerTwo.actualTank == 1)
            {
                cam_p2.Move(playerTwo.lightTank.screenposChassis);
            }
            else if (playerTwo.actualTank == 2)
            {
                cam_p2.Move(playerTwo.heavyTank.screenposChassis);
            }
            else if (playerTwo.actualTank == 3)
            {
                cam_p2.Move(playerTwo.rangedTank.screenposChassis);
            }

            float percentScale = 0.05f;
            if (percent1 > 0)
            {
                percent1 -= percentScale;
                damageFX.Parameters["percent"].SetValue(percent1);
            }
            if (percent2 > 0)
            {
                percent2 -= percentScale;
                damageFX.Parameters["percent"].SetValue(percent2);
            }

            #region Main Menu
            //ADDED//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            KeyboardState newState = Keyboard.GetState();
            GamePadState newStateG = GamePad.GetState(PlayerIndex.One);
            //if (!isMenu)
            //{
            //    MediaPlayer.Pause();
            //    if (Keyboard.GetState().IsKeyDown(Keys.End) && oldState != newState || GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed && oldStateG != newStateG)
            //    {
            //        isMenu = true;
            //    }
            //}
            //else
            //{
            //    if (!isSettings)
            //    {
            //        if (Keyboard.GetState().IsKeyDown(Keys.Down) && oldState != newState || GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed && oldStateG != newStateG)
            //        {
            //            click.Play();
            //            selection++;
            //            if (selection == 4)
            //                selection = 0;
            //        }
            //        if (Keyboard.GetState().IsKeyDown(Keys.Up) && oldState != newState || GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed && oldStateG != newStateG)
            //        {
            //            click.Play();
            //            selection--;
            //            if (selection == -1)
            //                selection = 3;
            //        }
            //        arrowRec.Y = 30 + (25 * selection);
            //    }
            //    else
            //    {
            //        if (Keyboard.GetState().IsKeyDown(Keys.Down) && oldState != newState || GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed && oldStateG != newStateG)
            //        {
            //            click.Play();
            //            if (!isKey)
            //            {
            //                settingSelection++;
            //                if (settingSelection == 4)
            //                    settingSelection = 0;
            //            }
            //            else
            //            {
            //                keySelection++;
            //                if (keySelection == 20)
            //                    keySelection = 0;
            //            }
            //        }
            //        if (Keyboard.GetState().IsKeyDown(Keys.Up) && oldState != newState || GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed && oldStateG != newStateG)
            //        {
            //            click.Play();
            //            if (!isKey)
            //            {
            //                settingSelection--;
            //                if (settingSelection == -1)
            //                    settingSelection = 3;
            //            }
            //            else
            //            {
            //                keySelection--;
            //                if (keySelection == -1)
            //                    keySelection = 19;
            //            }
            //        }
            //        if (Keyboard.GetState().IsKeyDown(Keys.Right) && oldState != newState || GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed && oldStateG != newStateG)
            //        {
            //            click.Play();
            //            switch (settingSelection)
            //            {
            //                case 0:
            //                    players++;
            //                    if (players == 3)
            //                        players = 1;
            //                    break;
            //                case 1:
            //                    isLoading = true;
            //                    map++;
                                
            //                    if (map == 6)
            //                        map = 1;
            //                    break;
            //            }
            //        }
            //        if (Keyboard.GetState().IsKeyDown(Keys.Left) && oldState != newState || GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed && oldStateG != newStateG)
            //        {
            //            click.Play();
            //            switch (settingSelection)
            //            {
            //                case 0:
            //                    players--;
            //                    if (players == 0)
            //                        players = 2;
            //                    break;
            //                case 1:
            //                    isLoading = true;
            //                    map--;
            //                    if (map == 0)
            //                        map = 3;
            //                    break;
                            

            //            }
            //        }
            //        if (isKey)
            //        {
            //            if (keySelection <= 9)
            //            {
            //                arrowRec.X = 50;
            //                arrowRec.Y = 185 + (22 * keySelection);
            //            }
            //            else
            //            {
            //                arrowRec.X = 300;
            //                arrowRec.Y = 185 + (22 * (keySelection - 10));
            //            }
            //        }
            //        else
            //        {
            //            arrowRec.X = 555;
            //            arrowRec.Y = 170 + (23 * settingSelection);
            //        }
            //    }
            //    if (Keyboard.GetState().IsKeyDown(Keys.Enter) && oldState != newState || GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed && oldStateG != newStateG)
            //    {
            //        click.Play();
            //        if (settingSelection == 2)
            //        {
            //            if (isKey)
            //                isKey = false;
            //            else
            //                isKey = true;
            //        }
            //        else
            //        {
            //            isKey = false;
            //            switch (selection)
            //            {
            //                case 0:
            //                    isMenu = false;
            //                    break;
            //                case 1:
            //                    isSettings = false;
            //                    if (isInstruction)
            //                        isInstruction = false;
            //                    else
            //                        isInstruction = true;
            //                    break;
            //                case 2:
            //                    isInstruction = false;
            //                    if (isSettings)
            //                        isSettings = false;
            //                    else
            //                        isSettings = true;
            //                    break;
            //                case 3:
            //                    this.Exit();
            //                    break;
            //            }
            //        }
            //    }

                //if (isKey)
                //{
                //    if (keySelection <= 9)
                //    {
                //        if (Keyboard.GetState().IsKeyDown(Keys.End))
                //            keyMap.setKey(1, keySelection, checkKeys());
                //    }
                //    else
                //    {
                //        if (Keyboard.GetState().IsKeyDown(Keys.End))
                //            keyMap.setKey(2, keySelection - 10, checkKeys());
                //    }

                //}
            //}
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            #endregion

            // Allows the game to exit
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            //    Game.Exit();

            //if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    //isMenu = true;
            //    Game.Exit();

            //if (GameOptions.isGameOver())
            //{
            //    if (Keyboard.GetState().IsKeyDown(Keys.N) || GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed && oldStateG != newStateG)
            //    {
            //        GameOptions.gameOver();
            //    }
                //if (Keyboard.GetState().IsKeyDown(Keys.Y) || GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed && oldStateG != newStateG)
                //{
                //    //mapCounter++;
                //    //mapCounter = mapCounter % 6;
                //    playerOne = new PlayerOne(m_game);
                //    playerTwo = new PlayerTwo(m_game);
                //    //playerAi = new PlayerAI(this);
                //    //mapItems = new List<MapItem>();
                //    //environmentObjects = new List<Object>();
                //    //tankOrigins = new List<TankOrigins>();
                //    //mapProperties = new List<MapProperties>();
                //    //LoadMapData();
                //    //background = this.Content.Load<Texture2D>("Sprites/grass");
                //    //menu_43 = this.Content.Load<Texture2D>("TFmenuscreen");
                //    instruction = m_content.Load<Texture2D>("TFinstructionscreen");
                //    playerOneWins = m_content.Load<Texture2D>("PlayerOneWin");
                //    playerTwoWins = m_content.Load<Texture2D>("PlayerTwoWin");
                //    //CreateMapObjects();
                //    //foreach (Object k in environmentObjects)
                //    //{
                //    //    k.Initialize();
                //    //}
                //    playerOne.Initialize();
                //    playerTwo.Initialize();
                //    isVictory = false;                    
                //    base.Initialize();
                //}
            //}
            // TODO: Add your update logic here
            

            // COLLISION DETECTION //
            #region Bullet vs Objects
            //if (playerAi.lightTank.speedFactor > -1)
            //{
            //    playerAi.lightTank.speedFactor *= -1;
            //}
            //if (playerAi.heavyTank.speedFactor > -1)
            //{
            //    playerAi.heavyTank.speedFactor *= -1;
            //}

            if (playerOne.lightTank.speedFactor > -1)
            {
                playerOne.lightTank.speedFactor *= -1;
            }

            if (playerOne.heavyTank.speedFactor > -1)
            {
                playerOne.heavyTank.speedFactor *= -1;
            }
            if (playerOne.rangedTank.speedFactor > -1)
            {
                playerOne.rangedTank.speedFactor *= -1;
            }
            if (playerTwo.lightTank.speedFactor > -1)
            {
                playerTwo.lightTank.speedFactor *= -1;
            }

            if (playerTwo.heavyTank.speedFactor > -1)
            {
                playerTwo.heavyTank.speedFactor *= -1;
            }
            if (playerTwo.rangedTank.speedFactor > -1)
            {
                playerTwo.rangedTank.speedFactor *= -1;
            }


            #endregion

            #region Tank vs All Objects
            x = playerOne.lightTank.currentTankSpeed.X;
            y = playerOne.lightTank.currentTankSpeed.Y;
            foreach (Object obj in environmentObjects)
            {
                if (obj.visible)
                {
                    if (obj.solid)
                    {
                        if (playerOne.actualTank == 1)
                        {

                            if (playerOne.lightTank.checkTankIntersection(obj.rectangle))
                            {
                                if (obj.destructible)
                                {
                                    switch (obj.type)
                                    {
                                        case "tree":
                                        case "tree1":
                                        case "tree2":
                                        case "tree3":
                                        case "bush1":
                                            treeCrushing.Play();
                                            break;

                                        case "crates": crateCrushing.Play();
                                            break;
                                    }
                                    obj.visible = false;
                                    obj.solid = false;
                                }
                                else
                                {
                                    if (obj.type.Equals("rock1") || obj.type.Equals("rock2") || obj.type.Equals("rock3"))
                                    {
                                        playerOne.lightTank.speedFactor *= -1;
                                        playerOne.lightTank.MoveChassis();
                                        playerOne.lightTank.MoveChassisReverse();
                                    }
                                    else if (obj.type.Equals("powerup_range") && obj.visible)
                                    {
                                        powerup.Play();
                                        playerOne.powerupID = 1;
                                        playerOne.lightTank.rangeDefault += 10;
                                        obj.visible = false;
                                    }
                                    else if (obj.type.Equals("powerup_armor") && obj.visible)
                                    {

                                        powerup.Play();
                                        playerOne.powerupID = 2;
                                        playerOne.health += 1;
                                        obj.visible = false;
                                    }
                                    else if (obj.type.Equals("powerup_firepower") && obj.visible)
                                    {
                                        powerup.Play();
                                        playerOne.powerupID = 3;
                                        obj.visible = false;
                                        playerOne.lightTank.firePower++;
                                    }
                                    else if (obj.type.Equals("powerup_speed") && obj.visible)
                                    {
                                        powerup.Play();
                                        playerOne.powerupID = 4;
                                        playerOne.lightTank.speedFactor -= 1;
                                        obj.visible = false;
                                    }

                                }

                            }
                            if (playerOne.lightTank.checkBulletIntersection(obj.rectangle))
                            {
                                switch (obj.type)
                                {
                                    case "tree":
                                    case "tree1":
                                    case "tree2":
                                    case "tree3":
                                    case "bush1":
                                        treeCrushing.Play();
                                        break;

                                    case "crates": crateCrushing.Play();
                                        break;
                                }
                                if (obj.destructible)
                                {
                                    obj.visible = false;
                                    obj.solid = false;
                                }

                            }
                            obj.Update(gameTime);
                        }

                        else if (playerOne.actualTank == 2)
                        {
                            if (playerOne.heavyTank.checkTankIntersection(obj.rectangle))
                            {
                                switch (obj.type)
                                {
                                    case "tree":
                                    case "tree1":
                                    case "tree2":
                                    case "tree3":
                                    case "bush1":
                                        treeCrushing.Play();
                                        break;

                                    case "crates": crateCrushing.Play();
                                        break;
                                }
                                if (obj.destructible)
                                {
                                    obj.visible = false;
                                    obj.solid = false;
                                }
                                else
                                {
                                    if (obj.type.Equals("rock1") || obj.type.Equals("rock2") || obj.type.Equals("rock3"))
                                    {
                                        playerOne.heavyTank.speedFactor *= -1;
                                        //playerOne.heavyTank.speedFactor *= -1;
                                        playerOne.heavyTank.MoveChassis();
                                        playerOne.heavyTank.MoveChassisReverse();
                                    }
                                    else if (obj.type.Equals("powerup_range") && obj.visible)
                                    {
                                        powerup.Play();
                                        playerOne.powerupID = 1;
                                        playerOne.heavyTank.rangeDefault += 10;
                                        obj.visible = false;
                                    }
                                    else if (obj.type.Equals("powerup_armor") && obj.visible)
                                    {

                                        powerup.Play();
                                        playerOne.powerupID = 2;
                                        playerOne.health += 1;
                                        obj.visible = false;
                                    }
                                    else if (obj.type.Equals("powerup_firepower") && obj.visible)
                                    {
                                        powerup.Play();
                                        playerOne.powerupID = 3;
                                        obj.visible = false;
                                        playerOne.heavyTank.firePower++;
                                    }
                                    else if (obj.type.Equals("powerup_speed") && obj.visible)
                                    {
                                        powerup.Play();
                                        playerOne.powerupID = 4;
                                        playerOne.heavyTank.speedFactor -= 1;
                                        obj.visible = false;
                                    }
                                }
                            }
                            if (playerOne.heavyTank.checkBulletIntersection(obj.rectangle))
                            {
                                if (obj.destructible)
                                {
                                    switch (obj.type)
                                    {
                                        case "tree":
                                        case "tree1":
                                        case "tree2":
                                        case "tree3":
                                        case "bush1":
                                            treeCrushing.Play();
                                            break;

                                        case "crates": crateCrushing.Play();
                                            break;
                                    }


                                    obj.visible = false;
                                    obj.solid = false;
                                }
                            }
                            obj.Update(gameTime);
                        }
                        else
                        {
                            if (playerOne.rangedTank.checkTankIntersection(obj.rectangle))
                            {
                                switch (obj.type)
                                {
                                    case "tree":
                                    case "tree1":
                                    case "tree2":
                                    case "tree3":
                                    case "bush1":
                                        treeCrushing.Play();
                                        break;

                                    case "crates": crateCrushing.Play();
                                        break;
                                }
                                if (obj.destructible)
                                {
                                    obj.visible = false;
                                    obj.solid = false;
                                }
                                else
                                {
                                    if (obj.type.Equals("rock1") || obj.type.Equals("rock2") || obj.type.Equals("rock3"))
                                    {
                                        playerOne.rangedTank.speedFactor *= -1;
                                        //playerOne.heavyTank.speedFactor *= -1;
                                        playerOne.rangedTank.MoveChassis();
                                        playerOne.rangedTank.MoveChassisReverse();
                                    }
                                    else if (obj.type.Equals("powerup_range") && obj.visible)
                                    {
                                        powerup.Play();
                                        playerOne.powerupID = 1;
                                        playerOne.rangedTank.rangeDefault += 10;
                                        obj.visible = false;
                                    }
                                    else if (obj.type.Equals("powerup_armor") && obj.visible)
                                    {

                                        powerup.Play();
                                        playerOne.powerupID = 2;
                                        playerOne.health += 1;
                                        obj.visible = false;
                                    }
                                    else if (obj.type.Equals("powerup_firepower") && obj.visible)
                                    {
                                        powerup.Play();
                                        playerOne.powerupID = 3;
                                        obj.visible = false;
                                        playerOne.rangedTank.firePower++;
                                    }
                                    else if (obj.type.Equals("powerup_speed") && obj.visible)
                                    {
                                        powerup.Play();
                                        playerOne.powerupID = 4;
                                        playerOne.rangedTank.speedFactor -= 1;
                                        obj.visible = false;
                                    }
                                }
                            }
                            if (playerOne.rangedTank.checkBulletIntersection(obj.rectangle))
                            {
                                if (obj.destructible)
                                {
                                    switch (obj.type)
                                    {
                                        case "tree":
                                        case "tree1":
                                        case "tree2":
                                        case "tree3":
                                        case "bush1":
                                            treeCrushing.Play();
                                            break;

                                        case "crates": crateCrushing.Play();
                                            break;
                                    }


                                    obj.visible = false;
                                    obj.solid = false;
                                }
                            }
                            obj.Update(gameTime);
                        }
            //            //Player Ai
            //            if (playerAi.actualTank == 1)
            //            {

            //                if (playerAi.lightTank.checkTankIntersection(obj.rectangle))
            //                {
            //                    if (obj.destructible)
            //                    {
            //                        switch (obj.type)
            //                        {
            //                            case "tree":
            //                            case "tree1":
            //                            case "tree2":
            //                            case "tree3":
            //                            case "bush1":
            //                                treeCrushing.Play();
            //                                break;

            //                            case "crates": crateCrushing.Play();
            //                                break;
            //                        }
            //                        obj.visible = false;
            //                        obj.solid = false;
            //                    }
            //                    else
            //                    {
            //                        if (obj.type.Equals("rock1") || obj.type.Equals("rock2") || obj.type.Equals("rock3"))
            //                        {
            //                            playerAi.lightTank.speedFactor *= -1;
            //                            playerAi.lightTank.MoveChassis();
            //                            playerAi.lightTank.MoveChassisReverse();
            //                            playerAi.canMove = false;
            //                            playerAi.collideRock = true;
            //                            playerAi.movementCount = 50;
            //                        }
            //                        else if (obj.type.Equals("powerup_range") && obj.visible)
            //                        {
            //                            powerup.Play();
            //                            playerAi.powerupID = 1;
            //                            playerAi.lightTank.rangeDefault += 10;
            //                            obj.visible = false;
            //                        }
            //                        else if (obj.type.Equals("powerup_armor") && obj.visible)
            //                        {

            //                            powerup.Play();
            //                            playerAi.powerupID = 2;
            //                            playerAi.health += 1;
            //                            obj.visible = false;
            //                        }
            //                        else if (obj.type.Equals("powerup_firepower") && obj.visible)
            //                        {
            //                            powerup.Play();
            //                            playerAi.powerupID = 3;
            //                            obj.visible = false;
            //                            playerAi.lightTank.firePower++;
            //                        }
            //                        else if (obj.type.Equals("powerup_speed") && obj.visible)
            //                        {
            //                            powerup.Play();
            //                            playerAi.powerupID = 4;
            //                            playerAi.lightTank.speedFactor -= 1;
            //                            obj.visible = false;
            //                        }

            //                    }

            //                }
            //                if (playerAi.lightTank.checkBulletIntersection(obj.rectangle))
            //                {
            //                    switch (obj.type)
            //                    {
            //                        case "tree":
            //                        case "tree1":
            //                        case "tree2":
            //                        case "tree3":
            //                        case "bush1":
            //                            treeCrushing.Play();
            //                            break;

            //                        case "crates": crateCrushing.Play();
            //                            break;
            //                    }
            //                    if (obj.destructible)
            //                    {
            //                        obj.visible = false;
            //                        obj.solid = false;
            //                    }

            //                }
            //                obj.Update(gameTime);
            //            }

            //            else if (playerAi.actualTank == 2)
            //            {
            //                if (playerAi.heavyTank.checkTankIntersection(obj.rectangle))
            //                {
            //                    switch (obj.type)
            //                    {
            //                        case "tree":
            //                        case "tree1":
            //                        case "tree2":
            //                        case "tree3":
            //                        case "bush1":
            //                            treeCrushing.Play();
            //                            break;

            //                        case "crates": crateCrushing.Play();
            //                            break;
            //                    }
            //                    if (obj.destructible)
            //                    {
            //                        obj.visible = false;
            //                        obj.solid = false;
            //                    }
            //                    else
            //                    {
            //                        if (obj.type.Equals("rock1") || obj.type.Equals("rock2") || obj.type.Equals("rock3"))
            //                        {
            //                            playerAi.heavyTank.speedFactor *= -1;
            //                            //playerOne.heavyTank.speedFactor *= -1;
            //                            playerAi.heavyTank.MoveChassis();
            //                            playerAi.heavyTank.MoveChassisReverse();
            //                        }
            //                        else if (obj.type.Equals("powerup_range") && obj.visible)
            //                        {
            //                            powerup.Play();
            //                            playerAi.powerupID = 1;
            //                            playerAi.heavyTank.rangeDefault += 10;
            //                            obj.visible = false;
            //                        }
            //                        else if (obj.type.Equals("powerup_armor") && obj.visible)
            //                        {

            //                            powerup.Play();
            //                            playerAi.powerupID = 2;
            //                            playerAi.health += 1;
            //                            obj.visible = false;
            //                        }
            //                        else if (obj.type.Equals("powerup_firepower") && obj.visible)
            //                        {
            //                            powerup.Play();
            //                            playerAi.powerupID = 3;
            //                            obj.visible = false;
            //                            playerAi.heavyTank.firePower++;
            //                        }
            //                        else if (obj.type.Equals("powerup_speed") && obj.visible)
            //                        {
            //                            powerup.Play();
            //                            playerAi.powerupID = 4;
            //                            playerAi.heavyTank.speedFactor -= 1;
            //                            obj.visible = false;
            //                        }
            //                    }
            //                }
            //                if (playerAi.heavyTank.checkBulletIntersection(obj.rectangle))
            //                {
            //                    if (obj.destructible)
            //                    {
            //                        switch (obj.type)
            //                        {
            //                            case "tree":
            //                            case "tree1":
            //                            case "tree2":
            //                            case "tree3":
            //                            case "bush1":
            //                                treeCrushing.Play();
            //                                break;

            //                            case "crates": crateCrushing.Play();
            //                                break;
            //                        }


            //                        obj.visible = false;
            //                        obj.solid = false;
            //                    }
            //                }
            //                obj.Update(gameTime);
            //            }
            //            //Player Two
                        if (playerTwo.actualTank == 1)
                        {
                            if (playerTwo.lightTank.checkTankIntersection(obj.rectangle))
                            {
                                switch (obj.type)
                                {
                                    case "tree":
                                    case "tree1":
                                    case "tree2":
                                    case "tree3":
                                    case "bush1":
                                        treeCrushing.Play();
                                        break;

                                    case "crates": crateCrushing.Play();
                                        break;
                                }
                                if (obj.destructible)
                                {
                                    obj.visible = false;
                                    obj.solid = false;
                                }
                                else
                                {
                                    if (obj.type.Equals("rock1") || obj.type.Equals("rock2") || obj.type.Equals("rock3"))
                                    {
                                        playerTwo.lightTank.speedFactor *= -1;
                                        //playerTwo.lightTank.speedFactor *= -1;
                                        playerTwo.lightTank.MoveChassis();
                                        playerTwo.lightTank.MoveChassisReverse();
                                    }
                                    else if (obj.type.Equals("powerup_range") && obj.visible)
                                    {
                                        playerTwo.powerupID = 1;
                                        powerup.Play();
                                        playerTwo.lightTank.rangeDefault += 10;
                                        obj.visible = false;
                                    }
                                    else if (obj.type.Equals("powerup_armor") && obj.visible)
                                    {

                                        playerTwo.powerupID = 2;
                                        powerup.Play();
                                        playerTwo.health += 1;
                                        obj.visible = false;
                                    }
                                    else if (obj.type.Equals("powerup_firepower") && obj.visible)
                                    {
                                        powerup.Play();
                                        playerTwo.powerupID = 3;
                                        obj.visible = false;
                                        playerTwo.lightTank.firePower++;
                                    }
                                    else if (obj.type.Equals("powerup_speed") && obj.visible)
                                    {
                                        powerup.Play();
                                        playerTwo.powerupID = 4;
                                        playerTwo.lightTank.speedFactor -= 1;
                                        obj.visible = false;
                                    }
                                }

                            }
                            if (playerTwo.lightTank.checkBulletIntersection(obj.rectangle))
                            {
                                if (obj.destructible)
                                {
                                    switch (obj.type)
                                    {
                                        case "tree":
                                        case "tree1":
                                        case "tree2":
                                        case "tree3":
                                        case "bush1":
                                            treeCrushing.Play();
                                            break;

                                        case "crates": crateCrushing.Play();
                                            break;
                                    }
                                    obj.visible = false;
                                    obj.solid = false;
                                }
                            }
                            obj.Update(gameTime);
                        }
                        else if (playerTwo.actualTank == 2)
                        {
                            if (playerTwo.heavyTank.checkTankIntersection(obj.rectangle))
                            {
                                if (obj.destructible)
                                {
                                    switch (obj.type)
                                    {
                                        case "tree":
                                        case "tree1":
                                        case "tree2":
                                        case "tree3":
                                        case "bush1":
                                            treeCrushing.Play();
                                            break;

                                        case "crates": crateCrushing.Play();
                                            break;
                                    }
                                    obj.visible = false;
                                    obj.solid = false;
                                }
                                else
                                {
                                    if (obj.type.Equals("rock1") || obj.type.Equals("rock2") || obj.type.Equals("rock3"))
                                    {
                                        playerTwo.heavyTank.speedFactor *= -1;
                                        //playerTwo.heavyTank.speedFactor *= -1;
                                        playerTwo.heavyTank.MoveChassis();
                                        playerTwo.heavyTank.MoveChassisReverse();
                                    }
                                    else if (obj.type.Equals("powerup_range") && obj.visible)
                                    {
                                        powerup.Play();
                                        playerTwo.powerupID = 1;
                                        playerTwo.heavyTank.rangeDefault += 10;
                                        obj.visible = false;
                                    }
                                    else if (obj.type.Equals("powerup_armor") && obj.visible)
                                    {

                                        powerup.Play();
                                        playerTwo.powerupID = 2;
                                        playerTwo.health += 1;
                                        obj.visible = false;
                                    }
                                    else if (obj.type.Equals("powerup_firepower") && obj.visible)
                                    {
                                        powerup.Play();
                                        playerTwo.powerupID = 3;
                                        obj.visible = false;
                                        playerTwo.heavyTank.firePower++;
                                    }
                                    else if (obj.type.Equals("powerup_speed") && obj.visible)
                                    {
                                        powerup.Play();
                                        playerTwo.powerupID = 4;
                                        playerTwo.heavyTank.speedFactor -= 1;
                                        obj.visible = false;
                                    }
                                }
                            }
                            if (playerTwo.heavyTank.checkBulletIntersection(obj.rectangle))
                            {
                                if (obj.destructible)
                                {
                                    switch (obj.type)
                                    {
                                        case "tree":
                                        case "tree1":
                                        case "tree2":
                                        case "tree3":
                                        case "bush1":
                                            treeCrushing.Play();
                                            break;

                                        case "crates": crateCrushing.Play();
                                            break;
                                    }
                                    obj.visible = false;
                                    obj.solid = false;
                                }
                            }

                            obj.Update(gameTime);
                        }
                        else
                        {
                            if (playerTwo.rangedTank.checkTankIntersection(obj.rectangle))
                            {
                                if (obj.destructible)
                                {
                                    switch (obj.type)
                                    {
                                        case "tree":
                                        case "tree1":
                                        case "tree2":
                                        case "tree3":
                                        case "bush1":
                                            treeCrushing.Play();
                                            break;

                                        case "crates": crateCrushing.Play();
                                            break;
                                    }
                                    obj.visible = false;
                                    obj.solid = false;
                                }
                                else
                                {
                                    if (obj.type.Equals("rock1") || obj.type.Equals("rock2") || obj.type.Equals("rock3"))
                                    {
                                        playerTwo.rangedTank.speedFactor *= -1;
                                        //playerTwo.heavyTank.speedFactor *= -1;
                                        playerTwo.rangedTank.MoveChassis();
                                        playerTwo.rangedTank.MoveChassisReverse();
                                    }
                                    else if (obj.type.Equals("powerup_range") && obj.visible)
                                    {
                                        powerup.Play();
                                        playerTwo.powerupID = 1;
                                        playerTwo.rangedTank.rangeDefault += 10;
                                        obj.visible = false;
                                    }
                                    else if (obj.type.Equals("powerup_armor") && obj.visible)
                                    {

                                        powerup.Play();
                                        playerTwo.powerupID = 2;
                                        playerTwo.health += 1;
                                        obj.visible = false;
                                    }
                                    else if (obj.type.Equals("powerup_firepower") && obj.visible)
                                    {
                                        powerup.Play();
                                        playerTwo.powerupID = 3;
                                        obj.visible = false;
                                        playerTwo.rangedTank.firePower++;
                                    }
                                    else if (obj.type.Equals("powerup_speed") && obj.visible)
                                    {
                                        powerup.Play();
                                        playerTwo.powerupID = 4;
                                        playerTwo.rangedTank.speedFactor -= 1;
                                        obj.visible = false;
                                    }
                                }
                            }
                            if (playerTwo.rangedTank.checkBulletIntersection(obj.rectangle))
                            {
                                if (obj.destructible)
                                {
                                    switch (obj.type)
                                    {
                                        case "tree":
                                        case "tree1":
                                        case "tree2":
                                        case "tree3":
                                        case "bush1":
                                            treeCrushing.Play();
                                            break;

                                        case "crates": crateCrushing.Play();
                                            break;
                                    }
                                    obj.visible = false;
                                    obj.solid = false;
                                }
                            }

                            obj.Update(gameTime);
                        }
                    }
                }
            }
            #endregion
            #region BulletsVsTanks
            if (playerOne.actualTank == 1)
            {
                if (playerTwo.actualTank == 1)
                {
                    if (playerTwo.lightTank.checkBulletIntersection(playerOne.lightTank.recChassis))
                    {
                        playerOne.health = playerOne.health - playerTwo.lightTank.firePower;
                        if (percent1 <= 0)
                            percent1 = 1;
                    }
                }
                else if (playerTwo.actualTank == 2)
                {
                    if (playerTwo.heavyTank.checkBulletIntersection(playerOne.lightTank.recChassis))
                    {
                        playerOne.health = playerOne.health - playerTwo.heavyTank.firePower;
                        if (percent1 <= 0)
                            percent1 = 1;
                    }
                }
                else if (playerTwo.actualTank == 3)
                {
                    if (playerTwo.rangedTank.checkBulletIntersection(playerOne.lightTank.recChassis))
                    {
                        playerOne.health = playerOne.health - playerTwo.rangedTank.firePower;
                        if (percent1 <= 0)
                            percent1 = 1;
                    }
                }
                //if (playerAi.actualTank == 1)
                //{
                //    if (playerAi.lightTank.checkBulletIntersection(playerOne.lightTank.recChassis))
                //    {
                //        playerOne.health = playerOne.health - playerAi.lightTank.firePower;
                //        if (percent1 <= 0)
                //            percent1 = 1;
                //    }
                //}
                //else if (playerAi.actualTank == 2)
                //{
                //    if (playerAi.heavyTank.checkBulletIntersection(playerOne.lightTank.recChassis))
                //    {
                //        playerOne.health = playerOne.health - playerAi.heavyTank.firePower;
                //        if (percent1 <= 0)
                //            percent1 = 1;
                //    }
                //}
            }
            else if (playerOne.actualTank == 2)
            {
                if (playerTwo.actualTank == 1)
                {
                    if (playerTwo.lightTank.checkBulletIntersection(playerOne.heavyTank.recChassis))
                    {
                        playerOne.health = playerOne.health - playerTwo.lightTank.firePower;
                        if (percent1 <= 0)
                            percent1 = 1;
                    }
                }
                else if (playerTwo.actualTank == 2)
                {
                    if (playerTwo.heavyTank.checkBulletIntersection(playerOne.heavyTank.recChassis))
                    {
                        playerOne.health = playerOne.health - playerTwo.heavyTank.firePower;
                        if (percent1 <= 0)
                            percent1 = 1;
                    }
                }
                else if (playerTwo.actualTank == 3)
                {
                    if (playerTwo.rangedTank.checkBulletIntersection(playerOne.heavyTank.recChassis))
                    {
                        playerOne.health = playerOne.health - playerTwo.rangedTank.firePower;
                        if (percent1 <= 0)
                            percent1 = 1;
                    }
                } 
                //if (playerAi.actualTank == 1)
                //{
                //    if (playerAi.lightTank.checkBulletIntersection(playerOne.heavyTank.recChassis))
                //    {
                //        playerOne.health = playerOne.health - playerAi.lightTank.firePower;
                //        if (percent1 <= 0)
                //            percent1 = 1;
                //    }
                //}
                //else if (playerAi.actualTank == 2)
                //{
                //    if (playerAi.heavyTank.checkBulletIntersection(playerOne.heavyTank.recChassis))
                //    {
                //        playerOne.health = playerOne.health - playerAi.heavyTank.firePower;
                //        if (percent1 <= 0)
                //            percent1 = 1;
                //    }
                //}
            }
            else if (playerOne.actualTank == 3)
            {
                if (playerTwo.actualTank == 1)
                {
                    if (playerTwo.lightTank.checkBulletIntersection(playerOne.rangedTank.recChassis))
                    {
                        playerOne.health = playerOne.health - playerTwo.lightTank.firePower;
                        if (percent1 <= 0)
                            percent1 = 1;
                    }
                }
                else if (playerTwo.actualTank == 2)
                {
                    if (playerTwo.heavyTank.checkBulletIntersection(playerOne.rangedTank.recChassis))
                    {
                        playerOne.health = playerOne.health - playerTwo.heavyTank.firePower;
                        if (percent1 <= 0)
                            percent1 = 1;
                    }
                }
                else if (playerTwo.actualTank == 3)
                {
                    if (playerTwo.rangedTank.checkBulletIntersection(playerOne.rangedTank.recChassis))
                    {
                        playerOne.health = playerOne.health - playerTwo.rangedTank.firePower;
                        if (percent1 <= 0)
                            percent1 = 1;
                    }
                }
                //if (playerAi.actualTank == 1)
                //{
                //    if (playerAi.lightTank.checkBulletIntersection(playerOne.rangedTank.recChassis))
                //    {
                //        playerOne.health = playerOne.health - playerAi.lightTank.firePower;
                //        if (percent1 <= 0)
                //            percent1 = 1;
                //    }
                //}
                //else if (playerAi.actualTank == 2)
                //{
                //    if (playerAi.heavyTank.checkBulletIntersection(playerOne.rangedTank.recChassis))
                //    {
                //        playerOne.health = playerOne.health - playerAi.heavyTank.firePower;
                //        if (percent1 <= 0)
                //            percent1 = 1;
                //    }
                //}

            }
            /////////Player AI
            //if (playerAi.actualTank == 1)
            //{
            //    if (playerOne.actualTank == 1)
            //    {
            //        if (playerOne.lightTank.checkBulletIntersection(playerAi.lightTank.recChassis))
            //        {
            //            playerAi.health = playerAi.health - playerOne.lightTank.firePower;
            //            if (percent2 <= 0)
            //                percent2 = 1;
            //        }
            //    }
            //    else if (playerOne.actualTank == 2)
            //    {
            //        if (playerOne.heavyTank.checkBulletIntersection(playerAi.lightTank.recChassis))
            //        {
            //            playerAi.health = playerAi.health - playerOne.heavyTank.firePower;
            //            if (percent2 <= 0)
            //                percent2 = 1;
            //        }
            //    }
            //    else if (playerOne.actualTank == 3)
            //    {
            //        if (playerOne.rangedTank.checkBulletIntersection(playerAi.lightTank.recChassis))
            //        {
            //            playerAi.health = playerAi.health - playerOne.rangedTank.firePower;
            //            if (percent2 <= 0)
            //                percent2 = 1;
            //        }
            //    }
            //}
            //else if (playerAi.actualTank == 2)
            //{
            //    if (playerOne.actualTank == 1)
            //    {
            //        if (playerOne.lightTank.checkBulletIntersection(playerAi.heavyTank.recChassis))
            //        {
            //            playerAi.health = playerAi.health - playerOne.heavyTank.firePower;
            //            if (percent2 <= 0)
            //                percent2 = 1;
            //        }
            //    }
            //    else if (playerOne.actualTank == 2)
            //    {
            //        if (playerOne.heavyTank.checkBulletIntersection(playerAi.heavyTank.recChassis))
            //        {
            //            playerAi.health = playerAi.health - playerOne.heavyTank.firePower;
            //            if (percent2 <= 0)
            //                percent2 = 1;
            //        }
            //    }
            //    else if (playerOne.actualTank == 3)
            //    {
            //        if (playerOne.rangedTank.checkBulletIntersection(playerAi.heavyTank.recChassis))
            //        {
            //            playerAi.health = playerAi.health - playerOne.rangedTank.firePower;
            //            if (percent2 <= 0)
            //                percent2 = 1;
            //        }
            //    }
            //}
            /////////Player Two
            if (playerTwo.actualTank == 1)
            {
                if (playerOne.actualTank == 1)
                {
                    if (playerOne.lightTank.checkBulletIntersection(playerTwo.lightTank.recChassis))
                    {
                        playerTwo.health = playerTwo.health - playerOne.lightTank.firePower;
                        if (percent2 <= 0)
                            percent2 = 1;
                    }
                }
                else if (playerOne.actualTank == 2)
                {
                    if (playerOne.heavyTank.checkBulletIntersection(playerTwo.lightTank.recChassis))
                    {
                        playerTwo.health = playerTwo.health - playerOne.heavyTank.firePower;
                        if (percent2 <= 0)
                            percent2 = 1;
                    }
                }
                else if (playerOne.actualTank == 3)
                {
                    if (playerOne.rangedTank.checkBulletIntersection(playerTwo.lightTank.recChassis))
                    {
                        playerTwo.health = playerTwo.health - playerOne.rangedTank.firePower;
                        if (percent2 <= 0)
                            percent2 = 1;
                    }
                }
            }
            else if (playerTwo.actualTank == 2)
            {
                if (playerOne.actualTank == 1)
                {
                    if (playerOne.lightTank.checkBulletIntersection(playerTwo.heavyTank.recChassis))
                    {
                        playerTwo.health = playerTwo.health - playerOne.heavyTank.firePower;
                        if (percent2 <= 0)
                            percent2 = 1;
                    }
                }
                else if (playerOne.actualTank == 2)
                {
                    if (playerOne.heavyTank.checkBulletIntersection(playerTwo.heavyTank.recChassis))
                    {
                        playerTwo.health = playerTwo.health - playerOne.heavyTank.firePower;
                        if (percent2 <= 0)
                            percent2 = 1;
                    }
                }
                else if (playerOne.actualTank == 3)
                {
                    if (playerOne.rangedTank.checkBulletIntersection(playerTwo.heavyTank.recChassis))
                    {
                        playerTwo.health = playerTwo.health - playerOne.rangedTank.firePower;
                        if (percent2 <= 0)
                            percent2 = 1;
                    }
                }
            }
            else if (playerTwo.actualTank == 3)
            {
                if (playerOne.actualTank == 1)
                {
                    if (playerOne.lightTank.checkBulletIntersection(playerTwo.rangedTank.recChassis))
                    {
                        playerTwo.health = playerTwo.health - playerOne.heavyTank.firePower;
                        if (percent2 <= 0)
                            percent2 = 1;
                    }
                }
                else if (playerOne.actualTank == 2)
                {
                    if (playerOne.heavyTank.checkBulletIntersection(playerTwo.rangedTank.recChassis))
                    {
                        playerTwo.health = playerTwo.health - playerOne.heavyTank.firePower;
                        if (percent2 <= 0)
                            percent2 = 1;
                    }
                }
                else if (playerOne.actualTank == 3)
                {
                    if (playerOne.rangedTank.checkBulletIntersection(playerTwo.rangedTank.recChassis))
                    {
                        playerTwo.health = playerTwo.health - playerOne.rangedTank.firePower;
                        if (percent2 <= 0)
                            percent2 = 1;
                    }
                }
            }
            /////////


            #endregion
            #region Player1 vs Player 2
            if (playerTwo.lightTank.checkTankIntersection(playerOne.lightTank.recChassis))
            {
                playerTwo.lightTank.speedFactor *= -1;
                playerOne.lightTank.speedFactor *= -1;
            }
            else if (playerTwo.lightTank.checkTankIntersection(playerOne.heavyTank.recChassis))
            {
                playerTwo.lightTank.speedFactor *= -1;
                playerOne.heavyTank.speedFactor *= -1;
            }
            else if (playerTwo.lightTank.checkTankIntersection(playerOne.rangedTank.recChassis))
            {
                playerTwo.lightTank.speedFactor *= -1;
                playerOne.rangedTank.speedFactor *= -1;
            }
            else if(playerTwo.heavyTank.checkTankIntersection(playerOne.lightTank.recChassis))
            {
                playerTwo.heavyTank.speedFactor *= -1;
                playerOne.lightTank.speedFactor *= -1;
            }
            else if(playerTwo.heavyTank.checkTankIntersection(playerOne.heavyTank.recChassis))
            {
                playerTwo.heavyTank.speedFactor *= -1;
                playerOne.heavyTank.speedFactor *= -1;
            }
            else if(playerTwo.heavyTank.checkTankIntersection(playerOne.rangedTank.recChassis))
            {
                playerTwo.heavyTank.speedFactor *= -1;
                playerOne.rangedTank.speedFactor *= -1;
            }
            if (playerOne.lightTank.checkTankIntersection(playerTwo.lightTank.recChassis))
            {
                playerTwo.lightTank.speedFactor *= -1;
                playerOne.lightTank.speedFactor *= -1;
            }
            else if (playerOne.lightTank.checkTankIntersection(playerTwo.heavyTank.recChassis))
            {
                playerTwo.lightTank.speedFactor *= -1;
                playerOne.heavyTank.speedFactor *= -1;
            }
            else if (playerOne.lightTank.checkTankIntersection(playerTwo.rangedTank.recChassis))
            {
                playerTwo.lightTank.speedFactor *= -1;
                playerOne.rangedTank.speedFactor *= -1;
            }
            else if (playerOne.heavyTank.checkTankIntersection(playerTwo.lightTank.recChassis))
            {
                playerTwo.heavyTank.speedFactor *= -1;
                playerOne.lightTank.speedFactor *= -1;
            }
            else if (playerOne.heavyTank.checkTankIntersection(playerTwo.heavyTank.recChassis))
            {
                playerTwo.heavyTank.speedFactor *= -1;
                playerOne.heavyTank.speedFactor *= -1;
            }
            else if (playerOne.heavyTank.checkTankIntersection(playerTwo.rangedTank.recChassis))
            {
                playerTwo.heavyTank.speedFactor *= -1;
                playerOne.rangedTank.speedFactor *= -1;
            }
            #endregion

            if (playerOne.health <= 0)
            {
                isVictoryOne = true;
                GameOptions.gameOver();
            }
            if (playerTwo.health <= 0)
            {
                isVictoryTwo = true;
                GameOptions.gameOver();
            }
            //if (playerAi.health <= 0)
            //{
            //    isVictory = true;
            //}

            if (!GameOptions.isGameOver())
            {
                //if(!isMenu)
                playerOne.Update(gameTime);
                if (players == 1)
                {
                    //if (!isMenu) playerAi.Update(gameTime, playerOne);
                }
                else
                {
                    if (!isMenu) playerTwo.Update(gameTime);
                }
            }

            oldState = newState;
            oldStateG = newStateG;
            //base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(GameTime gameTime)
        {
            m_graphics_device.Clear(Color.WhiteSmoke);
            #region Main Menu
            //Added///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //if (isMenu == true)
            //{
            //    spriteBatch.Begin();
            //    //Console.WriteLine(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width);
            //    if (aspectRatio < 1.34 && aspectRatio > 1.33)
            //        spriteBatch.Draw(menu_43, menuRectangle43, Color.White);
            //    else
            //        spriteBatch.Draw(menu_169, menuRectangle169, Color.White);

            //    spriteBatch.Draw(menuBox, menuBoxRec, color);
            //    //spriteBatch.Draw(arrow, arrowRec, Color.White);

            //    if (isInstruction)
            //    {
            //        spriteBatch.Draw(menuBox, instructionRectangle, color);
            //        spriteBatch.Draw(menuBox, instructionRectangle2, color);
            //        spriteBatch.Draw(menuBox, instructionRectangle3, color);

            //        spriteBatch.DrawString(font, "Player 1:", new Vector2(325, 165), Color.White);
            //        spriteBatch.DrawString(font, "W- Forward\nS- Backwards\nA- Rotate Left\nD- Rotate Right\nQ-Rotate Turret\nE-Rotate Turret\nZ-Shoot\nGarage:\nT-Tank Cycle\nF-Tank Cycle\nG-Tank Select", new Vector2(325, 190), Color.White);

            //        spriteBatch.DrawString(font, "Player 2:", new Vector2(575, 165), Color.White);
            //        spriteBatch.DrawString(font, "UP- Forward\nDown- Backwards\nLeft- Rotate Left\nRight- Rotate Right\nNum 1-Rotate Turret\nNum 2-Rotate Turret\nCtrl-Shoot\nGarage:\nP-Tank Cycle\nL-Tank Cycle\nENTER-Tank Select", new Vector2(575, 190), Color.White);

            //        spriteBatch.DrawString(font, "Objectives:", new Vector2(75, 165), Color.White);
            //        spriteBatch.DrawString(font, "Destroy the Other \nTank! Watch Your\nEngine Heat levels!\n\nUpgrades:\nSpeed - \nPower - \nArmor - \nRange - ", new Vector2(75, 190), Color.White);
            //        //spriteBatch.Draw(speed, speedRec, Color.White);
            //        //spriteBatch.Draw(power, powerRec, Color.White);
            //        //spriteBatch.Draw(armor, armorRec, Color.White);
            //        //spriteBatch.Draw(range, rangeRec, Color.White);
            //    }
            //    if (isSettings)
            //    {
            //        spriteBatch.Draw(menuBox, settingRec, color);
            //        if (isKey)
            //        {
            //            spriteBatch.Draw(menuBox, keyRec, color);
            //        }
            //        spriteBatch.DrawString(font, "Players: " + players, new Vector2(575, 165), Color.White);
            //        //spriteBatch.DrawString(font, "\nLives: " + lives, new Vector2(575, 165), Color.White);
            //        spriteBatch.DrawString(font, "\nMap: " + map, new Vector2(575, 165), Color.White);
            //        spriteBatch.DrawString(font, "\n\nKeys", new Vector2(575, 165), Color.White);
                    
            //        spriteBatch.DrawString(font, "\n\n\nDone", new Vector2(575, 165), Color.White);
            //    }
            //    if (isKey)
            //    {

            //        spriteBatch.DrawString(font, "Player 1: ", new Vector2(75, 155), Color.White);
            //        spriteBatch.DrawString(font, "Forward -" + keyMap.getKey(1, 0).ToString() + "\nBackwards -" + keyMap.getKey(1, 1).ToString() + "\nRotate Left -" + keyMap.getKey(1, 2).ToString() + "\nRotate Right -" + keyMap.getKey(1, 3).ToString() + "\nRotate Turret L. -" + keyMap.getKey(1, 4).ToString() + "\nRotate Turret R. -" + keyMap.getKey(1, 5).ToString() + "\nShoot -" + keyMap.getKey(1, 6).ToString() + "\nTank Cycle Up -" + keyMap.getKey(1, 7).ToString() + "\nTank Cycle D. -" + keyMap.getKey(1, 8).ToString() + "\nTank Select -" + keyMap.getKey(1, 9).ToString(), new Vector2(75, 180), Color.White);
            //        spriteBatch.DrawString(font, "Player 2: ", new Vector2(325, 155), Color.White);
            //        spriteBatch.DrawString(font, "Forward -" + keyMap.getKey(2, 0).ToString() + "\nBackwards -" + keyMap.getKey(2, 1).ToString() + "\nRotate Left -" + keyMap.getKey(2, 2).ToString() + "\nRotate Right -" + keyMap.getKey(2, 3).ToString() + "\nRotate Turret L. -" + keyMap.getKey(2, 4).ToString() + "\nRotate Turret R. -" + keyMap.getKey(2, 5).ToString() + "\nShoot -" + keyMap.getKey(2, 6).ToString() + "\nTank Cycle Up -" + keyMap.getKey(2, 7).ToString() + "\nTank Cycle D. -" + keyMap.getKey(2, 8).ToString() + "\nTank Select -" + keyMap.getKey(2, 9).ToString(), new Vector2(325, 180), Color.White);
            //        spriteBatch.DrawString(font, "Hold down the End key to select a new key! \nRelease End before releasing the new key.", new Vector2(75, 395), Color.White);
            //        spriteBatch.DrawString(font, "Press Enter to return to settings", new Vector2(75, 435), Color.White);
            //    }

            //    spriteBatch.DrawString(font, "Play", new Vector2(575, 25), Color.White);
            //    spriteBatch.DrawString(font, "Instructions", new Vector2(575, 50), Color.White);
            //    spriteBatch.DrawString(font, "Game Setup", new Vector2(575, 75), Color.White);
            //    spriteBatch.DrawString(font, "Exit Game", new Vector2(575, 100), Color.White);
            //    spriteBatch.Draw(arrow, arrowRec, Color.White);

            //    if(isLoading)
            //        spriteBatch.DrawString(font, "Loading...", new Vector2(650, 188), Color.White);

            //    spriteBatch.End();
            //}
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            #endregion


            //if (isVictory)
            //{
            //    m_graphics_device.Viewport = view3;
            //    m_spriteBatch.Begin();
            //    if (playerOne.health <= 0)
            //    {

            //        m_spriteBatch.Draw(background, backGroundRectangle, Color.White);
            //        m_spriteBatch.End();
            //        m_spriteBatch.Begin();
            //        playerOne.Draw(gameTime, m_spriteBatch);
            //        playerTwo.Draw(gameTime, m_spriteBatch);

            //        //foreach (Object k in environmentObjects)
            //        //{
            //        //    k.Draw(gameTime);
            //        //}
            //        m_spriteBatch.Draw(playerTwoWins, instructionRectangle, Color.White);
            //    }
            //    else if (playerTwo.health <= 0 )//|| playerAi.health<=0)
            //    {

            //        m_spriteBatch.Draw(background, backGroundRectangle, Color.White);
            //        m_spriteBatch.End();
            //        m_spriteBatch.Begin();
            //        playerOne.Draw(gameTime, m_spriteBatch);
            //        playerTwo.Draw(gameTime, m_spriteBatch);

            //        //foreach (Object k in environmentObjects)
            //        //{
            //        //    k.Draw(gameTime);
            //        //}
            //      m_spriteBatch.Draw(playerOneWins, instructionRectangle, Color.White);
            //    }
            //    m_spriteBatch.End();
            //}
            //else
            //{

                // TODO: Add your drawing code here
                m_graphics_device.Viewport = view1;
                m_spriteBatch.Begin(SpriteSortMode.Deferred,
                        BlendState.AlphaBlend,
                        null,
                        null,
                        null,
                        null,
                        cam_p1.get_transformation(m_graphics_device));
                //m_spriteBatch.Draw(background, backGroundRectangle, Color.White);
                m_spriteBatch.Draw(world, new Rectangle(0,0,world.Width,world.Height), Color.White);
                //spriteBatch.End();
                //if (!isMenu)
                //{
                foreach (Object k in environmentObjects)
                {
                    k.Draw(gameTime,m_spriteBatch);
                }
                //}
                //spriteBatch.Begin();

                //spriteBatch.Begin();
                playerOne.Draw(gameTime, m_spriteBatch);
                if (players == 2)
                {
                    playerTwo.Draw(gameTime, m_spriteBatch);
                }
                //else
                //{
                //    playerAi.Draw(gameTime, spriteBatch);
                //}

                //spriteBatch.End();

                //if (percent1 > 0)
                //{
                //    //spriteBatch.Begin(0, null, null, null, null, damageFX);
                //    if(playerOne.actualTank == 1)
                //        playerOne.lightTank.Draw(gameTime, m_spriteBatch);
                //    if (playerOne.actualTank == 2)
                //        playerOne.heavyTank.Draw(gameTime, m_spriteBatch);
                //    if (playerOne.actualTank == 3)
                //        playerOne.rangedTank.Draw(gameTime, m_spriteBatch);              
                //    //spriteBatch.End();
                //}
                //if (percent2 > 0)
                //{
                //    if (players == 2)
                //    {
                //        //spriteBatch.Begin(0, null, null, null, null, damageFX);
                //        if (playerTwo.actualTank == 1)
                //            playerTwo.lightTank.Draw(gameTime, m_spriteBatch);
                //        if (playerTwo.actualTank == 2)
                //            playerTwo.heavyTank.Draw(gameTime, m_spriteBatch);
                //        if (playerTwo.actualTank == 3)
                //            playerTwo.rangedTank.Draw(gameTime, m_spriteBatch);
                //        //spriteBatch.End();
                //    }
                //    //else
                //    //{
                //    //    spriteBatch.Begin(0, null, null, null, null, damageFX);
                //    //    if (playerAi.actualTank == 1)
                //    //        playerAi.lightTank.Draw(gameTime, spriteBatch);
                //    //    if (playerAi.actualTank == 2)
                //    //        playerAi.heavyTank.Draw(gameTime, spriteBatch);
                        
                //    //    spriteBatch.End();
                //    //}
                //}
                m_spriteBatch.End();

                m_graphics_device.Viewport = view2;
                m_spriteBatch.Begin(SpriteSortMode.Deferred,
                        BlendState.AlphaBlend,
                        null,
                        null,
                        null,
                        null,
                        cam_p2.get_transformation(m_graphics_device));
                //m_spriteBatch.Draw(background, backGroundRectangle, Color.White);
                m_spriteBatch.Draw(world, new Rectangle(0, 0, world.Width, world.Height), Color.White);
                //spriteBatch.End();
                //if (!isMenu)
                //{
                foreach (Object k in environmentObjects)
                {
                    k.Draw(gameTime, m_spriteBatch);
                }
                //}
                //spriteBatch.Begin();

                //spriteBatch.Begin();
                playerOne.Draw(gameTime, m_spriteBatch);
                if (players == 2)
                {
                    playerTwo.Draw(gameTime, m_spriteBatch);
                }
                //else
                //{
                //    playerAi.Draw(gameTime, spriteBatch);
                //}

                //spriteBatch.End();

                //if (percent1 > 0)
                //{
                //    //spriteBatch.Begin(0, null, null, null, null, damageFX);
                //    if (playerOne.actualTank == 1)
                //        playerOne.lightTank.Draw(gameTime, m_spriteBatch);
                //    if (playerOne.actualTank == 2)
                //        playerOne.heavyTank.Draw(gameTime, m_spriteBatch);
                //    if (playerOne.actualTank == 3)
                //        playerOne.rangedTank.Draw(gameTime, m_spriteBatch);
                //    //spriteBatch.End();
                //}
                //if (percent2 > 0)
                //{
                //    if (players == 2)
                //    {
                //        //spriteBatch.Begin(0, null, null, null, null, damageFX);
                //        if (playerTwo.actualTank == 1)
                //            playerTwo.lightTank.Draw(gameTime, m_spriteBatch);
                //        if (playerTwo.actualTank == 2)
                //            playerTwo.heavyTank.Draw(gameTime, m_spriteBatch);
                //        if (playerTwo.actualTank == 3)
                //            playerTwo.rangedTank.Draw(gameTime, m_spriteBatch);
                //        //spriteBatch.End();
                //    }
                //    //else
                //    //{
                //    //    spriteBatch.Begin(0, null, null, null, null, damageFX);
                //    //    if (playerAi.actualTank == 1)
                //    //        playerAi.lightTank.Draw(gameTime, spriteBatch);
                //    //    if (playerAi.actualTank == 2)
                //    //        playerAi.heavyTank.Draw(gameTime, spriteBatch);

                //    //    spriteBatch.End();
                //    //}
                //}
                m_spriteBatch.End();

                m_graphics_device.Viewport = view3;
                m_spriteBatch.Begin();
                radar.Draw(m_spriteBatch, playerOne.getPosition(), playerTwo.getPosition());
                m_spriteBatch.End();


            //}
            //base.Draw(gameTime);
        }
    }
}
