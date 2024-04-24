using BootstrapBlazor.Components;
using FreeSql;
using Microsoft.VisualBasic;
using NSZX.Extensions;

namespace NSZX.Service;

public class FreeSqlDataService<TMode> : DataServiceBase<TMode> where TMode : class, new()
{
    private readonly IFreeSql _db = BaseEntity.Orm;

    /// <summary>
    /// 删除方法
    /// </summary>
    /// <param name="models"></param>
    /// <returns></returns>
    public override async Task<bool> DeleteAsync(IEnumerable<TMode> models)
    {
        //通过模型获取主键列数据，支持批量删除
        await _db.Delete<TMode>(models).ExecuteAffrowsAsync();
        return true;
    }

    
    // public async Task<bool> DeleteAsync(List<String> sqls)
    // {
    //     //通过模型获取主键列数据，支持批量删除
    //     //await _db.Delete<TMode>(models).ExecuteAffrowsAsync();
    //     return true;
    // }
    //
    // public async Task<bool> DeleteAsync(TMode model,  List<string> where)
    // {
    //     //通过 model 模型 where 条件的数据
    //     foreach (var sw in where)
    //     {
    //         await _db.Delete<TMode>().Where(where)
    //     }
    //     //await _db.Delete<TMode>(models).ExecuteAffrowsAsync();
    //     return true;
    // }
    
    
    
    
    
    /// <summary>
    /// 保存方法
    /// </summary>
    /// <param name="model"></param>
    /// <param name="changedType"></param>
    /// <returns></returns>
    public override async Task<bool> SaveAsync(TMode model, ItemChangedType changedType)
    {
        await _db.GetRepository<TMode>().InsertOrUpdateAsync(model);
        return true;
    }

    public override Task<QueryData<TMode>> QueryAsync(QueryPageOptions option)
    {
        var select = _db.Select<TMode>().WhereDynamicFilter(option.ToDynamicFilter())
            .OrderByPropertyNameIf(option.SortOrder != SortOrder.Unset, option.SortName,
                option.SortOrder == SortOrder.Asc)
            .Count(out var count);

        if (option.IsPage)
        {
            select = select.Page(option.PageIndex, option.PageItems);
        }
        var Items = select.ToList();
        var ret = new QueryData<TMode>()
        {
            TotalCount = (int)count,
            Items = Items,
            IsSorted = option.SortOrder != SortOrder.Unset,
            IsFiltered = option.Filters.Any(),
            IsAdvanceSearch = option.AdvanceSearches.Any(),
            IsSearch = option.Searches.Any() || option.CustomerSearches.Any()
        };
        return Task.FromResult(ret);
    }
}