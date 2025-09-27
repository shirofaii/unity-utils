using System.Runtime.CompilerServices;
using static Globals;

namespace SharedUtils {
public static class BasicTypesExtensions {
    [MethodImpl(inline)] public static bool IsNullOrWhitespace(this string str) {
        return string.IsNullOrWhiteSpace(str);
    }
}
}