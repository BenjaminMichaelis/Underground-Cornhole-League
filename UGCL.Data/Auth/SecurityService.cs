
namespace UGCL.Data.Auth;

[Coalesce, Service]
public class SecurityService()
{
    [Coalesce, Execute(HttpMethod = HttpMethod.Get)]
    public UserInfo WhoAmI(ClaimsPrincipal user, AppDbContext db)
    {

        return new UserInfo
        {
            Id = user.GetUserId(),
            UserName = user.GetUserName(),

            Email = user.GetEmail(),
            FullName = user.FindFirstValue(AppClaimTypes.FullName),

            Roles = user.Claims
                .Where(c => c.Type == AppClaimTypes.Role)
                .Select(c => c.Value)
                .ToList(),

            Permissions = user.Claims
                .Where(c => c.Type == AppClaimTypes.Permission)
                .Select(c => c.Value)
                .ToList(),

        };
    }
}
