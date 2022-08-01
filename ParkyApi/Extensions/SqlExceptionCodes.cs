namespace ParkyApi.Extensions
{
    public static class SqlExceptionCodes
    {
        private const int CouldNotOpenConnection = 53;
        private const int Deadlock = 1205;
        private const int Timeout = -2;
        private const int TransportFail = 121;

        public static List<int> ErrorCodes
        {
            get
            {
                var ints = new List<int>
                {
                    CouldNotOpenConnection,
                    Deadlock,
                    Timeout,
                    TransportFail
                };

                return ints;
            }
        }
    }
}
