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
            Globals.testPigTexture = Content.Load<Texture2D>("dXdGz");
            chunkList.Add(Tuple.Create(0, 0), new Chunk(0, 0));
            chunkList.Add(Tuple.Create(-1, -1), new Chunk(-1, -1));
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

            Player.updatePosition();

            Globals.currentWindowHeight = this.Window.ClientBounds.Height;
            Globals.currentWindowWidth = this.Window.ClientBounds.Width;

            //Console.WriteLine("x:" + this.Window.ClientBounds.X + " y:" + this.Window.ClientBounds.Y + " height:" + this.Window.ClientBounds.Height + " width:" + this.Window.ClientBounds.Width);
            //Console.WriteLine("height:" + graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height + " width:" + graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width);
            


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            
            
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, Matrix.CreateTranslation(Globals.currentWindowWidth / 2 - Player.getPosition().X, Globals.currentWindowHeight / 2 - Player.getPosition().Y, 0));


            foreach (KeyValuePair<Tuple<int, int>, Chunk> kvp in chunkList)
            {
                kvp.Value.draw(spriteBatch);
            }

            spriteBatch.Draw(Globals.testPigTexture, new Rectangle(Player.getPosition().X, Player.getPosition().Y, 32, 32), Color.White);
                
            
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
