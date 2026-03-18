namespace UPTEST.ViewModels;

public class AuthPageViewModel
{
    public LoginViewModel Login { get; set; } = new();
    public RegisterViewModel Register { get; set; } = new();
    public string ActiveTab { get; set; } = "login";
}
