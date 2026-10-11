using System.Collections.Generic;
using NUnit.Framework;
using SharedUtils;

public class Test_Rand {
    [Test] public void FromRandomStart_VisitsEveryListItemOnce([Values(0, 1, 2, 5)] int count) {
        var list = new List<int>();
        for (var i = 0; i < count; i++) list.Add(i);

        for (var attempt = 0; attempt < 20; attempt++) {
            var seen = new List<int>();
            foreach (var x in list.FromRandomStart()) seen.Add(x);

            Assert.AreEqual(count, seen.Count);
            CollectionAssert.AreEquivalent(list, seen);
            for (var i = 1; i < seen.Count; i++) Assert.AreEqual((seen[i - 1] + 1) % count, seen[i], "wraps around in order");
        }
    }

    [Test] public void FromRandomStart_VisitsEveryRawListItemOnce([Values(0, 1, 3)] int count) {
        var list = new RawList<int>();
        for (var i = 0; i < count; i++) list.Add(i);

        var seen = new List<int>();
        foreach (var x in list.FromRandomStart()) seen.Add(x);

        Assert.AreEqual(count, seen.Count);
        CollectionAssert.AreEquivalent(list.ToArray(), seen);
    }

    [Test] public void Dice_ZeroDiceRollNothing() {
        Assert.AreEqual(0, (0 * Dice.d6).Roll());
        Assert.AreEqual(2, (0 * Dice.d6.best + 2).Roll());
        Assert.AreEqual(2, (0 * Dice.d6.worst + 2).Roll());
    }

    [Test] public void Dice_BestAndWorstLeaveTheOriginalAlone() {
        var sum = new Rand.Dice(1, 3);
        var best = sum.best;
        var worst = sum.worst;

        Assert.AreEqual(1, best.Roll());
        Assert.AreEqual(1, worst.Roll());
        Assert.AreEqual(3, sum.Roll());
    }

    [Test] public void Dice_StayInRange() {
        for (var i = 0; i < 1000; i++) {
            int d20 = Dice.d20;
            Assert.That(d20, Is.InRange(1, 20));
            int two = 2 * Dice.d6 + 1;
            Assert.That(two, Is.InRange(3, 13));
        }
    }

    [Test] public void Pcg32_IsDeterministicAndBounded() {
        var a = new Pcg32(42);
        var b = new Pcg32(42);
        var c = new Pcg32(43);
        var differs = 0;
        var counts = new int[7];
        for (var i = 0; i < 10000; i++) {
            var x = a.Next(7);
            Assert.AreEqual(x, b.Next(7));
            if (x != c.Next(7)) differs++;
            counts[x]++;
        }

        Assert.Greater(differs, 5000);
        foreach (var n in counts) Assert.Greater(n, 1000);
        Assert.Throws<System.ArgumentOutOfRangeException>(() => a.Next(0));
    }

    [Test] public void Seed_RestartsTheSharedSequence() {
        Rand.Seed(7);
        var first = new[] { Rand.rng.Next(100), Rand.rng.Next(100), Rand.rng.Next(100) };
        Rand.Seed(7);
        var second = new[] { Rand.rng.Next(100), Rand.rng.Next(100), Rand.rng.Next(100) };

        CollectionAssert.AreEqual(first, second);
    }

    [Test] public void Shuffle_KeepsEveryItem() {
        var array = new int[50];
        for (var i = 0; i < array.Length; i++) array[i] = i;
        var list = new List<int>(array);
        var raw = new RawList<int>();
        foreach (var x in array) raw.Add(x);

        array.Shuffle();
        list.Shuffle();
        raw.Shuffle();

        var expected = new int[50];
        for (var i = 0; i < expected.Length; i++) expected[i] = i;
        CollectionAssert.AreEquivalent(expected, array);
        CollectionAssert.AreEquivalent(expected, list);
        CollectionAssert.AreEquivalent(expected, raw.ToArray());
        Assert.IsFalse(System.Linq.Enumerable.SequenceEqual(expected, array), "a 50-item shuffle landing on identity is a bug, not luck");
    }
}
