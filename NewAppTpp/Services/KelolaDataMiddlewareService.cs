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

        public string SelectedNip { get; set; }
        public string SelectedNama { get; set; }
        public string SelectedKdSatker { get; set; }
        public string SelectedNorek { get; set; }
        public string SelectedKdPangkat { get; set; }
        public int SelectedPiwp { get; set; }
        public string SelectedNmSkpd { get; set; }
        public int SelectedPaguTppBk {  get; set; }
        public int SelectedPaguTppKk { get; set; }


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
