using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OZ.MonoGame.MathFun.GameEngine.Cards
{
    public interface ICard
    {
        bool IsValueVisible { get; set; }
        float Value { get; }

        void HiddenValue();
        void VisibleValue();

        event EventHandler Visibled;
        event EventHandler VisibleChanged;

    }

    public static  class ICardExtension
    {
        public static bool CheckMatch(this ICard lCard, ICard rCard)
        {
            return lCard.Value == rCard.Value;
        }
    }
}
