using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace wasp.Website.Pages;

public class Note
{
    public string Message { get; set; }
    public Guid Guid { get; set; }

    public DateTimeOffset Created { get; }

    public Note(string message)
    {
        Guid = Guid.NewGuid();
        Message = message;
        Created = DateTimeOffset.UtcNow;
    }
}

public interface IDynamicTitle
{
    string GetTitle();
}

public partial class Index : IDynamicTitle
{
    public string GetTitle() => "// Index";
    
    public IList<Note> Notes { get; set; } = new List<Note>();
    public string NewComment { get; set; }
    public Guid CurrentGuid { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        Notes = new List<Note>();

        await base.OnInitializedAsync();
    }

    protected void OnDeleteNote(MouseEventArgs mouseEventArgs, Note note)
    {
        if (Notes?.Any(n => n == note) ?? false)
        {
            Notes.Remove(Notes.First(n => n == note));
        }
    }
    protected void OnEditNote(MouseEventArgs mouseEventArgs, Note note)
    {
        if (Notes?.Any(n => n.Guid == note.Guid) ?? false)
        {
            NewComment = note.Message;
            CurrentGuid = note.Guid;
        }
    }
    
    protected void OnSubmitNote(MouseEventArgs mouseEventArgs)
    {
        if (CurrentGuid == Guid.Empty)
        {
            Notes.Add(new Note(NewComment));
            NewComment = string.Empty;    
        }
        else
        {
            Note note = Notes.Single(n => n.Guid == CurrentGuid);
            note.Message = NewComment;
            NewComment = string.Empty;
        }

        CurrentGuid = Guid.Empty;
    }

}