using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using RandN;
using RandN.Distributions;
using SharedUtils;
using Unity.PerformanceTesting;
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
    
    [Test, Performance] public void RandomAccess() {
        var rand = StandardRng.Create();
        var s = Uniform.NewInclusive(0, 1023);
        var list = new RectData<int>(new Vector2Int(1024, 1024));
        
        Measure.Method(() => {
            var r = new Vector2Int(s.Sample(rand), s.Sample(rand));
            list[r]++;
        })
        .WarmupCount(5)
        .IterationsPerMeasurement(10000)
        .MeasurementCount(200)
        .Run();
    }
    
    [Test, Performance] public void LocalAccess() {
        var rand = StandardRng.Create();
        var s = Uniform.NewInclusive(16, 32);
        var list = new RectData<int>(new Vector2Int(1024, 1024));

        Measure.Method(() => {
                var r = new Vector2Int(s.Sample(rand), s.Sample(rand));
                list[r]++;
            })
            .WarmupCount(5)
            .IterationsPerMeasurement(10000)
            .MeasurementCount(200)
            .Run();
    }

}
