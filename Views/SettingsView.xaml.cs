using System;
using System.Diagnostics;
using System.Windows.Controls;

namespace Check.Views
{
    public partial class SettingsView
    {
        public SettingsView(PositionView positionView)
        {
            InitializeComponent();

            Debug.Assert(positionView != null);

            FontSize = 18d;

            TabControl tabControl = new TabControl();

            tabControl.Items.Add(new TabItem { Header = "Playing", Content = new SettingsPlayingView(positionView) } );
            tabControl.Items.Add(new TabItem { Header = "Editing", Content = new SettingsEditingView(positionView) } );

            tabControl.SelectionChanged += (object sender, SelectionChangedEventArgs ea) =>
            {
                switch (tabControl.SelectedContent)
                {
                    case SettingsEditingView _:
                        positionView.OperationStatus = PositionView.OperationStatusEnum.Editing;
                        break;
                    case SettingsPlayingView _:
                        positionView.OperationStatus = PositionView.OperationStatusEnum.Playing;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(tabControl.SelectedContent), "Invalid switch value");
                }
            } ;

            Content = tabControl;
        }
    }
}
