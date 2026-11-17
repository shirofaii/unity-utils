using System;
using System.Runtime.CompilerServices;
using SharedUtils;
using static System.Runtime.CompilerServices.MethodImplOptions;

public readonly struct RectMapULong {
    private readonly RectData<CacheSafeULong> cells;

    public RectMapULong(int width, int height) {
        cells = new RectData<CacheSafeULong>(width, height);
    }

    public (int x, int y) size {
        [MethodImpl(AggressiveInlining)] get => cells.size;
    }

    [MethodImpl(AggressiveInlining)] public bool Contains(int x, int y) => cells.Contains(x, y);

    public Span<ulong> this[int x, int y] {
        [MethodImpl(AggressiveInlining)] get => cells[x, y].GetSpan();
    }

    [MethodImpl(AggressiveInlining)] public void Add(int x, int y, ulong item) {
        if (!cells.Contains(x, y)) throw new ArgumentOutOfRangeException(nameof(x), $"({x}, {y}) is outside the {cells.width}x{cells.height} map");
        cells[x, y].AddOnce(item);
    }

    [MethodImpl(AggressiveInlining)] public void Remove(int x, int y, ulong item) {
        if (cells.Contains(x, y)) cells[x, y].Remove(item);
    }

    [MethodImpl(AggressiveInlining)] public void Clear() {
        foreach (ref var cell in cells) cell.Clear();
    }
}
