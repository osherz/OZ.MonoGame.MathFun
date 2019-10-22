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

}/*


        /*public Game Parent { get; set; }

        public static Texture2D CardReg { get; set; }
        public static Texture2D HoverCard { get; set; }
        public static Texture2D CardPressed { get; set; }
        public static SpriteFont TextFont { get; set; }

        public Texture2D CardToDraw
        {
            get
            {
                if (IsPressed)
                {
                    return CardPressed;
                }
                else if (IsHover)
                {
                    return HoverCard;
                }
                else
                {
                    return CardReg;
                }
            }
        }

        public Vector2 Size { get; set; }
        public Vector2 Location { get; set; }
        


        private bool _isHover = false;
        public bool IsHover
        {
            get => _isHover;

            set
            {
                bool old = _isHover;
                _isHover = value;
                if(IsHover && !old)
                {
                    OnHovered();
                }
            }
        }


        protected abstract string Text { get; }

        public static SoundEffect CardHoverEffect { get; set; }
        public static SoundEffect CardPressedEffect { get; set; }

        public event EventHandler Hovered;

        public virtual void Initialize()
        {

        }

        public virtual void LoadContent(ContentManager content)
        {

        }

        public virtual void UnloadContent(ContentManager content)
        {

        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public bool IsContains(Point point)
        {
            return new Rectangle(Location.ToPoint(), Size.ToPoint()).Contains(point);
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 scale = new Vector2()
            {
                X = Size.X / CardToDraw.Width,
                Y = Size.Y / CardToDraw.Height
            };

            spriteBatch.Draw(CardToDraw,
                            Location + Size / 2,
                            null,
                            Color.White,
                            0,//rotation: (float)gameTime.TotalGameTime.TotalMilliseconds/(1000*1.1f),
                            new Vector2(CardToDraw.Width, CardToDraw.Height) / 2,
                            scale,
                            SpriteEffects.None,
                            1);

            DrawCardText(spriteBatch);
        }

        private void DrawCardText(SpriteBatch spriteBatch)
        {
            if (base.IsVisible)
            {
                string text = Text;
                Vector2 originalSize = TextFont.MeasureString(text);
                Vector2 textLocation = new Vector2()
                {
                    Y = Location.Y + (Size.Y - originalSize.Y) / 2,
                    X = Location.X + (Size.X - originalSize.X) / 2
                };

                spriteBatch.DrawString(TextFont,
                                        text,
                                        textLocation,
                                        Color.Black,
                                        0,
                                        Vector2.One,
                                        1,
                                        SpriteEffects.None,
                                        1);
            }
        }




        protected virtual void OnHovered()
        {
            if (!IsPressed)
            {
                CardHoverEffect.Play();
            }

            Hovered?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnVisibled()
        {
            CardPressedEffect.Play();

            base.OnVisibled();
        }*/
    