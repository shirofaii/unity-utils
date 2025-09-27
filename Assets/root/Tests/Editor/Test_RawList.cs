using System.Collections;
using NUnit.Framework;
using SharedUtils;
using UnityEngine;
using UnityEngine.TestTools;

public class Test_RawList {
    [Test] public void Test1() {
        var list = new RawList<int>();
        list.Add(1);
        list.Add(2);
        list.Add(3);

        foreach(ref var x in list) {
            x += 1;
        }
        
        Assert.AreEqual(list[0], 2);
        Assert.AreEqual(list[1], 3);
        Assert.AreEqual(list[2], 4);
    }
    
    [Test] public void Test2() {
    }
    
    [Test] public void Test3() {
    }
}
