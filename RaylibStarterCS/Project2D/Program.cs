using static Raylib.Raylib;

namespace Project2D
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();

            InitWindow(640, 480, "Tween Editor Implementation");

            game.Init();

            while (!WindowShouldClose())
            {
                game.Update();
                game.Draw();
            }

            game.Shutdown();

            CloseWindow();
        }
    }
}
