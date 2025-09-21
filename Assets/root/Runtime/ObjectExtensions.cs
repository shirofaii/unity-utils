using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class ObjectExtension {
    public static byte[] SerializeToByteArray(this object obj) {
        if (obj == null) {
            return null;
        }

        var bf = new BinaryFormatter();
        using var mem = new MemoryStream();
        bf.Serialize(mem, obj);
        return mem.ToArray();
    }

    public static T Deserialize<T>(this byte[] byteArray) where T : class {
        if (byteArray == null) {
            return null;
        }

        using var mem = new MemoryStream();
        var bf = new BinaryFormatter();
        mem.Write(byteArray, 0, byteArray.Length);
        mem.Seek(0, SeekOrigin.Begin);
        return (T)bf.Deserialize(mem);
    }

    public static T Clone<T>(this T original) where T : class => original.SerializeToByteArray().Deserialize<T>();
}