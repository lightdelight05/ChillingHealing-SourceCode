using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;

public class DuoConverter<P, C> : JsonConverter where P : class, new() where C : P, new()
{
    private string _name;
    private int _startChildNum;
    private int _lastChildNum;

    public DuoConverter(string variableName, int startChildNum, int lastChildNumber)
    {
        _name = variableName;
        _startChildNum = startChildNum;
        _lastChildNum = lastChildNumber;
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(Mission);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject jsonObject = JObject.Load(reader);
        var id = int.Parse(jsonObject[_name]?.ToString());
        bool isChild = id >= _startChildNum && id <= _lastChildNum;

        P parent;
        if (isChild == true)
        {
            parent = new C();
        }
        else
        {
            parent = new P();
        }

        serializer.Populate(jsonObject.CreateReader(), parent);
        return parent;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}

public class DuoConverter2<P, C> : JsonConverter where P : class, new() where C : P, new()
{
    private string _name;
    private CampingType _startChildNum;
    private CampingType _lastChildNum;

    public DuoConverter2(string variableName, CampingType startChildNum, CampingType lastChildNumber)
    {
        _name = variableName;
        _startChildNum = startChildNum;
        _lastChildNum = lastChildNumber;
    }

    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(P);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject jsonObject = JObject.Load(reader);
        CampingType id = (CampingType)Enum.Parse(typeof(CampingType), jsonObject[_name]?.ToString());
        bool isChild = id >= _startChildNum && id <= _lastChildNum;

        P parent;
        if (isChild == true)
        {
            parent = new C();
        }
        else
        {
            parent = new P();
        }

        serializer.Populate(jsonObject.CreateReader(), parent);
        return parent;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}