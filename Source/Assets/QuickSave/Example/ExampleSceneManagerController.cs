using System.Collections.Generic;
using CI.QuickSave;
using UnityEngine;

public class ExampleSceneManagerController : MonoBehaviour
{
    public void Start()
    {
    }

    public void Save()
    {
        QuickSaveRaw.Save("Root3", new byte[] { 1, 2, 3, 4, 5 });

        QuickSaveManager.Save("Root1", "Key1", new Class2(), false);

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

        //SaveManager.Save("Root1", "Key1", one, true);
        //SaveManager.Save("Root1", "Key2", two, true);

        QuickSaveWriter writer = QuickSaveWriter.Create("Root2");
        writer.Write("Key1", one, true);
        writer.Write("Key2", two, true);
        writer.Commit();
    }

    public void Load()
    {
        byte[] bytes = QuickSaveRaw.Load<byte[]>("Root3");

        //Class2 result = SaveManager.Load<Class2>("Root1", "Key2");

        QuickSaveReader reader = QuickSaveReader.Create("Root2");
        var one = reader.Read<Class1>("Key1");
        var two = reader.Read<Class2>("Key2");
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