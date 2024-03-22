namespace Miner
{
    public class Level
    {
        public int Width;
        public int Height;
        public int Mines;
        public Level(int width, int height, int mines)
        {
            Width = width; 
            Height = height; 
            Mines = mines;
        }
    }
}
