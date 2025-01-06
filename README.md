Your post is describing come complex logic interactions, and shows an attempt to manage this using an `IMultiValueConverter`, It goes on to describe some behaviors that could perhaps be attributed to circularity in the logic, which is sometimes tricky to avoid with bound properties. And finally, you ask:

>is there a better way to do this?

I can only say that there is at least one _alternative_ way to go about it: Instead of overburdening an `IMultiValueConverter` you could try moving all of the logic into the view model/data context where you could mitigate circularity and/or conflicting logic with a boolean, semaphore, or reference-counting scheme that is owned by the "first property to change" and that persists until all the changes have propagated. In short, that is my suggestion, with of this answer just being details to hopefully make it clearer what I mean by that.
___

**Simple Case using One-Hot Checkboxes**

Here's the basic idea: suppose that any of four checkboxes will cancel the other three if it becomes checked. This is trivial to implement in the VM if checkboxes follow this representative boolean binding:

~~~
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
                if(One)
                {
                    Two = false;
                    Three = false;
                    Four = false;
                }
                OnPropertyChanged();
            }
        }
    }
    bool _one = default;
    .
    .
    .
}
~~~

**Minimal Example of Deliberately Conflicting Logic**

But if four checkboxes for All, Odd, Even and None are added, it creates an immediate and obvious conflict. These, too, are one-hot with respect to each other, but in addition to that it's plain to see that if `All` attempts to check all the singles, when every single is wired to cancel aLL the other singles when toggled true, then we've got a real problem.

##### Pathological

This _will not work_. It conflicts with the one-hot behavior of the single checkboxes.

~~~
public bool All
{
    get => _all;
    set
    {
        if (!Equals(_all, value))
        {
            _all = value;
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
bool _all = default;
~~~

**Solution using IDisposable Ref Counting**

The logic scheme can be made stateful very easily, where the first property to change checks out an `IDisposable` token that suppresses the property change logic and events of all the other properties that occur within a using block. Then, when the token disposes, the UI is updated en masse by firing all of the property changes to push the backing store values to the UI. You might be able to do this with a simple boolean, or using various ref counting schemes. I use this particular NuGet because I'm familiar with it. Here, a singleton instance of `DisposableHost` is set up to fire the notifications when the token count goes to zero, and two representative properties show how the `using` blocks work.

~~~
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
    .
    .
    .
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
    .
    .
    .

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
~~~
