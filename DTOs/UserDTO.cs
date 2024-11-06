﻿using MedicineStorage.Models;
using System.ComponentModel.DataAnnotations;

namespace MedicineStorage.DTOs
{
    public class UserRegistrationDTO
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        public string UserName { get; set; } = string.Empty; // ???
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public List<string> Roles { get; set; } = new List<string>();

    }

    public class UserLoginDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class UserReturnDTO
    {
        [Required]  
        public string UserName { get; set; }
        [Required]
        public string Token { get; set; }
    }

    public class UserKnownDTO
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new List<string>();
    }
}