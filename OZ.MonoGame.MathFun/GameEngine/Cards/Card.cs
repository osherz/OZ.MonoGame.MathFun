using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OZ.MonoGame.MathFun.GameEngine.Cards
{
    abstract public class Card : ICard
    {
        private bool _isVisible = false;
        public bool IsValueVisible
        {
            get
            {
                return _isVisible;
            }

            set
            {
                bool old = _isVisible;
                if(_isVisible != value)
                {
                    _isVisible = value;
                    OnVisibleChaned();
                }
                if (IsValueVisible && !old)
                {
                    OnVisibled();
                }
            }
        }


        abstract public float Value { get; }

        public event EventHandler Visibled;
        public event EventHandler VisibleChanged;

        public void HiddenValue()
        {
            IsValueVisible = false;
        }

        public void VisibleValue()
        {
            IsValueVisible = true;
        }

        public override bool Equals(object obj)
        {
            if(!(obj is Card))
            {
                return false;
            }

            return this == obj as Card;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        static public bool operator==(Card card1, Card card2)
        {
            if (card1 is null && card2 is null) return true;
            if (card1 is null || card2 is null) return false;
            return card1.Value == card2.Value;
        }

        static public bool operator !=(Card card1, Card card2)
        {
            return !(card1 == card2);
        }



        protected virtual void OnVisibled()
        {

            Visibled?.Invoke(this, EventArgs.Empty);
        }
        protected virtual void OnVisibleChaned()
        {
            VisibleChanged?.Invoke(this, EventArgs.Empty);
        }


    }
}
