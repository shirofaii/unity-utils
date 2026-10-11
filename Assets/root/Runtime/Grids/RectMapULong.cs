using System;
using System.Runtime.CompilerServices;
using SharedUtils;
using UnityEngine;
using static System.Runtime.CompilerServices.MethodImplOptions;

public readonly struct RectMapULong {
    private readonly RectData<CacheSafeULong> cells;

    public RectMapULong(Vector2Int size) {
        cells = new RectData<CacheSafeULong>(size);
    }

    public Vector2Int size {
        [MethodImpl(AggressiveInlining)] get => cells.size;
    }

    [MethodImpl(AggressiveInlining)] public bool Contains(int x, int y) => cells.Contains(x, y);
    [MethodImpl(AggressiveInlining)] public bool Contains(Vector2Int pos) => cells.Contains(pos);

    public Span<ulong> this[int x, int y] {
        [MethodImpl(AggressiveInlining)] get => cells[x, y].GetSpan();
    }

    public Span<ulong> this[Vector2Int pos] {
        [MethodImpl(AggressiveInlining)] get => cells[pos].GetSpan();
    }

    [MethodImpl(AggressiveInlining)] public void Add(int x, int y, ulong item) {
        if (!cells.Contains(x, y)) throw new ArgumentOutOfRangeException(nameof(x), $"({x}, {y}) is outside the {cells.size.x}x{cells.size.y} map");
        cells[x, y].AddOnce(item);
    }

    [MethodImpl(AggressiveInlining)] public void Remove(int x, int y, ulong item) {
        if (cells.Contains(x, y)) cells[x, y].Remove(item);
    }

    [MethodImpl(AggressiveInlining)] public void Clear() {
        foreach (ref var cell in cells) cell.Clear();
    }
}
