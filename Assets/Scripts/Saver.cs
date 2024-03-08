using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices.WindowsRuntime;
using System;
using UnityEngine.SceneManagement;
using UnityEditor.AssetImporters;

public static class Saver{
    private static readonly string PreThing = Application.persistentDataPath + Path.DirectorySeparatorChar;

    public static void Save(string name){
        Metadata data = Metadata.CreateMetadataObject();
        string serialized = JsonUtility.ToJson(data);

        Debug.Log(serialized);

        if(!name.EndsWith(".json")) name += ".json";

        File.WriteAllText(PreThing + name, serialized);
    }
    public static void Read(string name){
        if(!name.EndsWith(".json")) name += ".json";
        if(!Exists(name)){
            Debug.LogError("tried reading from a non existant file");
            throw new Exception("File that u tried to read didnt exist");
        }

        string serialized = File.ReadAllText(PreThing + name);
        Metadata.ReadMetadataObject(JsonUtility.FromJson<Metadata>(serialized));
        Debug.Log(JsonUtility.ToJson(Metadata.CreateMetadataObject()));
    }
    public static bool Exists(string name){
        if(!name.EndsWith(".json")) name += ".json";
        return File.Exists(PreThing + name);
    }
}

[Serializable]
public struct Metadata{
    [Serializable]
    public class SceneDictionary : SerializableDictionary<string, SceneFlag> {}

    public static int LevelNumber = 0; // number of vials collected
    public static SceneDictionary SceneFlags = new SceneDictionary(); // key = name

    public int levelNumber;
    public SceneDictionary sceneFlags;

    public static Metadata CreateMetadataObject(){
        return new Metadata{
            levelNumber = LevelNumber,
            sceneFlags = SceneFlags,
        };
    }
    public static void ReadMetadataObject(Metadata data){
        LevelNumber = data.levelNumber;
        SceneFlags = data.sceneFlags;
    }
    public static void ApplyMetadata(){
        SceneManager.LoadScene(PlayingScene(), LoadSceneMode.Single);
    }
    public static string PlayingScene(){
        foreach(KeyValuePair<string, SceneFlag> kvp in SceneFlags){
            if((kvp.Value & SceneFlag.Playing) != 0){
                return kvp.Key;
            }
        }
        Debug.LogWarning("no playing scene was found in Metadata.SceneFlags");
        return "";
    }
}
[Serializable]
public enum SceneFlag : byte{
    Unplayed   = 0b00000000,
    Completed  = 0b00000001,
    Playing    = 0b00000010,
    Replaying  = 0b00000011,
    EscPlaying = 0b00000100,
    Escaping   = 0b00000111,
}