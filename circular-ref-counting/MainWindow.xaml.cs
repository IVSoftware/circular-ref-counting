using IVSoftware.Portable.Disposable;
using System.ComponentModel;
using System.Configuration.Internal;
using System.Diagnostics;
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
                    if (RefCount.IsZero())
                    {
                        using (RefCount.GetToken())
                        {
                            if (One)
                            {
                                Two = false;
                                Three = false;
                                Four = false;
                                All = false;
                                Odd = false;
                                Even = false;
                                None = false;
                            }
                            OnPropertyChanged();
                        }
                    }
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
                    if (RefCount.IsZero())
                    {
                        using (RefCount.GetToken())
                        {
                            if (Two)
                            {
                                One = false;
                                Three = false;
                                Four = false;
                                All = false;
                                Odd = false;
                                Even = false;
                                None = false;
                            }
                            OnPropertyChanged();
                        }
                    }
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
                    if (RefCount.IsZero())
                    {
                        using (RefCount.GetToken())
                        {
                            if (Three)
                            {
                                One = false;
                                Two = false;
                                Four = false;
                                All = false;
                                Odd = false;
                                Even = false;
                                None = false;
                            }
                            OnPropertyChanged();
                        }
                    }
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
                    if (RefCount.IsZero())
                    {
                        using (RefCount.GetToken())
                        {
                            if (Four)
                            {
                                One = false;
                                Two = false;
                                Three = false;
                                All = false;
                                Odd = false;
                                Even = false;
                                None = false;
                            }
                            OnPropertyChanged();
                        }
                    }
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
                    if (RefCount.IsZero())
                    {
                        using (RefCount.GetToken())
                        {
                            if (All)
                            {
                                Odd = false;
                                Even = false;
                                None = false;
                                One = true;
                                Two = true;
                                Three = true;
                                Four = true;
                            }
                            OnPropertyChanged();
                        }
                    }
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
                    if (RefCount.IsZero())
                    {
                        using (RefCount.GetToken())
                        {
                            if (Even)
                            {
                                All = false;
                                Odd = false;
                                None = false;
                                One = false;
                                Two = true;
                                Three = false;
                                Four = true;
                            }
                            OnPropertyChanged();
                        }
                    }
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
                    if (RefCount.IsZero())
                    {
                        using (RefCount.GetToken())
                        {
                            if (Odd)
                            {
                                All = false;
                                Even = false;
                                None = false;
                                All = false;
                                One = true;
                                Two = false;
                                Three = true;
                                Four = false;
                            }
                            OnPropertyChanged();
                        }
                    }
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
                    if (RefCount.IsZero())
                    {
                        using (RefCount.GetToken())
                        {
                            if (None)
                            {
                                All = false;
                                Even = false;
                                Odd = false;
                                One = false;
                                Two = false;
                                Three = false;
                                Four = false;
                            }
                            OnPropertyChanged();
                        }
                    }
                }
            }
        }
        bool _none = default;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            Debug.WriteLine($"Originator: {propertyName}" );
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        // <PackageReference Include="IVSoftware.Portable.Disposable" Version="1.2.0" />
        public DisposableHost RefCount
        {
            get
            {
                if (_refCount is null)
                {
                    _refCount = new DisposableHost();
                    _refCount.FinalDispose += (sender, e) =>
                    {
                        foreach (var propertyName in new[]
                        {
                            nameof(One), nameof(Two), nameof(Three), nameof(Four),
                            nameof(All), nameof(Even), nameof(Odd), nameof(None),
                        })
                        {
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                        }
                    };
                }
                return _refCount;
            }
        }
        DisposableHost? _refCount = default;
    }
}