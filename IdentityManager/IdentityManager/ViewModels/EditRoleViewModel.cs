using System.ComponentModel.DataAnnotations;

namespace IdentityManager.ViewModels
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
            Claims = new List<string>();
            Roles = new List<string>();
        }
        public string Id { get; set; }
        public string UserName { get; set; }
        [Required(ErrorMessage="Role Name is required")]
        public string RoleNames { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        public List<string> Users { get; set; }
        public List<string> Claims { get; set; }
        public IList<string> Roles { get; set; }

    }
}
