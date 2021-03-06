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
using Gridrift.Server;
using Gridrift.Utility;
using Gridrift.Rendering;
using System.Threading;
using Gridrift.GUIs;
using System.Net.Sockets;
using System.Net;

namespace Gridrift
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //Dictionary<Tuple<int, int>, Chunk> chunkList;
        InternalServer internalServer;
        Thread singlePlayerServerThread;
        Point chunkLoadRange = new Point(7, 5); //set to 7,5 for more lag but no glapping. lowest: 5,3 high: 7,5
        Point offset = new Point(3, 2); //set to 3,2 for more lag but no glapping. lowest: 2,1 high: 3,2
        long lastsyncUpdate;
        GameState gameState;
        GUI toolbar;
        String line = "";

        Thread listeningThread;
        TcpClient client;

        #region debug
        public static bool debuggingActive = false;
        #endregion debug


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
            
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
            Globals.testPlayerTexture = Content.Load<Texture2D>("playerSheet");
            Globals.testBackgroundTexture = Content.Load<Texture2D>("dXdGz");
            Globals.testGUITexture = Content.Load<Texture2D>("gui");
            Globals.testFont = Content.Load<SpriteFont>("font");

            //chunkList = new Dictionary<Tuple<int, int>, Chunk>();

            internalServer = new InternalServer(false);
            Player.healthPercent = 1.0f;
            toolbar = new ToolBar();
            
            lastsyncUpdate = DateTime.Now.Ticks;

           
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


            AsynchronousClient.StopClient();
            
            internalServer.closeServer();
            //clientConnection.stopListening();

            //while (internalServer.isRunning) { }
            //singlePlayerServerThread.Abort();



        }

        private void startListening()
        {
            client = new TcpClient();
            client.Connect("127.0.0.1", 8888);
            while (true)
            {
                Thread.Sleep(1000);
                try
                {
                    while (!client.Connected)
                    {
                        //wait for connection
                    }
                    NetworkStream networkStream = client.GetStream();
                    if (networkStream != null)
                    {
                        byte[] buffer = { 1, 2, 3, 4 };
                        networkStream.Write(buffer, 0, 4);
                        networkStream.Read(buffer, 0, 4);
                        line = "{ " + buffer[0] + ", " + buffer[1] + ", " + buffer[2] + ", " + buffer[3] + " }";
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" >> " + ex.ToString());
                }
            }
        }


        protected override void Update(GameTime gameTime)
        {
            KeyMouseReader.update(gameTime);

            syncUpdate();


            if (KeyMouseReader.KeyPressed(Keys.Escape))
            {
                //graphics.IsFullScreen = false;
                //graphics.ApplyChanges();
                this.Exit();
            }

            if (KeyMouseReader.KeyPressed(Keys.F3))
            {
                debuggingActive = !debuggingActive;
            }


            if (KeyMouseReader.KeyPressed(Keys.Space))
            {
                listeningThread = new Thread(new ThreadStart(AsynchronousClient.StartClient));
                listeningThread.Start();
                //graphics.PreferredBackBufferHeight = graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height;
                //graphics.PreferredBackBufferWidth = graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
                //graphics.IsFullScreen = true;
                //graphics.ApplyChanges();
            }

            float multiplier = 1.0f; //almost not noticable
            if (KeyMouseReader.KeyHold(Keys.A))
            {
                Player.changeVelocity(new Vector2(-0.7f * multiplier, 0));
            }
            else if (KeyMouseReader.KeyHold(Keys.D))
            {
                Player.changeVelocity(new Vector2(0.7f * multiplier, 0));
            }
            if (KeyMouseReader.KeyHold(Keys.W))
            {
                Player.changeVelocity(new Vector2(0, -0.7f * multiplier));
            }
            else if (KeyMouseReader.KeyHold(Keys.S))
            {
                Player.changeVelocity(new Vector2(0, 0.7f * multiplier));
            }

            
            Player.update(gameTime);
            internalServer.syncUpdate();
            toolbar.update(gameTime);

            Globals.currentWindowHeight = this.Window.ClientBounds.Height;
            Globals.currentWindowWidth = this.Window.ClientBounds.Width;

            //Console.WriteLine("x:" + this.Window.ClientBounds.X + " y:" + this.Window.ClientBounds.Y + " height:" + this.Window.ClientBounds.Height + " width:" + this.Window.ClientBounds.Width);
            //Console.WriteLine("height:" + graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height + " width:" + graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width);
            


            base.Update(gameTime);
        }


        public void syncUpdate()
        {
            DateTime now = DateTime.Now;
            long nowTicks = now.Ticks;
            if (nowTicks > (lastsyncUpdate + (TimeSpan.TicksPerSecond) / 20))
            {
                lastsyncUpdate = lastsyncUpdate + (TimeSpan.TicksPerSecond / 20);

                Point currentChunkCoords = Translation.exactPosToChunkCoords(Player.getPosition());

                for (int i = 0; i < chunkLoadRange.Y; i++)
                {
                    for (int j = 0; j < chunkLoadRange.X; j++)
                    {
                        //if (!internalServer.chunkList.ContainsKey(Tuple.Create(currentChunkCoords.X + j - offset.X, currentChunkCoords.Y + i - offset.Y)))
                        //{
                            Point newChunkCoords = new Point(currentChunkCoords.X + j - offset.X, currentChunkCoords.Y + i - offset.Y);
                            Chunk newChunk = internalServer.getChunk(new World("world"), newChunkCoords);
                            //if (newChunk != null)
                           // {
                               // if (newChunk.terrainPopulated == 1)
                               // {
                                    //internalServer.chunkList.Add(Tuple.Create(currentChunkCoords.X + j - offset.X, currentChunkCoords.Y + i - offset.Y), newChunk);
                               // }
                           // }
                       // }
                    }
                }
                //foreach (KeyValuePair<Tuple<int, int>, Chunk> chunkPair in internalServer.chunkList)
                //{
                //    Point currentChunkID = new Point(chunkPair.Key.Item1, chunkPair.Key.Item2);
                //    bool withinReach = Translation.withinReach(currentChunkCoords, currentChunkID, offset.X);
                //    if (!withinReach)
                //    {
                //        //Console.WriteLine("Chunks in C chunkList: " + chunkList.Count);
                //        internalServer.unloadChunk(chunkPair.Key);
                //        break;
                //    }
                //}


                Point currentPlayerPos = Translation.exactPosToChunkCoords(Player.getPosition());

                Chunk newChunk2;
                internalServer.chunkList.TryGetValue(Tuple.Create(currentChunkCoords.X, currentChunkCoords.Y), out newChunk2);

                Point currentPlayerBlock = Translation.exactPosToBlockCoords(Player.getPosition());
                Point internalChunkBlock = Translation.blockCoordsToInternalChunkBlockCoords(currentPlayerBlock);
                newChunk2.blocks[internalChunkBlock.X + internalChunkBlock.Y * 16] = 1;
            }
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
                    if (internalServer.chunkList.ContainsKey(Tuple.Create(p.X + j - offset.X, p.Y + i - offset.Y)))
                    {
                        Chunk chu = internalServer.chunkList[Tuple.Create(p.X + j - offset.X, p.Y + i - offset.Y)];
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
            Vector2 cameraPos = Camera.cameraPosition();

            toolbar.draw(spriteBatch);
            spriteBatch.Draw(Globals.testGUITexture, new Vector2(cameraPos.X + (Globals.currentWindowWidth / 2) - (376 / 2), cameraPos.Y + Globals.currentWindowHeight - 53 - 10), new Rectangle(0, 0, 376, 53), Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1f);
                

            #region debug
            if (debuggingActive)
            {
                Point playerPos = Player.getPosition();
                int frameRate = FrameCounter.CalculateFrameRate();
                int offset = 0;
                spriteBatch.DrawString(Globals.testFont, "Player: x:" + playerPos.X + " y:" + playerPos.Y, new Vector2(cameraPos.X, cameraPos.Y + offset), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                offset += 32;
                spriteBatch.DrawString(Globals.testFont, "Player Chunk: x:" + p.X + " y:" + p.Y, new Vector2(cameraPos.X, cameraPos.Y + offset), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                offset += 32;
                spriteBatch.DrawString(Globals.testFont, "Client Chunks Loaded(same now):" + internalServer.chunkList.Count, new Vector2(cameraPos.X, cameraPos.Y + offset), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                offset += 32;
                spriteBatch.DrawString(Globals.testFont, "FPS:" + frameRate, new Vector2(cameraPos.X, cameraPos.Y + offset), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                offset += 32;
                spriteBatch.DrawString(Globals.testFont, "Chunks Drawn:" + chunksdrawn, new Vector2(cameraPos.X, cameraPos.Y + offset), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                offset += 32;
                spriteBatch.DrawString(Globals.testFont, "IS Chunks Loaded:" + InternalServer.ISchunkCount, new Vector2(cameraPos.X, cameraPos.Y + offset), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                offset += 32;
                spriteBatch.DrawString(Globals.testFont, "IS Regions Loaded:" + InternalServer.ISregionCount, new Vector2(cameraPos.X, cameraPos.Y + offset), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                offset += 32;
                spriteBatch.DrawString(Globals.testFont, line, new Vector2(cameraPos.X, cameraPos.Y + offset), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

                //char[] char1 = new char[96];
                //for (int i = 32; i < 128; i++)
                //{
                //    char1[i-32] = (char)(i+1);
                //}

                string str = "!\"#$%&'()*+,-./0123456789:;<=>?@�ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]^_`abcdefghijklmnopqrstuvwxyz{|}~";
                string str2 = "The quick brown fox jumps over the lazy dog. Something really long written that I can use to check if this font would be okay to use for GridRift.";
                offset += 32;
                spriteBatch.DrawString(Globals.testFont, str2, new Vector2(cameraPos.X, cameraPos.Y + offset), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

            }
            #endregion debug

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
