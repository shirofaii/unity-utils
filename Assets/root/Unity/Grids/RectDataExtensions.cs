using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using static System.Runtime.CompilerServices.MethodImplOptions;

public static class RectDataExtensions {
    [MethodImpl(AggressiveInlining)] public static Vector2Int Size<T>(this RectData<T> rect) => new(rect.width, rect.height);
    [MethodImpl(AggressiveInlining)] public static RectInt Area<T>(this RectData<T> rect) => new(0, 0, rect.width, rect.height);
    [MethodImpl(AggressiveInlining)] public static ref T At<T>(this RectData<T> rect, Vector2Int pos) => ref rect[pos.x, pos.y];
    [MethodImpl(AggressiveInlining)] public static bool Contains<T>(this RectData<T> rect, Vector2Int pos) => rect.Contains(pos.x, pos.y);
    [MethodImpl(AggressiveInlining)] public static void Fill<T>(this RectData<T> rect, RectInt area, T value) => rect.Fill(area.x, area.y, area.width, area.height, value);
    [MethodImpl(AggressiveInlining)] public static bool IsNotEmpty<T>(this RectData<T> rect, RectInt area) => rect.IsNotEmpty(area.x, area.y, area.width, area.height);
    [MethodImpl(AggressiveInlining)] public static bool IsEmpty<T>(this RectData<T> rect, RectInt area) => rect.IsEmpty(area.x, area.y, area.width, area.height);
    [MethodImpl(AggressiveInlining)] public static RectData<T>.DataEnumerator DataInRect<T>(this RectData<T> rect, RectInt area) => rect.DataInRect(area.x, area.y, area.width, area.height);
    [MethodImpl(AggressiveInlining)] public static RectData<T>.IndexEnumerator IndexesInRect<T>(this RectData<T> rect, RectInt area) => rect.IndexesInRect(area.x, area.y, area.width, area.height);

    [MethodImpl(AggressiveInlining)] public static Vector2Int Size(this RectMapULong map) => new(map.size.x, map.size.y);
    [MethodImpl(AggressiveInlining)] public static Span<ulong> At(this RectMapULong map, Vector2Int pos) => map[pos.x, pos.y];
    [MethodImpl(AggressiveInlining)] public static bool Contains(this RectMapULong map, Vector2Int pos) => map.Contains(pos.x, pos.y);
}
