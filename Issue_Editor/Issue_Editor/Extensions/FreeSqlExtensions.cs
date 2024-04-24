using BootstrapBlazor.Components;
using FreeSql.Internal.Model;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Xml;

namespace NSZX.Extensions;

/// <summary>
/// FreeSql 扩展方法
/// </summary>
public static class FreeSqlExtensions
{
    public static DynamicFilterInfo ToDynamicFilter(this QueryPageOptions option)
    {
        var ret = new DynamicFilterInfo() { Filters = new List<DynamicFilterInfo>() };
        if (option.Searches.Any())
        {
            //处理模糊搜索
            ret.Filters.Add(new DynamicFilterInfo()
            {
                Logic = DynamicFilterLogic.Or,
                Filters = option.Searches.Select(i => i.ToDynamicFilter()).ToList()
            });

            //处理自定义搜索
            if (option.CustomerSearches.Any())
            {
                ret.Filters.AddRange(option.CustomerSearches.Select(i => i.ToDynamicFilter()));
            }
            //处理高级搜索
            if (option.AdvanceSearches.Any())
            {
                ret.Filters.AddRange(option.AdvanceSearches.Select(i => i.ToDynamicFilter()));
            }
            //处理表格过滤条件
            if (option.Filters.Any())
            {
                ret.Filters.AddRange(option.Filters.Select(i => i.ToDynamicFilter()));
            }
        }
        return ret;
    }
    private static DynamicFilterInfo ToDynamicFilter(this IFilterAction filter)
    {
        var actions = filter.GetFilterConditions();
        var item = new DynamicFilterInfo();
        if (actions.Filters != null)
        {
            if (actions.Filters.Count == 2)
            {
                item.Logic = actions.FilterLogic.ToDynamicFilterLogic();
                item.Filters = actions.Filters.Select(i => new DynamicFilterInfo()
                {
                    Field = i.FieldKey,
                    Value = i.FieldValue,
                    Operator = i.FilterAction.ToDynamicFilterOperator()
                }).ToList();
            }

            else
            {
                var c = actions.Filters.First();
                item.Field = c.FieldKey;
                item.Value = c.FieldValue;
                item.Operator = c.FilterAction.ToDynamicFilterOperator();
            }
        }
        return item;
    }

    private static DynamicFilterLogic ToDynamicFilterLogic(this FilterLogic logic) => logic switch
    {
            FilterLogic.And=> DynamicFilterLogic.And,
            _ =>DynamicFilterLogic.Or
    };

    private static DynamicFilterOperator ToDynamicFilterOperator(this FilterAction aciton) => aciton switch
    {
        FilterAction.Equal=>DynamicFilterOperator.Equal,
        FilterAction.NotEqual => DynamicFilterOperator.NotEqual,
        FilterAction.Contains => DynamicFilterOperator.Contains,
        FilterAction.NotContains => DynamicFilterOperator.NotContains,
        FilterAction.GreaterThan => DynamicFilterOperator.GreaterThan,
        FilterAction.GreaterThanOrEqual => DynamicFilterOperator.GreaterThanOrEqual,
        FilterAction.LessThan => DynamicFilterOperator.LessThan,
        FilterAction.LessThanOrEqual    => DynamicFilterOperator.LessThanOrEqual,
        _ => throw new NotSupportedException()
    };
    
}
