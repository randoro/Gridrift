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

namespace Gridrift
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Dictionary<Tuple<int, int>, Chunk> chunkList;
        Queue<Tuple<int, int>> chunkQueue;
        InternalServer testServer;

        #region debug
        bool debuggingActive = false;
        #endregion debug


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            chunkList = new Dictionary<Tuple<int, int>, Chunk>();
            
            
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferHeight = graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height / 2;
            graphics.PreferredBackBufferWidth = graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width / 2;
            graphics.ApplyChanges();
            this.Window.AllowUserResizing = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.IsMouseVisible = true;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            chunkQueue = new Queue<Tuple<int, int>>();
            Globals.testPlayerTexture = Content.Load<Texture2D>("playerSheet");
            Globals.testBackgroundTexture = Content.Load<Texture2D>("dXdGz");
            Globals.testFont = Content.Load<SpriteFont>("font");
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    chunkList.Add(Tuple.Create(j-5, i-5), new Chunk(j-5, i-5));
                }
            }
            testServer = new InternalServer(false);
            
        }
        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                //graphics.IsFullScreen = false;
                //graphics.ApplyChanges();
                this.Exit();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.F3))
            {
                debuggingActive = !debuggingActive;
            }
            

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                graphics.ToggleFullScreen();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                Player.changeVelocity(new Vector2(-0.7f, 0));
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                Player.changeVelocity(new Vector2(0.7f, 0));
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                Player.changeVelocity(new Vector2(0,-0.7f));
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Player.changeVelocity(new Vector2(0, 0.7f));
            }

            Player.update(gameTime);

            Globals.currentWindowHeight = this.Window.ClientBounds.Height;
            Globals.currentWindowWidth = this.Window.ClientBounds.Width;

            //Console.WriteLine("x:" + this.Window.ClientBounds.X + " y:" + this.Window.ClientBounds.Y + " height:" + this.Window.ClientBounds.Height + " width:" + this.Window.ClientBounds.Width);
            //Console.WriteLine("height:" + graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height + " width:" + graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width);
            


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            
            
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Matrix.CreateTranslation(Globals.currentWindowWidth / 2 - Player.getPosition().X, Globals.currentWindowHeight / 2 - Player.getPosition().Y, 0));

            Point p = Translation.exactPosToChunkCoords(Player.getPosition());

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (chunkList.ContainsKey(Tuple.Create(p.X + j - 1, p.Y + i - 1)))
                    {
                        Chunk chu = chunkList[Tuple.Create(p.X + j - 1, p.Y + i - 1)];
                        chu.draw(spriteBatch);
                    }
                }
            }
            
            //foreach (KeyValuePair<Tuple<int, int>, Chunk> kvp in chunkList)
            //{
            //    kvp.Value.draw(spriteBatch);
            //}

            //spriteBatch.Draw(Globals.testPigTexture, new Rectangle(Player.getPosition().X, Player.getPosition().Y, 32, 32), Color.White);
            //spriteBatch.DrawString(Globals.testFont, "This is a test string", new Vector2(0, 0), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            Player.draw(spriteBatch);

            #region debug
            if (debuggingActive)
            {
                Point playerPos = Player.getPosition();
                Vector2 cameraPos = Camera.cameraPosition();
                spriteBatch.DrawString(Globals.testFont, "Player: x:" + playerPos.X + " y:" + playerPos.Y, new Vector2(cameraPos.X, cameraPos.Y), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                spriteBatch.DrawString(Globals.testFont, "Player Chunk: x:" + p.X + " y:" + p.Y, new Vector2(cameraPos.X, cameraPos.Y + 32), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                spriteBatch.DrawString(Globals.testFont, "Chunks Loaded:" + chunkList.Count, new Vector2(cameraPos.X, cameraPos.Y + 64), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            
            }
            #endregion debug

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
