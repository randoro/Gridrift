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
        InternalServer internalServer;
        Point chunkLoadRange = new Point(5, 3); //set to 5,5 for more lag but no glapping
        Point offset = new Point(2, 1); //set to 2,2 for more lag but no glapping

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
            internalServer = new InternalServer(false);

            //for (int i = 0; i < 10; i++)
            //{
            //    for (int j = 0; j < 10; j++)
            //    {
            //        Point chunkID = new Point(j - 5, i - 5);
            //        if (chunkID.X == 0 && chunkID.Y == 0)
            //        {
            //            int test = 1;
            //        }
            //        chunkList.Add(Tuple.Create(j - 5, i - 5), internalServer.getChunk(new World("world"), chunkID));
            //    }
            //}
            
            
        }
        protected override void UnloadContent()
        {
            internalServer.closeServer();


        }

        protected override void Update(GameTime gameTime)
        {
            Point currentChunkCoords = Translation.exactPosToChunkCoords(Player.getPosition());

            for (int i = 0; i < chunkLoadRange.Y; i++)
            {
                for (int j = 0; j < chunkLoadRange.X; j++)
                {
                    if (!chunkList.ContainsKey(Tuple.Create(currentChunkCoords.X + j - offset.X, currentChunkCoords.Y + i - offset.Y)))
                    {
                        Point newChunkCoords = new Point(currentChunkCoords.X + j - offset.X, currentChunkCoords.Y + i - offset.Y);
                        Chunk newChunk = internalServer.getChunk(new World("world"), newChunkCoords);
                        if (newChunk != null)
                        {
                            if (newChunk.terrainPopulated == 1)
                            {
                                chunkList.Add(Tuple.Create(currentChunkCoords.X + j - offset.X, currentChunkCoords.Y + i - offset.Y), newChunk);
                            }
                        }
                    }
                }
            }
            foreach (KeyValuePair<Tuple<int, int>, Chunk> chunkPair in chunkList)
            {
                Point currentChunkID = new Point(chunkPair.Key.Item1, chunkPair.Key.Item2);
                bool withinReach = Translation.withinReach(currentChunkCoords, currentChunkID, offset.X);
                if (!withinReach)
                {
                    //Console.WriteLine("Chunks in C chunkList: " + chunkList.Count);
                    chunkList.Remove(chunkPair.Key);
                    break;
                }
            }



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

            float multiplier = 1.0f; //almost not noticable
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                Player.changeVelocity(new Vector2(-0.7f * multiplier, 0));
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                Player.changeVelocity(new Vector2(0.7f * multiplier, 0));
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                Player.changeVelocity(new Vector2(0, -0.7f * multiplier));
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                Player.changeVelocity(new Vector2(0, 0.7f * multiplier));
            }

            Player.update(gameTime);
            internalServer.syncUpdate();

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
            int chunksdrawn = 0;
            for (int i = 0; i < chunkLoadRange.Y; i++)
            {
                for (int j = 0; j < chunkLoadRange.X; j++)
                {
                    if (chunkList.ContainsKey(Tuple.Create(p.X + j - offset.X, p.Y + i - offset.Y)))
                    {
                        Chunk chu = chunkList[Tuple.Create(p.X + j - offset.X, p.Y + i - offset.Y)];
                        if (chu != null)
                        {
                            chunksdrawn++;
                            chu.draw(spriteBatch);
                        }
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
                int frameRate = Utility.CalculateFrameRate();
                int offset = 0;
                spriteBatch.DrawString(Globals.testFont, "Player: x:" + playerPos.X + " y:" + playerPos.Y, new Vector2(cameraPos.X, cameraPos.Y + offset), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                offset += 32;
                spriteBatch.DrawString(Globals.testFont, "Player Chunk: x:" + p.X + " y:" + p.Y, new Vector2(cameraPos.X, cameraPos.Y + offset), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                offset += 32;
                spriteBatch.DrawString(Globals.testFont, "Client Chunks Loaded:" + chunkList.Count, new Vector2(cameraPos.X, cameraPos.Y + offset), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                offset += 32;
                spriteBatch.DrawString(Globals.testFont, "FPS:" + frameRate, new Vector2(cameraPos.X, cameraPos.Y + offset), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                offset += 32;
                spriteBatch.DrawString(Globals.testFont, "Chunks Drawn:" + chunksdrawn, new Vector2(cameraPos.X, cameraPos.Y + offset), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                offset += 32;
                spriteBatch.DrawString(Globals.testFont, "IS Chunks Loaded:" + InternalServer.ISchunkCount, new Vector2(cameraPos.X, cameraPos.Y + offset), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                offset += 32;
                spriteBatch.DrawString(Globals.testFont, "IS Regions Loaded:" + InternalServer.ISregionCount, new Vector2(cameraPos.X, cameraPos.Y + offset), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                
            }
            #endregion debug

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
