using System;

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
        public double SelectedPercentKehadiran { get; set; }

        private event Action _onDataSaved;
        public event Action OnDataSaved
        {
            add
            {
                _onDataSaved += value;
            }
            remove
            {
                _onDataSaved -= value;
            }
        }

        public void InvokeDataSaved()
        {
            _onDataSaved.Invoke();
        }
    }
}
