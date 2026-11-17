using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif
using static System.Runtime.CompilerServices.MethodImplOptions;

// ReSharper disable InconsistentNaming

namespace SharedUtils {
public static class Rand {
    public static Pcg32 rng = new((ulong)DateTime.UtcNow.Ticks);
    public static void Seed(ulong seed) => rng = new Pcg32(seed);

    [MethodImpl(AggressiveInlining)] public static int Index<T>(List<T> list) => rng.Next(0, list.Count);
    [MethodImpl(AggressiveInlining)] public static int Index<T>(RawList<T> list) => rng.Next(0, list.size);
    [MethodImpl(AggressiveInlining)] public static int Index<T>(T[] list) => rng.Next(0, list.Length);
    
#if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [MethodImpl(AggressiveInlining)] public static T AtRandom<T>(this List<T> list, T ifEmpty = default) {
        if(list == null || list.Count == 0) return ifEmpty;
        return list[Index(list)];
    }

#if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [MethodImpl(AggressiveInlining)] public static T AtRandom<T>(this RawList<T> list, T ifEmpty = default) {
        if(list == null || list.size == 0) return ifEmpty;
        return list[Index(list)];
    }

#if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [MethodImpl(AggressiveInlining)] public static T AtRandom<T>(this T[] list, T ifEmpty = default) {
        if(list == null || list.Length == 0) return ifEmpty;
        return list[Index(list)];
    }
    
    [MethodImpl(AggressiveInlining)] public static float Float(float to) => (float)(rng.NextDouble() * to);

    public struct Dice {
        public enum Keep { Sum, Best, Worst }

        private readonly int sides;
        private int numberOfDices;
        private int additive;
        private Keep keep;

        public Dice(int sides = 6, int numberOfDices = 1, int additive = 0, Keep keep = Keep.Sum) {
            this.sides = sides;
            this.numberOfDices = numberOfDices;
            this.additive = additive;
            this.keep = keep;
        }

        public Dice best {
            [MethodImpl(AggressiveInlining)] get {
                var dice = this;
                dice.keep = Keep.Best;
                return dice;
            }
        }

        public Dice worst {
            [MethodImpl(AggressiveInlining)] get {
                var dice = this;
                dice.keep = Keep.Worst;
                return dice;
            }
        }

        [MethodImpl(AggressiveInlining)] public static implicit operator int(Dice d) => d.Roll();

        [MethodImpl(AggressiveInlining)] public static Dice operator *(int times, Dice dice) {
            dice.numberOfDices *= times;
            return dice;
        }

        [MethodImpl(AggressiveInlining)] public static Dice operator +(Dice dice, int add) {
            dice.additive += add;
            return dice;
        }

        [MethodImpl(AggressiveInlining)] private int OneDiceRoll() => rng.Next(1, sides + 1);
        
        public int Roll() {
            if (numberOfDices <= 0) return additive;
            switch (keep) {
                case Keep.Worst: {
                    var min = OneDiceRoll();
                    for (var i = 1; i < numberOfDices; i++) {
                        var roll = OneDiceRoll();
                        if (roll < min) {
                            min = roll;
                        }
                    }

                    return min + additive;
                }
                case Keep.Best: {
                    var max = OneDiceRoll();
                    for (var i = 1; i < numberOfDices; i++) {
                        var roll = OneDiceRoll();
                        if (roll > max) {
                            max = roll;
                        }
                    }

                    return max + additive;
                }
                case Keep.Sum: {
                    var sum = 0;
                    for(var i = 0; i < numberOfDices; i++) {
                        sum += OneDiceRoll();
                    }
                    return sum + additive;
                }
                default: throw new Exception();
            }
        }

        public void Times(Action<int> action) {
            var roll = Roll() - additive;
            for(var i = 0; i < roll; i++) {
                action.Invoke(i);
            }
        }

        public void Times(Action action) {
            var roll = Roll() - additive;
            for(var i = 0; i < roll; i++) {
                action.Invoke();
            }
        }
    }
    
#if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [MethodImpl(AggressiveInlining)] public static RandomStartEnumeratorForListOf<T> FromRandomStart<T>(this List<T> list) {
        return new RandomStartEnumeratorForListOf<T> {
            data = list,
            index = list.Count == 0 ? -1 : Index(list) - 1,
            remaining = list.Count,
        };
    }
    
#if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    public struct RandomStartEnumeratorForListOf<T> {
        public List<T> data;
        public int index;
        public int remaining;

        [MethodImpl(AggressiveInlining)] public RandomStartEnumeratorForListOf<T> GetEnumerator() => this;
        [MethodImpl(AggressiveInlining)] public bool MoveNext() {
            if(remaining <= 0) return false;
            remaining--;
            index++;
            if(index >= data.Count) index = 0;
            return true;
        }

        public T Current {
            [MethodImpl(AggressiveInlining)] get => data[index];
        }
    }
    
#if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [MethodImpl(AggressiveInlining)] public static RandomStartEnumeratorForRawListOf<T> FromRandomStart<T>(this RawList<T> list) {
        return new RandomStartEnumeratorForRawListOf<T> {
            data = list,
            index = list.size == 0 ? -1 : Index(list) - 1,
            remaining = list.size,
        };
    }
    
#if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    public struct RandomStartEnumeratorForRawListOf<T> {
        public RawList<T> data;
        public int index;
        public int remaining;

        [MethodImpl(AggressiveInlining)] public RandomStartEnumeratorForRawListOf<T> GetEnumerator() => this;
        [MethodImpl(AggressiveInlining)] public bool MoveNext() {
            if(remaining <= 0) return false;
            remaining--;
            index++;
            if(index >= data.size) index = 0;
            return true;
        }

        public ref T Current {
            [MethodImpl(AggressiveInlining)] get => ref data[index];
        }
    }
    
#if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    public static void Shuffle<T>(this T[] array) {
        for (var i = array.Length - 1; i > 0; i--) {
            var j = rng.Next(i + 1);
            (array[i], array[j]) = (array[j], array[i]);
        }
    }
    
#if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    public static void Shuffle<T>(this List<T> list) {
        for (var i = list.Count - 1; i > 0; i--) {
            var j = rng.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
    
#if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    public static void Shuffle<T>(this RawList<T> list) {
        for (var i = list.size - 1; i > 0; i--) {
            var j = rng.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}

[Serializable]
public struct Pcg32 {
    public ulong state;
    public ulong inc;

    public Pcg32(ulong seed, ulong stream = 0xda3e39cb94b95bdbUL) {
        state = 0;
        inc = (stream << 1) | 1;
        NextUInt();
        state += seed;
        NextUInt();
    }

    [MethodImpl(AggressiveInlining)] public uint NextUInt() {
        var old = state;
        state = old * 6364136223846793005UL + inc;
        var xorshifted = (uint)(((old >> 18) ^ old) >> 27);
        var rot = (int)(old >> 59);
        return (xorshifted >> rot) | (xorshifted << (-rot & 31));
    }

    public int Next(int bound) {
        if(bound <= 0) throw new ArgumentOutOfRangeException(nameof(bound), bound, "must be positive");
        var b = (uint)bound;
        var m = (ulong)NextUInt() * b;
        var low = (uint)m;
        if(low < b) {
            var threshold = (0u - b) % b;
            while(low < threshold) {
                m = (ulong)NextUInt() * b;
                low = (uint)m;
            }
        }
        return (int)(m >> 32);
    }

    [MethodImpl(AggressiveInlining)] public int Next(int min, int max) => min + Next(max - min);

    [MethodImpl(AggressiveInlining)] public double NextDouble() => NextUInt() * (1.0 / 4294967296.0);
}
}