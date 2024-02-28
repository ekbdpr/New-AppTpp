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
        }

        private void YesButton()
        {
            if (ConfirmationPopupUid == "DeleteUserConfirmation")
            {
                UserAccessMiddlewareService.Instance.InvokeDataDeletion();
                return;
            }

            if (ConfirmationPopupUid == "DuplicateFileConfirmation")
            {
                ConfirmationPopupMiddlewareService.Instance.ConfirmationState = true;
                ConfirmationPopupMiddlewareService.Instance.InvokeCloseConfirmationPopup();
                return;
            }

            if (ConfirmationPopupUid == "DeletePegawaiConfirmation")
            {
                KelolaDataMiddlewareService.Instance.InvokeDataDeletion();
                return;
            }
        }

        private void NoButton()
        {
            if (ConfirmationPopupUid == "DeleteUserConfirmation")
            {
                UserAccessMiddlewareService.Instance.InvokeCancelAction();
                return;
            }

            if (ConfirmationPopupUid == "DuplicateFileConfirmation")
            {
                ConfirmationPopupMiddlewareService.Instance.ConfirmationState = false;
                ConfirmationPopupMiddlewareService.Instance.InvokeCloseConfirmationPopup();
                return;
            }

            if (ConfirmationPopupUid == "DeletePegawaiConfirmation")
            {
                KelolaDataMiddlewareService.Instance.InvokeCancelAction();
                return;
            }
        }
    }
}
