using System.Text.Json;

namespace ExpenseManagementSystem.Managers;

public class serializerAndDeserializerManager : IserializerAndDeserializerManager
{
   
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

