namespace Api.UserController.Views;

public class EmailBusyResponseView
{
    public bool IsEmailBusy { get; }

    public EmailBusyResponseView(bool isEmailBusy)
    {
        IsEmailBusy = isEmailBusy;
    }
}