using System;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace SharedUtils {
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
public class MemoryArena<T> where T : struct {
    private int capacity;
    private int size;
    private T[] array;

    private bool isAddMode;
    private int fromIdx;
    
    public MemoryArena(int capacity) {
        Debug.Assert(capacity > 0);
        
        this.capacity = capacity;
        array = new T[capacity];
    }

    public void Clear() {
        size = 0;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public MemoryArena<T> Start() {
        Debug.Assert(!isAddMode);
        
        isAddMode = true;
        fromIdx = size;
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public MemoryArena<T> Add(T x) {
        Debug.Assert(isAddMode);
        
        CheckForAvailableSpace(1);
        array[size] = x;
        size++;
        
        return this;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public MemoryArena<T> Add(T[] x) {
        Debug.Assert(isAddMode);

        var len = x.Length;
        CheckForAvailableSpace(len);
        Array.Copy(x, 0, array, fromIdx, len);
        size += len;
        
        return this;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Memory<T> Finish() {
        Debug.Assert(isAddMode);
        
        isAddMode = false;
        return new Memory<T>(array, fromIdx, size - fromIdx);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Memory<T> Alloc(int amount) {
        Debug.Assert(!isAddMode);
        
        CheckForAvailableSpace(amount);
        fromIdx = size;
        size += amount;
        return new Memory<T>(array, fromIdx, amount);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void CheckForAvailableSpace(int i) {
        if(size + i <= capacity) return;
        
        capacity *= 2;
        Array.Resize(ref array, capacity);
    }
}
}