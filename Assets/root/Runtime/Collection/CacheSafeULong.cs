using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif
using static System.Runtime.CompilerServices.MethodImplOptions;

namespace SharedUtils {

[Serializable]
#if ENABLE_IL2CPP
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
[StructLayout(LayoutKind.Explicit)]
public struct CacheSafeULong {
    public const int maxSize = 6;

    [FieldOffset(0)] private int size;
    [FieldOffset(8)] private ulong[] overflow;
    [FieldOffset(16)] private ulong e0;
    [FieldOffset(24)] private ulong e1;
    [FieldOffset(32)] private ulong e2;
    [FieldOffset(40)] private ulong e3;
    [FieldOffset(48)] private ulong e4;
    [FieldOffset(56)] private ulong e5;

    public int Count {
        [MethodImpl(AggressiveInlining)] get => size;
    }

    public bool IsInline {
        [MethodImpl(AggressiveInlining)] get => overflow == null;
    }

    private Span<ulong> inline {
        [MethodImpl(AggressiveInlining)] get => MemoryMarshal.CreateSpan(ref e0, maxSize);
    }

    [MethodImpl(AggressiveInlining)] public Span<ulong> GetSpan() {
        return overflow == null ? inline.Slice(0, size) : overflow.AsSpan(0, size);
    }

    [MethodImpl(AggressiveInlining)] public bool Contains(ulong item) => GetSpan().IndexOf(item) >= 0;

    public void AddOnce(ulong item) {
        if (Contains(item)) return;

        if (overflow == null) {
            if (size < maxSize) {
                inline[size] = item;
                size++;
                return;
            }

            overflow = new ulong[maxSize * 2];
            inline.CopyTo(overflow);
        } else if (size == overflow.Length) {
            Array.Resize(ref overflow, size * 2);
        }

        overflow[size] = item;
        size++;
    }

    public void Remove(ulong item) {
        var items = GetSpan();
        var index = items.IndexOf(item);
        if (index < 0) return;

        size--;
        items[index] = items[size];

        if (overflow != null && size <= maxSize / 2) {
            overflow.AsSpan(0, size).CopyTo(inline);
            overflow = null;
        }
    }

    [MethodImpl(AggressiveInlining)] public void Clear() {
        size = 0;
        overflow = null;
    }
}
}
