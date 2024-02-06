using HandyControl.Controls;
using System.Windows;

namespace NewAppTpp.MVVM.View
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    [TemplatePart(Name = "ElementNonClientArea", Type = typeof(UIElement))]
    public partial class Login : HandyControl.Controls.Window
    {
        public Login()
        {
            InitializeComponent();
        }
    }
}
