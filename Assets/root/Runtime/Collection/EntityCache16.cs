using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Scellecs.Morpeh;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using static Globals;

namespace SharedUtils {

[Serializable]
[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[StructLayout(LayoutKind.Sequential)]
public struct EntityCache16 {
    public int size;
    public RawList<Entity> list;
    
    private Entity e0;
    private Entity e1;
    private Entity e2;
    private Entity e3;
    private Entity e4;
    private Entity e5;
    private Entity e6;
    private Entity e7;
    private Entity e8;
    private Entity e9;
    private Entity e10;
    private Entity e11;
    private Entity e12;
    private Entity e13;
    private Entity e14;
    private Entity e15;
    
    public ref Entity this[int index] {
        [MethodImpl(inline)] get {
            if (index >= size || index < 0) {
                return ref list[index];
            }
            
            unsafe {
                fixed (Entity* arr = &e0) {
                    return ref arr[index];
                }
            }
        }
    }
    
    [MethodImpl(inline)] public void Add(Entity item) {
#if DEVELOPMENT_BUILD || UNITY_EDITOR
        Debug.Assert(list == null || !list.data.Contains(item));
        
        unsafe {
            fixed (Entity* arr = &e0) {
                for (var i = 0; i < size; i++) {
                    if (arr[i] == item) {
                        throw new Exception("Trying add duplicate entity");
                    }
                }
            }
        }
#endif
        
        if (size == 16 && list == null) {
            list = new RawList<Entity>(32);
            for (var i = 0; i < size; i++) {
                list.Add(this[i]);
            }
        }

        if (list != null) {
            list.Add(item);
            return;
        }
        
        this[size] = item;
        size++;
    }

    [MethodImpl(inline)] public void Remove(Entity item) {
        if (list != null) {
            list.Remove(item);
            return;
        }
        
        unsafe {
            fixed (Entity* arr = &e0) {
                for (var i = 0; i < size; i++) {
                    if (arr[i] == item) {
                        arr[i] = arr[size - 1];
                        size--;
                        return;
                    }
                }
            }
        }
    }
}
}