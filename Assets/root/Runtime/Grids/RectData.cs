#define RECT_DATA_TILED

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static Globals;

// Tiled by 4x4 tile size for cache friendliness
[Serializable] public struct RectData<T> {
    public readonly Vector2Int size;
    public readonly T[] data;

    public RectData(Vector2Int size) {
#if RECT_DATA_TILED
        Debug.Assert(size.x % 4 == 0 && size.y % 4 == 0);
#endif
        
        this.size = size;
        data = new T[size.x * size.y];
    }
    
    public RectData(T[] src, Vector2Int size) : this(size) {
        Fill(src);
    }

    public ref T this[int x, int y] {
#if RECT_DATA_TILED
        [MethodImpl(inline)]
        get {
            var widthInTiles = size.x >> 2;

            var tileX = x >> 2;
            var tileY = y >> 2;
            var inTileX = x % 4;
            var inTileY = y % 4;

            return ref data[(tileY * widthInTiles + tileX) * 16 + (inTileY * 4) + inTileX];
        }
#else
        [MethodImpl(inline)] get => ref data[x * size.y + y];
#endif
    }

    public ref T this[Vector2Int pos] {
#if RECT_DATA_TILED
        [MethodImpl(inline)]
        get {
            var widthInTiles = size.x >> 2;

            var tileX = pos.x >> 2;
            var tileY = pos.y >> 2;
            var inTileX = pos.x % 4;
            var inTileY = pos.y % 4;

            return ref data[((tileY * widthInTiles + tileX) << 4) + (inTileY >> 2) + inTileX];
        }
#else
        [MethodImpl(inline)] get => ref data[pos.x * size.y + pos.y];
#endif
    }

    public T this[RectInt rect] {
        [MethodImpl(inline)] set {
            foreach (ref var x in DataInRect(rect)) {
                x = value;
            }
        }
    }

    public void Fill(Span<T> array) {
        for (var i = 0; i < size.x; i++) {
            for (var j = 0; j < size.y; j++) {
                data[i + j * size.x] = array[j];
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

