using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace T1PR2_Client.Tools
{
    public class TokenAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TokenAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Try to get token from session
            var token = Context.Session.GetString("AuthToken");
            
            if (string.IsNullOrEmpty(token))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }
            
            try
            {
                // Parse the token to extract claims
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                
                // Create a ClaimsPrincipal from the token claims
                var identity = new ClaimsIdentity(jwtToken.Claims, "TokenAuth");
                var principal = new ClaimsPrincipal(identity);
                
                // Create an authentication ticket
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            catch (Exception ex)
            {
                return Task.FromResult(AuthenticateResult.Fail($"Invalid token: {ex.Message}"));
            }
        }
    }
}
