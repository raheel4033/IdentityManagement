using System.ComponentModel.DataAnnotations;

namespace IdentityManager.ViewModels
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
