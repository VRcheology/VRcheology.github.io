using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class Parser
{
    static float width = 50;
    static float depth = 5;

    [MenuItem("Data/ParseDig")]
    static void ParseDig()
    {
        string textData = GetTextData();
        if (!string.IsNullOrEmpty(textData))
        {
            DigData digData = MakeDigData(textData);

            AssetDatabase.CreateAsset(digData, "Assets/Data/DigData.asset");
            AssetDatabase.SaveAssets();
        }
    }

    static string GetTextData()
    {
        string filepath = EditorUtility.OpenFilePanel("Select JSON data", "Assets/Data", "txt");
        filepath = filepath.Substring(filepath.IndexOf("Assets"));

        TextAsset data = (TextAsset)AssetDatabase.LoadAssetAtPath(filepath, typeof(TextAsset));
        if (data != null)
        {
            return data.text;
        }
        Debug.LogWarning("JSON data was not imported");
        return "";
    }

    static DigData MakeDigData(string textData)
    {
        DigData digData = ScriptableObject.CreateInstance<DigData>();

        JObject data = JObject.Parse(textData);
        List<Feature> features = new List<Feature>();
        Vector4 locationRange = GetLocationRange(data);
        int i = 1;
        while (data[i.ToString()] != null)
        {
            features.Add(ParseFeature(data[i.ToString()], locationRange));
            i++;
        }
        digData.features = SortFeatureList(features);

        return digData;
    }

    static Vector4 GetLocationRange(JObject data)
    {
        int i = 1;
        float xMin = Mathf.Infinity;
        float xMax = 0;
        float yMin = Mathf.Infinity;
        float yMax = 0;
        while (data[i.ToString()] != null)
        {
            float x = 0;
            float y = 0;
            float.TryParse(data[i.ToString()]["x"].ToString(), out x);
            float.TryParse(data[i.ToString()]["y"].ToString(), out y);
            if (x > 1 && x < xMin)
            {
                xMin = x;
            }
            if (x > xMax)
            {
                xMax = x;
            }
            if (y > 1 && y < yMin)
            {
                yMin = y;
            }
            if (y > yMax)
            {
                yMax = y;
            }
            i++;
        }

        return new Vector4(xMin, xMax, yMin, yMax);
    }

    static Feature ParseFeature(JToken data, Vector4 locationRange)
    {
        string id = data["id"].ToString();
        FeatureType category = ParseFeatureType(data["category"].ToString());
        float x, y;
        float.TryParse(data["x"].ToString(), out x);
        float.TryParse(data["y"].ToString(), out y);
        x = NormalizeValue(x, locationRange.x, locationRange.y, 0, width - 1);
        y = NormalizeValue(y, locationRange.z, locationRange.w, 0, width - 1);
        float z = Mathf.Round((depth - 1) * Random.Range(0, 1f));

        return new Feature(new Vector3(x, z, y), category, null);
    }

    static float NormalizeValue(float value, float originalMin, float originalMax, float targetMin, float targetMax)
    {
        return Mathf.Round(targetMin + (targetMax - targetMin) * (value - originalMin) / (originalMax - originalMin));
    }

    static FeatureType ParseFeatureType(string type)
    {
        switch (type)
        {
            case "Architectural Element":
                return FeatureType.ArchitecturalElement;

            case "Animal Bone":
                return FeatureType.AnimalBone;

            case "Pottery":
                return FeatureType.Pottery;

            case "Object":
                return FeatureType.Object;

            default:
                return FeatureType.None;
        }
    }

    static Feature[] SortFeatureList(List<Feature> features)
    {
        //sort list by features[i].location.x, within features with same x sort by features[i].location.z

        return features.ToArray();
    }
}
