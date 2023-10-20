using System.IO;
using UnityEngine;

public static class SaveTextSystem
{
    private static readonly string SaveFile = Application.dataPath + "save.txt";

    public static void Save(string text)
    {
        StreamWriter writer = new StreamWriter(SaveFile, false);

        writer.WriteLine(text);

        writer.Close();
    }

    public static string Load()
    {
        if(File.Exists(SaveFile))
        {
            var reader = new StreamReader(SaveFile);

            var fileContent = reader.ReadToEnd();

            reader.Close();

            return fileContent;
        }
        else
        {
            return null;
        }
    }
}
