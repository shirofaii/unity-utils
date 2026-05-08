using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;
using static System.Runtime.CompilerServices.MethodImplOptions;

namespace SharedUtils {
public static class UnityObjectExtension {
    
#if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [MethodImpl(AggressiveInlining)] public static void SetActive(this Component comp, bool active) {
        if (comp == null) return;

        comp.gameObject.SetActive(active);
    }
    
#if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [MethodImpl(AggressiveInlining)] public static void DestroyAll<T>(this List<T> list) where T : MonoBehaviour {
        if ((list == null) || (list.Count == 0)) return;

        for (var i = 0; i < list.Count; i++) {
            if (list[i] != null) {
                Object.Destroy(list[i].gameObject);
            }
        }

        list.Clear();
    }

#if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [MethodImpl(AggressiveInlining)] public static void DestroyAll(this List<GameObject> list) {
        if ((list == null) || (list.Count == 0)) return;

        for (var i = 0; i < list.Count; i++) {
            if (list[i] != null) {
                Object.Destroy(list[i]);
            }
        }

        list.Clear();
    }

#if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [MethodImpl(AggressiveInlining)] public static void SetActiveAll<T>(this List<T> list, bool value = true) where T : MonoBehaviour {
        if ((list == null) || (list.Count == 0)) return;

        for (var i = 0; i < list.Count; i++) {
            list[i].gameObject.SetActive(value);
        }
    }
}
}