using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.IL2CPP.CompilerServices;
using static Globals;

namespace SharedUtils {

[Serializable]
#if ENABLE_IL2CPP
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
[StructLayout(LayoutKind.Explicit)]
public struct CacheSafeULong {
    public const int maxSize = 7;
    
    [FieldOffset(0)] private int size;
    [FieldOffset(4)] private bool useFallback;
    [FieldOffset(8)] private InlinedRawList<ulong> fallbackList;
    
    [FieldOffset(8)] private ulong e0;
    [FieldOffset(16)] private ulong e1;
    [FieldOffset(24)] private ulong e2;
    [FieldOffset(32)] private ulong e3;
    [FieldOffset(40)] private ulong e4;
    [FieldOffset(48)] private ulong e5;
    [FieldOffset(56)] private ulong e6;
    
    [MethodImpl(inline)] public Span<ulong> GetSpan() {
        if (useFallback) {
            return fallbackList.data.AsSpan(0, size);
        }
        
        unsafe {
            fixed (ulong* arr = &e0) {
                return new Span<ulong>(arr, size);
            }
        }
    }

    [MethodImpl(inline)] public void AddOnce(ulong item) {
        if (!useFallback && size == maxSize) {
            unsafe {
                fixed (ulong* arr = &e0) {
                    for (var i = 0; i < size; i++) {
                        if (arr[i].Equals(item)) {
                            return;
                        }
                    }
                }
            }
            
            Span<ulong> values = stackalloc ulong[maxSize] { e0, e1, e2, e3, e4, e5, e6 };
            fallbackList = new InlinedRawList<ulong>(maxSize * 4);
            fallbackList.AddRange(values);
            useFallback = true;
        }
        
        if (useFallback) {
            for (var i = 0; i < fallbackList.size; i++) {
                if (fallbackList.data[i].Equals(item)) {
                    return;
                }
            }
            
            fallbackList.Add(item);
            return;
        }

        unsafe {
            fixed (ulong* arr = &e0) {
                for (var i = 0; i < size; i++) {
                    if (arr[i].Equals(item)) {
                        return;
                    }
                }
                
                var idx = size;
                size++;
                arr[idx] = item;
            }
        }
    }
    
    [MethodImpl(inline)] public void Remove(ulong item) {
        if (useFallback) {
            for (var i = 0; i < fallbackList.size; i++) {
                if (fallbackList.data[i].Equals(item)) {
                    fallbackList.RemoveAtSwapBackFast(i);
                    return;
                }
            }
        }
        
        unsafe {
            fixed (ulong* arr = &e0) {
                for (var i = 0; i < size; i++) {
                    if (arr[i].Equals(item)) {
                        arr[i] = arr[size - 1];
                        size--;
                        return;
                    }
                }
            }
        }
    }
}
}