using ExpenseManagementSystem.Models;
using ExpenseManagementSystem.DTOs.Tags;
using ExpenseManagementSystem.Filters.Tags;
using ExpenseManagementSystem.Repositories;
using ExpenseManagementSystem.Managers.Helper;
using ExpenseManagementSystem.Models.Constant;
using ExpenseManagementSystem.Services.Interfaces;

namespace ExpenseManagementSystem.Services;

public class TagService(IGenericRepository genericRepository, IUserServices userService) : ITagServices
{
    public GetTagDto GetTagFromID(Guid tagId)
    {
        var tagList = genericRepository.GetAll<Tag>(Constants.FilePath.AppTagsDirectoryPath);

        var tag = tagList.FirstOrDefault(x => x.Id == tagId);

        if (tag == null)
        {
            throw new Exception("A tag with following identifier couldn't be found.");
        }

        var result = new GetTagDto
        {
            Id = tag.Id,
            Name = tag.Name,
            BackgroundColor = tag.BackgroundColor,
            TextColor = tag.TextColor,
            IsDefault = tag.IsDefault
        };

        return result;
    }

    public async Task<List<GetTagDto>> GetAllTags(GetTagFilterRequestDto tagFilterRequest)
    {
        var tagList = genericRepository.GetAll<Tag>(Constants.FilePath.AppTagsDirectoryPath);
        
        var userDetails = await userService.FetchUserDetail();

        if (userDetails == null)
        {
            throw new Exception("You are not logged in.");
        }
        
        tagList = tagList.Where(x => x.IsDefault || x.CreatedBy == userDetails.Id).ToList();

        if (!string.IsNullOrEmpty(tagFilterRequest.Search))
        {
            tagList = tagList.Where(x => x.Name.Contains(tagFilterRequest.Search, StringComparison.OrdinalIgnoreCase)).ToList();
        }
        
        if (!string.IsNullOrEmpty(tagFilterRequest.OrderBy))
        {
            tagList = tagFilterRequest.OrderBy switch
            {
                "Name" => tagFilterRequest.IsDescending ? tagList.OrderByDescending(x => x.Name).ToList() : tagList.OrderBy(x => x.Name).ToList(),
                _ => tagList
            };
        }
        
        // Initialization of GetTagDto List
        var result = new List<GetTagDto>();

        // Iterating through all records and data of tags
        foreach (var tag in tagList)
        {
            // Addition of new data transfer object to the result list.
            result.Add(new GetTagDto
            {
                Id = tag.Id,
                Name = tag.Name,
                BackgroundColor = tag.BackgroundColor,
                TextColor = tag.TextColor,
                IsDefault = tag.IsDefault
            }); 
        }

        return result;
    }

    public async Task InsertTag(InsertTagDto tag)
    {
        var userDetails = await userService.FetchUserDetail();

        if (userDetails == null)
        {
            throw new Exception("You are not logged in.");
        }

        var tagModel = new Tag
        {
            Id = Guid.NewGuid(), 
            Name = tag.Name,
            BackgroundColor = tag.BackgroundColor.ToHexCode(),
            TextColor = tag.TextColor.ToHexCode(),
            CreatedBy = userDetails.Id,
            CreatedAt = DateTime.Now,
        };

        var tags = genericRepository.GetAll<Tag>(Constants.FilePath.AppTagsDirectoryPath);

        tags.Add(tagModel);

        genericRepository.SaveAll(tags, Constants.FilePath.AppDataDirectoryPath, Constants.FilePath.AppTagsDirectoryPath);
    }

    public async Task UpdateTag(UpdateTagDto tag)
    {
        var userDetails = await userService.FetchUserDetail();

        if (userDetails == null)
        {
            throw new Exception("You are not logged in.");
        }

        var tags = genericRepository.GetAll<Tag>(Constants.FilePath.AppTagsDirectoryPath);

        var tagModel = tags.FirstOrDefault(x => x.Id == tag.Id);

        if (tagModel == null)
        {
            throw new Exception("A tag with the following identifier couldn't be found.");
        }

        tagModel.Name = tag.Name;
        tagModel.BackgroundColor = tag.BackgroundColor.ToHexCode();
        tagModel.TextColor = tag.TextColor.ToHexCode();
        tagModel.LastModifiedBy = userDetails.Id;
        tagModel.LastModifiedAt = DateTime.Now;

        genericRepository.SaveAll(tags, Constants.FilePath.AppDataDirectoryPath, Constants.FilePath.AppTagsDirectoryPath);
    }

    public void ActivateDeactivateTag(ActivateDeactivateTagDto tag)
    {
        var tagList = genericRepository.GetAll<Tag>(Constants.FilePath.AppTagsDirectoryPath);

        var tagModel = tagList.FirstOrDefault(x => x.Id == tag.Id);

        if (tagModel == null)
        {
            throw new Exception("A tag with the following identifier couldn't be found.");
        }

        tagList.Remove(tagModel);

        genericRepository.SaveAll(tagList, Constants.FilePath.AppDataDirectoryPath, Constants.FilePath.AppTagsDirectoryPath);
    }
}
