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
        public MainWindowDataContext()
        {
            using (RefCount.GetToken(properties: new Dictionary<string, object>
            {
                { "Loading", true }
            }))
            {
                One = Properties.Settings.Default.One;
                Two = Properties.Settings.Default.Two;
                Three = Properties.Settings.Default.Three;
                Four = Properties.Settings.Default.Four;
                All = Properties.Settings.Default.All;
                Odd = Properties.Settings.Default.Odd;
                Even = Properties.Settings.Default.Even;
                None = Properties.Settings.Default.None;
            }
        }
        public bool One
        {
            get => Properties.Settings.Default.One;
            set
            {
                if (!Equals(Properties.Settings.Default.One, value))
                {
                    Properties.Settings.Default.One = value;
                    if (RefCount.IsZero())
                    {
                        using (RefCount.GetToken())
                        {
                            if (One)
                            {
                                Two = false;
                                Three = false;
                                Four = false;
                            }
                            All = false;
                            Odd = false;
                            Even = false;
                            None = false;
                            OnPropertyChanged();
                        }
                    }
                }
            }
        }

        public bool Two
        {
            get => Properties.Settings.Default.Two;
            set
            {
                if (!Equals(Properties.Settings.Default.Two, value))
                {
                    Properties.Settings.Default.Two = value;
                    if (RefCount.IsZero())
                    {
                        using (RefCount.GetToken())
                        {
                            if (Two)
                            {
                                One = false;
                                Three = false;
                                Four = false;
                            }
                            All = false;
                            Odd = false;
                            Even = false;
                            None = false;
                            OnPropertyChanged();
                        }
                    }
                }
            }
        }

        public bool Three
        {
            get => Properties.Settings.Default.Three;
            set
            {
                if (!Equals(Properties.Settings.Default.Three, value))
                {
                    Properties.Settings.Default.Three = value;
                    if (RefCount.IsZero())
                    {
                        using (RefCount.GetToken())
                        {
                            if (Three)
                            {
                                One = false;
                                Two = false;
                                Four = false;
                            }
                            All = false;
                            Odd = false;
                            Even = false;
                            None = false;
                            OnPropertyChanged();
                        }
                    }
                }
            }
        }

        public bool Four
        {
            get => Properties.Settings.Default.Four;
            set
            {
                if (!Equals(Properties.Settings.Default.Four, value))
                {
                    Properties.Settings.Default.Four = value;
                    if (RefCount.IsZero())
                    {
                        using (RefCount.GetToken())
                        {
                            if (Four)
                            {
                                One = false;
                                Two = false;
                                Three = false;
                            }
                            All = false;
                            Odd = false;
                            Even = false;
                            None = false;
                            OnPropertyChanged();
                        }
                    }
                }
            }
        }

        public bool All
        {
            get => Properties.Settings.Default.All;
            set
            {
                if (!Equals(Properties.Settings.Default.All, value))
                {
                    Properties.Settings.Default.All = value;
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

        public bool Odd
        {
            get => Properties.Settings.Default.Odd;
            set
            {
                if (!Equals(Properties.Settings.Default.Odd, value))
                {
                    Properties.Settings.Default.Odd = value;
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

        public bool Even
        {
            get => Properties.Settings.Default.Even;
            set
            {
                if (!Equals(Properties.Settings.Default.Even, value))
                {
                    Properties.Settings.Default.Even = value;
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

        public bool None
        {
            get => Properties.Settings.Default.None;
            set
            {
                if (!Equals(Properties.Settings.Default.None, value))
                {
                    Properties.Settings.Default.None = value;
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
                        if(_refCount.TryGetValue("Loading", out var value) && Equals(value, true))
                        {
                            // Being called as a result of the initial load.
                        }
                        else
                        {
                            Properties.Settings.Default.Save();
                        }
                    };
                }
                return _refCount;
            }
        }
        DisposableHost? _refCount = default;
    }
}