using System.Collections.Generic;
using NUnit.Framework;
using SharedUtils;

public class Test_CollectionExtensions {
    [Test] public void AtWrap() {
        var array = new int[] {0, 1, 2, 3};

        for (var i = -8; i <= 11; i++) Assert.AreEqual(((i % 4) + 4) % 4, array.AtWrap(i), $"{i}");
    }

    [Test] public void Fold_UsesEachElementOnce() {
        var array = new[] { 1, 2, 3, 4 };
        var list = new List<int>(array);

        Assert.AreEqual(10, array.Fold((a, b) => a + b));
        Assert.AreEqual(10, list.Fold((a, b) => a + b));
        Assert.AreEqual(10, array.Fold((acc, x) => acc + x, 0));
        Assert.AreEqual(10, list.Fold((acc, x) => acc + x, 0));
        Assert.AreEqual(7, new[] { 7 }.Fold((a, b) => a + b));
    }

    [Test] public void IndexOfAndContains_HandleNullsAndStructs() {
        var strings = new[] { "a", null, "b" };
        Assert.AreEqual(1, strings.IndexOf((string)null));
        Assert.AreEqual(2, strings.IndexOf("b"));
        Assert.AreEqual(-1, strings.IndexOf("c"));
        Assert.IsTrue(strings.Contains(null));
        Assert.IsTrue(strings.Contains("a"));
        Assert.IsFalse(strings.Contains("c"));

        var ints = new[] { 5, 6, 7 };
        Assert.AreEqual(1, ints.IndexOf(6));
        Assert.IsTrue(ints.Contains(7));
        Assert.IsFalse(ints.Contains(8));
    }

    [Test] public void MinByAndMaxBy_PickTheExtremeAndSurviveNaN() {
        var array = new[] { 3f, 1f, 2f };
        var list = new List<float>(array);

        Assert.AreEqual(1f, array.MinBy(x => x));
        Assert.AreEqual(1f, list.MinBy(x => x));
        Assert.AreEqual(3f, array.MaxBy(x => x));
        Assert.AreEqual(3f, list.MaxBy(x => x));

        var nan = new[] { float.NaN, float.NaN };
        Assert.IsNaN(nan.MinBy(x => x));
        Assert.IsNaN(nan.MaxBy(x => x));
        Assert.IsNaN(new List<float>(nan).MinBy(x => x));

        Assert.AreEqual(-1f, new float[0].MinBy(x => x, -1f));
        Assert.AreEqual(-1f, ((List<float>)null).MaxBy(x => x, -1f));
    }

    [Test] public void FirstAndLast_FallBackOnEmptyOrNull() {
        Assert.AreEqual(9, new int[0].First(9));
        Assert.AreEqual(9, new int[0].Last(9));
        Assert.AreEqual(9, ((int[])null).First(9));
        Assert.AreEqual(9, ((int[])null).Last(9));
        Assert.AreEqual(9, new List<int>().Last(9));
        Assert.AreEqual(9, ((List<int>)null).Last(9));
        Assert.AreEqual(3, new[] { 1, 2, 3 }.Last());
        Assert.AreEqual(1, new List<int> { 1, 2, 3 }.First());
    }
}
