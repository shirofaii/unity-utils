using SharedUtils;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static System.Runtime.CompilerServices.MethodImplOptions;

public readonly struct Str : IEquatable<Str> {
    private readonly string literalString;

    [MethodImpl(AggressiveInlining)] public Str(string literalString) {
        Debug.Assert(literalString != null);
        Debug.Assert(string.IsInterned(literalString) != null);
        this.literalString = literalString;
    }
    
    [MethodImpl(AggressiveInlining)] public static implicit operator string(Str d) => d.literalString;
    [MethodImpl(AggressiveInlining)] public static explicit operator Str(string s) => new(s);
    
    [MethodImpl(AggressiveInlining)] public bool Equals(Str other) => ReferenceEquals(literalString, other.literalString);
    [MethodImpl(AggressiveInlining)] public override bool Equals(object obj) => obj is Str other && Equals(other);
    [MethodImpl(AggressiveInlining)] public override int GetHashCode() => literalString.GetHashCode();
    [MethodImpl(AggressiveInlining)] public static bool operator ==(Str left, Str right) {
        return left.Equals(right);
    }
    [MethodImpl(AggressiveInlining)] public static bool operator !=(Str left, Str right) {
        return !left.Equals(right);
    }

    private sealed class Comparer : IEqualityComparer<Str> {
        [MethodImpl(AggressiveInlining)] public bool Equals(Str x, Str y) {
            return ReferenceEquals(x.literalString, y.literalString);
        }
        [MethodImpl(AggressiveInlining)] public int GetHashCode(Str obj) {
            return obj.literalString.GetHashCode();
        }
    }
    
    public static IEqualityComparer<Str> comparer { [MethodImpl(AggressiveInlining)] get; } = new Comparer();
}