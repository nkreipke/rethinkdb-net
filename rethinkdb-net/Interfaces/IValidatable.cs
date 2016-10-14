using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RethinkDb
{
    public interface IValidatable
    {
        void Validate();
    }

    public static class ValidatableExtensions
    {
        public static void Validate(this IEnumerable<IValidatable> list)
        {
            foreach (var validatable in list)
                validatable.Validate();
        }

        public static void ValidateWithAnnotations(this IValidatable validatable)
        {
            Validator.ValidateObject(validatable, new ValidationContext(validatable));
        }
    }
}