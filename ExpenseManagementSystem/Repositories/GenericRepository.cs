﻿using System.Text.Json;
using ExpenseManagementSystem.Managers;

namespace ExpenseManagementSystem.Repositories;

public class GenericRepository(IserializerAndDeserializerManager serializerAndDeserializerManager) : IGenericRepository
{
    public List<T> GetAll<T>(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return new List<T>();
        }

        var json = File.ReadAllText(filePath);
        
        var result = serializerAndDeserializerManager.Deserialize<T>(json);

        return result;
    }

    public void SaveAll<T>(List<T> entity, string directoryPath, string filePath)
    {
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        
        var result = serializerAndDeserializerManager.Serialize<T>(entity);

        File.WriteAllText(filePath, result);    
    }
}
