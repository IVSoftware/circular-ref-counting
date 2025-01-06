Your post is describing come complex logic interactions, and shows an attempt to manage this using an `IMultiValueConverter`, It goes on to describe some behaviors that could perhaps be attributed to circularity in the logic, which is sometimes tricky to avoid with bound properties. And finally, you ask:

>is there a better way to do this?

So what I want to do is describe _another_ way to go achieve a result with as minimal an example as I can come up with, with emphasis on implementing the logic in the view model instead of overburdening a, `IMultiValueCOnverter.`
___

**Simple Case using One-Hot Checkboxes**

To demonstrate the basic idea, suppose that any of four checkboxes will cancel the other three if it becomes checked. This is trivial to implement in the VM if checkboxes follow this representative boolean binding:

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

Now we will add four checkboxes for All, Odd, Even and None, creating an immediate and obvious conflict. These, too, are one-hot with respect to each other, but in addition to that it's plain to see that if `All` attemts to check all the singles, but every single cancels the other singles when checked, we've got a real problem.

