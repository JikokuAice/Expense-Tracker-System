﻿using Microsoft.AspNetCore.Components;

namespace ExpenseManagementSystem.Components.Layout;

public partial class ModalDialog
{
    [Parameter] public string Title { get; set; }
    
    [Parameter] public string Size { get; set; } = "";
    
    [Parameter] public RenderFragment BodyContent { get; set; }
}