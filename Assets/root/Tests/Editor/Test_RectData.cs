using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Test_RectData {
    [Test] public void Indexer_RoundTripsEveryCell([Values(4, 32)] int side) {
        var rect = new RectData<Vector2Int>(new Vector2Int(side, side));
        for (var i = 0; i < side; i++) {
            for (var j = 0; j < side; j++) {
                rect[i, j] = new Vector2Int(i, j);
            }
        }

        for (var i = 0; i < side; i++) {
            for (var j = 0; j < side; j++) {
                Assert.AreEqual(new Vector2Int(i, j), rect[i, j]);
            }
        }
    }

    [Test] public void Contains_MatchesTheSize() {
        var rect = new RectData<int>(new Vector2Int(3, 5));

        Assert.IsTrue(rect.Contains(0, 0));
        Assert.IsTrue(rect.Contains(2, 4));
        Assert.IsFalse(rect.Contains(3, 0));
        Assert.IsFalse(rect.Contains(0, 5));
        Assert.IsFalse(rect.Contains(-1, 0));
        Assert.IsFalse(rect.Contains(new Vector2Int(0, -1)));
    }

    [Test] public void Enumerator_VisitsEveryCellExactlyOnce([Values(1, 2, 3, 7)] int width, [Values(1, 4, 5)] int height) {
        var rect = new RectData<int>(new Vector2Int(width, height));
        foreach (ref var cell in rect) cell++;

        var visited = 0;
        foreach (var (pos, value) in rect.WithIndexes()) {
            Assert.AreEqual(1, value, $"{pos}");
            visited++;
        }

        Assert.AreEqual(width * height, visited);
        Assert.AreEqual(width * height, System.Array.FindAll(rect.data, v => v == 1).Length);
    }

    [Test] public void Enumerator_WithIndexesReportsThePositionOfEachValue() {
        var rect = new RectData<Vector2Int>(new Vector2Int(4, 3));
        for (var x = 0; x < 4; x++) for (var y = 0; y < 3; y++) rect[x, y] = new Vector2Int(x, y);

        var positions = new List<Vector2Int>();
        foreach (var (pos, value) in rect.WithIndexes()) {
            Assert.AreEqual(pos, value);
            positions.Add(pos);
        }

        Assert.AreEqual(12, positions.Count);
        CollectionAssert.AllItemsAreUnique(positions);
        CollectionAssert.Contains(positions, Vector2Int.zero);
        CollectionAssert.Contains(positions, new Vector2Int(3, 2));
    }

    [Test] public void DataInRect_UsesAnExclusiveMaxLikeRectInt() {
        var rect = new RectData<int>(new Vector2Int(6, 6));
        rect[new RectInt(1, 2, 3, 2)] = 1;

        var marked = new List<Vector2Int>();
        foreach (var (pos, value) in rect.WithIndexes()) if (value == 1) marked.Add(pos);

        Assert.AreEqual(6, marked.Count);
        for (var x = 1; x < 4; x++) for (var y = 2; y < 4; y++) CollectionAssert.Contains(marked, new Vector2Int(x, y));
    }

    [Test] public void DataInRect_ClipsToTheBoardAndSkipsEmptyRects() {
        var rect = new RectData<int>(new Vector2Int(4, 4));
        rect[new RectInt(-2, 2, 10, 10)] = 1;
        Assert.AreEqual(8, System.Array.FindAll(rect.data, v => v == 1).Length);

        var visited = 0;
        foreach (var _ in rect.DataInRect(new RectInt(1, 1, 0, 3))) visited++;
        foreach (var _ in rect.DataInRect(new RectInt(1, 1, 3, 0))) visited++;
        foreach (var _ in rect.DataInRect(new RectInt(9, 9, 3, 3))) visited++;
        Assert.AreEqual(0, visited);
    }

    [Test] public void IsEmpty_MeansEveryCellIsDefault() {
        var rect = new RectData<int>(new Vector2Int(5, 5));
        Assert.IsTrue(rect.IsEmpty());
        Assert.IsFalse(rect.IsNotEmpty());

        rect[2, 2] = 1;
        Assert.IsFalse(rect.IsEmpty());
        Assert.IsTrue(rect.IsNotEmpty(new RectInt(2, 2, 1, 1)));
        Assert.IsTrue(rect.IsEmpty(new RectInt(0, 0, 2, 5)));
        Assert.IsTrue(rect.IsEmpty(new RectInt(3, 0, 2, 5)));

        rect.Clear();
        Assert.IsTrue(rect.IsEmpty());
    }
}
