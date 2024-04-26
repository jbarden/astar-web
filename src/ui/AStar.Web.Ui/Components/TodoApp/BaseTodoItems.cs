using Microsoft.AspNetCore.Components;

namespace AStar.Web.UI.Components.TodoApp;

public abstract class BaseTodoItems : ComponentBase
{
    protected Validations? validations;

    protected string? description;

    protected Filter filter = Filter.All;

    protected List<Todo> todos = new()
        {
            new() { Description = "Buy milk" },
            new() { Description = "Call John regarding the meeting" },
            new() { Description = "Walk a dog" },
        };

    protected IEnumerable<Todo> Todos
    {
        get
        {
            var query = from t in todos select t;

            if(filter == Filter.Active)
            {
                query = from q in query where !q.Completed select q;
            }

            if(filter == Filter.Completed)
            {
                query = from q in query where q.Completed select q;
            }

            return query;
        }
    }

    protected void SetFilter(Filter filter) => this.filter = filter;

    protected void OnCheckAll(bool isChecked) => todos.ForEach(x => x.Completed = isChecked);

    protected async Task OnAddTodo()
    {
        if(await validations!.ValidateAll())
        {
            todos.Add(new() { Description = description });
            description = null;

            await validations.ClearAll();
        }
    }

    protected void OnClearCompleted()
    {
        _ = todos.RemoveAll(x => x.Completed);
        filter = Filter.All;
    }

#pragma warning disable IDE0060 // Remove unused parameter

    protected Task OnTodoStatusChanged(bool isChecked) => InvokeAsync(StateHasChanged);
}
