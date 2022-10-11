namespace DieteticConsultationAPI.Extensions;

public static class EnumerableExtensions
{
   // this could be extended
   public static bool IsEmpty<T>(this IEnumerable<T> enumerable) => !enumerable.Any();
}