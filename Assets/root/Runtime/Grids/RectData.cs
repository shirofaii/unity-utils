using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static System.Runtime.CompilerServices.MethodImplOptions;

[Serializable] public readonly struct RectData<T> {
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

    [MethodImpl(AggressiveInlining)] public bool Contains(int x, int y) => x >= 0 && y >= 0 && x < size.x && y < size.y;
    [MethodImpl(AggressiveInlining)] public bool Contains(Vector2Int pos) => Contains(pos.x, pos.y);

    [MethodImpl(AggressiveInlining)] public bool IsNotEmpty(RectInt rect) {
        foreach (ref readonly var x in DataInRect(rect)) {
            if (!EqualityComparer<T>.Default.Equals(x, default)) return true;
        }

        return false;
    }
    [MethodImpl(AggressiveInlining)] public bool IsEmpty(RectInt rect) => !IsNotEmpty(rect);
    [MethodImpl(AggressiveInlining)] public bool IsNotEmpty() => IsNotEmpty(area);
    [MethodImpl(AggressiveInlining)] public bool IsEmpty() => !IsNotEmpty();

    [MethodImpl(AggressiveInlining)] public void Clear() => Array.Clear(data, 0, data.Length);

    public RectInt area {
        [MethodImpl(AggressiveInlining)] get => new(0, 0, size.x, size.y);
    }

    [MethodImpl(AggressiveInlining)] public DataEnumerator GetEnumerator() => DataInRect(area);
    [MethodImpl(AggressiveInlining)] public IndexEnumerator WithIndexes() => IndexesInRect(area);

    [MethodImpl(AggressiveInlining)] public DataEnumerator DataInRect(RectInt rect) => new(this, rect);
    [MethodImpl(AggressiveInlining)] public IndexEnumerator IndexesInRect(RectInt rect) => new(this, rect);

    private struct RectCursor {
        private readonly Vector2Int min;
        private readonly Vector2Int max;
        public Vector2Int current;

        [MethodImpl(AggressiveInlining)] public RectCursor(RectData<T> host, RectInt rect) {
            min = Vector2Int.Max(rect.min, Vector2Int.zero);
            max = Vector2Int.Min(rect.max, host.size);
            current = max.x > min.x && max.y > min.y ? new Vector2Int(min.x - 1, min.y) : max;
        }

        [MethodImpl(AggressiveInlining)] public bool MoveNext() {
            current.x++;
            if (current.x < max.x) return true;

            current.x = min.x;
            current.y++;

            return current.x < max.x && current.y < max.y;
        }
    }

    public struct DataEnumerator {
        private RectCursor cursor;
        private readonly RectData<T> rectData;

        [MethodImpl(AggressiveInlining)] public DataEnumerator(RectData<T> rectData, RectInt rect) {
            cursor = new RectCursor(rectData, rect);
            this.rectData = rectData;
        }

        [MethodImpl(AggressiveInlining)] public DataEnumerator GetEnumerator() => this;

        [MethodImpl(AggressiveInlining)] public bool MoveNext() => cursor.MoveNext();

        public ref T Current {
            [MethodImpl(AggressiveInlining)] get => ref rectData[cursor.current];
        }
    }

    public struct IndexEnumerator {
        private RectCursor cursor;
        private readonly RectData<T> rectData;

        [MethodImpl(AggressiveInlining)] public IndexEnumerator(RectData<T> rectData, RectInt rect) {
            cursor = new RectCursor(rectData, rect);
            this.rectData = rectData;
        }

        [MethodImpl(AggressiveInlining)] public IndexEnumerator GetEnumerator() => this;

        [MethodImpl(AggressiveInlining)] public bool MoveNext() => cursor.MoveNext();

        public (Vector2Int, T) Current {
            [MethodImpl(AggressiveInlining)] get => (cursor.current, rectData[cursor.current]);
        }
    }
}
