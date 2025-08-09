namespace HGD_SSO_API.Models.AuthManagement;

public class SignUpRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string PasswordRepeat { get; set; }
}