using System.Reflection;

namespace Delegate;

public class Account
{
    public delegate void NotifyAction(string a);

    public event NotifyAction Notify;

    public Account(NotifyAction notifyAction)
    {
        Notify = notifyAction;
    }

    // public void AddNotify(NotifyAction notifyAction)
    // {
    //     Notify += notifyAction;
    // }

    public void Invoke(string s)
    {
        Notify.Invoke(s);
    }
}