using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static Globals;

// Tiled by 4x4 tile size for cache friendliness
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
        [MethodImpl(inline)] get => ref data[x * size.y + y];
    }

    public ref T this[Vector2Int pos] {
        [MethodImpl(inline)] get => ref data[pos.x * size.y + pos.y];
    }

    public T this[RectInt rect] {
        [MethodImpl(inline)] set {
            foreach (ref var x in DataInRect(rect)) {
                x = value;
            }
        }
    }
    
    [MethodImpl(inline)] public bool IsNotEmpty(RectInt rect) {
        foreach (ref readonly var x in DataInRect(rect)) {
            if (EqualityComparer<T>.Default.Equals(x, default)) return true;
        }
        
        return false;
    }
    [MethodImpl(inline)] public bool IsEmpty(RectInt rect) => !IsNotEmpty(rect);
    [MethodImpl(inline)] public bool IsNotEmpty() => !IsNotEmpty(area);
    [MethodImpl(inline)] public bool IsEmpty() => !IsNotEmpty();
    
    public RectInt area {
        [MethodImpl(inline)] get => new(0, 0, size.x - 1, size.y - 1);
    }
    
    [MethodImpl(inline)] public DataEnumerator GetEnumerator() => DataInRect(area);
    [MethodImpl(inline)] public IndexEnumerator WithIndexes() => new(this, area.min, area.max);
    
    [MethodImpl(inline)] public DataEnumerator DataInRect(RectInt rect) => new(this, rect.min, rect.max);

    public struct DataEnumerator {
        private readonly Vector2Int min;
        private readonly Vector2Int max;
        private Vector2Int current;
        private RectData<T> rectData;

        [MethodImpl(inline)] public DataEnumerator(RectData<T> rectData, Vector2Int min, Vector2Int max) {
            this.min = min;
            this.max = max;
            current = min;
            this.rectData = rectData;
        }

        [MethodImpl(inline)] public DataEnumerator GetEnumerator() => this;

        [MethodImpl(inline)] public bool MoveNext() {
            current.x++;
            if (current.x <= max.x) return true;

            current.x = min.x;
            current.y++;
            
            return current.y <= max.y;
        }

        public ref T Current {
            [MethodImpl(inline)] get => ref rectData[current];
        }
    }
    
    public struct IndexEnumerator {
        private readonly Vector2Int min;
        private readonly Vector2Int max;
        private Vector2Int current;
        private RectData<T> rectData;

        [MethodImpl(inline)] public IndexEnumerator(RectData<T> rectData, Vector2Int min, Vector2Int max) {
            this.min = min;
            this.max = max;
            current = min;
            this.rectData = rectData;
        }

        [MethodImpl(inline)] public IndexEnumerator GetEnumerator() => this;

        [MethodImpl(inline)] public bool MoveNext() {
            current.x++;
            if (current.x <= max.x) return true;

            current.x = min.x;
            current.y++;
            
            return current.y <= max.y;
        }

        public (Vector2Int, T) Current {
            [MethodImpl(inline)] get => (current, rectData[current]);
        }
    }
}

