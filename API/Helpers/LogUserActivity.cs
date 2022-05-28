using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;
using API.Extensions;
using API.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
namespace API.Helpers
{
    public class LogUserActivity: IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next){

            var resultContext = await next();

            if(!resultContext.HttpContext.User.Identity.IsAuthenticated){
                return;
            }

            var userId = resultContext.HttpContext.User.GetUserId();

            var repo = resultContext.HttpContext.RequestServices.GetService<IUserRepository>();

            var user = await repo.GetUserByIdAsync(userId);

            user.LastActivated = DateTime.Now;

            repo.Update(user);
            await repo.SaveAllAsync();
        }
    }
}