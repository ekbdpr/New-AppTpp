using System.Windows;
using System.Windows.Controls;

namespace NewAppTpp.Assets.CustomControls
{
    /// <summary>
    /// Interaction logic for BindablePasswordBox.xaml
    /// </summary>
    public partial class BindablePasswordBox : UserControl
    {
        public BindablePasswordBox()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(BindablePasswordBox));

        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        public void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = PasswordBox.UnsafePassword;
        }
    }
}
