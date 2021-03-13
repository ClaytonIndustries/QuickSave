using System.Collections.Generic;
using CI.QuickSave;
using UnityEngine;
using UnityEngine.UI;

public class Test
{
    public Quaternion q;
    public Matrix4x4 m;
    public Color c;
    public List<int> l;
}

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
        Texture2D texture2D = QuickSaveRaw.LoadResource<Texture2D>("bat");

        Sprite p = Sprite.Create(texture2D, new Rect(0f, 0f, 40, 10), new Vector2(0.5f, 0.5f));

        Mesh mesh = new Mesh() { vertices = new Vector3[] { new Vector3(1, 1, 1) }, boneWeights = new BoneWeight[] { new BoneWeight() { boneIndex0 = 12 } } };

        QuickSaveWriter.Create("Inputs", new QuickSaveSettings())
                       .Write("Input1", new Matrix4x4(new Vector4(1, 2, 3, 4), new Vector4(1, 2, 3, 4), new Vector4(1, 2, 3, 4), new Vector4(1, 2, 3, 4)))
                       .Write("Input2", new Bounds(new Vector3(1, 2, 3), new Vector3(23.5f, 34.2f, 234f)))
                       .Write("Input3", mesh)
                       .Write("Input4", p)
                       .Commit();

        //var test = new Test()
        //{
        //    q = new Quaternion(),
        //    m = new Matrix4x4(),
        //    c = new Color(),
        //    l = new List<int>()
        //    {
        //        1, 2, 3, 4, 5, 6
        //    }
        //};

        //QuickSaveWriter.Create("Inputs", new QuickSaveSettings())
        //               .Write("Input1", new Matrix4x4(new Vector4(1, 2, 3, 4), new Vector4(1, 2, 3, 4), new Vector4(1, 2, 3, 4), new Vector4(1, 2, 3, 4)))
        //               .Write("Input2", "Hello")
        //               .Write("Input3", new Vector2())
        //               .Write("Input4", test)
        //               .Commit();


        Content.text = QuickSaveRaw.LoadString("Inputs.json");
    }

    public void Load()
    {
        //Sprite sprite;

        //QuickSaveReader.Create("Inputs", new QuickSaveSettings() { CompressionMode = CompressionMode.Gzip, SecurityMode = SecurityMode.Aes, Password = "HelloWorld" })
        //               .Read<Matrix4x4>("Input1", (r) => { Input5.text = r.ToString(); })
        //               .Read<Bounds>("Input2", (r) => { Input6.text = r.ToString(); })
        //               .Read<Mesh>("Input3", (r) => { Input7.text = r.ToString(); })
        //               .Read<Sprite>("Input4", (r) => { Input8.text = r.ToString(); sprite = r; });



        var reader =  QuickSaveReader.Create("Inputs", new QuickSaveSettings());

        var hi = reader.Read<Mesh>("Input4");
        Input8.text = hi.ToString();
    }

    public void QuickSaveRawExample()
    {
        // Use QuickSaveRaw to directly save / load text or binary data to / from files

        QuickSaveRaw.SaveString("TextFile.txt", "Some text to save");
        QuickSaveRaw.SaveBytes("BytesFile.txt", new byte[] { 1, 2, 3, 4 });

#pragma warning disable 0219
        string text = QuickSaveRaw.LoadString("TextFile.txt");
        byte[] bytes = QuickSaveRaw.LoadBytes("BytesFile.txt");
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
}