using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using OZ.MonoGame.GameObjects;
using OZ.MonoGame.GameObjects.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OZ.MonoGame.MathFun.GameObjects.Effects
{
    public class CardsMatchEffect : IAnimation
    {
        public static Texture2D Star { get; set; }

        public Vector2 StartLocation { get; set; }
        public Vector2 DestinationLocation { get; set; }
        public double TimeToReach { get; private set; }

        private Vector2 _velocity;
        private Vector2 _location;

        public bool IsStarted { get; private set; }
        public bool IsFinished => !IsStarted;

        public void StartAnimate(Vector2 source, Vector2 destination, double timeToReach)
        {
            StartLocation = source;
            DestinationLocation = destination;
            TimeToReach = timeToReach;

            _velocity = DestinationLocation - StartLocation;
            _velocity.Normalize();
            _velocity *= (float)((DestinationLocation - StartLocation).Length() / TimeToReach);
            _location = StartLocation;

            IsStarted = true;
        }

        public void StopAnimate()
        {
            IsStarted = false;
        }

        public void Update(GameTime gameTime)
        {
            // Check whether the star already reac
            if (IsStarted && IsStarReachDestination())
            {
                StopAnimate();
            }

            if (IsStarted)
            {
                _location += _velocity * (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            }
        }

        private bool IsStarReachDestination()
        {
            return (DestinationLocation - StartLocation).Length() <= (_location - StartLocation).Length();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (IsStarted)
            {
                spriteBatch.Draw(Star,
                                _location,
                                null,
                                Color.White,
                                (float)gameTime.TotalGameTime.TotalMilliseconds,
                                new Vector2((float)Star.Width / 2, (float)Star.Height / 2),
                                0.4f,
                                SpriteEffects.None,
                                1);
            }
        }
    }
}
