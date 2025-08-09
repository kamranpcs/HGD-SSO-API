using HGD_SSO_API.Models.Common;

namespace HGD_SSO_API.Models.UserManagement;

public class User : BaseEntity
{
    public string Username { get; set; }
    public string PhoneNumber { get; set; }
    public string PasswordHash { get; set; }
}