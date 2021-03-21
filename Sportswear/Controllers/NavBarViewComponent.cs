using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sportswear.Controllers
{
    public class NavBarViewComponent : ViewComponent
    {
        
        public async Task<IViewComponentResult> InvokeAsync()
        {
            /*if(UserLoginStaus == true){
                //Get list of user here (IEnumerable<Sportswear.Areas.Identity.Data.SportswearUser>)
                //The list of user must contain only the current login user.

                users = getCurrentLoginUser();
                return View(users);
            }
            else
            {
                return View();
            }*/

            return View();
        }
    }
}
