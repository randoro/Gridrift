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
        Texture2D testPigTexture;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            
            
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferHeight = graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            graphics.PreferredBackBufferWidth = graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width;
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.IsMouseVisible = true;
            spriteBatch = new SpriteBatch(GraphicsDevice);
            testPigTexture = Content.Load<Texture2D>("dXdGz");
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
                
            
            //Console.WriteLine("x:" + this.Window.ClientBounds.X + " y:" + this.Window.ClientBounds.Y + " height:" + this.Window.ClientBounds.Height + " width:" + this.Window.ClientBounds.Width);
            Console.WriteLine("height:" + graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Height + " width:" + graphics.GraphicsDevice.Adapter.CurrentDisplayMode.Width);
            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(testPigTexture, new Rectangle(100, 100, 512, 512), Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
