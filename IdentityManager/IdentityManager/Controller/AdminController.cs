using IdentityManager.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityManager.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrator,User")]
    public class AdminController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = _userManager.Users.ToList();
            return Ok(users);
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = _roleManager.Roles;
            return Ok(roles);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByNameAsync(id);
            if (user == null)
            {
                ModelState.AddModelError("", "User with Id not found");
                return NotFound();
            }
            else
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return Content("ListUsers");
                }
                foreach (var error in result.Errors) 
                {
                    ModelState.AddModelError("", "User with Id Cannot Found");
                }
                return Ok("Deleted");
            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByNameAsync(id);
            if (role == null)
            {
                ModelState.AddModelError("", "Role with Id not found");
                return NotFound();
            }
            else
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return Content("ListUsers");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", "User with Id Cannot Found");
                }
                return Ok("Deleted Roles");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };
                IdentityResult result = await _roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return Content("Added");
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return Ok(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(EditRoleViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Id);
            if (user == null)
            {
                ModelState.AddModelError("", $"User with Id={model.Id} cannot be found");
                return NotFound();
            }
            else
            {
                user.Email = model.Email;
                user.UserName = model.UserName;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Content("List Added");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", "Error Occured");
                }
                return Ok(result);

            }
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ModelState.AddModelError("", "Roles doesnot found");
                return BadRequest();
            }
            else
            {
                role.Name = model.RoleNames;
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return Content("ListRoles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", "Role Edited");
                }
            }
            return Ok(model);

        }
        [HttpGet]
        public async Task<IActionResult> EditUserInRole(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ModelState.AddModelError("", "Role with Id cannot be found");
                return NotFound();
            }

            var model = new List<UserRoleViewModel>();

            foreach (var user in _userManager.Users)
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }
                model.Add(userRoleViewModel);
            }
            return Ok(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                ModelState.AddModelError("", "Role with Id cannot be found");
                return NotFound();
            }
            for (int i = 0; i < model.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;
                if (model[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }
                if (result.Succeeded)
                {
                    if (i < model.Count - 1)
                    {
                        continue;
                    }
                    else
                    {
                        return Content("Role Added");
                    }
                }

            }
            return Content("Role Added");

        }


    }
}
