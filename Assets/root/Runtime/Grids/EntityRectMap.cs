using System;
using System.Runtime.CompilerServices;
using SharedUtils;
using UnityEngine;
using static System.Runtime.CompilerServices.MethodImplOptions;

public readonly struct RectMapULong {
    public readonly RectData<CacheSafeULong> data;

    public RectMapULong(Vector2Int size) {
        data = new RectData<CacheSafeULong>(size);
    }
    
    public Span<ulong> this[int x, int y] {
        [MethodImpl(AggressiveInlining)] get => data[x, y].GetSpan();
    }

    public Span<ulong> this[Vector2Int pos] {
        [MethodImpl(AggressiveInlining)] get => data[pos.x, pos.y].GetSpan();
    }
}