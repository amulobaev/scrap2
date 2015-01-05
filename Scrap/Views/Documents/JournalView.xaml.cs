using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Scrap.Views.Documents
{
    /// <summary>
    /// Логика взаимодействия для JournalView.xaml
    /// </summary>
    public partial class JournalView : UserControl
    {
        public JournalView()
        {
            InitializeComponent();
        }

        private void ToggleButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            ToggleButtonAdd.Focus();
            ToggleButtonAdd.ContextMenu.IsEnabled = true;
            ToggleButtonAdd.ContextMenu.PlacementTarget = ToggleButtonAdd;
            ToggleButtonAdd.ContextMenu.Placement = PlacementMode.Bottom;
            ToggleButtonAdd.ContextMenu.IsOpen = true;

            ToggleButtonAdd.ContextMenu.Closed += delegate
            {
                ToggleButtonAdd.IsChecked = false;
            };
        }

        //private void Popup_Click(object sender, RoutedEventArgs e)
        //{
        //    Popup popup = sender as Popup;
        //    if (popup != null)
        //        popup.IsOpen = false;
        //}
    }
}