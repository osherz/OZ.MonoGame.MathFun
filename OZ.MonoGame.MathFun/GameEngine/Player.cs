using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OZ.MonoGame.MathFun.GameEngine
{
    public class Player
    {
        public int Id { get; private set; }
        public string Name { get; set; }

        private int _score;
        public int Score 
        {
            get => _score;
            set
            {
                if(Score != value)
                {
                    _score = value;
                    OnScoreChanged();
                }
            }
        }

        private void OnScoreChanged()
        {
            ScoreChanged?.Invoke(this, EventArgs.Empty);
        }

        #region Events
        public event EventHandler ScoreChanged;
        #endregion

        private static int idCounter = 1;
        public Player()
        {
            Id = idCounter++;
        }


        public static bool operator==(Player left, Player right)
        {
            if (left is null || right is null)
            {
                return left is null && right is null;
            }
            return left.Id == right.Id;

        }

        public static bool operator !=(Player left, Player right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (obj is Player)
            {
                return obj as Player == this;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
