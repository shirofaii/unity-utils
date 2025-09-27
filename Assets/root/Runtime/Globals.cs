using SharedUtils;
using System.Runtime.CompilerServices;

public static class Globals
{
    public const MethodImplOptions inline = MethodImplOptions.AggressiveInlining;
    
    public static Rand.Dice d2 { [MethodImpl(inline)] get => new(2); }
    public static Rand.Dice d3 { [MethodImpl(inline)] get => new(3); }
    public static Rand.Dice d4 { [MethodImpl(inline)] get => new(4); }
    public static Rand.Dice d6 { [MethodImpl(inline)] get => new(6); }
    public static Rand.Dice d8 { [MethodImpl(inline)] get => new(8); }
    public static Rand.Dice d10 { [MethodImpl(inline)] get => new(10); }
    public static Rand.Dice d12 { [MethodImpl(inline)] get => new(12); }
    public static Rand.Dice d20 { [MethodImpl(inline)] get => new(20); }
    public static Rand.Dice d100 { [MethodImpl(inline)] get => new(100); }
}