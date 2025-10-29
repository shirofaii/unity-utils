using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Scellecs.Morpeh;
using SharedUtils;
using UnityEngine;
using static Globals;

public readonly struct EntityRectMap {
    public readonly RectData<EntityCacheSafe> data;

    public EntityRectMap(Vector2Int size) {
        data = new RectData<EntityCacheSafe>(size);
    }
    
    public Span<Entity> this[int x, int y] {
        [MethodImpl(inline)] get => data[x, y].GetSpan();
    }

    public Span<Entity> this[Vector2Int pos] {
        [MethodImpl(inline)] get => data[pos.x, pos.y].GetSpan();
    }
}