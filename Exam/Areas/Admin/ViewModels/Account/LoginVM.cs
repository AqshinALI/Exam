using System.ComponentModel.DataAnnotations;

namespace Exam.Areas.Admin.ViewModels.Account
{
    public class LoginVM
    {
        [Required]
        public string UserOrEmail { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsRemembered { get; set; }

    }
}
