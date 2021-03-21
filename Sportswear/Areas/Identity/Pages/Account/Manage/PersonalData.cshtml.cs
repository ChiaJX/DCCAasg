using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Sportswear.Areas.Identity.Data;

namespace Sportswear.Areas.Identity.Pages.Account.Manage
{
    public class PersonalDataModel : PageModel
    {
        private readonly UserManager<SportswearUser> _userManager;
        private readonly SignInManager<SportswearUser> _signInManager;
        private readonly ILogger<PersonalDataModel> _logger;

        public PersonalDataModel(
            UserManager<SportswearUser> userManager,
            SignInManager<SportswearUser> signInManager,
            ILogger<PersonalDataModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel // Related Profile Page
        {
            [DataType(DataType.Text)]
            [Display(Name = "Full Name")]
            public string FullName { get; set; }


            [DataType(DataType.Date)]
            [Display(Name = "Date Of Birth")]
            public DateTime DOB { get; set; }


            [DataType(DataType.Text)]
            [Display(Name = "Address")]
            public string Address { get; set; }
        }

        private async Task LoadAsync(SportswearUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                FullName = user.FullName,
                Address = user.Address,
                DOB = user.DOB
            };
        }

        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync() //Update the new data inside the table
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            if (Input.FullName != user.FullName)
            {
                user.FullName = Input.FullName;
            }

            if (Input.DOB != user.DOB)
            {
                user.DOB = Input.DOB;
            }

            if (Input.Address != user.Address)
            {
                user.Address = Input.Address;
            }

            await _userManager.UpdateAsync(user);
            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}