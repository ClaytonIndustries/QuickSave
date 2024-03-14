////////////////////////////////////////////////////////////////////////////////
//  
// @module Quick Save for Unity3D 
// @author Michael Clayton
// @support clayton.inds+support@gmail.com 
//
////////////////////////////////////////////////////////////////////////////////

using System.IO;
using CI.QuickSave;
using UnityEngine;
using UnityEngine.UI;

public class ExampleSceneManagerController : MonoBehaviour
{
    public InputField Input1;
    public InputField Input2;
    public InputField Input3;
    public InputField Input4;
    public InputField Input5;
    public InputField Input6;
    public InputField Input7;
    public InputField Input8;
    public InputField Content;

    public void Save()
    {
        QuickSaveWriter.Create("Inputs")
                       .Write("Input1", Input1.text)
                       .Write("Input2", Input2.text)
                       .Write("Input3", Input3.text)
                       .Write("Input4", Input4.text)
                       .Commit();

        Content.text = QuickSaveRaw.LoadString(Path.Combine(QuickSaveGlobalSettings.StorageLocation, "QuickSave", "Inputs.json"));
    }

    public void Load()
    {
        QuickSaveReader.Create("Inputs")
                       .Read<string>("Input1", (r) => { Input5.text = r; })
                       .Read<string>("Input2", (r) => { Input6.text = r; })
                       .Read<string>("Input3", (r) => { Input7.text = r; })
                       .Read<string>("Input4", (r) => { Input8.text = r; });
    }

    public void QuickSaveRawExample()
    {
        // Use QuickSaveRaw to directly save / load text or binary data to / from files

        QuickSaveRaw.SaveString($"{Application.persistentDataPath}/TextFile.txt", "Some text to save");
        QuickSaveRaw.SaveBytes($"{Application.persistentDataPath}/BytesFile.txt", new byte[] { 1, 2, 3, 4 });

#pragma warning disable 0219
        string text = QuickSaveRaw.LoadString($"{Application.persistentDataPath}/TextFile.txt");
        byte[] bytes = QuickSaveRaw.LoadBytes($"{Application.persistentDataPath}/BytesFile.txt");
#pragma warning restore 0219
    }

    public void QuickSaveReaderExample()
    {
        // Use a QuickSaveReader to read content from a file saved with QuickSaveWriter

        // An exception will be thrown if the root doesn't exist

#pragma warning disable 0219
        string one;
        double two;
        Vector2 three;
        Color four;
#pragma warning restore 0219

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
        // Use a QuickSaveWriter to save content to a file, multiple items can be saved to the save file by specifying different keys

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

    public void QuickSaveCsvExample()
    {
        // Use QuickSaveCsv to save and load csv files

        var csv = new QuickSaveCsv();

        csv.SetCell(0, 0, "Test");
        csv.SetCell(3, 5, "Yooooo");
        csv.SetCell(0, 10, 32);
        csv.SetCell(8, 3, true);
        csv.SetCell(8, 0, 23.567);
        csv.Save($"{Application.persistentDataPath}/test.csv");

        csv = QuickSaveCsv.Load($"{Application.persistentDataPath}/test.csv");
#pragma warning disable 0219
        var cell = csv.GetCell(0, 0);
#pragma warning restore 0219
    }
}