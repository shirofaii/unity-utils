using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using static System.Runtime.CompilerServices.MethodImplOptions;

[Serializable] public readonly struct RectData<T> {
    public readonly int width;
    public readonly int height;
    public readonly T[] data;

    public RectData(int width, int height) {
        this.width = width;
        this.height = height;
        data = new T[width * height];
    }

    public RectData(T[] src, int width, int height) {
        this.width = width;
        this.height = height;
        data = src;
    }

    public (int x, int y) size {
        [MethodImpl(AggressiveInlining)] get => (width, height);
    }

    public ref T this[int x, int y] {
        [MethodImpl(AggressiveInlining)] get => ref data[x * height + y];
    }

    public void Fill(int x, int y, int w, int h, T value) {
        var minX = Math.Max(x, 0);
        var minY = Math.Max(y, 0);
        var maxX = Math.Min(x + w, width);
        var maxY = Math.Min(y + h, height);
        if (maxY <= minY) return;
        for (var column = minX; column < maxX; column++) Array.Fill(data, value, column * height + minY, maxY - minY);
    }

    [MethodImpl(AggressiveInlining)] public bool Contains(int x, int y) => x >= 0 && y >= 0 && x < width && y < height;

    public bool IsNotEmpty(int x, int y, int w, int h) {
        foreach (ref readonly var cell in DataInRect(x, y, w, h)) {
            if (!EqualityComparer<T>.Default.Equals(cell, default)) return true;
        }

        return false;
    }
    [MethodImpl(AggressiveInlining)] public bool IsEmpty(int x, int y, int w, int h) => !IsNotEmpty(x, y, w, h);
    [MethodImpl(AggressiveInlining)] public bool IsNotEmpty() => IsNotEmpty(0, 0, width, height);
    [MethodImpl(AggressiveInlining)] public bool IsEmpty() => !IsNotEmpty();

    [MethodImpl(AggressiveInlining)] public void Clear() => Array.Clear(data, 0, data.Length);

    [MethodImpl(AggressiveInlining)] public DataEnumerator GetEnumerator() => DataInRect(0, 0, width, height);
    [MethodImpl(AggressiveInlining)] public IndexEnumerator WithIndexes() => IndexesInRect(0, 0, width, height);

    [MethodImpl(AggressiveInlining)] public DataEnumerator DataInRect(int x, int y, int w, int h) => new(this, x, y, w, h);
    [MethodImpl(AggressiveInlining)] public IndexEnumerator IndexesInRect(int x, int y, int w, int h) => new(this, x, y, w, h);

    private struct RectCursor {
        private readonly int minY;
        private readonly int maxX;
        private readonly int maxY;
        public int x;
        public int y;

        [MethodImpl(AggressiveInlining)] public RectCursor(RectData<T> host, int x, int y, int w, int h) {
            var minX = Math.Max(x, 0);
            minY = Math.Max(y, 0);
            maxX = Math.Min(x + w, host.width);
            maxY = Math.Min(y + h, host.height);
            var any = maxX > minX && maxY > minY;
            this.x = any ? minX : maxX;
            this.y = any ? minY - 1 : maxY;
        }

        [MethodImpl(AggressiveInlining)] public bool MoveNext() {
            y++;
            if (y < maxY) return true;

            y = minY;
            x++;

            return y < maxY && x < maxX;
        }
    }

    public struct DataEnumerator {
        private RectCursor cursor;
        private readonly RectData<T> rectData;

        [MethodImpl(AggressiveInlining)] public DataEnumerator(RectData<T> rectData, int x, int y, int w, int h) {
            cursor = new RectCursor(rectData, x, y, w, h);
            this.rectData = rectData;
        }

        [MethodImpl(AggressiveInlining)] public DataEnumerator GetEnumerator() => this;

        [MethodImpl(AggressiveInlining)] public bool MoveNext() => cursor.MoveNext();

        public ref T Current {
            [MethodImpl(AggressiveInlining)] get => ref rectData[cursor.x, cursor.y];
        }
    }

    public struct IndexEnumerator {
        private RectCursor cursor;
        private readonly RectData<T> rectData;

        [MethodImpl(AggressiveInlining)] public IndexEnumerator(RectData<T> rectData, int x, int y, int w, int h) {
            cursor = new RectCursor(rectData, x, y, w, h);
            this.rectData = rectData;
        }

        [MethodImpl(AggressiveInlining)] public IndexEnumerator GetEnumerator() => this;

        [MethodImpl(AggressiveInlining)] public bool MoveNext() => cursor.MoveNext();

        public ((int x, int y), T) Current {
            [MethodImpl(AggressiveInlining)] get => ((cursor.x, cursor.y), rectData[cursor.x, cursor.y]);
        }
    }
}
