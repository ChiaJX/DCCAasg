using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sportswear.Areas.Identity.Data;
using Sportswear.Data;
using Sportswear.Models;
using System;
using System.Collections.Generic;
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


        public IActionResult AdminPanel(String msg=null)
        {
            ViewBag.msg = msg;
            return View(userManager.Users);
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

        public async Task<IActionResult> updateStaff(string id)
        {
            SportswearUser user = await userManager.FindByIdAsync(id);
            Boolean a = false; Boolean b = false; Boolean c = false;
            if(user.userRole == "Admin")
            {
                a = true;
            }
            else if(user.userRole == "Staff")
            {
                b = true;
            }
            else
            {
                c = true;
            }
            ViewBag.users = new List<SelectListItem>
            {
                //new SelectListItem{Selected = c, Text = "Select Option",Value= ""},
                new SelectListItem{Selected = b, Text = "Staff",Value= "Staff"},
                new SelectListItem{Selected = a, Text = "Admin",Value= "Admin"}
            };
            if (user != null)
                return View(user);
            else
                return RedirectToAction("AdminPanel");

        }

        [HttpPost]
        public async Task<IActionResult>updateStaff(string id,string email,string userrole)
        {
            SportswearUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                //user.Email = email;
                //user.UserName = email;
                user.userRole = userrole;

                IdentityResult result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("AdminPanel", new { msg = "User updated!" });
                else
                    return View(user);
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View(user);
        }

        public async Task<IActionResult> deleteStaff(String id)
        {
            SportswearUser user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                await userManager.DeleteAsync(user);
            }
            else
            {
                return RedirectToAction("AdminPanel", new { msg = "Fail to delete user" });
            }
            return RedirectToAction("AdminPanel", new { msg = "User deleted!" });
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
