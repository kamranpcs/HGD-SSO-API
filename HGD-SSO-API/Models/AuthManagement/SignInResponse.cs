namespace HGD_SSO_API.Models.AuthManagement;

public class SignInResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpiresAt { get; set; }
}