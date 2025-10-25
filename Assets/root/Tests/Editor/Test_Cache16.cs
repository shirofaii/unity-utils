using System.Collections;
using NUnit.Framework;
using SharedUtils;
using UnityEngine;
using UnityEngine.TestTools;

public class Test_Cache16 {
    [Test] public void Test1() {
        var cache = new Cache16Unsafe<int>();
        for (var i = 0; i < 16; i++) {
            cache.AddOnce(i);
        }
        
        for (var i = 0; i < 16; i++) {
            Assert.AreEqual(cache[i], i);    
        }
    }
}
