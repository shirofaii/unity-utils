using Scellecs.Morpeh.Collections;
using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using static Globals;

namespace SharedUtils {
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
public class MemoryArena<T> where T : struct {
    private int capacity;
    private int size;
    private T[] array;

    private bool isAddMode;
    private int startMarker;
    
    public MemoryArena(int capacity) {
        Debug.Assert(capacity > 0);
        
        this.capacity = capacity;
        array = new T[capacity];
    }

    public void Clear() {
        size = 0;
    }

    [MethodImpl(inline)] public MemoryArena<T> Start() {
        Debug.Assert(!isAddMode);
        
        isAddMode = true;
        startMarker = size;
        return this;
    }

    [MethodImpl(inline)] public MemoryArena<T> Add(T x) {
        Debug.Assert(isAddMode);
        
        CheckForAvailableSpace(1);
        array[size] = x;
        size++;
        
        return this;
    }

    [MethodImpl(inline)] public MemoryArena<T> Add(T[] x) {
        Debug.Assert(isAddMode);

        var len = x.Length;
        CheckForAvailableSpace(len);
        Array.Copy(x, 0, array, startMarker, len);
        size += len;
        
        return this;
    }
    
    [MethodImpl(inline)] public Memory<T> Finish() {
        Debug.Assert(isAddMode);
        
        isAddMode = false;
        return new Memory<T>(array, startMarker, size - startMarker);
    }
    
    [MethodImpl(inline)] public Memory<T> Alloc(int amount) {
        Debug.Assert(!isAddMode);
        
        CheckForAvailableSpace(amount);
        size += amount;
        var mem = new Memory<T>(array, startMarker, amount);
        startMarker += amount;
        return mem;
    }
    
    [MethodImpl(inline)] private void CheckForAvailableSpace(int i) {
        if(size + i <= capacity) return;
        
        Debug.LogWarning($"Adjust buffer size, {capacity} is too small");
        capacity *= 2;
        
        var newArray = new T[capacity];
        if(isAddMode) {
            size = array.Length - startMarker;
            Array.Copy(array, startMarker, newArray, 0, array.Length - startMarker);
        } else {
            size = 0;
        }
        
        startMarker = 0;
        array = newArray;
    }
}
}