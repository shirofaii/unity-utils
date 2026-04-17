using SharedUtils;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static Globals;

public readonly struct Str : IEquatable<Str> {
    private readonly string literalString;

    [MethodImpl(inline)] public Str(string literalString) {
        Debug.Assert(literalString != null);
        Debug.Assert(string.IsInterned(literalString) != null);
        this.literalString = literalString;
    }
    
    [MethodImpl(inline)] public static implicit operator string(Str d) => d.literalString;
    [MethodImpl(inline)] public static explicit operator Str(string s) => new(s);
    
    [MethodImpl(inline)] public bool Equals(Str other) => ReferenceEquals(literalString, other.literalString);
    [MethodImpl(inline)] public override bool Equals(object obj) => obj is Str other && Equals(other);
    [MethodImpl(inline)] public override int GetHashCode() => literalString.GetHashCode();
    [MethodImpl(inline)] public static bool operator ==(Str left, Str right) {
        return left.Equals(right);
    }
    [MethodImpl(inline)] public static bool operator !=(Str left, Str right) {
        return !left.Equals(right);
    }

    private sealed class Comparer : IEqualityComparer<Str> {
        [MethodImpl(inline)] public bool Equals(Str x, Str y) {
            return ReferenceEquals(x.literalString, y.literalString);
        }
        [MethodImpl(inline)] public int GetHashCode(Str obj) {
            return obj.literalString.GetHashCode();
        }
    }
    
    public static IEqualityComparer<Str> comparer { [MethodImpl(inline)] get; } = new Comparer();
}