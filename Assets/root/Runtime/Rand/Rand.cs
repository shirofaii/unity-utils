using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RandN;
using RandN.Distributions;
using static System.Runtime.CompilerServices.MethodImplOptions;

// ReSharper disable InconsistentNaming

namespace SharedUtils {
public static class Rand {
    private static readonly StandardRng rand = StandardRng.Create();
    
    [MethodImpl(AggressiveInlining)] public static int Index<T>(List<T> list) => Uniform.NewInclusive(0, list.Count - 1).Sample(rand);
    [MethodImpl(AggressiveInlining)] public static int Index<T>(RawList<T> list) => Uniform.NewInclusive(0, list.size - 1).Sample(rand);
    [MethodImpl(AggressiveInlining)] public static int Index<T>(T[] list) => Uniform.NewInclusive(0, list.Length - 1).Sample(rand);
    
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
    
    [MethodImpl(AggressiveInlining)] public static float Float(float to) => Uniform.NewInclusive(0f, to).Sample(rand);

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
                keep = Keep.Best;
                return this;
            }
        }

        public Dice worst {
            [MethodImpl(AggressiveInlining)] get {
                keep = Keep.Worst;
                return this;
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

        [MethodImpl(AggressiveInlining)] private int OneDiceRoll() => Uniform.NewInclusive(1, sides).Sample(rand);  
        
        [MethodImpl(AggressiveInlining)] public int Roll() {
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

        [MethodImpl(AggressiveInlining)] public void Times(Action<int> action) {
            var roll = Roll() - additive;
            for(var i = 0; i < roll; i++) {
                action.Invoke(i);
            }
        }

        [MethodImpl(AggressiveInlining)] public void Times(Action action) {
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
        var idx = Index(list);
        return new RandomStartEnumeratorForListOf<T> {
            data = list,
            index = idx,
            stopIndex = idx, 
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
        public int stopIndex;
        
        [MethodImpl(AggressiveInlining)] public RandomStartEnumeratorForListOf<T> GetEnumerator() => this;
        [MethodImpl(AggressiveInlining)] public bool MoveNext() {
            index++;
            if(index >= data.Count) index = 0;
            if(index == stopIndex) return false;
            
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
        var idx = Index(list);
        return new RandomStartEnumeratorForRawListOf<T> {
            data = list,
            index = idx,
            stopIndex = idx, 
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
        public int stopIndex;
        
        [MethodImpl(AggressiveInlining)] public RandomStartEnumeratorForRawListOf<T> GetEnumerator() => this;
        [MethodImpl(AggressiveInlining)] public bool MoveNext() {
            index++;
            if(index >= data.size) index = 0;
            if(index == stopIndex) return false;
            
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
    [MethodImpl(AggressiveInlining)] public static void Shuffle<T>(this T[] array) {
        rand.ShuffleInPlace(array.AsSpan());
    }
    
#if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [MethodImpl(AggressiveInlining)] public static void Shuffle<T>(this List<T> list) {
        rand.ShuffleInPlace(list);
    }
    
#if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
#endif
    [MethodImpl(AggressiveInlining)] public static void Shuffle<T>(this RawList<T> list) {
        rand.ShuffleInPlace(list.AsSpan());
    }
}
}