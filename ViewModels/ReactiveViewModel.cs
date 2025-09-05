using System;
using ReactiveUI;

namespace Tabada_IntSys1_Calculator.ViewModels
{
    public class ReactiveViewModel : ReactiveObject
    {
        private string? _Name;
        
        public string? Name
        {
            get => _Name;
            set => this.RaiseAndSetIfChanged(ref _Name, value);
        }
        public string Greeting
        {
            get
            {
                if (string.IsNullOrEmpty(Name))
                {
                    return "Hello World from Avalonia.Samples";
                }
                else
                {
                    return $"Hello {Name}";
                }
            }
        }
        
        public ReactiveViewModel()
        {
            // We can listen to any property changes with "WhenAnyValue" and do whatever we want in "Subscribe".
            this.WhenAnyValue(o => o.Name)
                .Subscribe(o => this.RaisePropertyChanged(nameof(Greeting)));
        }

    }
}
