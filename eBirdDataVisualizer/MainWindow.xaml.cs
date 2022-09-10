using eBirdDataVisualizer.Helpers;

namespace eBirdDataVisualizer;

public sealed partial class MainWindow : WindowEx
{
    public MainWindow()
    {
        InitializeComponent();

        AppWindow.SetIcon(Path.Combine(AppContext.BaseDirectory, "Assets/bird.ico"));
        Content = null;
        Title = "AppDisplayName".GetLocalized();
    }
}
