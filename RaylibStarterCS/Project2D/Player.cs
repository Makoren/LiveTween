using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raylib;
using static Raylib.Raylib;
using LiveTween;

namespace Project2D
{
    class Player
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        private Tween tweenX;
        private Tween tweenY;

        public Player()
        {
            X = 100;
            Y = 300;

            tweenX = new Tween(EasingType.Quadratic, 1, true);
            tweenY = new Tween(EasingType.Quadratic, 1, true);
        }

        public Player(int x, int y) : this()
        {
            X = x;
            Y = y;
        }

        public void Update(float deltaTime)
        {
            if (IsKeyPressed(KeyboardKey.KEY_ENTER))
            {
                if (Tween.Socket.Connected)
                    tweenX.Link();
            }

            if (IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON))
            {
                tweenX.Play(X, GetMouseX());
                tweenY.Play(Y, GetMouseY());
            }

            // Only one tween can interact with the editor at a time, so tweenX gets the properties and copies them to tweenY.
            tweenY.CopyFrom(tweenX);

            if (tweenX.IsPlaying) X = (int)tweenX.Update(deltaTime);
            if (tweenY.IsPlaying) Y = (int)tweenY.Update(deltaTime);
        }

        public void Draw()
        {
            DrawCircle(X, Y, 25, Color.RED);
        }
    }
}
