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
    public class GameTextBox : TextBox
    {
        public GameTextBox(GamePrototype gameParent) : base(gameParent)
        {
            Size = new Vector2(200, 50)*1.6f;
        }

        public override void Initialize()
        {
            base.Initialize();
            MaxCharacters = 10;
            SamanColor = ForeColor = Color.White;
            TextAnchor = Anchor.Center | Anchor.Left;
            RectangleOfContentDrawing = new Rectangle()
            {
                Location = new Point(60, 10),
                Size = new Point(596, 140)
            };

            LookingChanged += GameTextBox_LookingChanged;
        }

        private void GameTextBox_LookingChanged(object sender, string e)
        {

        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            RegTexture = UIApearance.TextBoxReg;
            HoverTexture = UIApearance.TextBoxHovered;
            PressedTexture = UIApearance.TextBoxPressed;
            Font = UIApearance.Font;
        }

        
    }
}
