using System.Text.Json;
namespace DieteticConsultationAPI.Extensions;

public static class FormCollectionExtensions
{
    internal static TResult? Deserialize<TResult>(this IFormCollection form, string propertyName, Func<string, TResult?>? parser = null)
    {
        ArgumentNullException.ThrowIfNull(form, nameof(IFormCollection));

        form.TryGetValue(propertyName, out var formValue);

        var strValue = formValue.ToString();
        if (strValue.IsEmpty()) return default;

        return parser is null
            ? JsonSerializer.Deserialize<TResult>(strValue)
            : parser.Invoke(strValue);
    }
}