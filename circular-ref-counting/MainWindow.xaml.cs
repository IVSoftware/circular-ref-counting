using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace circular_ref_counting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
    class MainWindowDataContext : INotifyPropertyChanged
    {
        public bool One
        {
            get => _one;
            set
            {
                if (!Equals(_one, value))
                {
                    _one = value;
                    OnPropertyChanged();
                }
            }
        }
        bool _one = default;

        public bool Two
        {
            get => _two;
            set
            {
                if (!Equals(_two, value))
                {
                    _two = value;
                    OnPropertyChanged();
                }
            }
        }
        bool _two = default;

        public bool Three
        {
            get => _three;
            set
            {
                if (!Equals(_three, value))
                {
                    _three = value;
                    OnPropertyChanged();
                }
            }
        }
        bool _three = default;

        public bool Four
        {
            get => _four;
            set
            {
                if (!Equals(_four, value))
                {
                    _four = value;
                    OnPropertyChanged();
                }
            }
        }
        bool _four = default;

        public bool All
        {
            get => _all;
            set
            {
                if (!Equals(_all, value))
                {
                    _all = value;
                    OnPropertyChanged();
                }
            }
        }
        bool _all = default;

        public bool Even
        {
            get => _even;
            set
            {
                if (!Equals(_even, value))
                {
                    _even = value;
                    OnPropertyChanged();
                }
            }
        }
        bool _even = default;

        public bool Odd
        {
            get => _odd;
            set
            {
                if (!Equals(_odd, value))
                {
                    _odd = value;
                    OnPropertyChanged();
                }
            }
        }
        bool _odd = default;

        public bool None
        {
            get => _none;
            set
            {
                if (!Equals(_none, value))
                {
                    _none = value;
                    OnPropertyChanged();
                }
            }
        }
        bool _none = default;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}