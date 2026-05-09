namespace BackroomsShooter.Generation
{
    public struct TileVariant
    {
        public TileData Data;
        public int RotationIndex; // 0=0; 1=90; 2=180; 3=270

        public string UP, RIGHT, DOWN, LEFT;

        public TileVariant(TileData data, int rotation)
        {
            Data = data;
            RotationIndex = rotation;

            string[] s = { data.SocketTop, data.SocketRight, data.SocketBottom, data.SocketLeft };

            UP = s[(0 + 4 - rotation) % 4];
            RIGHT = s[(1 + 4 - rotation) % 4];
            DOWN = s[(2 + 4 - rotation) % 4];
            LEFT = s[(3 + 4 - rotation) % 4];
        }
    }
}