using System.Collections.Generic;
using CI.QuickSave;
using UnityEngine;

public class ExampleSceneManagerController : MonoBehaviour
{
    public void Save()
    {
        Class1 one = new Class1()
        {
            ItemD = 12,
            ItemE = "K",
            ItemC = 34.6789f,
            ItemB = 454545,
            ItemA = "Hello World!!!!",
            ItemF = 34.56,
            ItemG = new List<int>() { 1, 2, 3, 4, 5, 6 },
            ItemH = new List<IEnumerable<int>>()
            {
                new List<int>() { 1, 2, 3, 4, 5 },
                new List<int>() { 6, 7, 8, 9, 0 }
            }
        };

        Class2 two = new Class2()
        {
            KeyA = 12
        };

        Vector2 vector2 = new Vector2(1.5678f, 2f);

        QuickSaveWriter.Create("Root2")
                       .Write("Key1", one)
                       .Write("Key2", two)
                       .Write("Key3", vector2)
                       .Commit();
    }

    public void Load()
    {
        Class1 class1;
        Class2 class2;
        Vector2 vector2;

        QuickSaveReader.Create("Root2")
                       .Read<Class1>("Key1", (r) => { class1 = r; })
                       .Read<Class2>("Key2", (r) => { class2 = r; })
                       .Read<Vector2>("Key3", (r) => { vector2 = r; });
    }

    public void QuickSaveRootExample()
    {
        Texture2D texture2D = new Texture2D(1, 1);
        texture2D.LoadImage(new byte[] { 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4, 1, 2, 3, 4 });
        texture2D.Apply();

        QuickSaveRoot.Save("RootName", texture2D);

        Texture2D loaded = QuickSaveRoot.Load<Texture2D>("RootName");
    }

    public void QuickSaveRawExample()
    {
        QuickSaveRaw.SaveString("TextFile.txt", "Some text to save");
        QuickSaveRaw.SaveBytes("BytesFile.txt", new byte[] { 1, 2, 3, 4 });

        string text = QuickSaveRaw.LoadString("TextFile.txt");
        byte[] bytes = QuickSaveRaw.LoadBytes("BytesFile.txt");
    }

    public void QuickSaveReaderExample()
    {
        // An exception will be thrown if the root doesn't exist

        string one;
        double two;
        Vector2 three;
        Color four;

        QuickSaveReader.Create("RootName")
                       .Read<string>("Key1", (r) => { one = r; })
                       .Read<double>("Key2", (r) => { two = r; })
                       .Read<Vector2>("Key3", (r) => { three = r; })
                       .Read<Color>("Key4", (r) => { four = r; });

        // OR

        QuickSaveReader reader = QuickSaveReader.Create("RootName");
        one = reader.Read<string>("Key1");
        two = reader.Read<double>("Key2");
        three = reader.Read<Vector2>("Key3");
        four = reader.Read<Color>("Key4");
    }

    public void QuickSaveWriterExample()
    {
        string one = "Hello World!";
        double two = 45.6789;
        Vector2 three = new Vector2(34.0f, 78.92f);
        Color four = new Color(0.1f, 0.5f, 0.8f, 1.0f);

        QuickSaveWriter.Create("RootName")
                       .Write("Key1", one)
                       .Write("Key2", two)
                       .Write("Key3", three)
                       .Write("Key4", four)
                       .Commit();

        // OR

        QuickSaveWriter writer = QuickSaveWriter.Create("RootName");
        writer.Write("Key1", one);
        writer.Write("Key2", two);
        writer.Write("Key3", three);
        writer.Write("Key4", four);
        writer.Commit();
    }
}

public class Class1
{
    public string ItemA { get; set; }
    public int ItemB { get; set; }
    public double ItemC { get; set; }
    public int ItemD { get; set; }
    public string ItemE { get; set; }
    public double ItemF { get; set; }
    public IEnumerable<int> ItemG { get; set; }
    public IEnumerable<IEnumerable<int>> ItemH { get; set; }
}

public class Class2
{
    public int KeyA { get; set; }
}