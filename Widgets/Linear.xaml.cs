using HeraCrossController.Models;
using PropertyChanged;
using System.Windows.Input;

namespace HeraCrossController.Widgets;

public partial class Linear : Slider
{
    public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(Toggle), null);
    public int Divide { get; set; } = 8;
    private int max => Convert.ToInt32(GetValue(MaximumProperty));
    private int min => Convert.ToInt32(GetValue(MinimumProperty));
    private int cell => (max - min) / Divide;
    private int last_value;
    [DoNotNotify]
    public required ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }
    public required int CmdChannel { get; set; }
    public Linear() =>InitializeComponent();
    private void TryRun(int v)
    {
        var cmd = Command;
        if (v != last_value && cmd.CanExecute(1))
        {
            cmd.Execute(new LinearParameter(CmdChannel, v - min));
            last_value = v;
        }
    }

    private void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        int v = (int)e.NewValue/Divide*Divide;
        TryRun(v);
    }

    private void Slider_DragCompleted(object sender, EventArgs e)
    {
        Value = 0;
        TryRun(0);
    }
}