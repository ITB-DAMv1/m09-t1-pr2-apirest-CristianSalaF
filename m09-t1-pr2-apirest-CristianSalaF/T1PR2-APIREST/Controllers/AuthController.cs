using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using T1PR2_APIREST.DTOs;
using T1PR2_APIREST.Models;

namespace T1PR2_APIREST.Controllers
{
    /// <summary>
    /// Controller for user authentication and registration.
    /// This controller handles user login, registration, and token generation.
    /// It uses ASP.NET Core Identity for user management and JWT for token generation.
    /// It provides endpoints for checking token validity, registering users, and logging in users.
    /// It also includes methods for registering users as admins.
    /// This controller is secured with authentication and authorization mechanisms.
    /// It is used to manage user access to the application and its resources.
    /// It provides a way to authenticate users and issue tokens that can be used to access protected resources.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AuthController> _logger;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor for the AuthController class.
        /// This constructor takes UserManager, ILogger, and IConfiguration as parameters.
        /// It is used to initialize the controller with the required dependencies.
        /// UserManager is used for managing user accounts and authentication.
        /// ILogger is used for logging information and errors.
        /// IConfiguration is used for accessing application configuration settings. 
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        public AuthController(UserManager<User> userManager, ILogger<AuthController> logger, IConfiguration configuration)
        {
            _userManager = userManager;
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// CheckToken method to verify the validity of the JWT token.
        /// This method returns the username, user ID, and role of the authenticated user.
        /// It is used to check if the token is still valid and to retrieve user information.
        /// </summary>
        /// <returns></returns>
        [HttpGet("check")]
        public IActionResult CheckToken()
        {
            return Ok(new
            {
                Username = User.Identity?.Name,
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                Role = User.FindFirst(ClaimTypes.Role)?.Value
            });
        }

        /// <summary>
        /// Register method to create a new user account.
        /// This method takes a RegisterDTO object as input and creates a new user in the database.
        /// It also assigns the user to the "User" role.
        /// This method is accessible to all users.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO model)
        {
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                DisplayName = model.UserName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");
                return Ok("User registered successfully.");
            }

            return BadRequest(result.Errors);
        }

        /// <summary>
        /// RegisterAdmin method to create a new admin account.
        /// This method takes a RegisterDTO object as input and creates a new user in the database.
        /// It also assigns the user to the "Admin" role.
        /// This method is accessible to all users. Though, for production this should be changed.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("admin/register")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterDTO model)
        {
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                DisplayName = model.UserName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
                return Ok("Admin registered successfully.");
            }

            return BadRequest(result.Errors);
        }

        /// <summary>
        /// Login method to authenticate users and generate JWT tokens.
        /// This method checks the provided email and password against the stored user credentials.
        /// If the credentials are valid, it creates a JWT token with the user's claims and roles.
        /// The token is then returned in the response.
        /// This method is accessible to all users.
        /// It is used for user authentication and authorization in the application.
        /// The token can be used to access protected resources in the API.
        /// The token is signed using a symmetric key defined in the configuration.
        /// The token has an expiration time defined in the configuration.
        /// The token is returned in the response body.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return Unauthorized("Invalid email or password.");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("DisplayName", user.DisplayName)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));CreateToken(claims.ToArray());

            return Ok(CreateToken(claims.ToArray()));
        }

        private string CreateToken(Claim[] claims)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["ExpirationMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
