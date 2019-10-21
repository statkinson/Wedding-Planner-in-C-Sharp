using System;
using System.ComponentModel.DataAnnotations;
namespace wedding_planner.Models
{
    public class LoginUser
    {
        [Required(ErrorMessage="Invalid.")]
        public string Email { get; set; }
        [Required(ErrorMessage="Invalid.")]
        public string Password { get; set; }
    }
}