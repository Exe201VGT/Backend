using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Validation
{
    public class ValidDateOfBirthAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateOnly dateOfBirth)
            {
                if (dateOfBirth > DateOnly.FromDateTime(DateTime.UtcNow))
                {
                    return new ValidationResult("Date of birth cannot be in the future.");
                }

                var age = DateTime.UtcNow.Year - dateOfBirth.Year;
                if (dateOfBirth.AddYears(age) > DateOnly.FromDateTime(DateTime.UtcNow))
                {
                    age--;
                }

                if (age < 18)
                {
                    return new ValidationResult("User must be at least 18 years old.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
