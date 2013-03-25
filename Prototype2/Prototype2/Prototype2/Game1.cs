///////////////////////////////////////
 /* Hugh Desmond - Final year project
  * 
  * version: 1.0
  * release: 2
  * 
  * 
  * Keyboard Controls:
  * 
  * move                    -  arrow left/right
  * jump                    -  space
  * camera                  -  WASD
  * show physics box        -  B
  * code gen mode toggle    -  F
  * generate tarrain code   -  LMB
  * 
  *  
  */
///////////////////////////////////////


using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Prototype2;
using System.Collections.Generic;
using System.Collections;


namespace Prototype1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        const int screenWidth = 1280;
        const int screenHeight = 720;
        Vector2 screenCenter = new Vector2(screenWidth / 2, screenHeight / 2);

        //private Camera2D camera;
        //private List<Scene2DNode> nodeList;
        
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _batch;
        private KeyboardState _oldKeyState;
        private GamePadState _oldPadState;
        private MouseState _oldMouseState;
        public SpriteFont _font;

        private World _world;

        private Body _circleBody;
        private Body _groundBody;
        private Body playerBody;
        private Body backgroundBody;

        private List<PassableEdge> passableEdges;

        private CompositeCharacter box; 

        private Texture2D _circleSprite;
        private Texture2D _groundSprite;        
        private Texture2D player;
        private Texture2D playerTexture;
        private Texture2D playerJump;
        private Texture2D playerShoot;
        private Texture2D armgun;
        private Texture2D head;

        private Texture2D tile1;
        private Texture2D tile2;
        private Texture2D A_0_0;
        private Texture2D A_0_720;
        private Texture2D A_1280_0;
        private Texture2D A_1280_720;
        private Texture2D A_2560_1440;
        private Texture2D A_2560_720;
        private Texture2D A_3840_1440;
        private Texture2D A_3840_720;
        private Texture2D A_5120_1440;
        private Texture2D A_5120_720;
        private Texture2D A_M1280_0;
        private Texture2D A_M1280_720;        

        private Queue<Bullet> bulletQueue;
        private Array tempBulletArray;
        private Texture2D bulletTex;

        //private Vector2 bulletPos;
        //private Boolean isBullet;
        //private Vector2 bulletDirection;       //the XY increment for the bullet
        //private int bulletSpeed = 2;
        //private Vector2 bulletOrigin;

        private SpriteEffects armgunEffects;
        private float armgunAngle;
        private double armgunAngleInDegrees;

        private float headAngle;      
        

        private Animation playerAnimation;
        
        private Activity oldActivity;

        private Texture2D squareTex;        

        private Vector2 playerPos;     

        // Simple camera controls
        private Matrix _view;
        private Vector2 _cameraPosition;
        private Vector2 _screenCenter;
        private bool freeViewOn = false;            //controllable camera   
        
        string Text = "BASIC HUD";

        FrameRateCounter fpsCounter;

        private bool showBox = false;
                
        // Farseer expects objects to be scaled to MKS (meters, kilos, seconds)
        // 1 meters equals 100 pixels here
        // (Objects should be scaled to be between 0.1 and 10 meters in size)
        private const float MeterInPixels = 100f;

        private Texture2D crosshair;
        private Texture2D marker;

        private Vector2 previousClickWorldPos = Vector2.Zero;
        //private ArrayList markers;
        

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = screenWidth;
            _graphics.PreferredBackBufferHeight = screenHeight;

            //_graphics.IsFullScreen = true;

            //camera = new Camera2D(_batch);

            Content.RootDirectory = "Content";

            fpsCounter = new FrameRateCounter();

            _world = new World(new Vector2(0, 30f));
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Initialize camera controls
            _view = Matrix.Identity;
            _cameraPosition = new Vector2(0f,0f);

            _screenCenter = new Vector2(_graphics.GraphicsDevice.Viewport.Width / 2f,
                                                _graphics.GraphicsDevice.Viewport.Height / 2f);

            _batch = new SpriteBatch(_graphics.GraphicsDevice);
            _font = Content.Load<SpriteFont>("font");

            // Load sprites
            _circleSprite = Content.Load<Texture2D>("circleSprite"); //  96px x 96px => 0.96m x 0.96m
            _groundSprite = Content.Load<Texture2D>("groundSprite"); // 512px x 64px =>   5.12m x 0.64m            
            player = Content.Load<Texture2D>("player1"); // 95px x 80px =>   1m x 1.25m
            armgun = Content.Load<Texture2D>("armgun"); // 28px x 67px =>   1m x 1.25m
            head = Content.Load<Texture2D>("head"); // 41px x 37px   
            bulletTex = Content.Load<Texture2D>("bullet");

            squareTex = Content.Load<Texture2D>("square");

            playerJump = Content.Load<Texture2D>("jumping");

            //editing textures
            crosshair = Content.Load<Texture2D>("crosshair");
            marker = Content.Load<Texture2D>("marker");

            //load level tiles
            tile1 = Content.Load<Texture2D>("background1"); //  1280px x 720px => 12.8m x 7.2m
            tile2 = Content.Load<Texture2D>("prototype_Tile2"); //  1280px x 720px => 12.8m x 7.2m

            A_0_0 = Content.Load<Texture2D>("level tiles/A_0_0");
            A_0_720 = Content.Load<Texture2D>("level tiles/A_0_720");
            A_1280_0 = Content.Load<Texture2D>("level tiles/A_1280_0");
            A_1280_720 = Content.Load<Texture2D>("level tiles/A_1280_720");
            A_2560_1440 = Content.Load<Texture2D>("level tiles/A_2560_1440");
            A_2560_720 = Content.Load<Texture2D>("level tiles/A_2560_720");
            A_3840_1440 = Content.Load<Texture2D>("level tiles/A_3840_1440");
            A_3840_720 = Content.Load<Texture2D>("level tiles/A_3840_720");
            A_5120_1440 = Content.Load<Texture2D>("level tiles/A_5120_1440");
            A_5120_720 = Content.Load<Texture2D>("level tiles/A_5120_720");
            A_M1280_0 = Content.Load<Texture2D>("level tiles/A_M1280_0");
            A_M1280_720 = Content.Load<Texture2D>("level tiles/A_M1280_720");

            //testings
            playerTexture = Content.Load<Texture2D>("run");
            playerAnimation = new Animation();
            playerAnimation.Initialize(playerTexture, Vector2.Zero, 86, 119, 25, 300, Color.White, 1f, true, new Vector2(0, 0));
         
            box = new CompositeCharacter(_world, new Vector2(300f, 600f), 64, 128, 0.3f, squareTex);
            box.forcePower = 100;

            //init bullet stuff
            //bulletDirection = new Vector2(0, 0);
            bulletQueue = new Queue<Bullet>();
            tempBulletArray = bulletQueue.ToArray();

            //markers = new ArrayList();

            /*
            //player
            Vector2 playerPos1 = new Vector2(1f, screenCenter.Y / MeterInPixels);

            // Create the player fixture
            //playerBody = BodyFactory.CreateRectangle(_world, player.Width / MeterInPixels, player.Height / MeterInPixels, 0.7f, playerPos1 + (new Vector2(player.Width / 2, player.Height / 2) / MeterInPixels));
            playerBody = BodyFactory.CreateCircle(_world, player.Height / (2f * MeterInPixels), 1f, playerPos1 + (new Vector2(player.Width / 2, player.Height / 2) / MeterInPixels));
            playerBody.BodyType = BodyType.Dynamic;
            playerBody.Restitution = 0.0f;
            playerBody.Friction = 0.1f; 


            //Circle 
            Vector2 circlePosition = new Vector2((1f + (_groundSprite.Width / MeterInPixels) / 2) - ((_circleSprite.Width / MeterInPixels) / 2), 0f) + (new Vector2(_circleSprite.Width / MeterInPixels, _circleSprite.Height / MeterInPixels) / 2);

            // Create the circle fixture
            _circleBody = BodyFactory.CreateCircle(_world, 96f / (2f * MeterInPixels), 1f, circlePosition);
            _circleBody.BodyType = BodyType.Dynamic;
            _circleBody.Restitution = 0.3f;
            _circleBody.Friction = 0.5f;

            //Ground
            Vector2 groundPosition = new Vector2(1f, 6.5f) + (new Vector2(_groundSprite.Width / MeterInPixels, _groundSprite.Height / MeterInPixels) / 2);

            // Create the ground fixture
            _groundBody = BodyFactory.CreateRectangle(_world, 512f / MeterInPixels, 64f / MeterInPixels, 1f, groundPosition);
            _groundBody.IsStatic = true;
            _groundBody.Restitution = 0.3f;
            _groundBody.Friction = 0.5f;
            
            */

            //edge

            // Create the ground fixture
            Body impassableEdge = BodyFactory.CreateEdge(_world, new Vector2(0f, 3f), new Vector2(0.25f, 3f));
            impassableEdge.IsStatic = true;
            impassableEdge.Restitution = 0.1f;
            impassableEdge.Friction = 0.7f;


            /*//passable edges
            passableEdges = new List<PassableEdge>();

            passableEdges.Add(new PassableEdge(_world, new Vector2(10f, 2.5f), new Vector2(12.8f, 2.5f)));    //right rased platform
            passableEdges.Add(new PassableEdge(_world, new Vector2(4f, 1.5f), new Vector2(8f, 1.5f)));    //centred raised platform
            */

            
            //From top of first cliff---------------------------------------------------------------------------------------
            FixtureFactory.AttachEdge(new Vector2(-2.06f, 1.215f), new Vector2(-0.32f, 1.185f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(-0.32f, 1.185f), new Vector2(1.29f, 1.285f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(1.29f, 1.285f), new Vector2(2.37f, 1.315f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(2.37f, 1.315f), new Vector2(2.2f, 1.585f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(2.2f, 1.585f), new Vector2(2.08f, 1.645f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(2.08f, 1.645f), new Vector2(2.03f, 1.885f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(2.03f, 1.885f), new Vector2(1.69f, 2.005f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(1.69f, 2.005f), new Vector2(1.47f, 2.225f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(1.47f, 2.225f), new Vector2(1.44f, 2.425f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(1.44f, 2.425f), new Vector2(1.34f, 2.735f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(1.34f, 2.735f), new Vector2(1.41f, 2.965f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(1.41f, 2.965f), new Vector2(1.38f, 3.225f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(1.38f, 3.225f), new Vector2(1.35f, 3.585f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(1.35f, 3.585f), new Vector2(1.36f, 3.885f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(1.36f, 3.885f), new Vector2(1.42f, 4.155f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(1.42f, 4.155f), new Vector2(1.56f, 4.405f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(1.56f, 4.405f), new Vector2(1.68f, 4.585f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(1.68f, 4.585f), new Vector2(1.56f, 4.825f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(1.56f, 4.825f), new Vector2(1.71f, 5.275f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(1.71f, 5.275f), new Vector2(1.81f, 5.405f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(1.81f, 5.405f), new Vector2(1.86f, 5.575f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(1.86f, 5.575f), new Vector2(1.98f, 5.755f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(1.98f, 5.755f), new Vector2(2.16f, 6.015f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(2.16f, 6.015f), new Vector2(2.26f, 6.085f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(2.26f, 6.085f), new Vector2(2.28f, 6.185f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(2.28f, 6.185f), new Vector2(2.35f, 6.325f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(2.35f, 6.325f), new Vector2(2.43f, 6.465f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(2.43f, 6.465f), new Vector2(16.38f, 6.465f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(16.38f, 6.465f), new Vector2(22.68f, 8.915f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(22.68f, 8.915f), new Vector2(23.85f, 8.125f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(23.85f, 8.125f), new Vector2(26.29f, 8.065f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(26.29f, 8.065f), new Vector2(26.48f, 8.165f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(26.48f, 8.165f), new Vector2(26.43f, 8.825f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(26.43f, 8.825f), new Vector2(26.24f, 9.395f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(26.24f, 9.395f), new Vector2(26.38f, 9.885f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(26.38f, 9.885f), new Vector2(26.15f, 10.215f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(26.15f, 10.215f), new Vector2(25.67f, 10.425f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(25.67f, 10.425f), new Vector2(25.95f, 10.945f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(25.95f, 10.945f), new Vector2(26.03f, 11.505f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(26.03f, 11.505f), new Vector2(25.8f, 12.015f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(25.8f, 12.015f), new Vector2(25.65f, 12.775f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(25.65f, 12.775f), new Vector2(25.66f, 13.225f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(25.66f, 13.225f), new Vector2(25.65f, 13.845f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(25.65f, 13.845f), new Vector2(25.62f, 14.405f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(25.62f, 14.405f), new Vector2(25.74f, 14.115f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(25.74f, 14.115f), new Vector2(21.72f, 16.385f), impassableEdge);
            //To bottom of rock jump on the left---------------------------------------------------------------------------------------
               
            //From bottom of rock jump on right----------------------------------------------------------------------------------
            FixtureFactory.AttachEdge(new Vector2(29.19f, 14.08f), new Vector2(29.19f, 13.36f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(29.19f, 13.36f), new Vector2(29.01f, 12.87f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(29.01f, 12.87f), new Vector2(28.92f, 12.63f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(28.92f, 12.63f), new Vector2(29.11f, 11.9f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(29.11f, 11.9f), new Vector2(29.18f, 11.23f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(29.18f, 11.23f), new Vector2(29.39f, 11.11f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(29.39f, 11.11f), new Vector2(29.28f, 10.72f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(29.28f, 10.72f), new Vector2(29.4f, 10.14f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(29.4f, 10.14f), new Vector2(29.55f, 9.99f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(29.55f, 9.99f), new Vector2(30.07f, 9.96f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(30.07f, 9.96f), new Vector2(30.61f, 10.03f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(30.61f, 10.03f), new Vector2(31.33f, 10.06f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(31.33f, 10.06f), new Vector2(31.9f, 10.05f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(31.9f, 10.05f), new Vector2(32.32f, 10.12f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(32.32f, 10.12f), new Vector2(32.67f, 10.29f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(32.67f, 10.29f), new Vector2(32.94f, 10.46f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(32.94f, 10.46f), new Vector2(33.18f, 10.68f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(33.18f, 10.68f), new Vector2(33.63f, 10.69f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(33.63f, 10.69f), new Vector2(34.57f, 10.7f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(34.57f, 10.7f), new Vector2(35.5f, 10.63f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(35.5f, 10.63f), new Vector2(36.26f, 10.67f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(36.26f, 10.67f), new Vector2(36.83f, 10.62f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(36.83f, 10.62f), new Vector2(36.96f, 10.42f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(36.96f, 10.42f), new Vector2(37.04f, 10.33f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(37.04f, 10.33f), new Vector2(37.07f, 10.21f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(37.07f, 10.21f), new Vector2(37.34f, 10.19f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(37.34f, 10.19f), new Vector2(37.77f, 10.23f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(37.77f, 10.23f), new Vector2(38.19f, 10.27f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(38.19f, 10.27f), new Vector2(38.36f, 10.29f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(38.36f, 10.29f), new Vector2(38.41f, 10.59f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(38.41f, 10.59f), new Vector2(38.61f, 10.77f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(38.61f, 10.77f), new Vector2(39f, 10.87f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(39f, 10.87f), new Vector2(42.73f, 11f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(42.73f, 11f), new Vector2(43.95f, 11f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(43.95f, 11f), new Vector2(43.79f, 11.63f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(43.79f, 11.63f), new Vector2(43.72f, 12.17f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(43.72f, 12.17f), new Vector2(43.68f, 12.78f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(43.68f, 12.78f), new Vector2(43.66f, 13.58f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(43.66f, 13.58f), new Vector2(43.7f, 13.87f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(43.7f, 13.87f), new Vector2(43.51f, 14.35f), impassableEdge);            
            //To just above the cave, on the cliff vertical edge

            //from tip of big tree branch platform          
            FixtureFactory.AttachEdge(new Vector2(45.928f, 13.036f), new Vector2(46.718f, 13.016f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(46.718f, 13.016f), new Vector2(47.618f, 12.976f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(47.618f, 12.976f), new Vector2(48.728f, 12.796f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(48.728f, 12.796f), new Vector2(48.988f, 12.616f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(48.988f, 12.616f), new Vector2(49.368f, 12.676f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(49.368f, 12.676f), new Vector2(49.348f, 12.306f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(49.348f, 12.306f), new Vector2(49.228f, 11.796f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(49.228f, 11.796f), new Vector2(49.238f, 11.116f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(49.238f, 11.116f), new Vector2(49.078f, 10.146f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(49.078f, 10.146f), new Vector2(48.808f, 9.256f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(48.808f, 9.256f), new Vector2(48.388f, 8.296f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(48.388f, 8.296f), new Vector2(47.718f, 7.056f), impassableEdge);
            //to above big tree

            //from top of cave            
            FixtureFactory.AttachEdge(new Vector2(42.92f, 14.67f), new Vector2(42.47f, 14.98f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(42.47f, 14.98f), new Vector2(42.34f, 15.52f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(42.34f, 15.52f), new Vector2(42.26f, 16.11f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(42.26f, 16.11f), new Vector2(42.28f, 16.56f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(42.28f, 16.56f), new Vector2(44.93f, 16.56f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(44.93f, 16.56f), new Vector2(45.98f, 16.8f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(45.98f, 16.8f), new Vector2(50.18f, 18.92f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(50.18f, 18.92f), new Vector2(57.04f, 19.23f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(57.04f, 19.23f), new Vector2(57f, 18.9f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(57f, 18.9f), new Vector2(57.13f, 18.35f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(57.13f, 18.35f), new Vector2(57.47f, 18.06f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(57.47f, 18.06f), new Vector2(58.06f, 18f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(58.06f, 18f), new Vector2(58.51f, 18.28f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(58.51f, 18.28f), new Vector2(58.69f, 18.39f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(58.69f, 18.39f), new Vector2(58.84f, 18.61f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(58.84f, 18.61f), new Vector2(58.99f, 18.91f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(58.99f, 18.91f), new Vector2(58.96f, 19.21f), impassableEdge);
            FixtureFactory.AttachEdge(new Vector2(58.96f, 19.21f), new Vector2(63.11f, 19.28f), impassableEdge);
            //To the ground just past the big tree 

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {            
            HandleGamePad();
            HandleKeyboard();
            HandleMouse();

            fpsCounter.Update(gameTime);

            if (box.activity == Activity.Idle && oldActivity != Activity.Idle)
            {
                Vector2 temp = playerAnimation.Position;
                playerAnimation.Initialize(player, temp, 100, 123, 1, 0, Color.White, 1f, true, new Vector2(0, 0));                
            }
            else if (box.activity == Activity.Jumping && oldActivity != Activity.Jumping)
            {
                Vector2 temp = playerAnimation.Position;
                playerAnimation.Initialize(playerJump, temp, 120, 125, 23, 10, Color.White, 1f, false, new Vector2(0, 0));
            }
            else if (box.activity == Activity.Running && oldActivity != Activity.Running)
            {
                Vector2 temp = playerAnimation.Position;
                playerAnimation.Initialize(playerTexture, temp, 86, 121, 26, 30, Color.White, 1f, true, new Vector2(0, 0));
            }            

            oldActivity = box.activity;              

            box.Update(gameTime);            
            
            playerAnimation.Update(gameTime);

            /*//update all passable edges
            for (int i = 0; i < passableEdges.Count; i++) 
            {
                passableEdges[i].Update(box);              
            }*/

            //We update the world
            _world.Step((float)gameTime.ElapsedGameTime.TotalMilliseconds * 0.001f);

            base.Update(gameTime);
        }

        private void HandleGamePad()
        {
            GamePadState padState = GamePad.GetState(PlayerIndex.One, GamePadDeadZone.Circular);

            //bullet stuff
            if (bulletQueue.Count > 0)
            {
                tempBulletArray = bulletQueue.ToArray();
                bulletQueue.Clear();                                  //convert to array, clear the queue and fill it up after changes

                for (int i = 0; i < tempBulletArray.Length; i++)
                {
                    ((Bullet)tempBulletArray.GetValue(i)).CurrentPos += ((Bullet)tempBulletArray.GetValue(i)).DirectionIncrement;

                    bulletQueue.Enqueue((Bullet)tempBulletArray.GetValue(i));
                }

                if (((Bullet)bulletQueue.Peek()).isDead())        //remove dead bullets from the queue
                {
                    bulletQueue.Dequeue();
                }

                tempBulletArray = bulletQueue.ToArray();
            }

            if (padState.IsConnected)
            {
                if (padState.Buttons.Back == ButtonState.Pressed)
                    Exit();
                                

                if (padState.Buttons.B == ButtonState.Pressed && _oldPadState.Buttons.B == ButtonState.Released)
                {
                    if (showBox == true)
                    {
                        showBox = false;
                    }
                    else
                    {
                        showBox = true;
                    }
                }

                if (padState.Triggers.Right > 0.5 && _oldPadState.Triggers.Right < 0.5)
                {
                    Bullet bullet = new Bullet();
                    bullet.Texture = bulletTex;                    

                    if (padState.ThumbSticks.Right.X > 0.50 || padState.ThumbSticks.Right.X < -0.50 || padState.ThumbSticks.Right.Y > 0.50 || padState.ThumbSticks.Right.Y < -0.50)
                    {
                        bullet.DirectionIncrement = (padState.ThumbSticks.Right * new Vector2(1, -1));
                    }
                    else
                    {
                        if (armgunEffects == SpriteEffects.None)
                        {
                            bullet.DirectionIncrement = new Vector2(1, 0);
                        }
                        else if (armgunEffects == SpriteEffects.FlipHorizontally)
                        {
                            bullet.DirectionIncrement = new Vector2(-1, 0);
                        }
                    }

                    Vector2 perpToDirection;

                    //arm pivot point + direction multiplyed by guess number to bring the bullet origin to the tip of the pistol. 
                    //also moved a few pixels 90 degrees perpendicular to direction to match the tip of the gun  
                    if (armgunEffects == SpriteEffects.None)
                    {
                        perpToDirection = new Vector2(bullet.DirectionIncrement.Y, bullet.DirectionIncrement.X * -1) * 12;

                        bullet.Origin = new Vector2(box.Position.X + 10, box.Position.Y - 20) + (bullet.DirectionIncrement * 55) + perpToDirection;
                    }
                    else if (armgunEffects == SpriteEffects.FlipHorizontally)
                    {
                        perpToDirection = new Vector2(bullet.DirectionIncrement.Y * -1, bullet.DirectionIncrement.X) * 12;

                        bullet.Origin = new Vector2(box.Position.X - 20, box.Position.Y - 20) + (bullet.DirectionIncrement * 55) + perpToDirection;
                    }

                    bullet.CurrentPos = bullet.Origin;
                    bullet.DirectionIncrement *= bullet.Speed; 

                    bulletQueue.Enqueue(bullet);    //add the bullet to the queue of bullets
                }

                if (padState.ThumbSticks.Right.X > 0.50 || padState.ThumbSticks.Right.X < -0.50 || padState.ThumbSticks.Right.Y > 0.50 || padState.ThumbSticks.Right.Y < -0.50)
                {
                    armgunAngle = (float)Math.Atan2(padState.ThumbSticks.Right.Y, padState.ThumbSticks.Right.X) * -1;

                    armgunAngleInDegrees = (armgunAngle * 180) / Math.PI;

                    if (armgunAngleInDegrees > 90 || armgunAngleInDegrees < -90)
                    {
                        playerAnimation.myEffect = SpriteEffects.FlipHorizontally;
                        armgunEffects = SpriteEffects.FlipHorizontally;
                    }
                    else
                    {
                        playerAnimation.myEffect = SpriteEffects.None;
                        armgunEffects = SpriteEffects.None;
                    }
                }
                else
                {                   
                    armgunAngle = 0.0f;
                    headAngle = 0.0f;                   
                }

                //run animation speed is determined by the degree to which the analog stick is tilted
                if(box.activity == Activity.Running)
                {                    
                    if (padState.ThumbSticks.Left.X < -0.1f || padState.ThumbSticks.Left.X > 0.1f)
                    {
                        playerAnimation.frameTime = (int)(playerAnimation.minFrameTime * (1 / Math.Abs(padState.ThumbSticks.Left.X)));
                    }                    
                } 

                //camera follow player  
                if (freeViewOn == false)
                {
                    _cameraPosition = (box.Position - _screenCenter) * -1;
                    _cameraPosition += new Vector2(-130,130); 
                }

                _view = Matrix.CreateTranslation(new Vector3(_cameraPosition - _screenCenter, 0f)) * Matrix.CreateTranslation(new Vector3(_screenCenter, 0f));

                _oldPadState = padState;
            }
        }

        private void HandleKeyboard()
        {
            KeyboardState state = Keyboard.GetState();

            
                // Move camera
                if (state.IsKeyDown(Keys.A))
                    _cameraPosition.X += 5f;

                if (state.IsKeyDown(Keys.D))
                    _cameraPosition.X -= 5f;

                if (state.IsKeyDown(Keys.W))
                    _cameraPosition.Y += 5f;

                if (state.IsKeyDown(Keys.S))
                    _cameraPosition.Y -= 5f;

                _view = Matrix.CreateTranslation(new Vector3(_cameraPosition - _screenCenter, 0f)) *
                        Matrix.CreateTranslation(new Vector3(_screenCenter, 0f));
            
            
            /*if (state.IsKeyDown(Keys.Left))
            {
                playerAnimation.myEffect = SpriteEffects.FlipHorizontally;
                shootAnimation.myEffect = SpriteEffects.FlipHorizontally;
                armgunEffects = SpriteEffects.FlipHorizontally;
            }
            else if (state.IsKeyDown(Keys.Right))
            {
                playerAnimation.myEffect = SpriteEffects.None;
                shootAnimation.myEffect = SpriteEffects.None;
                armgunEffects = SpriteEffects.None;
            }*/

            if (state.IsKeyDown(Keys.B) && _oldKeyState.IsKeyDown(Keys.B) == false)
            {
                if (showBox == true)
                {
                    showBox = false;
                }
                else
                {
                    showBox = true;
                }
            }
            
            if (state.IsKeyDown(Keys.F) && _oldKeyState.IsKeyDown(Keys.F) == false)       //f to toggle freeview on/off
            {
                if (freeViewOn == true)
                {
                    freeViewOn = false;
                }
                else
                {
                    freeViewOn = true;
                }
            }            

            if (state.IsKeyDown(Keys.Escape))
                Exit();

            _oldKeyState = state;
        }

        private void HandleMouse()
        {
            MouseState state = Mouse.GetState();

            if (freeViewOn)
            {
                //generate edge code from click coordinates
                if (state.LeftButton == ButtonState.Pressed && _oldMouseState.LeftButton == ButtonState.Released)
                {
                    Vector2 worldPos = (_cameraPosition * -1) + new Vector2(state.X, state.Y);
                    worldPos = new Vector2((float)Math.Round(worldPos.X, 1), (float)Math.Round(worldPos.Y, 1));

                    Console.WriteLine("FixtureFactory.AttachEdge(new Vector2(" + previousClickWorldPos.X / MeterInPixels + "f, " + previousClickWorldPos.Y / MeterInPixels + "f), new Vector2(" + worldPos.X / MeterInPixels + "f, " + worldPos.Y / MeterInPixels + "f), impassableEdge);");

                    //markers.Add(new Vector2(worldPos.X - 4, worldPos.Y - 4));

                    previousClickWorldPos = worldPos;
                }
            } 

            _oldMouseState = state;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SkyBlue);

            fpsCounter.frameCounter++;

            /* Circle position and rotation */
            // Convert physics position (meters) to screen coordinates (pixels)
            //Vector2 circlePos = _circleBody.Position * MeterInPixels;
            //float circleRotation = _circleBody.Rotation;                       

            /* Ground position and origin */
            //Vector2 groundPos = _groundBody.Position * MeterInPixels;
            //Vector2 groundOrigin = new Vector2(_groundSprite.Width / 2f, _groundSprite.Height / 2f);

            // Align sprite center to body position
            //Vector2 circleOrigin = new Vector2(_circleSprite.Width / 2f, _circleSprite.Height / 2f);

            //_batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, _view);

            //_batch.Draw(tile1, new Vector2(0, 0), Color.White);

            //player pos calc
            //playerPos = (playerBody.Position * MeterInPixels) - new Vector2(player.Width / 2, player.Height / 2);

            //Console.WriteLine("PLAYER BODY POS: x=" + playerBody.Position.X + "  y: " + playerBody.Position.Y);

            //draw player
            //_batch.Draw(player, playerPos, Color.White);
            
            //Draw circle
            //_batch.Draw(_circleSprite, circlePos, null, Color.White, circleRotation, circleOrigin, 1f, SpriteEffects.None, 0f);

            //Draw ground
            //_batch.Draw(_groundSprite, groundPos, null, Color.White, 0f, groundOrigin, 1f, SpriteEffects.None, 0f);

            //_batch.End();

            //_batch.Begin();

            _batch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, _view);

           // _batch.Draw(tile1, new Vector2(0, 0), Color.White);
           // _batch.Draw(tile2, new Vector2(1280, 0), Color.White); 
                      

            _batch.Draw(A_0_0, new Vector2(0, 0), Color.White);
            _batch.Draw(A_0_720, new Vector2(0, 720), Color.White);
            _batch.Draw(A_1280_0, new Vector2(1280, 0), Color.White);
            _batch.Draw(A_1280_720, new Vector2(1280, 720), Color.White);
            _batch.Draw(A_2560_1440, new Vector2(2560, 1440), Color.White);
            _batch.Draw(A_2560_720, new Vector2(2560, 720), Color.White);
            _batch.Draw(A_3840_1440, new Vector2(3840, 1440), Color.White);
            _batch.Draw(A_3840_720, new Vector2(3840, 720), Color.White);
            _batch.Draw(A_5120_1440, new Vector2(5120, 1440), Color.White);
            _batch.Draw(A_5120_720, new Vector2(5120, 720), Color.White);
            _batch.Draw(A_M1280_0, new Vector2(-1280, 0), Color.White);
            _batch.Draw(A_M1280_720, new Vector2(-1280, 720), Color.White);            
            

            if (showBox == true)
            {
                box.Draw(_batch);
            }

            int armgunXOffset = 0;            
            Vector2 armgunOrigin = Vector2.Zero;            

            if (armgunEffects == SpriteEffects.None)
            {
                armgunXOffset = 17;
                armgunOrigin = new Vector2(6, 17);                
            }
            else if (armgunEffects == SpriteEffects.FlipHorizontally)
            {
                armgunXOffset = -18;
                armgunOrigin = new Vector2(60, 17);                

                GamePadState padState = GamePad.GetState(PlayerIndex.One, GamePadDeadZone.Circular);

                if(padState.ThumbSticks.Right.X > 0.50 || padState.ThumbSticks.Right.X < -0.50 || padState.ThumbSticks.Right.Y > 0.50 || padState.ThumbSticks.Right.Y < -0.50)
                {
                    if (armgunAngleInDegrees < -180 || armgunAngleInDegrees > -90)
                    {
                        armgunAngleInDegrees = -180 + armgunAngleInDegrees;

                        armgunAngle = (float)(armgunAngleInDegrees * Math.PI) / 180;
                    }
                    else if (armgunAngleInDegrees < 180 || armgunAngleInDegrees > 90)
                    {
                        armgunAngleInDegrees = (armgunAngleInDegrees + 180);                       

                        armgunAngle = (float)(armgunAngleInDegrees * Math.PI) / 180;                        
                    }                    
                }
            }

            headAngle = armgunAngle / 3;             //head rotates with analog stick but 3 times slower

            //Console.WriteLine("drawangle: " + armgunAngle * 180 / Math.PI);

            _batch.Draw(armgun, new Rectangle((int)box.Position.X + armgunXOffset, (int)box.Position.Y - 15, armgun.Width, armgun.Height), new Rectangle(0, 0, armgun.Width, armgun.Height), Color.White, armgunAngle, armgunOrigin, armgunEffects, 0.0f);


            //testings 
            box.Update(gameTime);
            playerAnimation.Position = box.Position;  
            playerAnimation.Update(gameTime);
            playerAnimation.Draw(_batch);



            _batch.Draw(head, new Rectangle((int)playerAnimation.Position.X + 0, (int)playerAnimation.Position.Y - 35, head.Width, head.Height), new Rectangle(0, 0, head.Width, head.Height), Color.White, headAngle, new Vector2(20, 18), armgunEffects, 0.0f);
                           

            for (int i = 0; i < tempBulletArray.Length; i++)
            {
                _batch.Draw(((Bullet)tempBulletArray.GetValue(i)).Texture, ((Bullet)tempBulletArray.GetValue(i)).CurrentPos, Color.White);
            }

            //draw markers for the points clicked in edit mode (points which make up a collision edge)
            /*if (freeViewOn)
            {
                for (int i = 0; i < markers.Count; i++)
                {
                    _batch.Draw(marker, (Vector2)markers[i], Color.White);
                }
            }*/
                        
            _batch.End();


            //draw items attached to screen as aposed to world
            _batch.Begin();

            Text = "FPS: " + fpsCounter.frameRate + "    player effect: " + playerAnimation.myEffect;

            // Display instructions
            _batch.DrawString(_font, Text, new Vector2(34f, 34f), Color.Black);
            _batch.DrawString(_font, Text, new Vector2(32f, 32f), Color.White);

            _batch.DrawString(_font, box.activity.ToString(), new Vector2(90, 105), Color.Red);

            //draw crosshair at mouse
            if (freeViewOn)
            {          
                _batch.Draw(crosshair, new Vector2(Mouse.GetState().X - 4, Mouse.GetState().Y - 4), Color.White);             
            }

            _batch.End();


            base.Draw(gameTime);
        }

        private float DegreeToRadian(float angle)
        {
            float PI = (float)Math.PI;

            return PI * angle / 180.0f;
        }
    }
}