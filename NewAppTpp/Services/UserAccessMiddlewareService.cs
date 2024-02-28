using System;

namespace AppTpp.Services
{
    internal class UserAccessMiddlewareService
    {
        private static UserAccessMiddlewareService _instance = new();
        public static UserAccessMiddlewareService Instance
        {
            get
            {
                _instance ??= new UserAccessMiddlewareService();

                return _instance;
            }
        }

        public string SelectedNip { get; set; }
        public string SelectedNama { get; set; }
        public string SelectedJabatan { get; set; }
        public string SelectedUsername { get; set; }
        public string SelectedPrivilege { get; set; }

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

        private event Action _onCancelAction;
        public event Action OnCancelAction
        {
            add
            {
                _onCancelAction += value;
            }
            remove
            {
                _onCancelAction -= value;
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

        public void InvokeCancelAction()
        {
            _onCancelAction.Invoke();
        }

        public void InvokeDataDeletion()
        {
            _onDeleteData.Invoke();
        }
    }
}