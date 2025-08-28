using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace BusBooking.Shared.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class NotGreaterThanAttribute : ValidationAttribute, IClientModelValidator
{
    public string OtherProperty { get; }
    public NotGreaterThanAttribute(string otherProperty) => OtherProperty = otherProperty;

    protected override ValidationResult? IsValid(object? value, ValidationContext context)
    {
        var available = ToNullableInt(value);
        var otherProp = context.ObjectType.GetProperty(OtherProperty)
                       ?? throw new ArgumentException($"Property '{OtherProperty}' not found.");
        var total = ToNullableInt(otherProp.GetValue(context.ObjectInstance));

        if (available.HasValue && total.HasValue && available.Value > total.Value)
        {
            return new ValidationResult(ErrorMessage, new[] { context.MemberName! });
        }
        return ValidationResult.Success;
    }

    private static int? ToNullableInt(object? o)
        => o is null ? null : (int.TryParse(o.ToString(), out var n) ? n : null);

    // --- client-side hooks
    public void AddValidation(ClientModelValidationContext ctx)
    {
        Merge(ctx.Attributes, "data-val", "true");
        Merge(ctx.Attributes, "data-val-availnotgt", ErrorMessage ?? "≤ total seats.");
        Merge(ctx.Attributes, "data-val-availnotgt-other", $"*.{OtherProperty}");
    }

    private static void Merge(IDictionary<string, string> attrs, string key, string value)
    {
        if (!attrs.ContainsKey(key)) attrs.Add(key, value);
    }
}
