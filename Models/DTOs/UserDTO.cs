using System.ComponentModel.DataAnnotations;

namespace MedicineStorage.Models.DTOs
{

    public class ReturnUserLoginDTO
    {
        public ReturnUserTokenDTO returnUserTokenDTO { get; set; }

        public ReturnUserPersonalDTO returnUserDTO { get; set; }
    }
    public class ReturnUserPersonalDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? Position { get; set; }
        public string? Company { get; set; }
        public string Email { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new List<string>();

        public string? PhotoBase64 { get; set; }
    }
    public class ReturnUserGeneralDTO
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Position { get; set; }
        public string? Company { get; set; }
        public string? PhotoBase64 { get; set; }
    }


    public class ReturnUserTokenDTO
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }


    public class UserRefreshTokenDTO
    {
        public string RefreshToken { get; set; } = string.Empty;
    }

    public class UserRegistrationDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string LastName { get; set; } = string.Empty;
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string UserName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;

        [StringLength(100, MinimumLength = 3)]
        public string? Position { get; set; }

        [StringLength(100, MinimumLength = 3)]
        public string? Company { get; set; }
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


    public class UserUpdateDTO
    {
        [StringLength(100)]
        public string? FirstName { get; set; } = string.Empty;
        [StringLength(100)]
        public string? LastName { get; set; } = string.Empty;
        [StringLength(100)]
        public string? Position { get; set; }

        [StringLength(100)]
        public string? Company { get; set; }
        [EmailAddress]
        public string? Email { get; set; } = string.Empty;
    }

    public class ChangePasswordDTO
    {
        [Required]
        [MinLength(6)]
        public string CurrentPassword { get; set; }

        [Required]
        [MinLength(6)]
        public string NewPassword { get; set; }
    }
}
