using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace SharedUtils {
#if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    public static class ArrayHelpers {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Grow<T>(ref T[] array, int newSize) {
            var newArray = new T[newSize];
            if (array != null) Array.Copy(array, 0, newArray, 0, array.Length);
            array = newArray;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf<T>(T[] array, T value, EqualityComparer<T> comparer) {
            for (int i = 0, length = array.Length; i < length; ++i) {
                if (comparer.Equals(array[i], value)) {
                    return i;
                }
            }

            return -1;
        }
    }
}
