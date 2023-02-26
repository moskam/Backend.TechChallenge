using System.Collections.Generic;

namespace Backend.TechChallenge.Test.Helpers;

public class ValidationResult
{
    public string Title { get; set; }
    public IDictionary<string, string[]> Errors { get; set; }
}
