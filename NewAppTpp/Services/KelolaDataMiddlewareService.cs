using System;

namespace NewAppTpp.Services
{
    internal class KelolaDataMiddlewareService
    {
        private static KelolaDataMiddlewareService _instance = new();
        public static KelolaDataMiddlewareService Instance
        {
            get
            {
                _instance ??= new KelolaDataMiddlewareService();

                return _instance;
            }
        }

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

        private event Action _onDeleteData;
        public event Action OnDeleteData
        {
            add
            {
                _onDeleteData += value;
            }
            remove
            {
                _onDeleteData -= value;
            }
        }

        public void InvokeDataSaved()
        {
            _onDataSaved.Invoke();
        }

        public void InvokeDataDeletion()
        {
            _onDeleteData.Invoke();
        }
    }
}
