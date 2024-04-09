using IdentityManagerServiseAPI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SharedClassLibrary.Contract;
using SharedClassLibrary.DTOs;
using SharedClassLibrary.UserDTO;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static SharedClassLibrary.DTOs.ServiceResponce;

namespace IdentityManagerServiseAPI.Repositories
{
    public class AccountRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,IConfiguration config) : IUserAccount
    {
        public async Task<ServiceResponce.GenerenalResponse> CreateAccount(UserDTOs UserDTO)
        {
            try
            {
                if (UserDTO is null) return new GenerenalResponse(false, "Model is enty");

                var newuser = new ApplicationUser()
                {
                    Name = UserDTO.Name,
                    Email = UserDTO.Email,
                    PasswordHash = UserDTO.Password,
                    UserName = UserDTO.Email
                };
                var user = await userManager.FindByEmailAsync(newuser.Email);
                if (user is not null) return new GenerenalResponse(false, "User registered Already");
                var createUser = await userManager.CreateAsync(newuser!, UserDTO.Password);
                if (!createUser.Succeeded) return new GenerenalResponse(false, "Error occured... Please try Again");

                var checkAdmin = await roleManager.FindByNameAsync("Admin");
                if (checkAdmin is null)
                {
                    await roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
                    await userManager.AddToRoleAsync(newuser, "Admin");
                    return new GenerenalResponse(true, "Account Created");
                }
                else
                {
                    var checkUser = await roleManager.FindByNameAsync("User");
                    if (checkUser is null)
                        await roleManager.CreateAsync(new IdentityRole() { Name = "User" });

                    await userManager.AddToRoleAsync(newuser, "User");
                    return new GenerenalResponse(true, "Account Created");

                }
            }
            catch (Exception ex)
            {
                return new GenerenalResponse(false ,$"Error occured {ex.InnerException}" );
            }
            
        }

        public async Task<ServiceResponce.LoginResponse> LoginAccount(LoginDTO LoginDTO)
        {
            if (LoginDTO == null)
                return new LoginResponse(false, null, "Login container is empty");
            var getUser = await userManager.FindByEmailAsync(LoginDTO.Email);
            if(getUser is null)
                return new LoginResponse(false ,null,"User Not Found");

            bool checkuserpassword = await userManager.CheckPasswordAsync(getUser ,LoginDTO.Password);
            if (!checkuserpassword)
                return new LoginResponse(false, null, "Invalid email/password");

            var getuserRole = await userManager.GetRolesAsync(getUser);
            var usersession = new UserSession(getUser.Id ,getUser.Name,getUser.Email ,getuserRole.First());

            string token = GenerateToken(usersession);
            return new LoginResponse(true,token!,"Login Complated");
        }

        private string GenerateToken(UserSession usersession)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:key"]!));
            var crediantial = new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256);
            var userclaims = new[]
            {
                    new Claim(ClaimTypes.NameIdentifier,usersession.Id.ToString()),
                    new Claim(ClaimTypes.Name,usersession.Name),
                    new Claim (ClaimTypes.Email,usersession.Email),
                    new Claim(ClaimTypes.Role,usersession.Role),
            };
            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:audience"],
                claims: userclaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: crediantial
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
