using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using OZ.MonoGame.GameObjects.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OZ.MonoGame.MathFun.GameObjects.UI
{
    public class MenuButton : Button
    {
        public MenuButton(GamePrototype gameParent) : base(gameParent)
        {
            Size = new Vector2(200, 50)*1.6f;
            BkgTransparent = false;
        }

        public override void LoadContent(ContentManager content)
        {
            RegTexture = UIApearance.BtnReg;
            HoverTexture = UIApearance.BtnHovered;
            PressedTexture = UIApearance.BtnPressed;
            Font = UIApearance.Font;
            base.LoadContent(content);
        }

        protected override void OnHovered()
        {
            base.OnHovered();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}
