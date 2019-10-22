using Microsoft.Xna.Framework;
using OZ.MonoGame.MathFun.GameObjects.UI;
using OZ.MonoGame.GameObjects;
using OZ.MonoGame.GameObjects.Effects;
using OZ.MonoGame.GameObjects.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OZ.MonoGame.MathFun.GameObjects.Effects
{
    public class ReplaceMenus : Animation, IAnimation
    {
        public Menu MenuToReplace { get; set; }
        public Menu NextMenu { get; set; }
        public Vector2 WindowSize { get; set; }
        public double TimeToReach
        {
            get => _nextMenuAnimation.TimeToReach;
            set => _nextMenuAnimation.TimeToReach = _MenuToReplaceAnimation.TimeToReach = value;
        }

        public Direction AnimateDirection { get; set; }

        MoveToAnimation _MenuToReplaceAnimation, _nextMenuAnimation;

        public ReplaceMenus(GameMenu menuToReplace, GameMenu nextMenu)
        {
            MenuToReplace = menuToReplace;
            NextMenu = nextMenu;

            _MenuToReplaceAnimation = new MoveToAnimation()
            {
                Obj = MenuToReplace
            };

            _nextMenuAnimation = new MoveToAnimation()
            {
                Obj = NextMenu
            };
        }

        public override void StartAnimate()
        {
            _MenuToReplaceAnimation.Destination = CalcMenuToReplaceDestination();

            Rectangle windowRect = new Rectangle()
            {
                Location = Point.Zero,
                Size = WindowSize.ToPoint()
            };
            Vector2 midNextLocation = UIHelper.ToMiddle(windowRect, NextMenu.Size);
            NextMenu.Location = CalcNextMenuSource(midNextLocation);
            _nextMenuAnimation.Destination = midNextLocation;
            NextMenu.IsVisible = true;

            MenuToReplace.IsEnabled = NextMenu.IsEnabled = false;

            _MenuToReplaceAnimation.StartAnimate();
            _nextMenuAnimation.StartAnimate();

            base.StartAnimate();
        }

        private Vector2 CalcMenuToReplaceDestination()
        {
            Vector2 dest = MenuToReplace.Location;
            switch (AnimateDirection)
            {
                case Direction.Left:
                    dest.X = -MenuToReplace.Size.X;
                    break;
                case Direction.Right:
                    dest.X = WindowSize.X;
                    break;
                case Direction.Up:
                    dest.Y = -MenuToReplace.Size.Y;
                    break;
                case Direction.Down:
                    dest.Y = WindowSize.Y;
                    break;
                default:
                    break;
            }

            return dest;
        }

        private Vector2 CalcNextMenuSource(Vector2 dest)
        {
            Vector2 source = dest;
            switch (AnimateDirection)
            {
                case Direction.Left:
                    source.X = WindowSize.X;
                    break;
                case Direction.Right:
                    source.X = -NextMenu.Size.X;
                    break;
                case Direction.Up:
                    source.Y = WindowSize.Y;
                    break;
                case Direction.Down:
                    source.Y = -NextMenu.Size.Y;
                    break;
                default:
                    break;
            }

            return source;
        }

        protected override void InUpdate(GameTime gameTime)
        {
            _MenuToReplaceAnimation.Update(gameTime);
            _nextMenuAnimation.Update(gameTime);

            if(_MenuToReplaceAnimation.IsFinished && _nextMenuAnimation.IsFinished)
            {
                StopAnimate();
            }
        }

        protected override void OnEnd()
        {
            base.OnEnd();
            NextMenu.IsEnabled = true;
            MenuToReplace.IsVisible = MenuToReplace.IsEnabled = false;
        }
    }
}
