module ImGuiNET.FSharp.Styles

open System.Drawing
open System.Numerics

open ImGuiNET

let colorToVec4 (c:Color) = Vector4(float32(int c.R), float32(int c.G), float32(int c.B), float32(int c.A))

let setGreenColorScheme() = 
    let colors = ImGui.GetStyle().Colors

    let style = ImGui.GetStyle()

    //style.GrabRounding<-4.0f
    style.FramePadding <- Vector2(4f,5f);
    style.ItemSpacing <- Vector2(5f, 10f);

    style.PopupBorderSize <- 1f;
    style.FrameBorderSize <- 1f;

    colors.[int ImGuiCol.Text]                   <- Vector4(0f, 0f, 0f, 1f);
    colors.[int ImGuiCol.TextDisabled]           <- Vector4(0.50f, 0.50f, 0.50f, 1.00f);
    colors.[int ImGuiCol.WindowBg]               <- Vector4(1f, 1f, 1f, 1f);
    colors.[int ImGuiCol.ChildBg]                <- Vector4(0.00f, 0.00f, 0.00f, 0.00f);
    colors.[int ImGuiCol.PopupBg]                <- Vector4(1f, 1f, 1f, 1f);
    colors.[int ImGuiCol.Border]                 <- Vector4(0.43f, 0.43f, 0.50f, 0.50f);
    colors.[int ImGuiCol.BorderShadow]           <- Vector4(0.00f, 0.00f, 0.00f, 0.00f);
    colors.[int ImGuiCol.FrameBg]                <- Vector4(0.99f, 0.99f, 0.99f, 1f);
    colors.[int ImGuiCol.FrameBgHovered]         <- Vector4(0.57f, 0.57f, 0.57f, 0.70f);
    colors.[int ImGuiCol.FrameBgActive]          <- Vector4(0.76f, 0.76f, 0.76f, 0.80f);
    colors.[int ImGuiCol.TitleBg]                <- Vector4(0.04f, 0.04f, 0.04f, 1.00f);
    colors.[int ImGuiCol.TitleBgActive]          <- Vector4(0.16f, 0.16f, 0.16f, 1.00f);
    colors.[int ImGuiCol.TitleBgCollapsed]       <- Vector4(0.00f, 0.00f, 0.00f, 0.60f);
    colors.[int ImGuiCol.MenuBarBg]              <- Vector4(0.14f, 0.14f, 0.14f, 1.00f);
    colors.[int ImGuiCol.ScrollbarBg]            <- Vector4(1f, 1f, 1f, 1f);
    colors.[int ImGuiCol.ScrollbarGrab]          <- Vector4(0.31f, 0.31f, 0.31f, 1.00f);
    colors.[int ImGuiCol.ScrollbarGrabHovered]   <- Vector4(0.41f, 0.41f, 0.41f, 1.00f);
    colors.[int ImGuiCol.ScrollbarGrabActive]    <- Vector4(0.51f, 0.51f, 0.51f, 1.00f);
    colors.[int ImGuiCol.CheckMark]              <- Vector4(0.13f, 0.75f, 0.55f, 0.80f);
    colors.[int ImGuiCol.SliderGrab]             <- Vector4(0f, 0.7f, 0f, 1f);
    colors.[int ImGuiCol.SliderGrabActive]       <- Vector4(0f, 0.3f, 0f, 1f);
    colors.[int ImGuiCol.Button]                 <- Vector4(0f, 0.7f, 0f, 1f);
    colors.[int ImGuiCol.ButtonHovered]          <- Vector4(0.13f, 0.75f, 0.75f, 0.60f);
    colors.[int ImGuiCol.ButtonActive]           <- Vector4(0.13f, 0.75f, 1.00f, 0.80f);
    colors.[int ImGuiCol.Header]                 <- Vector4(0.13f, 0.75f, 0.55f, 0.40f);
    colors.[int ImGuiCol.HeaderHovered]          <- Vector4(0.13f, 0.75f, 0.75f, 0.60f);
    colors.[int ImGuiCol.HeaderActive]           <- Vector4(0.13f, 0.75f, 1.00f, 0.80f);
    colors.[int ImGuiCol.Separator]              <- Vector4(0.13f, 0.75f, 0.55f, 0.40f);
    colors.[int ImGuiCol.SeparatorHovered]       <- Vector4(0.13f, 0.75f, 0.75f, 0.60f);
    colors.[int ImGuiCol.SeparatorActive]        <- Vector4(0.13f, 0.75f, 1.00f, 0.80f);
    colors.[int ImGuiCol.ResizeGrip]             <- Vector4(1f, 1f, 1f, 0f);
    colors.[int ImGuiCol.ResizeGripHovered]      <- Vector4(0.13f, 0.75f, 0.75f, 0.60f);
    colors.[int ImGuiCol.ResizeGripActive]       <- Vector4(0.13f, 0.75f, 1.00f, 0.80f);
    colors.[int ImGuiCol.Tab]                    <- Vector4(0.13f, 0.75f, 0.55f, 0.80f);
    colors.[int ImGuiCol.TabHovered]             <- Vector4(0.13f, 0.75f, 0.75f, 0.80f);
    colors.[int ImGuiCol.TabActive]              <- Vector4(0.13f, 0.75f, 1.00f, 0.80f);
    colors.[int ImGuiCol.TabUnfocused]           <- Vector4(0.18f, 0.18f, 0.18f, 1.00f);
    colors.[int ImGuiCol.TabUnfocusedActive]     <- Vector4(0.36f, 0.36f, 0.36f, 0.54f);
    colors.[int ImGuiCol.DockingPreview]         <- Vector4(0.13f, 0.75f, 0.55f, 0.80f);
    colors.[int ImGuiCol.DockingEmptyBg]         <- Vector4(0.13f, 0.13f, 0.13f, 0.80f);
    colors.[int ImGuiCol.PlotLines]              <- Vector4(0.61f, 0.61f, 0.61f, 1.00f);
    colors.[int ImGuiCol.PlotLinesHovered]       <- Vector4(1.00f, 0.43f, 0.35f, 1.00f);
    colors.[int ImGuiCol.PlotHistogram]          <- Vector4(0.90f, 0.70f, 0.00f, 1.00f);
    colors.[int ImGuiCol.PlotHistogramHovered]   <- Vector4(1.00f, 0.60f, 0.00f, 1.00f);
    colors.[int ImGuiCol.TextSelectedBg]         <- Vector4(0.26f, 0.59f, 0.98f, 0.35f);
    colors.[int ImGuiCol.DragDropTarget]         <- Vector4(1.00f, 1.00f, 0.00f, 0.90f);
    colors.[int ImGuiCol.NavHighlight]           <- Vector4(0.26f, 0.59f, 0.98f, 1.00f);
    colors.[int ImGuiCol.NavWindowingHighlight]  <- Vector4(1.00f, 1.00f, 1.00f, 0.70f);
    colors.[int ImGuiCol.NavWindowingDimBg]      <- Vector4(0.80f, 0.80f, 0.80f, 0.20f);
    colors.[int ImGuiCol.ModalWindowDimBg]       <- Vector4(0.80f, 0.80f, 0.80f, 0.35f);


    //[ImGuiCol_Text] = The color for the text that will be used for the whole menu. 
    //[ImGuiCol_TextDisabled] = Color for "not active / disabled text". 
    //[ImGuiCol_WindowBg] = Background color. 
    //[ImGuiCol_PopupBg] = The color used for the background in ImGui :: Combo and ImGui :: MenuBar. 
    //[ImGuiCol_Border] = The color that is used to outline your menu. 
    //[ImGuiCol_BorderShadow] = Color for the stroke shadow. 
    //[ImGuiCol_FrameBg] = Color for ImGui :: InputText and for background ImGui :: Checkbox
    //[ImGuiCol_FrameBgHovered] = The color that is used in almost the same way as the one above, except that it changes color when guiding it to ImGui :: Checkbox. 
    //[ImGuiCol_FrameBgActive] = Active color. 
    //[ImGuiCol_TitleBg] = The color for changing the main place at the very top of the menu (where the name of your "top-of-the-table" is shown. 
    //ImGuiCol_TitleBgCollapsed = ImguiCol_TitleBgActive 
    //= The color of the active title window, ie if you have a menu with several windows , this color will be used for the window in which you will be at the moment. 
    //[ImGuiCol_MenuBarBg] = The color for the bar menu. (Not all sawes saw this, but still)
    //[ImGuiCol_ScrollbarBg] = The color for the background of the "strip", through which you can "flip" functions in the software vertically. 
    //[ImGuiCol_ScrollbarGrab] = Color for the scoll bar, ie for the "strip", which is used to move the menu vertically. 
    //[ImGuiCol_ScrollbarGrabHovered] = Color for the "minimized / unused" scroll bar. 
    //[ImGuiCol_ScrollbarGrabActive] = The color for the "active" activity in the window where the scroll bar is located. 
    //[ImGuiCol_ComboBg] = Color for the background for ImGui :: Combo. 
    //[ImGuiCol_CheckMark] = Color for your ImGui :: Checkbox. 
    //[ImGuiCol_SliderGrab] = Color for the slider ImGui :: SliderInt and ImGui :: SliderFloat. 
    //[ImGuiCol_SliderGrabActive] = Color of the slider,
    //[ImGuiCol_Button] = the color for the button. 
    //[ImGuiCol_ButtonHovered] = Color when hovering over the button. 
    //[ImGuiCol_ButtonActive] = Button color used. 
    //[ImGuiCol_Header] = Color for ImGui :: CollapsingHeader. 
    //[ImGuiCol_HeaderHovered] = Color, when hovering over ImGui :: CollapsingHeader. 
    //[ImGuiCol_HeaderActive] = Used color ImGui :: CollapsingHeader. 
    //[ImGuiCol_Column] = Color for the "separation strip" ImGui :: Column and ImGui :: NextColumn. 
    //[ImGuiCol_ColumnHovered] = Color, when hovering on the "strip strip" ImGui :: Column and ImGui :: NextColumn. 
    //[ImGuiCol_ColumnActive] = The color used for the "separation strip" ImGui :: Column and ImGui :: NextColumn.
    //[ImGuiCol_ResizeGrip] = The color for the "triangle" in the lower right corner, which is used to increase or decrease the size of the menu. 
    //[ImGuiCol_ResizeGripHovered] = Color, when hovering to the "triangle" in the lower right corner, which is used to increase or decrease the size of the menu. 
    //[ImGuiCol_ResizeGripActive] = The color used for the "triangle" in the lower right corner, which is used to increase or decrease the size of the menu. 
    //[ImGuiCol_CloseButton] = The color for the button-closing menu. 
    //[ImGuiCol_CloseButtonHovered] = Color, when you hover over the button-close menu. 
    //[ImGuiCol_CloseButtonActive] = The color used for the button-closing menu.
    //[ImGuiCol_TextSelectedBg] = The color of the selected text, in ImGui :: MenuBar. 
    //[ImGuiCol_ModalWindowDarkening] = The color of the "Blackout Window" of your menu. 
    //I rarely see these designations, but still decided to put them here. 
    //[ImGuiCol_Tab] = The color for tabs in the menu. 
    //[ImGuiCol_TabActive] = The active color of tabs, ie when you click on the tab you will have this color.
    //[ImGuiCol_TabHovered] = The color that will be displayed when hovering on the table. 
    //[ImGuiCol_TabSelected] = The color that is used when you are in one of the tabs. 
    //[ImGuiCol_TabText] = Text color that only applies to tabs. 
    //[ImGuiCol_TabTextActive] = Active text color for tabs.