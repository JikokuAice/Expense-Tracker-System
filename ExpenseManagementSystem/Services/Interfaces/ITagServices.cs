using ExpenseManagementSystem.DTOs.Tags;
using ExpenseManagementSystem.Filters.Tags;

namespace ExpenseManagementSystem.Services.Interfaces;

public interface ITagServices
{
 
    GetTagDto GetTagFromID(Guid tagId); 
    
    Task<List<GetTagDto>> GetAllTags(GetTagFilterRequestDto tagFilterRequest);

    Task InsertTag(InsertTagDto tag);

    Task UpdateTag(UpdateTagDto tag);

    void ActivateDeactivateTag(ActivateDeactivateTagDto tag);
}
