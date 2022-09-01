using System.Windows;
using System.Windows.Controls;

namespace DustInTheWind.VeloCity.Wpf.Presentation.Styles
{
    public class RefreshIcon : Control
    {
        static RefreshIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RefreshIcon), new FrameworkPropertyMetadata(typeof(RefreshIcon)));
        }
    }
}