using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace wasp.Website.Pages;

public partial class NoteViewComponent
{
    [Parameter]
    public Note Note { get; set; }
    
    [Parameter]
    public EventCallback<MouseEventArgs> OnDeleteNote { get; set; }
    
    [Parameter]
    public EventCallback<MouseEventArgs> OnEditNote { get; set; }
}