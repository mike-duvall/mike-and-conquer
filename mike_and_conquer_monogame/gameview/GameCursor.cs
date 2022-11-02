using Vector2 = Microsoft.Xna.Framework.Vector2;
using Texture2D = Microsoft.Xna.Framework.Graphics.Texture2D;
using Color = Microsoft.Xna.Framework.Color;
using GameTime = Microsoft.Xna.Framework.GameTime;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;
using SpriteEffects = Microsoft.Xna.Framework.Graphics.SpriteEffects;

// using ISpriteFrame = mike_and_conquer.openra.ISpriteFrame;


using Mouse = Microsoft.Xna.Framework.Input.Mouse;
using MouseState = Microsoft.Xna.Framework.Input.MouseState;

using MikeAndConquerGame = mike_and_conquer_monogame.main.MikeAndConquerGame;



using ImmutablePalette = mike_and_conquer_monogame.openra.ImmutablePalette;
using ShpD2Loader = mike_and_conquer_monogame.openra.ShpD2Loader;
using ISpriteFrame = mike_and_conquer_monogame.openra.ISpriteFrame;




namespace mike_and_conquer_monogame.gameview
{


    public class GameCursor
    {
        public Vector2 position { get; set; }

        public string StateAsString
        {
            get { return RenderCursorStateAsString(); }
        }

        private Texture2D texture;
        private Texture2D mainCursorTexture;
        private Texture2D moveToLocationCursorTexture;
        private Texture2D movementNotAllowedCursorTexture;
        private Texture2D attackEnemyCursor;
        private Texture2D buildConstructionYardCursorTexture;

        private Vector2 middleOfSprite;

        private GameCursor()
        {

        }

        public GameCursor(int x, int y)
        {

            this.mainCursorTexture = loadTextureFromD2ShpFile(MikeAndConquerGame.CONTENT_DIRECTORY_PREFIX + "mouse.shp", 0);
            this.moveToLocationCursorTexture = loadTextureFromD2ShpFile(MikeAndConquerGame.CONTENT_DIRECTORY_PREFIX + "mouse.shp", 10);
            this.movementNotAllowedCursorTexture = loadTextureFromD2ShpFile(MikeAndConquerGame.CONTENT_DIRECTORY_PREFIX + "mouse.shp", 11);
            this.attackEnemyCursor = loadTextureFromD2ShpFile(MikeAndConquerGame.CONTENT_DIRECTORY_PREFIX + "mouse.shp", 18);
            this.buildConstructionYardCursorTexture = loadTextureFromD2ShpFile(MikeAndConquerGame.CONTENT_DIRECTORY_PREFIX + "mouse.shp", 55);
            // 0 = main cursor
            // 10 = select movement location pointer
            // 11 = movement not allowed to this point, pointer
            // 18 through 25, attack enemy pointer
            // 54?? through 62??, build construction yard point

            this.texture = mainCursorTexture;
            position = new Vector2(x, y);

            middleOfSprite = new Vector2();
            middleOfSprite.X = 0;
            middleOfSprite.Y = 0;

        }

        public void SetToMainCursor()
        {
            this.texture = this.mainCursorTexture;
            middleOfSprite.X = 0;
            middleOfSprite.Y = 0;
        }

        public void SetToMovementNotAllowedCursor()
        {
            this.texture = movementNotAllowedCursorTexture;
            middleOfSprite.X = this.texture.Width / 2;
            middleOfSprite.Y = this.texture.Height / 2;
        }

        public void SetToMoveToLocationCursor()
        {
            this.texture = moveToLocationCursorTexture;
            middleOfSprite.X = this.texture.Width / 2;
            middleOfSprite.Y = this.texture.Height / 2;
        }

        public void SetToAttackEnemyLocationCursor()
        {
            this.texture = attackEnemyCursor;
            middleOfSprite.X = this.texture.Width / 2;
            middleOfSprite.Y = this.texture.Height / 2;
        }

        public void SetToBuildConstructionYardCursor()
        {
            this.texture = buildConstructionYardCursorTexture;
            middleOfSprite.X = this.texture.Width / 2;
            middleOfSprite.Y = this.texture.Height / 2;

        }


        public void Update(GameTime gameTime)
        {
            MouseState newState = Mouse.GetState();
            position = new Vector2(newState.X , newState.Y );
        }

        internal void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            float scale = GameWorldView.instance.MapZoom;
            spriteBatch.Draw(texture, position, null, Color.White, 0f, middleOfSprite, scale, SpriteEffects.None, 0f);
        }


        // TODO:  Put this where rest of shp file loading code is
        internal Texture2D loadTextureFromD2ShpFile(string shpFileName, int indexOfFrameToLoad)
        {

            int[] remap = { };

            ImmutablePalette palette = new ImmutablePalette(MikeAndConquerGame.CONTENT_DIRECTORY_PREFIX + "temperat.pal", remap);

            System.IO.FileStream shpStream = System.IO.File.Open(shpFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.None);

            ShpD2Loader shpD2Loader = new ShpD2Loader();
            ISpriteFrame[] frames = new ISpriteFrame[180];
            shpD2Loader.TryParseSprite(shpStream, out frames);

            ISpriteFrame frame = frames[indexOfFrameToLoad];
            byte[] frameData = frame.Data;

            Texture2D texture2D = new Texture2D(MikeAndConquerGame.instance.GraphicsDevice, frame.Size.Width, frame.Size.Height);
            int numPixels = texture2D.Width * texture2D.Height;
            Color[] texturePixelData = new Color[numPixels];

            for (int i = 0; i < numPixels; i++)
            {

                int paletteCode = frameData[i];


                if (indexOfFrameToLoad == 10 || indexOfFrameToLoad == 55)
                {
                    // TODO, BOGUS: Having to manually
                    // tweak the palette offsets for the 
                    // movement destination and
                    // "build construction yard" cursors
                    // Not sure why.  Other ones
                    // seems to need no tweak
                    if (paletteCode == 124)
                    {
                        paletteCode = 4;
                    }
                    if (paletteCode == 125)
                    {
                        paletteCode = 3;
                    }

                }

                uint paletteX = palette[paletteCode];
                System.Drawing.Color systemColor = System.Drawing.Color.FromArgb((int)paletteX);
                Color xnaColor = new Color(systemColor.R, systemColor.G, systemColor.B, systemColor.A);
                texturePixelData[i] = xnaColor;

            }
            texture2D.SetData(texturePixelData);
            shpStream.Close();
            return texture2D;

        }



        private string RenderCursorStateAsString()
        {
            if (texture == movementNotAllowedCursorTexture)
            {
                return "MovementNotAllowedCursor";
            }
            else if (texture == moveToLocationCursorTexture)
            {
                return "MoveToLocationCursor";
            }
            else if (texture == attackEnemyCursor)
            {
                return "AttackEnemyCursor";
            }
            else if (texture == mainCursorTexture)
            {
                return "DefaultArrowCursor";
            }
            else if (texture == buildConstructionYardCursorTexture)
            {
                return "BuildConstructionYardCursor";
            }
            else
            {
                return "UnknownCursorState";
            }
        }


    }



}
