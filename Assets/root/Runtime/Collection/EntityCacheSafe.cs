using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using static Globals;

namespace SharedUtils {

[Serializable]
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[StructLayout(LayoutKind.Explicit)]
public struct EntityCacheSafe {
    public const int maxSize = 7;
    
    [FieldOffset(0)] private int size;
    [FieldOffset(4)] private bool useFallback;
    [FieldOffset(8)] private InlinedRawList<Entity> fallbackList;
    
    [FieldOffset(8)] private Entity e0;
    [FieldOffset(16)] private Entity e1;
    [FieldOffset(24)] private Entity e2;
    [FieldOffset(32)] private Entity e3;
    [FieldOffset(40)] private Entity e4;
    [FieldOffset(48)] private Entity e5;
    [FieldOffset(56)] private Entity e6;
    
    [MethodImpl(inline)]
    public Span<Entity> GetSpan() {
        if (useFallback) {
            return fallbackList.data.AsSpan(0, size);
        }
        
        unsafe {
            fixed (Entity* arr = &e0) {
                return new Span<Entity>(arr, size);
            }
        }
    }

    [MethodImpl(inline)] public void AddOnce(Entity item) {
        if (!useFallback && size == maxSize) {
            unsafe {
                fixed (Entity* arr = &e0) {
                    for (var i = 0; i < size; i++) {
                        if (arr[i].Equals(item)) {
                            return;
                        }
                    }
                }
            }
            
            Span<Entity> values = stackalloc Entity[maxSize] { e0, e1, e2, e3, e4, e5, e6 };
            fallbackList = new InlinedRawList<Entity>(maxSize * 4);
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
            fixed (Entity* arr = &e0) {
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
    
    [MethodImpl(inline)] public void Remove(Entity item) {
        if (useFallback) {
            for (var i = 0; i < fallbackList.size; i++) {
                if (fallbackList.data[i].Equals(item)) {
                    fallbackList.RemoveAtSwapBackFast(i);
                    return;
                }
            }
        }
        
        unsafe {
            fixed (Entity* arr = &e0) {
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