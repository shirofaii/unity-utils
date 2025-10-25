using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using static Globals;

namespace SharedUtils {

[Serializable]
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[StructLayout(LayoutKind.Sequential)]
public struct Cache16Unsafe<T> where T : unmanaged {
    public int size;
    private T e0;
    private T e1;
    private T e2;
    private T e3;
    private T e4;
    private T e5;
    private T e6;
    private T e7;
    private T e8;
    private T e9;
    private T e10;
    private T e11;
    private T e12;
    private T e13;
    private T e14;
    private T e15;
    
    public ref T this[int index] {
        [MethodImpl(inline)] get {
            if (index >= size || index < 0) { 
                throw new IndexOutOfRangeException();
            }
            
            unsafe {
                fixed (T* arr = &e0) {
                    return ref arr[index];
                }
            }
        }
    }
    
    [MethodImpl(inline)] public void AddOnce(T item) {
        if (size >= 16) {
            throw new IndexOutOfRangeException();
        }

        unsafe {
            fixed (T* arr = &e0) {
                for (var i = 0; i < size; i++) {
                    if (arr[i].Equals(item)) {
                        return;
                    }
                }
            }
        }
        
        var idx = size;
        size++;
        this[idx] = item;
    }
    
    [MethodImpl(inline)] public void Remove(Entity item) {
        unsafe {
            fixed (T* arr = &e0) {
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