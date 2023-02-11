using IdentityModel;
using Mango.Services.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Mango.Services.Identity.DbContexts.Intializer
{
    public class dbInzializer : IdbInzializer
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public dbInzializer(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public void Intialize()
        {
            if (roleManager.FindByNameAsync(SD.Admin).Result == null)
            {
                roleManager.CreateAsync(new IdentityRole(SD.Admin)).GetAwaiter().GetResult();
                roleManager.CreateAsync(new IdentityRole(SD.Customer)).GetAwaiter().GetResult();
            }
            else
            {
                return;
            }
            ApplicationUser adminuser = new ApplicationUser()
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "11111111111",
                FirstName="Youssef",
                LastName="Sayed"
            };
            userManager.CreateAsync(adminuser, "Admin123*").GetAwaiter().GetResult();
            userManager.AddToRoleAsync(adminuser,SD.Admin).GetAwaiter().GetResult();
            var temp1=userManager.AddClaimsAsync(adminuser, new Claim[]
            {
                new Claim(JwtClaimTypes.Name,adminuser.FirstName+" "+adminuser.LastName),
                new Claim(JwtClaimTypes.GivenName,adminuser.FirstName),
                new Claim(JwtClaimTypes.FamilyName,adminuser.LastName),
                new Claim(JwtClaimTypes.Email,adminuser.Email),
                new Claim(JwtClaimTypes.Role,SD.Admin),
            }).Result;
            ApplicationUser Customeruser = new ApplicationUser()
            {
                UserName = "Customer@gmail.com",
                Email = "Customer@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "11111111111",
                FirstName = "Youssef",
                LastName = "Yahya"
            };
            userManager.CreateAsync(Customeruser, "Cust&324").GetAwaiter().GetResult();
            userManager.AddToRoleAsync(Customeruser, SD.Customer).GetAwaiter().GetResult();
            var temp2 = userManager.AddClaimsAsync(Customeruser, new Claim[]
            {
                new Claim(JwtClaimTypes.Name,Customeruser.FirstName+" "+Customeruser.LastName),
                new Claim(JwtClaimTypes.GivenName,Customeruser.FirstName),
                new Claim(JwtClaimTypes.FamilyName,Customeruser.LastName),
                new Claim(JwtClaimTypes.Email,Customeruser.Email),
                new Claim(JwtClaimTypes.Role,SD.Customer),
            }).Result;
        }
    }
}
