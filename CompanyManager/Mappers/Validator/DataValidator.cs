using System.ComponentModel.DataAnnotations;

namespace Company.Mappers.Validator
{
    public static class DataValidator
    {
        public static bool Validate<T>(T entity, out List<ValidationResult> validationErrors)
        {
            var context = new ValidationContext(entity);
            validationErrors = new List<ValidationResult>();
            return System.ComponentModel.DataAnnotations.Validator.TryValidateObject(entity, context, validationErrors, true);
        }
    }
}
