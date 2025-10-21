using System.Collections;
using NUnit.Framework;
using SharedUtils;
using UnityEngine;
using UnityEngine.TestTools;

public class Test_RectData {
    [Test] public void Test1() {
        var rect = new RectData<Vector2Int>(new Vector2Int(4, 4));
        for (var i = 0; i < 4; i++) {
            for (var j = 0; j < 4; j++) {
                rect[i, j] = new Vector2Int(i, j);
            }
        }

        for (var i = 0; i < 4; i++) {
            for (var j = 0; j < 4; j++) {
                Assert.AreEqual(rect[i, j], new Vector2Int(i, j));
            }
        }
    }
    
    [Test] public void Test2() {
        var rect = new RectData<Vector2Int>(new Vector2Int(32, 32));
        for (var i = 0; i < 32; i++) {
            for (var j = 0; j < 32; j++) {
                rect[i, j] = new Vector2Int(i, j);
            }
        }

        for (var i = 0; i < 32; i++) {
            for (var j = 0; j < 32; j++) {
                Assert.AreEqual(rect[i, j], new Vector2Int(i, j));
            }
        }
    }
}
