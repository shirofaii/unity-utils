using System.Collections;
using NUnit.Framework;
using SharedUtils;
using UnityEngine;
using UnityEngine.TestTools;

public class Test_MemoryArena {
    [Test] public void Test1() {
        var arena = new MemoryArena<int>(4);
        var mem = arena.Start()
            .Add(1)
            .Add(2)
            .Finish();

        Assert.AreEqual(mem.Length, 2);
        var span = mem.Span;
        Assert.AreEqual(span[0], 1);
        Assert.AreEqual(span[1], 2);
    }
    
    [Test] public void Test2() {
        var arena = new MemoryArena<int>(1);
        var mem = arena.Start()
            .Add(1)
            .Add(2)
            .Finish();

        Assert.AreEqual(mem.Length, 2);
        var span = mem.Span;
        Assert.AreEqual(span[0], 1);
        Assert.AreEqual(span[1], 2);
    }
    
    [Test] public void Test3() {
        var arena = new MemoryArena<int>(1);
        var mem = arena.Alloc(2);

        Assert.AreEqual(mem.Length, 2); 
    }
}
