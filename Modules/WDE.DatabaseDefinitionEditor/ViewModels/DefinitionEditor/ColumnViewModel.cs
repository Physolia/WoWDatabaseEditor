using System;
using System.Globalization;
using System.Linq;
using AvaloniaStyles.Controls.FastTableView;
using DynamicData.Binding;
using PropertyChanged.SourceGenerator;
using WDE.Common.Parameters;
using WDE.Common.Utils;
using WDE.DatabaseDefinitionEditor.ViewModels.DefinitionEditor.MetaColumns;
using WDE.DatabaseDefinitionEditor.Views.DefinitionEditor;
using WDE.DatabaseEditors.Data.Structs;
using WDE.MVVM;

namespace WDE.DatabaseDefinitionEditor.ViewModels.DefinitionEditor;

public partial class ColumnViewModel : ObservableBase, ITableColumnHeader, ITableCell
{
    [Notify] private int intWidth;
    [Notify] private DatabaseSourceColumnViewModel? databaseColumnName;
    [Notify] private string header;
    [Notify] private ColumnValueTypeViewModel? valueType;
    [Notify] [AlsoNotify(nameof(IsDatabaseColumnType), nameof(IsMetaColumnType), nameof(IsConditionColumnType))] private ColumnType columnType;
    [Notify] private bool isReadOnly;
    [Notify] private bool canBeNull;
    [Notify] private bool isZeroBlank;
    [Notify] private bool isAutoIncrement;
    [Notify] private string? autogenerateComment;
    [Notify] private string? stringValue;
    [Notify] private string? defaultValue;
    [Notify] private string? help;
    [Notify] private string? columnIdForUi;
    [Notify] private bool isPrimaryKey;
    [Notify] private BaseMetaColumnViewModel? metaViewModel;
    [Notify] private ColumnGroupViewModel parent;
    [Notify] private MetaColumnTypeViewModel? selectedMetaColumnType;
    public string? Preview { get; private set; }

    public ObservableCollectionExtended<DatabaseSourceColumnViewModel> DatabaseSourceColumns { get; }
    public ObservableCollectionExtended<ColumnValueTypeViewModel> AllParameters { get; }
    public AsyncAutoCommand<ColumnViewModel> PickParameterCommand { get; }
    
    public ColumnViewModel(ColumnGroupViewModel parent,
        DatabaseColumnJson model)
    {
        this.parent = parent;
        DatabaseSourceColumns = parent.Parent.DatabaseSourceColumns;
        AllParameters = parent.Parent.Parent.AllParameters;
        PickParameterCommand = parent.Parent.Parent.PickParameterCommand;
        header = model.Name;
        isReadOnly = model.IsReadOnly;
        canBeNull = model.CanBeNull;
        isZeroBlank = model.IsZeroBlank;
        isAutoIncrement = model.AutoIncrement;
        autogenerateComment = model.AutogenerateComment;
        help = model.Help;
        columnIdForUi = model.ColumnIdForUi;
        intWidth = (int?)model.PreferredWidth ?? 100;
        if (model.IsMetaColumn)
        {
            ColumnType = ColumnType.Meta;
        }
        else if (model.IsConditionColumn)
            ColumnType = ColumnType.Condition;
        else
            ColumnType = ColumnType.Database;
        
        On(() => StringValue, _ => UpdatePreview());
        On(() => IsZeroBlank, _ => UpdatePreview());
        On(() => ValueType, _ => UpdatePreview());
        On(() => ColumnType, x =>
        {
        });
        On(() => SelectedMetaColumnType, type =>
        {
            if (type != null)
                MetaViewModel = type.Factory(this);
        });
        if (IsMetaColumnType && model.Meta != null)
        {
            SelectedMetaColumnType = parent.Parent.Parent.MetaColumnFactory.Types.FirstOrDefault(x => model.Meta!.StartsWith(x.Prefix));
            MetaViewModel = parent.Parent.Parent.MetaColumnFactory.Factory(this, model.Meta);
        }
        
        if (model.Default != null && IsDatabaseColumnType)
        {
            if (model.IsTypeString || model.Default is string)
                defaultValue = model.Default.ToString();
            else if (model.IsTypeFloat)
                defaultValue = Convert.ToSingle(model.Default).ToString(CultureInfo.InvariantCulture);
            else if (model.IsTypeLong)
                defaultValue = Convert.ToInt64(model.Default).ToString();
            else
                throw new Exception("Unsupported column type: neither string nor float nor long");
            StringValue = defaultValue;
        }
    }

    private void UpdatePreview()
    {
        Preview = null;
        if (ValueType != null)
        {
            if (ValueType.IsParameter)
            {
                if (ValueType.Parameter is IParameter<long> longP)
                {
                    long.TryParse(stringValue, out var longValue);
                        
                    if (IsZeroBlank && longValue == 0)
                        Preview = "";
                    else
                        Preview = longP.ToString(longValue);
                }
                else if (ValueType.Parameter is IParameter<string> stringP)
                {
                    Preview = stringP.ToString(stringValue ?? "");
                }
            }
            else
            {
                if (ValueType.ValueTypeName == "string")
                    Preview = stringValue;
                else if (ValueType.ValueTypeName == "float")
                {
                    float.TryParse(stringValue, out var f);
                    if (isZeroBlank && f == 0)
                        Preview = "";
                    else
                        Preview = f.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    long.TryParse(stringValue, out var l);
                    if (IsZeroBlank && l == 0)
                        Preview = "";
                    else
                        Preview = l.ToString();
                }
            }
        }
        RaisePropertyChanged(nameof(Preview));
    }

    public bool IsDatabaseColumnType => ColumnType == ColumnType.Database;
    public bool IsMetaColumnType => ColumnType == ColumnType.Meta;
    public bool IsConditionColumnType => ColumnType == ColumnType.Condition;
    
    public bool IsVisible => true;

    public double Width
    {
        get => intWidth;
        set
        {
            intWidth = (int)value;
            RaisePropertyChanged(nameof(IntWidth));
        }
    }

    public void UpdateFromString(string newValue)
    {
        StringValue = newValue;
    }

    public override string ToString()
    {
        return Preview ?? "(null)";
    }
}