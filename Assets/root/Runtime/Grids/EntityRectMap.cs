using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Scellecs.Morpeh;
using SharedUtils;
using UnityEngine;
using static Globals;

// Tiled by 4x4 tile size for cache friendliness
[Serializable] public struct EntityRectMap {
    public readonly RectData<RawList<Entity>> data;

    public EntityRectMap(Vector2Int size) {
        data = new RectData<RawList<Entity>>(size);
        foreach (ref var rawList in data) {
            rawList = new RawList<Entity>(8);
        }
    }
    
    public RawList<Entity> this[int x, int y] {
        [MethodImpl(inline)] get => data[x, y];
    }

    public RawList<Entity> this[Vector2Int pos] {
        [MethodImpl(inline)] get => data[pos];
    }
}

