using System.IO;
using System.Xml.Serialization;
using UnityEngine;

// TODO: add namespace

public class XmlHandler {
    public static void Serialize<T>(T data, string path) where T : class, IXmlData {
        XmlSerializer serializer = new(typeof(T));
        using (StreamWriter writer = new(path)) {
            serializer.Serialize(writer, data);
        }
    }

    public static T Deserialize<T>(string path) where T : class, IXmlData {
        if (!File.Exists(path)) {
            // TODO: exception? dialog?
            Debug.Log($"File '{path}' not found!");
            return default;
        }

        T data;
        XmlSerializer serializer = new(typeof(T));
        using (StreamReader reader = new(path)) {
            data = serializer.Deserialize(reader) as T;
        }

        return data;
    }
}
