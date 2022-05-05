using System;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;


using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.IO;

namespace API.Middleware
{

    [AttributeUsage(AttributeTargets.Class)]
    public class CustomAuthorization : Attribute, IAuthorizationFilter
    {

        /// <summary>  
        /// This will Authorize User  
        /// </summary>  
        /// <returns></returns>  
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {

            if (filterContext != null)
            {
                Microsoft.Extensions.Primitives.StringValues authTokens;
                filterContext.HttpContext.Request.Headers.TryGetValue("Authorization", out authTokens);

                var _token = authTokens.FirstOrDefault();

                if (_token != null)
                {
                    string authToken = _token.Substring(7);
                    if (authToken != null)
                    {
                        if (IsValidToken(authToken))
                        {
                            filterContext.HttpContext.Response.Headers.Add("authToken", authToken);
                            filterContext.HttpContext.Response.Headers.Add("AuthStatus", "Authorized");

                            filterContext.HttpContext.Response.Headers.Add("storeAccessiblity", "Authorized");

                            return;
                        }
                        else
                        {
                            filterContext.HttpContext.Response.Headers.Add("authToken", authToken);
                            filterContext.HttpContext.Response.Headers.Add("AuthStatus", "NotAuthorized");

                            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            filterContext.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Not Authorized";
                            filterContext.Result = new JsonResult("NotAuthorized")
                            {
                                Value = new
                                {
                                    Status = "Error",
                                    Message = "Invalid Token"
                                },
                            };
                        }

                    }

                }
                else
                {
                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                    filterContext.HttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Please Provide authToken";
                    filterContext.Result = new JsonResult("Please Provide authToken")
                    {
                        Value = new
                        {
                            Status = "Error",
                            Message = "Please Provide authToken"
                        },
                    };
                }
            }
        }

        public bool IsValidToken(string authToken)
        {
            //validate Token here  

            var builder = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
                        .AddEnvironmentVariables();

            IConfigurationRoot config = builder.Build();

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(config["Token"]);
            try
            {
                tokenHandler.ValidateToken(authToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                
                if(jwtToken != null)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
    }
}