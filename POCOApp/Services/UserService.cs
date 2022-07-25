using DataLayer;
using DataLayer.Models;
using DataLayer.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using POCOApp.Services.Interface;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace POCOApp.Services
{
    public class UserService : IUserService
    {
        private readonly IConfiguration _config;
        private DataContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public UserService(IConfiguration config, DataContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        public List<Department> GetDepartmentList()
        {
            List<Department> departmentList;
            try
            {
                departmentList = _context.Set<Department>().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return departmentList;
        }


        public async Task<string> Register(RegisterRequest rr)
        {
            var user = new ApplicationUser { INumber = rr.INumber,UserName=rr.UserName,Email=rr.Email,PasswordHash=rr.Password };
            IdentityResult result = await _userManager.CreateAsync(user);
            if(result.Succeeded)
            {
                return "sucess";
            }
            else
            {
                return "failed";
            }
        }

        public async Task<AuthenticateResponse> Login(AuthenticateRequest ar)
        {
            //if (ar.UserName == "helloworld" && ar.Password == "test@123")
            //first validate if the credentials are correct

            var user = await _userManager.FindByNameAsync(ar.UserName);
            var roles = await _userManager.GetRolesAsync(user);
            if(user != null && 
                await _userManager.CheckPasswordAsync(user,ar.Password))
             {
                var identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier,ar.UserName));
                //if it is valid user then generate JWT token else return bad request
                var ticket = GenerateToken(ar, roles);
                return new AuthenticateResponse()
                {
                    Username = ar.UserName,
                    JwtToken = ticket,
                };

            }
            return new AuthenticateResponse()
                {
                Username="invalid",
                JwtToken="invalid",
                };
        }

        public string GenerateToken(AuthenticateRequest user,IList<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = _config.GetValue<string>("JWT:Secret");
            var etm = _config.GetValue<int>("JWT:ExpiresIn");
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { 
                    new Claim("username", user.UserName.ToString()), 
                    new Claim("roles", string.Join(",",roles)) 
                }),
                Expires = System.DateTime.UtcNow.AddMinutes(etm),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

       
    }
}
