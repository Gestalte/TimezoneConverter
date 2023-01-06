open ImGuiNET.FSharp
open Elmish
open System
open ImGuiNET

type Model = {
    InputText: string
    SelectedFromTimezone:int
    SelectedToTimezone:int
    Output:string
    Timezones: Converter.Timezone array
}

type Msg = 
    | InputTextChanged of string
    | SelectedFromTimezoneChanged of int
    | SelectedToTimezoneChanged of int

let init() = {
  InputText = System.DateTime.Now.ToShortTimeString()
  SelectedFromTimezone = 0
  SelectedToTimezone = 0
  Output = ""
  Timezones = Converter.Timezones.MakeTimezones()
}

let TimezoneNames model =
    let headArr = [|""|]
    let tailArr = model.Timezones |> Array.map(fun a -> a.Name)
    tailArr |> Array.append headArr

let tryParseInt s = 
    try 
        s |> int |> Some
    with :? FormatException -> 
        None

let GetTime (input:string) : Converter.Time Option=
    let time = input
    let split = time.Split(':')

    let hour = 
        try
            tryParseInt split[0]
        with :? Exception -> 
            None

    let minute = 
        try
            tryParseInt split[1]
        with :? Exception -> 
            None

    if (hour.IsSome=false || minute.IsSome=false) then
        None
    else
        Some {
            Hours = hour.Value
            Minutes = minute.Value
        }

let Calc model =
    let time = GetTime model.InputText
    let fromZone = 
        match model.SelectedFromTimezone with
        | 0 -> None
        | _ -> Converter.Conversions.GetTimezoneFromName (model.Timezones[model.SelectedFromTimezone-1]).Name
    let toZone = 
        match model.SelectedToTimezone with
        | 0 -> None
        | _ -> Converter.Conversions.GetTimezoneFromName (model.Timezones[model.SelectedToTimezone-1]).Name    

    match time with
    | None->""
    | Some t -> 
        match fromZone with
        | None -> ""
        | Some f ->
            match toZone with
            | None -> ""
            | Some tt -> 
                match (Converter.Conversions.CalculateTime t f tt) with
                | None -> ""
                | Some out -> out

let SetModel model =
    {model with Output = Calc(model)}

let update (msg: Msg) (model: Model) : Model =    
    match msg with 
    | InputTextChanged input -> SetModel {model with InputText = input}
    | SelectedFromTimezoneChanged timezone -> SetModel {model with SelectedFromTimezone = timezone}
    | SelectedToTimezoneChanged timezone -> SetModel {model with SelectedToTimezone = timezone}

let view (model:Model) (dispatch:Msg -> unit) = 
    let flags = 
        //ImGuiWindowFlags.NoResize +
        ImGuiWindowFlags.NoMove +
        ImGuiWindowFlags.NoCollapse +
        //ImGuiWindowFlags.NoBackground +
        ImGuiWindowFlags.NoTitleBar +
        //ImGuiWindowFlags.NoDecoration +
        ImGuiWindowFlags.NoSavedSettings
    
    let gui = 
        Gui.app [
            Gui.window "Demo" flags [
                
                Gui.text "Convert"

                Gui.InputText "input" (ref model.InputText) (fun a -> InputTextChanged(a) |> dispatch) (fun a -> InputTextChanged(a) |> dispatch)

                Gui.text "From timezone" 

                Gui.combobox "from-timezone" (ref model.SelectedFromTimezone) (fun a -> SelectedFromTimezoneChanged(a)|>dispatch) (TimezoneNames model)

                Gui.text "To timezone"

                Gui.combobox "to-timezone" (ref model.SelectedToTimezone) (fun a -> SelectedToTimezoneChanged(a)|>dispatch) (TimezoneNames model)

                Gui.text ""

                Gui.text model.Output
            ]
        ]
    startOrUpdateGuiWith "Demo" gui |> ignore
    resizeGui(550,180)

Program.mkSimple init update view
|> Program.run