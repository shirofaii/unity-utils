using Scellecs.Morpeh.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.IL2CPP.CompilerServices;

// ReSharper disable InconsistentNaming

namespace Dungeon.Utils {
public static class Rand {
    private static readonly Random rand = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    public static int Index<T>(List<T> list) => rand.Next(list.Count);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    public static int Index<T>(FastList<T> list) => rand.Next(list.length);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    public static int Index<T>(T[] list) => rand.Next(list.Length);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static T Value<T>(List<T> list) => list[rand.Next(list.Count)];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static T Value<T>(FastList<T> list) => list[rand.Next(list.length)];
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static T Value<T>(T[] list) => list[rand.Next(list.Length)];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static T AtRandom<T>(this List<T> list) => list[rand.Next(list.Count)];

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static T AtRandom<T>(this FastList<T> list) => list[rand.Next(list.length)];
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static T AtRandom<T>(this T[] list) => list[rand.Next(list.Length)];

    public static Dice d2 => new(2);
    public static Dice d3 => new(3);
    public static Dice d4 => new(4);
    public static Dice d6 => new(6);
    public static Dice d8 => new(8);
    public static Dice d10 => new(10);
    public static Dice d12 => new(12);
    public static Dice d20 => new(20);
    public static Dice d100 => new(100);

    public static float Float(float to) => (float)(rand.NextDouble() * to);

    public struct Dice {
        public enum Modifier { None, Best, Worst }

        private readonly int dice;
        private int times;
        private int add;
        private Modifier modifier;

        public Dice(int dice, int times = 1, int add = 0, Modifier modifier = Modifier.None) {
            this.dice = dice;
            this.times = times;
            this.add = add;
            this.modifier = modifier;
        }

        public Dice best {
            get {
                modifier = Modifier.Best;
                return this;
            }
        }

        public Dice worst {
            get {
                modifier = Modifier.Worst;
                return this;
            }
        }

        public static implicit operator int(Dice d) => d.Roll();

        public static Dice operator *(int times, Dice dice) {
            dice.times *= times;
            return dice;
        }

        public static Dice operator +(Dice dice, int add) {
            dice.add += add;
            return dice;
        }

        public int Roll() {
            switch (modifier) {
                case Modifier.Worst:
                    var all = new List<int>(times);
                    for(var i = 1; i < times; i++) {
                        all.Add(rand.Next(dice) + 1 + add);
                    }

                    return all.Min();
                case Modifier.Best:
                    all = new List<int>(times);
                    for(var i = 1; i < times; i++) {
                        all.Add(rand.Next(dice) + 1 + add);
                    }

                    return all.Max();
                case Modifier.None: return (rand.Next(dice) + 1) * times + add;
                default:            throw new Exception();
            }
        }

        public void Times(Action<int> action) {
            var roll = Roll();
            for(var i = 0; i < roll; i++) {
                action.Invoke(i);
            }
        }

        public void Times(Action action) {
            var roll = Roll();
            for(var i = 0; i < roll; i++) {
                action.Invoke();
            }
        }
    }

    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static IEnumerable<T> IterateFromRandomStart<T>(this List<T> list) {
        var offset = Index(list);
        for(var i = 0; i <= list.Count; i++) {
            yield return list.AtWrap(i + offset);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static IEnumerable<T> IterateFromRandomStart<T>(this FastList<T> list) {
        var offset = Index(list);
        for(var i = 0; i <= list.length; i++) {
            yield return list.AtWrap(i + offset);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static IEnumerable<T> IterateFromRandomStart<T>(this T[] array) {
        var offset = Index(array);
        for(var i = 0; i <= array.Length; i++) {
            yield return array.AtWrap(i + offset);
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static void Shuffle<T>(this T[] array) {
        var n = array.Length;
        while (n > 1) {
            n--;
            var k = rand.Next(n + 1);
            (array[n], array[k]) = (array[k], array[n]);
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static void Shuffle<T>(this List<T> list) {
        var n = list.Count;
        while (n > 1) {
            n--;
            var k = rand.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    public static void Shuffle<T>(this FastList<T> list) {
        var n = list.length;
        while (n > 1) {
            n--;
            var k = rand.Next(n + 1);
            (list[n], list[k]) = (list[k], list[n]);
        }
    }
}
}