
using BootstrapBlazor.Components;
using Microsoft.AspNetCore.Components;

namespace NSZX.Components.Layout;

public partial class MainLayout
{
    private bool IsOpen { get; set; }

    private string? Theme { get; set; }

    private string? LayoutClassString => CssBuilder.Default()
        .AddClass(Theme)
        .AddClass("is-fixed-tab", IsFixedTab)
        .Build();

    
    /// <summary>
    /// 获得/设置 是否固定 TabHeader
    /// </summary>
    [Parameter]
    public bool IsFixedTab { get; set; }

    /// <summary>
    /// 获得/设置 是否固定页头
    /// </summary>
    [Parameter]
    public bool IsFixedHeader { get; set; } = true;

    /// <summary>
    /// 获得/设置 是否固定页脚
    /// </summary>
    [Parameter]
    public bool IsFixedFooter { get; set; } = true;

    /// <summary>
    /// 获得/设置 侧边栏是否外置
    /// </summary>
    [Parameter]
    public bool IsFullSide { get; set; } = true;

    /// <summary>
    /// 获得/设置 是否显示页脚
    /// </summary>
    [Parameter]
    public bool ShowFooter { get; set; } = true;

    /// <summary>
    /// 获得/设置 是否开启多标签模式
    /// </summary>
    [Parameter]
    public bool UseTabSet { get; set; } = true;

    /// <summary>
    /// 更新组件方法
    /// </summary>
    public void Update() => StateHasChanged();

    private void ToggleDrawer()
    {
        IsOpen = !IsOpen;
    }
    
}