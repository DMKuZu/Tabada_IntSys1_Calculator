namespace Tabada_IntSys1_Calculator.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public SimpleViewModel SimpleViewModel { get; } = new SimpleViewModel();
    
    public ReactiveViewModel ReactiveViewModel { get; } = new ReactiveViewModel();
}