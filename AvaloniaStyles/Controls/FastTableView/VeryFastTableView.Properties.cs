using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using WDE.Common.Utils;

namespace AvaloniaStyles.Controls.FastTableView;

public class ColumnPressedEventArgs : RoutedEventArgs
{
    public int ColumnIndex { get; set; } = -1;
    public ITableColumnHeader? Column { get; set; }
    public KeyModifiers KeyModifiers { get; set; }
}

public partial class VeryFastTableView
{
    public event System.Action<string>? ValueUpdateRequest;
    public static readonly StyledProperty<IReadOnlyList<int>?> HiddenColumnsProperty = AvaloniaProperty.Register<VeryFastTableView, IReadOnlyList<int>?>(nameof(HiddenColumns));
    public static readonly StyledProperty<IReadOnlyList<ITableColumnHeader>?> ColumnsProperty = AvaloniaProperty.Register<VeryFastTableView, IReadOnlyList<ITableColumnHeader>?>(nameof(Columns));
    public static readonly StyledProperty<IReadOnlyList<ITableRowGroup>?> ItemsProperty = AvaloniaProperty.Register<VeryFastTableView, IReadOnlyList<ITableRowGroup>?>(nameof(Items));
    public static readonly StyledProperty<VerticalCursor> SelectedRowIndexProperty = AvaloniaProperty.Register<VeryFastTableView, VerticalCursor>(nameof(SelectedRowIndex), defaultValue: VerticalCursor.None, defaultBindingMode: BindingMode.TwoWay);
    public static readonly StyledProperty<int> SelectedCellIndexProperty = AvaloniaProperty.Register<VeryFastTableView, int>(nameof(SelectedCellIndex), defaultValue:-1, defaultBindingMode: BindingMode.TwoWay);
    public static readonly StyledProperty<ICustomCellDrawer?> CustomCellDrawerProperty = AvaloniaProperty.Register<VeryFastTableView, ICustomCellDrawer?>(nameof(CustomCellDrawer));
    public static readonly StyledProperty<ICustomCellInteractor?> CustomCellInteractorProperty = AvaloniaProperty.Register<VeryFastTableView, ICustomCellInteractor?>(nameof(CustomCellInteractor));
    public static readonly StyledProperty<ITableMultiSelection> MultiSelectionProperty = AvaloniaProperty.Register<VeryFastTableView, ITableMultiSelection>(nameof(MultiSelection), defaultValue: new TableMultiSelection());
    public static readonly StyledProperty<IDataTemplate?> GroupHeaderTemplateProperty = AvaloniaProperty.Register<VeryFastTableView, IDataTemplate?>(nameof(GroupHeaderTemplate));
    public static readonly StyledProperty<bool> InteractiveHeaderProperty = AvaloniaProperty.Register<VeryFastTableView, bool>(nameof(InteractiveHeader));
    public static readonly StyledProperty<string> RowFilterParameterProperty = AvaloniaProperty.Register<VeryFastTableView, string>("RowFilterParameter");
    public static readonly StyledProperty<bool> IsGroupingEnabledProperty = AvaloniaProperty.Register<VeryFastTableView, bool>("IsGroupingEnabled");

    public static readonly RoutedEvent<ColumnPressedEventArgs> ColumnPressedEvent = RoutedEvent.Register<VeryFastTableView, ColumnPressedEventArgs>("ColumnPressed", RoutingStrategies.Tunnel | RoutingStrategies.Bubble);

    private List<bool> columnVisibility = new List<bool>();
    public static readonly StyledProperty<IRowFilterPredicate?> RowFilterProperty = AvaloniaProperty.Register<VeryFastTableView, IRowFilterPredicate?>("RowFilter");

    public IReadOnlyList<int>? HiddenColumns
    {
        get => GetValue(HiddenColumnsProperty);
        set => SetValue(HiddenColumnsProperty, value);
    }
    
    public event EventHandler<ColumnPressedEventArgs> ColumnPressed
    {
        add => AddHandler(ColumnPressedEvent, value, RoutingStrategies.Direct | RoutingStrategies.Bubble, false);
        remove => RemoveHandler(ColumnPressedEvent, value);
    }
    
    public IReadOnlyList<ITableRowGroup>? Items
    {
        get => GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }
    
    public IReadOnlyList<ITableColumnHeader>? Columns
    {
        get => GetValue(ColumnsProperty);
        set => SetValue(ColumnsProperty, value);
    }

    public VerticalCursor SelectedRowIndex
    {
        get => GetValue(SelectedRowIndexProperty);
        set => SetValue(SelectedRowIndexProperty, value);
    }

    public int SelectedCellIndex
    {
        get => GetValue(SelectedCellIndexProperty);
        set => SetValue(SelectedCellIndexProperty, value);
    }
    
    public ICustomCellDrawer? CustomCellDrawer
    {
        get => GetValue(CustomCellDrawerProperty);
        set => SetValue(CustomCellDrawerProperty, value);
    }
    
    public ICustomCellInteractor? CustomCellInteractor
    {
        get => GetValue(CustomCellInteractorProperty);
        set => SetValue(CustomCellInteractorProperty, value);
    }

    public ITableMultiSelection MultiSelection
    {
        get => GetValue(MultiSelectionProperty);
        set => SetValue(MultiSelectionProperty, value);
    }

    public IDataTemplate? GroupHeaderTemplate
    {
        get => GetValue(GroupHeaderTemplateProperty);
        set => SetValue(GroupHeaderTemplateProperty, value);
    }

    public bool InteractiveHeader
    {
        get => GetValue(InteractiveHeaderProperty);
        set => SetValue(InteractiveHeaderProperty, value);
    }
    
    public string RowFilterParameter
    {
        get => GetValue(RowFilterParameterProperty);
        set => SetValue(RowFilterParameterProperty, value);
    }

    public IRowFilterPredicate? RowFilter
    {
        get => (IRowFilterPredicate?)GetValue(RowFilterProperty);
        set => SetValue(RowFilterProperty, value);
    }
    
    public bool IsGroupingEnabled
    {
        get => (bool)GetValue(IsGroupingEnabledProperty);
        set => SetValue(IsGroupingEnabledProperty, value);
    }
}