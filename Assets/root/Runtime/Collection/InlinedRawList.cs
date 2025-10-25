using Scellecs.Morpeh.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.IL2CPP.CompilerServices;
using static Globals;

namespace SharedUtils {

[Serializable]
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public struct InlinedRawList<T> {
    public T[] data;
    
    public int size;
    public int capacity;

    public EqualityComparer<T> comparer;

    public ref T this[int index] {
        [MethodImpl(inline)] get {
            if(index < 0 || index >= size) throw new ArgumentOutOfRangeException(nameof(index));

            return ref data[index];
        }
    }

    [MethodImpl(inline)] public InlinedRawList(int capacity) {
        this.capacity = HashHelpers.GetCapacitySmall(capacity) + 1;
        data = new T[this.capacity];
        size = 0;

        comparer = EqualityComparer<T>.Default;
    }
    
    [MethodImpl(inline)] public InlinedRawList(RawList<T> other) {
        capacity = other.capacity;
        data = new T[capacity];
        size = other.size;
        comparer = other.comparer;
        Array.Copy(other.data, 0, data, 0, size);
    }
    
    [MethodImpl(inline)] public Enumerator GetEnumerator() {
        Enumerator e;

        e.data = data;
        e.index = -1;
        e.size = size;

        return e;
    }
    
    [MethodImpl(inline)] public void Grow(int newCapacity) {
        newCapacity = HashHelpers.GetCapacitySmall(newCapacity - 1) + 1;
        if(newCapacity <= capacity) return;
        
        capacity = newCapacity;
        ArrayHelpers.Grow(ref data, capacity);
    }

    [MethodImpl(inline)] public int Add(T value) {
        if (size == capacity) {
            capacity = HashHelpers.GetCapacitySmall(capacity) + 1;
            ArrayHelpers.Grow(ref data, capacity);
        }

        var index = size;
        size += 1;

        data[index] = value;
        return index;
    }

    [MethodImpl(inline)] public void AddRange(RawList<T> other) {
        if(other.size == 0) return;
        
        var newSize = size + other.size;

        if (newSize > capacity) {
            capacity = HashHelpers.GetCapacitySmall(newSize - 1) + 1;
            ArrayHelpers.Grow(ref data, capacity);
        }

        Array.Copy(other.data, 0, data, size, other.size);
        size += other.size;
    }

    [MethodImpl(inline)] public void AddRange(Span<T> other) {
        if(other.Length == 0) return;
        
        var newSize = size + other.Length;

        if (newSize > capacity) {
            capacity = HashHelpers.GetCapacitySmall(newSize - 1) + 1;
            ArrayHelpers.Grow(ref data, capacity);
        }

        other.CopyTo(data.AsSpan().Slice(size, size + other.Length));
        
        size += other.Length;
    }
    
    [MethodImpl(inline)] public int IndexOf(T value) {
        for (var i = 0; i < size; i++) {
            if (comparer.Equals(value, data[i])) {
                return i;
            }
        }
        return -1;
    }

    [MethodImpl(inline)] public bool Remove(T value) {
        var index = IndexOf(value);
        var shouldRemove = index >= 0;
        if (shouldRemove) {
            RemoveAtFast(index);
        }

        return shouldRemove;
    }

    [MethodImpl(inline)] public void RemoveAt(int index) {
        if (index < 0 || index >= size) throw new ArgumentOutOfRangeException(nameof(index));

        size -= 1;
        Array.Copy(data, index + 1, data, index, size - index);
        data[size] = default;
    }

    [MethodImpl(inline)] public void RemoveAtFast(int index) {
        size -= 1;
        Array.Copy(data, index + 1, data, index, size - index);
        data[size] = default;
    }

    [MethodImpl(inline)] public bool RemoveSwapBack(T value) {
        var index = IndexOf(value);
        var shouldRemove = index >= 0;
        if (shouldRemove) {
            RemoveAtSwapBackFast(index);
        }

        return shouldRemove;
    }

    [MethodImpl(inline)] public void RemoveAtSwapBack(int index) {
        if (index < 0 || index >= size) throw new ArgumentOutOfRangeException(nameof(index));

        var lastIndex = size - 1;
        data[index] = data[lastIndex];
        data[lastIndex] = default;
        size--;
    }

    [MethodImpl(inline)] public void RemoveAtSwapBackFast(int index) {
        var lastIndex = size - 1;
        data[index] = data[lastIndex];
        data[lastIndex] = default;
        size--;
    }

    [MethodImpl(inline)] public T Pop() {
        var lastIndex = size - 1;
        size--;
        return data[lastIndex];
    }
    
    [MethodImpl(inline)] public void RemoveRange(int index, int count) {
        if (index < 0 || index >= size) throw new ArgumentOutOfRangeException(nameof(index));
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), "Non-negative number required");

        var elementsToMove = size - (index + count);
        switch (elementsToMove) {
            case < 0:
                throw new ArgumentException("Offset and size were out of bounds for the array or count is greater than the number of elements from index to the end of the source collection.", nameof(count));
            case > 0:
                Array.Copy(data, index + count, data, index, elementsToMove);
            break;
        }

        size -= count;
    }

    [MethodImpl(inline)] public void Clear() {
        if (size <= 0) return;

        Array.Clear(data, 0, size);
        size = 0;
    }

    [MethodImpl(inline)] public void CopyTo(T[] array) {
        Array.Copy(data, 0, array, 0, size);
    }

    [MethodImpl(inline)] public T[] ToArray() {
        var shouldCopy = size > 0;
        var newArray = shouldCopy ? new T[size] : Array.Empty<T>();
        if (shouldCopy) {
            CopyTo(newArray);
        }
        return newArray;
    }

    [MethodImpl(inline)] public void Sort(IComparer<T> customComparer = null) {
        Array.Sort(data, 0, size, customComparer ?? Comparer<T>.Default);
    }

    [MethodImpl(inline)] public Span<T> AsSpan() {
        return data.AsSpan(0, size);
    }
    
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct Enumerator {
        public T[] data;
        public int index;
        public int size;

        
        [MethodImpl(inline)] public bool MoveNext() {
            return ++index < size;
        }

        public ref T Current {
            [MethodImpl(inline)] get => ref data[index];
        }
    }
}
}