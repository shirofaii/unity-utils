using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using SharedUtils;
using Unity.PerformanceTesting;
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
    
    [Test, Performance] public void RawListPerf() {
        Measure.Method(() => {
                var list = new RawList<int>(10000);
                for(int i=0;i<10000;i++) {
                    list.Add(i);
                }
                for(int i=0;i<10000;i++) {
                    list[i]++;
                }
            })
            .WarmupCount(5)
            .IterationsPerMeasurement(10)
            .MeasurementCount(20)
            .Run();
    }
    
    [Test, Performance] public void ListPerf() {
        Measure.Method(() => {
                var list = new List<int>(10000);
                for(int i=0;i<10000;i++) {
                    list.Add(i);
                }
                for(int i=0;i<10000;i++) {
                    list[i]++;
                }
            })
            .WarmupCount(5)
            .IterationsPerMeasurement(10)
            .MeasurementCount(20)
            .Run();
    }
}
