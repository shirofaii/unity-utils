using SharedUtils;
using System.Runtime.CompilerServices;
using static System.Runtime.CompilerServices.MethodImplOptions;

public static class Dice {
    public static Rand.Dice d2 { [MethodImpl(AggressiveInlining)] get => new(2); }
    public static Rand.Dice d3 { [MethodImpl(AggressiveInlining)] get => new(3); }
    public static Rand.Dice d4 { [MethodImpl(AggressiveInlining)] get => new(4); }
    public static Rand.Dice d6 { [MethodImpl(AggressiveInlining)] get => new(6); }
    public static Rand.Dice d8 { [MethodImpl(AggressiveInlining)] get => new(8); }
    public static Rand.Dice d10 { [MethodImpl(AggressiveInlining)] get => new(10); }
    public static Rand.Dice d12 { [MethodImpl(AggressiveInlining)] get => new(12); }
    public static Rand.Dice d20 { [MethodImpl(AggressiveInlining)] get => new(20); }
    public static Rand.Dice d100 { [MethodImpl(AggressiveInlining)] get => new(100); }
}