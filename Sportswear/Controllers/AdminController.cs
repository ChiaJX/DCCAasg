using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sportswear.Areas.Identity.Data;
using Sportswear.Data;
using Sportswear.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Sportswear.Views.Admin
{
    public class AdminController : Controller
    {

        private readonly SportswearContext context;
        private UserManager<SportswearUser> userManager;

        public AdminController(UserManager<SportswearUser> usrMgr)
        {
            userManager = usrMgr;
        }


        public IActionResult AdminPanel()
        {
            return View();
        }

        public IActionResult registerStaff()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> registerStaff(registerStaff user)
        {
            
            if (ModelState.IsValid)
            {

                SportswearUser webUser = new SportswearUser
                {
                    UserName = user.Email,
                    Email = user.Email,
                    userRole = user.Role,
                    EmailConfirmed = true
                };

                IdentityResult result = await userManager.CreateAsync(webUser, user.Password);
                if (result.Succeeded)
                
                    return RedirectToAction("AdminPanel");

                
                else
                {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }

            return View(user);

        }
        //public async Task<IActionResult> AdminPanel()
        //{
        //    var staffList = from user in context.Users
        //                          join userRoles in context.UserRoles on user.Id equals userRoles.UserId
        //                          join role in context.Roles on userRoles.RoleId equals role.Id
        //                          select new { UserId = user.Id, UserName = user.UserName, RoleId = role.Id, RoleName = role.Name };

        //    return View(await staffList.ToListAsync());
        //}
    }
}
