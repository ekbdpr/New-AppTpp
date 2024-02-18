using AppTpp.Services;
using HandyControl.Tools;
using HandyControl.Tools.Command;
using NewAppTpp.Services;
using System;
using System.Windows.Input;

namespace NewAppTpp.MVVM.ViewModel
{
    class ConfirmationPopupViewModel : BindablePropertyBase
    {
        private string _confirmationPopupUid;
        public string ConfirmationPopupUid
        {
            get { return _confirmationPopupUid; }
            set { _confirmationPopupUid = value; RaisePropertyChanged(nameof(ConfirmationPopupUid)); }
        }

        public ICommand YesButtonCommand { get; }

        public ICommand NoButtonCommand { get; }

        public ConfirmationPopupViewModel()
        {
            ConfirmationPopupMiddlewareService.ConfirmationPopupUidChanged += UpdateConfirmationPopupUid;

            YesButtonCommand = new SimpleRelayCommand(new Action(YesButton));
            NoButtonCommand = new SimpleRelayCommand(new Action(NoButton));
        }

        private void UpdateConfirmationPopupUid(string uid)
        {
            ConfirmationPopupUid = uid;
            ConfirmationPopupMiddlewareService.ConfirmationPopupUidChanged -= UpdateConfirmationPopupUid;
        }

        private void YesButton()
        {
            if (ConfirmationPopupUid == "DeleteConfirmation")
            {
                UserAccessMiddlewareService.Instance.InvokeDataDeletion();
                return;
            }
        }

        private void NoButton()
        {
            UserAccessMiddlewareService.Instance.InvokeDataSaved();
        }
    }
}
