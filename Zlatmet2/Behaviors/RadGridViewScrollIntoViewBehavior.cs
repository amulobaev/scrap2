using System;
using System.Windows.Interactivity;
using Telerik.Windows.Controls;

namespace Zlatmet2.Behaviors
{
    public class RadGridViewScrollIntoViewBehavior : Behavior<RadGridView>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged;
        }

        private void AssociatedObject_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            RadGridView grid = sender as RadGridView;
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
