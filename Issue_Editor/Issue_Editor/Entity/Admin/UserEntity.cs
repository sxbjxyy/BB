
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using FreeSql;
using FreeSql.DataAnnotations;

namespace RR.Entity;

public class UserEntity:BaseEntity<UserEntity,int>
{
    [NotNull] [DisplayName("内容")] public string Content { get; set; } 
}
