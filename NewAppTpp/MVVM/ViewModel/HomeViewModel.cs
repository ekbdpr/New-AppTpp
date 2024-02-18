using HandyControl.Tools;
using NewAppTpp.MVVM.Model;
using NewAppTpp.Services;

namespace NewAppTpp.MVVM.ViewModel
{
    internal class HomeViewModel : BindablePropertyBase
    {
        private readonly UserAccessModel _userAccessModel = new();

        public string Nama
        {
            get { return _userAccessModel.Nama; }
            set { _userAccessModel.Nama = value; RaisePropertyChanged(nameof(Nama)); }
        }

        public HomeViewModel() 
        {
            Nama = UserAccessService.Instance.CurrentNama;
        }
    }
}
