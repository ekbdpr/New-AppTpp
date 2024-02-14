using System;

namespace AppTpp.Services
{
    internal class DataMiddlewareService
    {
        private static DataMiddlewareService _instance = new();
        public static DataMiddlewareService Instance
        {
            get
            {
                _instance ??= new DataMiddlewareService();

                return _instance;
            }
        }

        public string CurrentNip { get; set; }

        public string CurrentNama { get; set; }

        public string CurrentJabatan { get; set; }

        public string CurrentUsername { get; set; }

        public string CurrentPrivilege { get; set; }

        public bool ConfirmationDialogState { get; set; }

        public string ConfirmationDialogMessage { get; set; }


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
            _onDataSaved?.Invoke();
        }
    }
}