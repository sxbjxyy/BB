using BootstrapBlazor.Components;
using FreeSql;
using NSZX.Components;
using NSZX.Service;

var builder = WebApplication.CreateBuilder(args);

// Sqlite
// NuGet:FreeSql.Provider.Sqlite  而非：FreeSql.Provider.SqliteCore
// 代码将会自动在项目文件夹建立 document.db
var connStr = builder.Configuration["DB:SqliteConnStr"];
IFreeSql fsql = new FreeSql.FreeSqlBuilder()
    .UseConnectionString(FreeSql.DataType.Sqlite, connStr)
    .UseAutoSyncStructure(true)           //自动将实体结构同步到数据库
    .Build();                             //一定要定义为singleton模式
//=========================================================================

BaseEntity.Initialization(fsql, null);   //初始化 BaseEntity

// Add services to the container.

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddBootstrapBlazor();
builder.Services.AddScoped(typeof(IDataService<>), typeof(FreeSqlDataService<>));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
