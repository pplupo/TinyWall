using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace ImmenseWall.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _message;
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public ICommand ApplyCommand { get; }

        public ICommand CancelCommand { get; }

        //public ICommand ExitCommand => new RelayCommand(r =>
        //{
        //    Application.Current.Shutdown();
        //});

        public MainViewModel()
        {
            ApplyCommand = new RelayCommand(_ => Apply(), _ => !string.IsNullOrWhiteSpace(Message));
            CancelCommand = new RelayCommand(_ => Cancel());
        }

        private void Apply()
        {
            Message = "Hello from Command!";
        }

        private static void Cancel()
        {
            Application.Current.Shutdown();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}