namespace NewAppTpp.Services
{
    class ConfirmationPopupMiddlewareService
    {
        public delegate void ConfirmationPopupUidChangedEventHandler(string uid);
        public static event ConfirmationPopupUidChangedEventHandler ConfirmationPopupUidChanged;

        public static void NotifyConfirmationPopupUidChanged(string uid)
        {
            ConfirmationPopupUidChanged.Invoke(uid);
        }
    }
}
