using SharedUtils;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static Globals;

public struct Symbol : IEquatable<Symbol> {
    private string literalString;

    [MethodImpl(inline)] public Symbol(string literalString) {
        Debug.Assert(literalString != null);
        Debug.Assert(string.IsInterned(literalString) != null);
        this.literalString = literalString;
    }
    
    [MethodImpl(inline)] public static implicit operator string(Symbol d) => d.literalString;
    [MethodImpl(inline)] public static explicit operator Symbol(string s) => new(s);
    
    [MethodImpl(inline)] public bool Equals(Symbol other) => ReferenceEquals(literalString, other.literalString);
    [MethodImpl(inline)] public override bool Equals(object obj) => obj is Symbol other && Equals(other);
    [MethodImpl(inline)] public override int GetHashCode() => literalString.GetHashCode();
    [MethodImpl(inline)] public static bool operator ==(Symbol left, Symbol right) {
        return left.Equals(right);
    }
    [MethodImpl(inline)] public static bool operator !=(Symbol left, Symbol right) {
        return !left.Equals(right);
    }

    private sealed class Comparer : IEqualityComparer<Symbol> {
        [MethodImpl(inline)] public bool Equals(Symbol x, Symbol y) {
            return ReferenceEquals(x.literalString, y.literalString);
        }
        [MethodImpl(inline)] public int GetHashCode(Symbol obj) {
            return obj.literalString.GetHashCode();
        }
    }
    
    public static IEqualityComparer<Symbol> comparer { [MethodImpl(inline)] get; } = new Comparer();
}