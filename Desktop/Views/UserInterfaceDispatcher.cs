namespace Desktop.Views;

public class UserInterfaceDispatcher
{
    private readonly IEnumerable<IUserInterface> _interfaces;

    public UserInterfaceDispatcher(IEnumerable<IUserInterface> interfaces)
    {
        _interfaces = interfaces;
    }

    public void ShowWelcomes()
    {
        foreach (var ui in _interfaces)
        {
            ui.ShowWelcome();
        }
    }

    public void ShowCompletions()
    {
        foreach (var ui in _interfaces)
        {
            ui.ShowCompletion();
        }
    }
}