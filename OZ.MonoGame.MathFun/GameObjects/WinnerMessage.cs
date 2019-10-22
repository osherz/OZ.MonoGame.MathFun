using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using OZ.MonoGame.GameObjects;

namespace OZ.MonoGame.MathFun.GameObjects
{
    public class WinnerMessage : IGameObject
    {
        public string WinnerName { get; set; } = "player";

        public SpriteFont Font { get; set; }
        private Texture2D _bkg;

        public Rectangle Rect { get; set; }
        public Vector2 Size { get; set; }

        public WinnerMessage() { }

        public void Initialize()
        {

        }

        public void LoadContent(ContentManager content)
        {
            _bkg = content.Load<Texture2D>("sprites/winnerMessage");
        }

        public void UnloadContent(ContentManager content)
        {

        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Calculate scale for size
            float scale = .9f;
            Vector2 currentSize = new Vector2(_bkg.Width, _bkg.Height);
            Vector2 sizeOfBkg = Size;
            if (sizeOfBkg.X < currentSize.X || sizeOfBkg.Y < currentSize.Y)
            {

                scale = Size.X / _bkg.Width;
                sizeOfBkg.Y = _bkg.Height * scale;

                if (sizeOfBkg.Y > Size.Y)
                {
                    scale = Size.Y / _bkg.Height;
                    sizeOfBkg.X = _bkg.Width * scale;
                    sizeOfBkg.Y = _bkg.Height * scale;
                }
            }
            else
            {
                sizeOfBkg = currentSize * scale;
            }

            //Calculate middle location
            Vector2 location = new Vector2()
            {
                X = Rect.X + (Rect.Width - sizeOfBkg.X) / 2,
                Y = Rect.Y + (Rect.Height - sizeOfBkg.Y) / 2 - 
                                (float)Math.Sin(gameTime.TotalGameTime.TotalMilliseconds*1/300)*40 // Animate mwssage top and down
            };

            //Draw background
            spriteBatch.Draw(_bkg,
                            location,
                            null,
                            Color.White,
                            0,
                            Vector2.Zero,
                            scale,
                            SpriteEffects.None,
                            1);

            //Draw text
            Rectangle textRect = new Rectangle()
            {
                Location = (new Vector2(166, 62) * scale).ToPoint(),
                Size = (new Vector2(230, 40) * scale).ToPoint()
            };

            Vector2 textSize = Font.MeasureString(WinnerName);
            float textScale = 1;
            if(textSize.X > textRect.Width)
            {
                textScale = textRect.Width / textSize.X;
            }
            if(textSize.Y*scale > textRect.Height)
            {
                textScale = textRect.Height / textSize.Y;
            }

            Vector2 textLocationFromRight = new Vector2()
            {
                Y = location.Y + textRect.Y,
                X = location.X + textRect.Right
            };

            Vector2 middleLocation = new Vector2()
            {
                X = (textRect.Width - textSize.X) / 2,
                Y = (textRect.Height - textSize.Y) / 2
            } + location + textRect.Location.ToVector2();

            spriteBatch.DrawString(Font,
                                    WinnerName,
                                    middleLocation,
                                    Color.Black,
                                    0,
                                    Vector2.Zero, //new Vector2(textSize.X, 0), // for origin to right
                                    scale,
                                    SpriteEffects.None,
                                    1);

        }
    }
}
