using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;

namespace OZ.MonoGame.MathFun.GameObjects.UI
{
    public class LevelsMenu : GameMenu
    {
        #region EVENTS
        public event EventHandler<int> LevelChoosed;
        public event EventHandler BackClicked
        {
            add
            {
                _backBtn.Clicked += value;
            }

            remove
            {
                _backBtn.Clicked -= value;
            }
        }

        protected virtual void OnLevelChoosed(int level)
        {

            LevelChoosed?.Invoke(this, level);
        }

        #endregion EVENTS

        MenuButton[] _buttons;
        MenuButton _backBtn;

        public int Levels => _buttons.Length;

        public LevelsMenu(GamePrototype gameParent, int levels):base(gameParent)
        {
            Text = "Choose Level";

            _buttons = new MenuButton[levels];

        }

        public override void Initialize()
        {
            InitButtons();
            base.Initialize();
        }

        private void InitButtons()
        {
            for (int i = 0; i < Levels; i++)
            {
                _buttons[i] = new MenuButton(GameParent)
                {
                    Text = "Level " + (i + 1),
                    IsEnabled = true,
                    Tag = i+1,
                    GameParent = GameParent,
                    ControlApearance = ButtonsApearance
                };
                RegisterEvent(_buttons[i]);

                Add(_buttons[i]);
            }

            _backBtn = new MenuButton(GameParent)
            {
                Text = "Back",
                IsEnabled = true,
                GameParent = GameParent,
                ControlApearance = ButtonsApearance
            };
            Add(_backBtn);

        }

        #region EVENTS REGISTRATION
        private void RegisterEvent(MenuButton button)
        {
            button.Clicked += ButtonClicked;
        }

        private void UnRegisterEventsAllButtons()
        {
            foreach (var button in _buttons)
            {
                UnRegisterEvents(button);
            }
        }

        private void UnRegisterEvents(MenuButton button)
        {
            button.Clicked -= ButtonClicked;
        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            object tag = (sender as MenuButton).Tag;
            OnLevelChoosed((int)tag);
        }
        #endregion EVENTS REGISTRATION

        public override void LoadContent(ContentManager content)
        {
            InnerToMiddleX();
            base.LoadContent(content);
        }

        public override void UnloadContent(ContentManager content)
        {
            UnRegisterEventsAllButtons();
            base.UnloadContent(content);
        }
    }
}
