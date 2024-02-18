namespace NewAppTpp.MVVM.View
{
    /// <summary>
    /// Interaction logic for ConfirmationPopup.xaml
    /// </summary>
    public partial class ConfirmationPopup
    {
        public ConfirmationPopup(string message)
        {
            InitializeComponent();
            ConfirmationMessage.Text = message;
        }
    }
}
