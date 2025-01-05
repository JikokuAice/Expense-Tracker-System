﻿using System.Text.Json;

namespace ExpenseManagementSystem.Managers;

public class serializerAndDeserializerManager : IserializerAndDeserializerManager
{
    /// <summary>
    /// To help with redundancy
    /// To remove code redundancy such that a single method is created and thus called on both generic repo and authentication service
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    public string Serialize<T>(List<T> entity)
    {
        var json = JsonSerializer.Serialize(entity);

        return json;
    }

    public List<T> Deserialize<T>(string value)
    {
        var result = JsonSerializer.Deserialize<List<T>>(value);

        return result ?? new List<T>();
    } 
}
