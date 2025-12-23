namespace Blazor.Component.Card.TaskListOverview.Dialog.TaskList;

public partial class HubAddTaskItem : ComponentBase
{
    [CascadingParameter]
    private IMudDialogInstance MudDialog { get; set; }

    private string _taskName = "";
    private string _room = "";
    private string _assignee = "";
    private Frequency _frequency = Frequency.Daily;

    private void Cancel() => MudDialog.Cancel();

    private void Submit()
    {
        // Return a new TaskItem record to the caller
        var newItem = new TaskItem(0, _taskName, _room, _assignee, _frequency);
        MudDialog.Close(DialogResult.Ok(newItem));
    }
}