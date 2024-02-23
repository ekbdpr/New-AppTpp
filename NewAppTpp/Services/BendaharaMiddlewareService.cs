namespace NewAppTpp.Services
{
    internal class BendaharaMiddlewareService
    {
        private static BendaharaMiddlewareService _instance = new();
        public static BendaharaMiddlewareService Instance
        {
            get
            {
                _instance ??= new BendaharaMiddlewareService();

                return _instance;
            }
        }

        public string SelectedNip { get; set; }
        public string SelectedNama { get; set; }
        public int SelectedCapaiKinerja { get; set; }
        public int SelectedPercentKehadiran { get; set; }
    }
}
