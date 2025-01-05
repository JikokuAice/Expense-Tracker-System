namespace ExpenseManagementSystem.Managers;

public interface IserializerAndDeserializerManager
{
    string Serialize<T>(List<T> entity);

    List<T> Deserialize<T>(string value);
}
