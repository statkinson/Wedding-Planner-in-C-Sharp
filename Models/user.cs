using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

    namespace wedding_planner.Models
    {
        public class User
        {
            [Key]
            public int UserId {get;set;}
            [Required]
            [MinLength(5, ErrorMessage= "First Name Must be 5 characters or longer!")]
            public string FirstName { get; set;}
            public List<RSVP> Action {get; set;}
            [Required]
            [MinLength(5, ErrorMessage= "Last Name Must be 5 characters or longer!")]
            public string LastName {get; set; }
            [Required]
            [MinLength(15, ErrorMessage= "Email Must be 15 characters or longer!")]
            public string Email {get; set;}
            [Required]
            [MinLength(10, ErrorMessage= "Password Must be 10 characters or longer!")]
            public string Password {get; set;}
            public DateTime CreatedAt {get;set;} = DateTime.Now;
            public DateTime UpdatedAt {get;set;} = DateTime.Now;
        }
    }