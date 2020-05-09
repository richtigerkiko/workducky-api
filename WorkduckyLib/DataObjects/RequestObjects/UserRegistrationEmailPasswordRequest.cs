using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WorkduckyLib.Enums;
using WorkduckyLib.Interfaces;

namespace WorkduckyLib.DataObjects.RequestObjects
{
    public class UserRegistrationEmailPasswordRequest : IEmailPasswordRequest, IValidatableObject
    {
        public string Uid { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string PasswordVerification { get; set; }

        public CompanyReferences Companyreferences { get; set; }
        public AuthenticationTypes AuthType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Password != PasswordVerification)
            {
                yield return new ValidationResult("Passwords don't match", new string[] { "Password", "PasswordVerification" });
            }

            if (Password.Length < 6)
            {
                yield return new ValidationResult("Passwords too short", new string[] { "Password", "PasswordVerification" });
            }
        }
    }
}