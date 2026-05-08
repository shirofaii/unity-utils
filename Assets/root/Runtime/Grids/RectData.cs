using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static System.Runtime.CompilerServices.MethodImplOptions;

[Serializable] public struct RectData<T> {
    public readonly Vector2Int size;
    public readonly T[] data;

    public RectData(Vector2Int size) {
        this.size = size;
        data = new T[size.x * size.y];
    }
    
    public RectData(T[] src, Vector2Int size) {
        this.size = size;
        data = src;
    }

    public ref T this[int x, int y] {
        [MethodImpl(AggressiveInlining)] get => ref data[x * size.y + y];
    }

    public ref T this[Vector2Int pos] {
        [MethodImpl(AggressiveInlining)] get => ref data[pos.x * size.y + pos.y];
    }

    public T this[RectInt rect] {
        [MethodImpl(AggressiveInlining)] set {
            foreach (ref var x in DataInRect(rect)) {
                x = value;
            }
        }
    }
    
    [MethodImpl(AggressiveInlining)] public bool IsNotEmpty(RectInt rect) {
        foreach (ref readonly var x in DataInRect(rect)) {
            if (EqualityComparer<T>.Default.Equals(x, default)) return true;
        }
        
        return false;
    }
    [MethodImpl(AggressiveInlining)] public bool IsEmpty(RectInt rect) => !IsNotEmpty(rect);
    [MethodImpl(AggressiveInlining)] public bool IsNotEmpty() => !IsNotEmpty(area);
    [MethodImpl(AggressiveInlining)] public bool IsEmpty() => !IsNotEmpty();
    
    public RectInt area {
        [MethodImpl(AggressiveInlining)] get => new(0, 0, size.x - 1, size.y - 1);
    }
    
    [MethodImpl(AggressiveInlining)] public DataEnumerator GetEnumerator() => DataInRect(area);
    [MethodImpl(AggressiveInlining)] public IndexEnumerator WithIndexes() => new(this, area.min, area.max);
    
    [MethodImpl(AggressiveInlining)] public DataEnumerator DataInRect(RectInt rect) => new(this, rect.min, rect.max);

    public struct DataEnumerator {
        private readonly Vector2Int min;
        private readonly Vector2Int max;
        private Vector2Int current;
        private RectData<T> rectData;

        [MethodImpl(AggressiveInlining)] public DataEnumerator(RectData<T> rectData, Vector2Int min, Vector2Int max) {
            this.min = min;
            this.max = max;
            current = min;
            this.rectData = rectData;
        }

        [MethodImpl(AggressiveInlining)] public DataEnumerator GetEnumerator() => this;

        [MethodImpl(AggressiveInlining)] public bool MoveNext() {
            current.x++;
            if (current.x <= max.x) return true;

            current.x = min.x;
            current.y++;
            
            return current.y <= max.y;
        }

        public ref T Current {
            [MethodImpl(AggressiveInlining)] get => ref rectData[current];
        }
    }
    
    public struct IndexEnumerator {
        private readonly Vector2Int min;
        private readonly Vector2Int max;
        private Vector2Int current;
        private RectData<T> rectData;

        [MethodImpl(AggressiveInlining)] public IndexEnumerator(RectData<T> rectData, Vector2Int min, Vector2Int max) {
            this.min = min;
            this.max = max;
            current = min;
            this.rectData = rectData;
        }

        [MethodImpl(AggressiveInlining)] public IndexEnumerator GetEnumerator() => this;

        [MethodImpl(AggressiveInlining)] public bool MoveNext() {
            current.x++;
            if (current.x <= max.x) return true;

            current.x = min.x;
            current.y++;
            
            return current.y <= max.y;
        }

        public (Vector2Int, T) Current {
            [MethodImpl(AggressiveInlining)] get => (current, rectData[current]);
        }
    }
}

