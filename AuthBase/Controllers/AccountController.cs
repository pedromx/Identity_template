using API.Constants;
using API.Models;
using AuthBase.Models;
using DbContext.Entities;
using DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Services.Providers;
using System;
using System.Threading.Tasks;
using System.Web;

namespace API.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailProvider _emailProvider;
        private readonly IOptions<EmailProviderSettingsDTO> _emailOptions;

        public AccountController(ILogger<AccountController> logger,
           UserManager<User> userManager,
           RoleManager<IdentityRole> roleManager,
           IEmailProvider emailProvider,
           IOptions<EmailProviderSettingsDTO> emailOptions)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailOptions = emailOptions;
            _emailProvider = emailProvider;
        }

        public IActionResult Registration()
        {            
            return View();
        }

        public async Task<IActionResult> Register(UserRegistrationViewModel model)
        {
            if (!(await _roleManager.RoleExistsAsync(Roles.Professional)))
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.Professional));               
            }
            
            var professional = new User
            {
                UserName = model.Email,
                Email = model.Email,
                LastName = model.LastName,
                FirstName = model.FirstName                
            };

            var result = await _userManager.CreateAsync(professional, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result);
            }

            //Send Email
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(professional);    
            var confirmEmailUrl = "https://localhost:5001/Email-confirm";
            var uriBuilder = new UriBuilder(confirmEmailUrl);
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);
            query["token"] = token;
            query["userid"] = professional.Id;
            uriBuilder.Query = query.ToString();
            var urlString = uriBuilder.ToString();

            var emailBody = $"Please confirm your email by clicking on the link below {urlString}";
            _emailProvider.Send(model.Email, emailBody, _emailOptions.Value);

            //////////////////

            var userFromDb = await _userManager.FindByNameAsync(professional.UserName);
            await _userManager.AddToRoleAsync(userFromDb, Roles.Professional);
                    
            return View();
        }
              
        [HttpGet("Email-confirm")]
        public async Task<IActionResult> ConfirmEmail([FromQuery]EmailConfirmViewModel model)
        {
            var employer = await _userManager.FindByIdAsync(model.UserId);
            var confirm = await _userManager.ConfirmEmailAsync(employer, Uri.UnescapeDataString(model.Token));

            if (confirm.Succeeded)
            {
                return RedirectToAction("Index", "/");
            }

            return Unauthorized();
        }
    }
}
