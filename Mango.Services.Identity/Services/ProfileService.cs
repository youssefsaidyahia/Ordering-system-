using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Mango.Services.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Mango.Services.Identity.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ProfileService(UserManager<ApplicationUser> userManager, IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _roleManager = roleManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            string sub = context.Subject.GetSubjectId();
            ApplicationUser user= await _userManager.FindByIdAsync(sub);
            ClaimsPrincipal claims = await _userClaimsPrincipalFactory.CreateAsync(user);

            List<Claim> claims1=claims.Claims.ToList();   
            claims1=claims1.Where(claim =>context.RequestedClaimTypes.Contains(claim.Type)).ToList();

            if (_userManager.SupportsUserRole)
            {
                var roles=await _userManager.GetRolesAsync(user);
                foreach (var role in roles)
                {
                    claims1.Add(new Claim(JwtClaimTypes.Role, role));
                    claims1.Add(new Claim(JwtClaimTypes.GivenName, user.FirstName));
                    claims1.Add(new Claim(JwtClaimTypes.FamilyName, user.LastName));
                }
            }
            context.IssuedClaims = claims1;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            string sub = context.Subject.GetSubjectId();
            ApplicationUser user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;

        }
    }
}
