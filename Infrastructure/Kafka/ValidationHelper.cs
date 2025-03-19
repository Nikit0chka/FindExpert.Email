using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Kafka;

/// <summary>
/// Logic for validation
/// </summary>
internal static class ValidationHelper
{
    /// <summary>
    /// Try validate object
    /// </summary>
    /// <param name="obj">Object to validate</param>
    /// <param name="results">Validation result</param>
    /// <returns></returns>
    public static bool TryValidateObject(object obj, out ICollection<ValidationResult> results)
    {
        var context = new ValidationContext(obj, null, null);
        results = new List<ValidationResult>();
        return Validator.TryValidateObject(obj, context, results, true);
    }
}