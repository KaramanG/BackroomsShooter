namespace BackroomsShooter.Generation
{
    [System.Serializable]
    public struct TileVariant
    {
        public TileData Data;
        public int RotationIndex;
        public string[] Sockets;

        public TileVariant(TileData data, int rotation)
        {
            Data = data;
            RotationIndex = rotation;
            Sockets = new string[4];

            string[] original = { 
                data.SocketTop,
                data.SocketRight,
                data.SocketBottom,
                data.SocketLeft
            };

            for (int i = 0; i < 4; i++)
            {
                Sockets[i] = original[(i + (4 - rotation)) % 4];
            }
        }

    }
}