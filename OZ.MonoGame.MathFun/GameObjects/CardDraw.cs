using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using OZ.MonoGame.MathFun.GameEngine.Cards;
using OZ.MonoGame.GameObjects;
using OZ.MonoGame.GameObjects.UI;
using OZ.MonoGame.MathFun.GameObjects.UI;

namespace OZ.MonoGame.MathFun.GameObjects
{
    public class CardDraw : Button, ICard, IGameObject
    {
        private ICard _card;
        protected virtual ICard Card
        {
            get => _card;

            set
            {
                OnBeforeCardChanged();
                _card = value;
                OnCardChanged();
            }
        }

        public bool IsValueVisible { get => Card is null? false : Card.IsValueVisible; set => Card.IsValueVisible = value; }
        public float Value => Card.Value;
        public Vector2 Middle => LocationInWindow + Size / 2;

        protected override Texture2D TextureToDrawIfEnabled => IsValueVisible ? PressedTexture : base.TextureToDrawIfEnabled;

        public Point Position { get; internal set; }

        public event EventHandler Visibled { add => Card.Visibled += value; remove => Card.Visibled -= value; }
        public event EventHandler VisibleChanged { add => Card.VisibleChanged += value; remove => Card.VisibleChanged -= value; }


        public CardDraw(GamePrototype gameParent) : base(gameParent)
        {
            BkgTransparent = false;
            BlackTextureWhenNotEnabled = false;
            IsEnabled = true;
            IsVisible = true;
            ForeColor = Color.Black;
        }


        public void HiddenValue()
        {
            Card.HiddenValue();
        }
        public void VisibleValue()
        {
            Card.VisibleValue();
        }

        protected override void OnClicked()
        {
            base.OnClicked();
        }

        private void OnCardChanged()
        {
            RegisterToEvents();
        }

        private void RegisterToEvents()
        {
            if(!(Card is null))
            { 
                Card.VisibleChanged += Card_VisibleChanged;
            }

        }

        private void OnBeforeCardChanged()
        {
            UnregisterFromCardEvrnts();
        }

        private void UnregisterFromCardEvrnts()
        {
            if (!(Card is null))
            {
                Card.VisibleChanged -= Card_VisibleChanged;
            }
        }


        private void Card_VisibleChanged(object sender, EventArgs e)
        {
            Text = IsValueVisible ? ToString() : "";
        }



        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            RegTexture = UIApearance.CardReg;
            HoverTexture = UIApearance.HoverCard;
            PressedTexture = UIApearance.CardPressed;

            Font = UIApearance.CardTextFont;

            AudioInHovered = UIApearance.CardHoverEffect;
            AudioInPressed = UIApearance.CardPressedEffect;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override string ToString()
        {
            return Card.ToString();
        }

    }

}


    