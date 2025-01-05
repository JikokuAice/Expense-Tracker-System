namespace ExpenseManagementSystem.Managers;

public interface IlocalStorage
{
    Task<T?> GetItemAsync<T>(string key);

    Task SetItemAsync<T>(string key, T value);

    Task ClearItemAsync(string key);
}
