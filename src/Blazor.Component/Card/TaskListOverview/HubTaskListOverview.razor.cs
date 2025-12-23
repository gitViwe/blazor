using Blazor.Component.Card.TaskListOverview.Dialog.TaskList;

namespace Blazor.Component.Card.TaskListOverview;

public record TaskItem(
    int Id,
    string Name,
    string Room,
    string Assignee,
    Frequency Frequency)
{
    public bool Completed { get; set; }
}

public enum Frequency
{
    Daily = 1,
    Weekly,
    Monthly,
}

internal record GroupedTaskItem(
    string Key,
    IEnumerable<TaskItem> TaskItems)
{
    public int DoneCount => TaskItems.Count(i => i.Completed);
    public int TotalCount => TaskItems.Count();
}

public partial class HubTaskListOverview : ComponentBase
{
    [Inject] private IDialogService DialogService { get; set; }
    
    /// <summary>
    /// The task items
    /// </summary>
    [Parameter, EditorRequired]
    public required IList<TaskItem> TaskItemCollection { get; init; }
    
    [Parameter, EditorRequired]
    public EventCallback OnToggleButtonClick { get; init; }
    
    [Parameter, EditorRequired]
    public EventCallback OnDeleteButtonClick { get; init; }
    
    [Parameter, EditorRequired]
    public EventCallback OnAddButtonClick { get; init; }
    
    private int _selectedPanelFrequency = 0;
    
    private IEnumerable<GroupedTaskItem> GetGroupedTasks()
    {
        // 1. Determine the filter criteria once
        var filterApplied = Enum.IsDefined(typeof(Frequency), _selectedPanelFrequency);
        var targetFrequency = (Frequency)_selectedPanelFrequency;

        return TaskItemCollection
            // 2. Group the full collection to ensure every Room key exists
            .GroupBy(item => item.Room)
            .Select(grouping => 
            {
                // 3. Filter the items within the group
                var filteredItems = filterApplied 
                    ? grouping.Where(c => c.Frequency == targetFrequency).ToArray()
                    : grouping.ToArray();

                // 4. Return the group, even if filteredItems is empty
                return new GroupedTaskItem(grouping.Key, TaskItems: filteredItems);
            })
            .OrderBy(item => item.Key);
    }

    private void OnPanelIndexChanged(int index) => _selectedPanelFrequency = index;

    private static Color GetFrequencyColor(Frequency frequency) => frequency switch {
        Frequency.Daily => Color.Primary,
        Frequency.Weekly => Color.Secondary,
        _ => Color.Tertiary
    };
    
    private async Task ToggleTaskItemAsync(int id)
    {
        var task = TaskItemCollection.FirstOrDefault(t => t.Id == id);
        task?.Completed = !task.Completed;
        await OnToggleButtonClick.InvokeAsync();
    }
    
    private async Task DeleteTaskItemAsync(TaskItem item)
    {
        TaskItemCollection.Remove(item);
        await OnDeleteButtonClick.InvokeAsync();
    }

    private async Task AddTaskItemAsync()
    {
        var options = new DialogOptions { CloseOnEscapeKey = true, FullWidth = true };
        var dialog = await DialogService.ShowAsync<HubAddTaskItem>("Add New Task", options);
        var result = await dialog.Result;
        
        if (result is { Canceled: false, Data: TaskItem newTask })
        {
            // 1. Generate a simple ID (in a real app, the DB would do this)
            int newId = TaskItemCollection.Any() ? TaskItemCollection.Max(t => t.Id) + 1 : 1;

            var finalTask = newTask with { Id = newId };

            TaskItemCollection.Add(finalTask);

            await OnAddButtonClick.InvokeAsync(); 
        }
    }
}