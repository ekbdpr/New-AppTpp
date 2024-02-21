using System;

namespace NewAppTpp.Services
{
    class ConfirmationPopupMiddlewareService
    {
        private static ConfirmationPopupMiddlewareService _instance = new();
        public static ConfirmationPopupMiddlewareService Instance
        {
            get
            {
                _instance ??= new ConfirmationPopupMiddlewareService();

                return _instance;
            }
        }

        public bool ConfirmationState { get; set; }

        private event Action _onButtonClick;
        public event Action OnButtonClick
        {
            add
            {
                _onButtonClick += value;
            }
            remove
            {
                _onButtonClick -= value;
            }
        }

        public void InvokeCloseConfirmationPopup()
        {
            _onButtonClick?.Invoke();
        }

        public delegate void ConfirmationPopupUidChangedEventHandler(string uid);
        public static event ConfirmationPopupUidChangedEventHandler ConfirmationPopupUidChanged;

        public static void NotifyConfirmationPopupUidChanged(string uid)
        {
            ConfirmationPopupUidChanged.Invoke(uid);
        }
    }
}
