using Flaadestation.Repository.Database;
using Flaadestation.Service.DTO.CompanyDTO;
using Flaadestation.Service.DTO.LicenseDTO;
using Flaadestation.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Flaadestation.ASP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ICompanyService _companyService;
        private readonly ILicenseService _licenseService;
        public UserController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ICompanyService companyService, ILicenseService licenseService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _companyService = companyService;
            _licenseService = licenseService;
        }

        [HttpGet]
        public IActionResult GetUser()
        {
            try
            {
                if (User.Identity is null || !User.Identity.IsAuthenticated)
                    return Unauthorized();

                var claims = User.Claims.ToDictionary(x => x.Type, x => x.Value);

                return Ok(claims);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(loginRequest.Email, loginRequest.Password, true, false);

                if (result.Succeeded)
                    return Ok();

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register-owner")]
        public async Task<IActionResult> RegisterOwner(RegisterOwnerRequest registerOwnerRequest)
        {
            try
            {
                var createdCompany = await _companyService.CreateCompanyAsync(new CompanyRequestDTO
                {
                    Name = registerOwnerRequest.CompanyName,
                    AddressId = registerOwnerRequest.CompanyAddressId
                });

                var user = new ApplicationUser
                {
                    UserName = registerOwnerRequest.Email,
                    Email = registerOwnerRequest.Email,
                    CompanyId = createdCompany.CompanyId,
                    IsCompanyOwner = true
                };

                var result = await _userManager.CreateAsync(user, registerOwnerRequest.Password);

                if (!result.Succeeded)
                    return BadRequest(result.Errors);

                var createdUser = await _userManager.FindByEmailAsync(user.Email);

                if (createdUser is null)
                    return BadRequest("Couldn't find new user");

                await _userManager.AddClaimAsync(createdUser, new Claim("IsCompanyOwner", createdUser.IsCompanyOwner.ToString().ToLower(), ClaimValueTypes.Boolean));

                await _licenseService.CreateLicenseAsync(new LicenseRequestDTO
                {
                    CompanyId = createdCompany.CompanyId,
                    UserId = createdUser.Id,
                });

                if (registerOwnerRequest.AmountOfLicenses > 1)
                    await _licenseService.AddRangeByCompanyIdAsync(createdCompany.CompanyId, registerOwnerRequest.AmountOfLicenses - 1);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("register-staff")]
        public async Task<IActionResult> RegisterStaff(RegisterStaffRequest registerStaffRequest)
        {
            try
            {
                var license = await _licenseService.GetLicenseByLicenseKeyAsync(registerStaffRequest.LicenseKey);
                if (license is null)
                    return NotFound("License doesn't exist");

                var user = new ApplicationUser
                {
                    UserName = registerStaffRequest.Email,
                    Email = registerStaffRequest.Email,
                    CompanyId = license.Company!.CompanyId,
                    IsCompanyOwner = false
                };

                var result = await _userManager.CreateAsync(user, registerStaffRequest.Password);

                if (!result.Succeeded)
                    return BadRequest(result.Errors);

                var createdUser = await _userManager.FindByEmailAsync(user.Email);

                if (createdUser is null)
                    return BadRequest("Couldn't find new user");

                await _userManager.AddClaimAsync(createdUser, new Claim("IsCompanyOwner", createdUser.IsCompanyOwner.ToString().ToLower(), ClaimValueTypes.Boolean));

                await _licenseService.UpdateLicenseByIdAsync(license.Company.CompanyId, new LicenseRequestDTO
                {
                    UserId = createdUser.Id,
                });

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public class LoginRequest
        {
            [Required]
            public required string Email { get; set; }

            [Required]
            public required string Password { get; set; }

        }

        public class RegisterOwnerRequest
        {
            [Required]
            public required string Email { get; set; }

            [Required]
            public required string Password { get; set; }

            [Required]
            public required string CompanyName { get; set; }

            [Required]
            public Guid CompanyAddressId { get; set; }

            [Required]
            public int AmountOfLicenses { get; set; }
        }

        public class RegisterStaffRequest
        {
            [Required]
            public required string Email { get; set; }

            [Required]
            public required string Password { get; set; }

            [Required]
            public required Guid LicenseKey { get; set; }
        }
    }
}
