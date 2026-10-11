using NUnit.Framework;
using SharedUtils;

public class Test_CacheSafeULong {
    private static ulong[] Items(CacheSafeULong cache) => cache.GetSpan().ToArray();

    [Test] public void AddOnce_KeepsOneCopyOfEachItem() {
        var cache = new CacheSafeULong();
        for (var i = 0; i < 4; i++) {
            cache.AddOnce(7);
            cache.AddOnce(9);
        }

        Assert.AreEqual(2, cache.Count);
        CollectionAssert.AreEquivalent(new ulong[] { 7, 9 }, Items(cache));
    }

    [Test] public void AddOnce_FillsTheInlineSlotsFirst() {
        var cache = new CacheSafeULong();
        for (var i = 1; i <= CacheSafeULong.maxSize; i++) cache.AddOnce((ulong)i);

        Assert.IsTrue(cache.IsInline);
        Assert.AreEqual(CacheSafeULong.maxSize, cache.Count);
        for (var i = 1; i <= CacheSafeULong.maxSize; i++) Assert.IsTrue(cache.Contains((ulong)i), $"{i}");
    }

    [Test] public void AddOnce_SpillsPastTheInlineSlotsAndKeepsEverything() {
        var cache = new CacheSafeULong();
        const int count = CacheSafeULong.maxSize * 5;
        for (var i = 1; i <= count; i++) {
            cache.AddOnce((ulong)i);
            cache.AddOnce((ulong)i);
        }

        Assert.IsFalse(cache.IsInline);
        Assert.AreEqual(count, cache.Count);
        var seen = Items(cache);
        Assert.AreEqual(count, seen.Length);
        for (var i = 1; i <= count; i++) CollectionAssert.Contains(seen, (ulong)i);
    }

    [Test] public void Remove_TakesAnInlineItemOut() {
        var cache = new CacheSafeULong();
        cache.AddOnce(1);
        cache.AddOnce(2);
        cache.AddOnce(3);

        cache.Remove(2);
        cache.Remove(42);

        Assert.AreEqual(2, cache.Count);
        CollectionAssert.AreEquivalent(new ulong[] { 1, 3 }, Items(cache));
        Assert.IsFalse(cache.Contains(2));
    }

    [Test] public void Remove_TakesASpilledItemOut() {
        var cache = new CacheSafeULong();
        const int count = CacheSafeULong.maxSize + 3;
        for (var i = 1; i <= count; i++) cache.AddOnce((ulong)i);

        cache.Remove(1);
        cache.Remove(count);

        Assert.AreEqual(count - 2, cache.Count);
        Assert.IsFalse(cache.Contains(1));
        Assert.IsFalse(cache.Contains(count));
        for (var i = 2; i < count; i++) Assert.IsTrue(cache.Contains((ulong)i), $"{i}");
    }

    [Test] public void Remove_ComesBackInlineWhenTheCellThinsOut() {
        var cache = new CacheSafeULong();
        const int count = CacheSafeULong.maxSize * 3;
        for (var i = 1; i <= count; i++) cache.AddOnce((ulong)i);
        Assert.IsFalse(cache.IsInline);

        for (var i = 1; i <= count - 2; i++) cache.Remove((ulong)i);

        Assert.IsTrue(cache.IsInline);
        Assert.AreEqual(2, cache.Count);
        CollectionAssert.AreEquivalent(new ulong[] { count - 1, count }, Items(cache));

        cache.AddOnce(1000);
        Assert.AreEqual(3, cache.Count);
        Assert.IsTrue(cache.Contains(1000));
    }

    [Test] public void GetSpan_WritesThrough() {
        var cache = new CacheSafeULong();
        cache.AddOnce(5);
        cache.GetSpan()[0] = 6;

        Assert.IsTrue(cache.Contains(6));
        Assert.IsFalse(cache.Contains(5));
    }

    [Test] public void Clear_EmptiesBothModes() {
        var inline = new CacheSafeULong();
        inline.AddOnce(1);
        var spilled = new CacheSafeULong();
        for (var i = 1; i <= CacheSafeULong.maxSize + 1; i++) spilled.AddOnce((ulong)i);

        inline.Clear();
        spilled.Clear();

        Assert.AreEqual(0, inline.Count);
        Assert.AreEqual(0, spilled.Count);
        Assert.IsTrue(spilled.IsInline);
        Assert.AreEqual(0, inline.GetSpan().Length);
    }
}
