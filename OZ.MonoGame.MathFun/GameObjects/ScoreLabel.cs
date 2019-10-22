using OZ.MonoGame.GameObjects.UI;
using OZ.MonoGame.MathFun.GameEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace OZ.MonoGame.MathFun.GameObjects
{
    internal class ScoreLabel : Label
    {
        private Player _player;

        public ScoreLabel(GamePrototype gameParent) : base(gameParent)
        {
        }

        public Player Player
        {
            get { return _player; }
            set 
            {
                if (Player != value)
                {
                    UnregisterEventFromPlayer();
                    _player = value;
                    Text = _player.Score.ToString();
                    RegisterEventToPlayer();
                    OnPlayerChanged();
                }
            }
        }


        #region Events
        public event EventHandler PlayerChanged;

        private void UnregisterEventFromPlayer()
        {
            if (!(Player is null))
            {
                Player.ScoreChanged -= Player_ScoreChanged;
            }
        }

        private void RegisterEventToPlayer()
        {
            if(!(Player is null))
            {
                Player.ScoreChanged += Player_ScoreChanged;
            }
        }

        private void Player_ScoreChanged(object sender, EventArgs e)
        {
            Text = Player.Score.ToString();
        }
        #endregion


        private void OnPlayerChanged()
        {
            PlayerChanged?.Invoke(this, EventArgs.Empty);
        }

    }
}
