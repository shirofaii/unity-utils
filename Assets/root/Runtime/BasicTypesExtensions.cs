using System.Runtime.CompilerServices;

namespace SharedUtils {
public static class BasicTypesExtensions {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNullOrWhitespace(this string str) {
        return string.IsNullOrWhiteSpace(str);
    }
}
}