using System.Threading.Tasks;
using WDE.Common.Managers;
using WDE.Common.Providers;
using WDE.Common.TableData;
using WDE.Module.Attributes;

namespace WoWDatabaseEditorCore.Services.TabularDataPickerService;

[AutoRegister]
public class TabularDataPicker : ITabularDataPicker
{
    private readonly IWindowManager windowManager;

    public TabularDataPicker(IWindowManager windowManager)
    {
        this.windowManager = windowManager;
    }
    
    public async Task<T?> PickRow<T>(ITabularDataArgs<T> args, int defaultSelection)
    {
        using var viewModel = new Avalonia.Services.TabularDataPickerService.TabularDataPickerViewModel(args.AsObject(), defaultSelection);
        await windowManager.ShowDialog(viewModel);
        return viewModel.SelectedItem == null ? default : (T)viewModel.SelectedItem;
    }
}