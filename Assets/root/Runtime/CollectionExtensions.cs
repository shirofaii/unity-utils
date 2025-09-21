using Scellecs.Morpeh.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;
// ReSharper disable InconsistentNaming

namespace SharedUtils {
public static class CollectionExtensions {
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static T First<T>(this T[] array, T ifEmpty = default) {
        if (array == null || array.Length == 0) {
            return ifEmpty;
        }

        return array[0];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static T First<T>(this List<T> list, T ifEmpty = default) {
        if (list == null || list.Count == 0) {
            return ifEmpty;
        }

        return list[0];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static T First<T>(this FastList<T> list, T ifEmpty = default) {
        if (list == null || list.length == 0) {
            return ifEmpty;
        }

        return list[0];
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static T Last<T>(this T[] array, T ifEmpty = default) {
        return array.Length == 0 ? ifEmpty : array[^1];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static T Last<T>(this List<T> list, T ifEmpty = default) {
        return list.Count == 0 ? ifEmpty : list[^1];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static T Last<T>(this FastList<T> list, T ifEmpty = default) {
        return list.length == 0 ? ifEmpty : list[list.length - 1];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static T At<T>(this T[] array, int index, T ifNotInRange = default) {
        if (array == null || index >= array.Length || index < 0) {
            return ifNotInRange;
        }

        return array[index];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static T At<T>(this List<T> list, int index, T ifNotInRange = default) {
        if (list == null || index >= list.Count || index < 0) {
            return ifNotInRange;
        }

        return list[index];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static T At<T>(this IList<T> list, int index, T ifNotInRange = default) {
        if (list == null || index >= list.Count || index < 0) {
            return ifNotInRange;
        }

        return list[index];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static T At<T>(this FastList<T> list, int index, T ifNotInRange = default) {
        if (list == null || index >= list.length || index < 0) {
            return ifNotInRange;
        }

        return list[index];
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static V At<K, V>(this Dictionary<K, V> dict, K key, V ifAbsent = default) {
        if (dict == null || !dict.TryGetValue(key, out var value)) {
            return ifAbsent;
        }

        return value;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static T AtLast<T>(this T[] array, int index, T ifNotInRange = default) {
        if (array == null || index >= array.Length || index < 0) {
            return ifNotInRange;
        }

        return array[array.Length - index - 1];
    }

    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static T AtWrap<T>(this T[] array, int index, T ifEmpty = default) {
        if (array == null || array.Length == 0) {
            return ifEmpty;
        }

        if (index >= 0) {
            return array[index % array.Length];
        }

        var idx = index % array.Length;
        idx = array.Length - idx - 1;
        return array[idx];
    }

    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static T AtWrap<T>(this List<T> list, int index, T ifEmpty = default) {
        if (list == null || list.Count == 0) {
            return ifEmpty;
        }

        if (index >= 0) {
            return list[index % list.Count];
        }

        var idx = index % list.Count;
        idx = list.Count - idx - 1;
        return list[idx];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static T AtWrap<T>(this FastList<T> list, int index, T ifEmpty = default) {
        if (list == null || list.length == 0) {
            return ifEmpty;
        }

        if (index >= 0) {
            return list[index % list.length];
        }

        var idx = index % list.length;
        idx = list.length - idx - 1;
        return list[idx];
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static List<T> Filter<T>(this T[] array, Func<T, bool> func) {
        var result = new List<T>(array.Length);
        for (var i = 0; i < array.Length; i++) {
            if (func.Invoke(array[i])) {
                result.Add(array[i]);
            }
        }

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static List<T> Filter<T>(this List<T> list, Func<T, bool> func) {
        var result = new List<T>(list.Count);
        for (var i = 0; i < list.Count; i++) {
            if (func.Invoke(list[i])) {
                result.Add(list[i]);
            }
        }

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static List<R> Map<T, R>(this List<T> list, Func<T, R> func) {
        var result = new List<R>(list.Count);
        for (var i = 0; i < list.Count; i++) {
            result.Add(func.Invoke(list[i]));
        }

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static List<R> Map<T, R>(this List<T> list, Func<T, int, R> func) {
        var result = new List<R>(list.Count);
        for (var i = 0; i < list.Count; i++) {
            result.Add(func.Invoke(list[i], i));
        }

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static List<R> Map<T, R>(this T[] array, Func<T, R> func) {
        var result = new List<R>(array.Length);
        for (var i = 0; i < array.Length; i++) {
            result.Add(func.Invoke(array[i]));
        }

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static List<R> Map<T, R>(this T[] array, Func<T, int, R> func) {
        var result = new List<R>(array.Length);
        for (var i = 0; i < array.Length; i++) {
            result.Add(func.Invoke(array[i], i));
        }

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static R Fold<T, R>(this List<T> list, Func<R, T, R> func, R acc) {
        for (var i = 0; i < list.Count; i++) {
            acc = func.Invoke(acc, list[i]);
        }

        return acc;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static T Fold<T>(this List<T> list, Func<T, T, T> func) {
        var acc = list.At(0);
        for (var i = 1; i < list.Count; i++) {
            acc = func.Invoke(acc, list[i]);
        }

        return acc;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static R Fold<T, R>(this T[] array, Func<R, T, R> func, R acc) {
        for (var i = 0; i < array.Length; i++) {
            acc = func.Invoke(acc, array[i]);
        }

        return acc;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static T Fold<T>(this T[] array, Func<T, T, T> func) {
        var acc = array.At(0);
        for (var i = 0; i < array.Length; i++) {
            acc = func.Invoke(acc, array[i]);
        }

        return acc;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static int Sum<T>(this List<T> list, Func<T, int> func, int acc = 0) {
        for (var i = 0; i < list.Count; i++) {
            acc += func.Invoke(list[i]);
        }

        return acc;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static float Sum<T>(this List<T> list, Func<T, float> func, float acc = 0f) {
        for (var i = 0; i < list.Count; i++) {
            acc += func.Invoke(list[i]);
        }

        return acc;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static T MinBy<T>(this List<T> list, Func<T, float> func, T ifEmpty = default) {
        if (list.IsNullOrEmpty()) {
            return ifEmpty;
        }

        var minValue = float.MaxValue;
        var idx = -1;
        for (var i = 0; i < list.Count; i++) {
            var v = func.Invoke(list[i]);
            if (v <= minValue) {
                minValue = v;
                idx = i;
            }
        }

        return list[idx];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static T MinBy<T>(this T[] array, Func<T, float> func, T ifEmpty = default) {
        if (array.IsNullOrEmpty()) {
            return ifEmpty;
        }

        var minValue = float.MaxValue;
        var idx = -1;
        for (var i = 0; i < array.Length; i++) {
            var v = func.Invoke(array[i]);
            if (v <= minValue) {
                minValue = v;
                idx = i;
            }
        }

        return array[idx];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static T MaxBy<T>(this List<T> list, Func<T, float> func, T ifEmpty = default) {
        if (list.IsNullOrEmpty()) {
            return ifEmpty;
        }

        var maxValue = float.MinValue;
        var idx = -1;
        for (var i = 0; i < list.Count; i++) {
            var v = func.Invoke(list[i]);
            if (v >= maxValue) {
                maxValue = v;
                idx = i;
            }
        }

        return list[idx];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static T MaxBy<T>(this T[] array, Func<T, float> func, T ifEmpty = default) {
        if (array.IsNullOrEmpty()) {
            return ifEmpty;
        }

        var maxValue = float.MinValue;
        var idx = -1;
        for (var i = 0; i < array.Length; i++) {
            var v = func.Invoke(array[i]);
            if (v >= maxValue) {
                maxValue = v;
                idx = i;
            }
        }

        return array[idx];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static int Count<T>(this T[] array, Func<T, bool> func) {
        var count = 0;
        for (var i = 0; i < array.Length; i++) {
            count += func.Invoke(array[i]) ? 1 : 0;
        }

        return count;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static int Count<T>(this List<T> list, Func<T, bool> func) {
        var count = 0;
        for (var i = 0; i < list.Count; i++) {
            count += func.Invoke(list[i]) ? 1 : 0;
        }

        return count;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static bool Any<T>(this T[] array, Func<T, bool> func) {
        for (var i = 0; i < array.Length; i++) {
            if (func.Invoke(array[i])) {
                return true;
            }
        }

        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static bool Any<T>(this List<T> list, Func<T, bool> func) {
        for (var i = 0; i < list.Count; i++) {
            if (func.Invoke(list[i])) {
                return true;
            }
        }

        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static bool All<T>(this List<T> list, Func<T, bool> func) {
        for (var i = 0; i < list.Count; i++) {
            if (!func.Invoke(list[i])) {
                return false;
            }
        }

        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static bool All<T>(this T[] array, Func<T, bool> func) {
        for (var i = 0; i < array.Length; i++) {
            if (!func.Invoke(array[i])) {
                return false;
            }
        }

        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static List<T> Except<T>(this List<T> list, T item) {
        var result = new List<T>(list);
        result.Remove(item);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static int IndexOf<T>(this List<T> list, Func<T, bool> func, int ifAbsent = -1) {
        for (var i = 0; i < list.Count; i++) {
            if (func.Invoke(list[i])) {
                return i;
            }
        }

        return ifAbsent;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static int IndexOf<T>(this T[] array, Func<T, bool> func, int ifAbsent = -1) {
        for (var i = 0; i < array.Length; i++) {
            if (func.Invoke(array[i])) {
                return i;
            }
        }

        return ifAbsent;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static int IndexOf<T>(this T[] array, T item, int ifAbsent = -1) {
        for (var i = 0; i < array.Length; i++) {
            if (array[i].Equals(item)) {
                return i;
            }
        }

        return ifAbsent;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static bool IsNullOrEmpty<T>(this List<T> list) => list == null || list.Count == 0;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static bool IsNullOrEmpty<T>(this T[] list) => list == null || list.Length == 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static bool Contains<T>(this T[] array, T item) {
        for (var i = 0; i < array.Length; i++) {
            if (Equals(array[i], item)) {
                return true;
            }
        }

        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static T Find<T>(this T[] array, Func<T, bool> func, T ifAbsent = default) {
        for (var i = 0; i < array.Length; i++) {
            if (func.Invoke(array[i])) {
                return array[i];
            }
        }

        return ifAbsent;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static T[] ForEach<T>(this T[] array, Action<T> action) {
        for (var i = 0; i < array.Length; i++) {
            action.Invoke(array[i]);
        }

        return array;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static T[] ForEach<T>(this T[] array, Action<T, int> action) {
        for (var i = 0; i < array.Length; i++) {
            action.Invoke(array[i], i);
        }

        return array;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static List<T> ForEach<T>(this List<T> list, Action<T, int> action) {
        for (var i = 0; i < list.Count; i++) {
            action.Invoke(list[i], i);
        }

        return list;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static T[] ForEachNotNull<T>(this T[] array, Action<T> action) {
        for (var i = 0; i < array.Length; i++) {
            if (array[i] != null) {
                action.Invoke(array[i]);
            }
        }

        return array;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static T Pop<T>(this List<T> list, T ifEmpty = default) {
        if (list.Count == 0) {
            return ifEmpty;
        }

        var result = list[^1];
        list.RemoveAt(list.Count - 1);
        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static T AddNotNull<T>(this List<T> list, T value) where T : class {
        if (value == null) {
            return null;
        }

        list.Add(value);
        return value;
    }
}
}