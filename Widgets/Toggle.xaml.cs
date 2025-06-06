using PropertyChanged;
using System.Windows.Input;

namespace HeraCrossController.Widgets;
[AddINotifyPropertyChangedInterface]
public partial class Toggle : ContentView
{
    public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(Toggle), null);
    public string Text { get; set; } = "";
    public string OnText { get; set; } = "¿ªÆô";
    public string OffText { get; set; } = "¹Ø±Õ";
    public int OnCmd { get; set; }
    public int OffCmd { get; set; }
    [DoNotNotify]
    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }
    public bool IsToggled { get; set; }
    public string TextShow => $"{Text}ÒÑ{(IsToggled?OnText:OffText)}";
    public Toggle() =>InitializeComponent();

    private void Switch_Toggled(object sender, ToggledEventArgs e)
    {
        var cmd = Command;
        if(cmd!=null&&cmd.CanExecute(1))cmd?.Execute(e.Value?OnCmd:OffCmd);
    }
}