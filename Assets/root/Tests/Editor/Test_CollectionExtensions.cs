using System.Collections;
using NUnit.Framework;
using SharedUtils;
using UnityEngine;
using UnityEngine.TestTools;

public class Test_CollectionExtensions {
    [Test] public void AtWrap() {
        var array = new int[] {0, 1, 2, 3};

        Assert.AreEqual(array.AtWrap(-4), 0);
        Assert.AreEqual(array.AtWrap(-3), 1);
        Assert.AreEqual(array.AtWrap(-2), 2);
        Assert.AreEqual(array.AtWrap(-1), 3);
        
        Assert.AreEqual(array.AtWrap(0), 0);
        Assert.AreEqual(array.AtWrap(1), 1);
        Assert.AreEqual(array.AtWrap(2), 2);
        Assert.AreEqual(array.AtWrap(3), 3);
        
        Assert.AreEqual(array.AtWrap(4), 0);
        Assert.AreEqual(array.AtWrap(5), 1);
        Assert.AreEqual(array.AtWrap(6), 2);
        Assert.AreEqual(array.AtWrap(7), 3);
        
        Assert.AreEqual(array.AtWrap(8), 0);
        Assert.AreEqual(array.AtWrap(9), 1);
        Assert.AreEqual(array.AtWrap(10), 2);
        Assert.AreEqual(array.AtWrap(11), 3);
    }
}
