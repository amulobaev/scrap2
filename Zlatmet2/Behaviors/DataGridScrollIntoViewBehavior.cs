using System;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Zlatmet2.Behaviors
{
    public class DataGridScrollIntoViewBehavior : Behavior<DataGrid>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged;
        }

        void AssociatedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            if (grid == null)
                return;

            grid.Dispatcher.BeginInvoke(new Action(() =>
            {
                if (grid.SelectedItem == null)
                    return;
                
                grid.Focus();
                grid.ScrollIntoView(grid.SelectedItem);
            }));
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;
        }
    }
}
