using DataLayer;
using DataLayer.Models;
using DataLayer.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using POCOApp.Authorization;
using POCOApp.Services;
using POCOApp.Services.Interface;

namespace POCOApp.Controllers
{
    [CustomAuthorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {

        IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(AuthenticateRequest ar)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            var arr = await _userService.Login(ar);
          
            if(arr.Username == "invalid")
            {
                return BadRequest();
            }
            else
            {
                return Ok(arr);
            }


        }
        //// POST: api/account
        //[AllowAnonymous]
        //[HttpPost("login")]
        //public List<Department> Login(AuthenticateRequest ar)
        //{
        //    //ar.UserName = "helloworld";
        //    //ar.Password = "test@123";
        //    _userService.Login(ar);
        //    return _userService.GetDepartmentList();
        //}
        // POST: api/account
        //[AllowAnonymous]
        //[HttpPost("Register")]
        //public async Task<IActionResult> Register(string username, string password)
        //{
            //var user = new ApplicationUser
            //{
            //    UserName = username,
            //};
            //var result = await _userManager.CreateAsync(user,password);
            //_userManager.AddToRoleAsync(user, "admin");
            //if(result.Succeeded)
            //{
            //    await _signInManager.SignInAsync(user, isPersistent: false);
            //    return Ok(new { data= result, success=true });
            //}
            //return BadRequest("invalid resigtration");
            
    //    }

      //  [AllowAnonymous]
     //   [HttpPost("login")]
      //  public async Task<IActionResult> Login(AuthenticateRequest ar)
     //   {
            //if(ModelState.IsValid)
            //{
            //    var result = await _signInManager.PasswordSignInAsync(ar.UserName, ar.Password, isPersistent: false,lockoutOnFailure:false);
            //    if(result.Succeeded)
            //    {
            //        return Ok(result);
            //    }
            //    //else 
            //    //{
            //    //    ModelState.AddModelError(string.Empty, "Invalid login attempt");
            //    //    return BadRequest(result);
            //    //}
                

            //}
            //return Ok("logged in");
       // }

    }
}
