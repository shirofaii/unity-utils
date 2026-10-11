using NUnit.Framework;
using SharedUtils;

public class Test_RawList {
    [Test] public void Enumerator_GivesRefsToEveryItem() {
        var list = new RawList<int>();
        list.Add(1);
        list.Add(2);
        list.Add(3);

        foreach(ref var x in list) {
            x += 1;
        }

        Assert.AreEqual(2, list[0]);
        Assert.AreEqual(3, list[1]);
        Assert.AreEqual(4, list[2]);
    }

    [Test] public void Add_GrowsPastTheInitialCapacity() {
        var list = new RawList<int>(2);
        for (var i = 0; i < 1000; i++) list.Add(i);

        Assert.AreEqual(1000, list.size);
        for (var i = 0; i < 1000; i++) Assert.AreEqual(i, list[i]);
    }

    [Test] public void IndexOf_FindsItemsAndNulls() {
        var list = new RawList<string>();
        list.Add("a");
        list.Add(null);
        list.Add("b");

        Assert.AreEqual(0, list.IndexOf("a"));
        Assert.AreEqual(1, list.IndexOf(null));
        Assert.AreEqual(2, list.IndexOf("b"));
        Assert.AreEqual(-1, list.IndexOf("c"));
    }

    [Test] public void RemoveRange_ClearsTheVacatedTail() {
        var list = new RawList<string>();
        for (var i = 0; i < 6; i++) list.Add(i.ToString());

        list.RemoveRange(1, 3);

        Assert.AreEqual(3, list.size);
        Assert.AreEqual("0", list[0]);
        Assert.AreEqual("4", list[1]);
        Assert.AreEqual("5", list[2]);
        for (var i = list.size; i < list.data.Length; i++) Assert.IsNull(list.data[i], $"slot {i}");
    }

    [Test] public void Remove_KeepsOrderAndSwapBackDoesNot() {
        var ordered = new RawList<int>();
        var swapped = new RawList<int>();
        for (var i = 0; i < 5; i++) {
            ordered.Add(i);
            swapped.Add(i);
        }

        Assert.IsTrue(ordered.Remove(1));
        Assert.IsFalse(ordered.Remove(42));
        Assert.IsTrue(swapped.RemoveSwapBack(1));

        CollectionAssert.AreEqual(new[] { 0, 2, 3, 4 }, ordered.ToArray());
        CollectionAssert.AreEquivalent(new[] { 0, 2, 3, 4 }, swapped.ToArray());
        Assert.AreEqual(4, swapped[1]);
    }

    [Test] public void Copy_IsIndependentOfTheOriginal() {
        var list = new RawList<int>();
        list.Add(1);
        list.Add(2);
        var copy = new RawList<int>(list);

        copy.Add(3);
        copy[0] = 10;

        Assert.AreEqual(2, list.size);
        Assert.AreEqual(1, list[0]);
        Assert.AreEqual(3, copy.size);
    }
}
