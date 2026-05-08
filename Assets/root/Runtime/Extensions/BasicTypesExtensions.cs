using System.Runtime.CompilerServices;
using static System.Runtime.CompilerServices.MethodImplOptions;

namespace SharedUtils {
public static class BasicTypesExtensions {
    [MethodImpl(AggressiveInlining)] public static bool IsNullOrWhitespace(this string str) {
        return string.IsNullOrWhiteSpace(str);
    }
}
}