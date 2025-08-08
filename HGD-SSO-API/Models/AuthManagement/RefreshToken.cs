using HGD_SSO_API.Models.Common;
using HGD_SSO_API.Models.UserManagement;

namespace HGD_SSO_API.Models.AuthManagement;

public class RefreshToken : BaseEntity
{
    public string Token { get; set; }
    public Guid UserId { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool IsRevoked { get; set; }

    public User User { get; set; }
}