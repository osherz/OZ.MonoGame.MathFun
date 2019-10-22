using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using OZ.MonoGame.GameObjects;
using OZ.MonoGame.GameObjects.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OZ.MonoGame.MathFun.GameObjects.UI
{
    public class GameMenu : Menu, ILocation
    {

        public ControlApearance ButtonsApearance { get; internal set; }

        public GameMenu(GamePrototype gameParent) : base(gameParent)
        {
            TextRectangle = new Rectangle(168, 12, 218, 69);
            RectangleOfContentDrawing = new Rectangle(100, 155, 350, 475);
            RectangleOfContentAfterResizeing = false;
            BkgColor = Color.White;
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);          
        }

        protected override void OnGameParentChanged(EventArgs e)
        {
            float size = GameParent.Graphics.PreferredBackBufferHeight * 4 / 5;
            Size = new Vector2(size);

            base.OnGameParentChanged(e);
        }

        protected override void InDraw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.InDraw(gameTime, spriteBatch);
        }

    }
}
