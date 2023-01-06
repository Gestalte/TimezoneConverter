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
    Page:int
    FromOffset:int
    ToOffset:int
}

type Msg = 
    | InputTextChanged of string
    | SelectedFromTimezoneChanged of int
    | SelectedToTimezoneChanged of int
    | PageChanged of int
    | FromOffsetChanged of int
    | ToOffsetChanged of int

let init() = {
  InputText = System.DateTime.Now.ToShortTimeString()
  SelectedFromTimezone = 0
  SelectedToTimezone = 0
  Output = ""
  Timezones = Converter.Timezones.MakeTimezones()
  Page = 1
  FromOffset = 0
  ToOffset = 0
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

let CalcOffset model =
    let time = GetTime model.InputText

    let fOffset:Converter.Offset =
        {
            Hours = Math.Abs(model.FromOffset)
            Minutes = 0
            Sign =
                match model.FromOffset with
                | a when a >= 0 -> Converter.Sign.Plus
                | _ -> Converter.Sign.Minus
        }

    let tOffset:Converter.Offset =
        {
            Hours = Math.Abs(model.ToOffset)
            Minutes = 0
            Sign =
                match model.ToOffset with
                | a when a >= 0 -> Converter.Sign.Plus
                | _ -> Converter.Sign.Minus
        }

    match time with
    | None -> ""
    | Some t -> match (Converter.Conversions.CalculateTimeBetweenOffsets t fOffset tOffset) with
        | None -> ""
        | Some s -> s

let SetModel model =
    {model with Output = Calc(model)}

let SetModelOffset model =
    {model with Output = CalcOffset(model)}

let update (msg: Msg) (model: Model) : Model =    
    match msg with 
    | InputTextChanged input -> SetModel {model with InputText = input}
    | SelectedFromTimezoneChanged timezone -> SetModel {model with SelectedFromTimezone = timezone}
    | SelectedToTimezoneChanged timezone -> SetModel {model with SelectedToTimezone = timezone}
    | PageChanged page -> {model with Page = page; Output = ""}
    | FromOffsetChanged offset -> SetModelOffset {model with FromOffset = offset}
    | ToOffsetChanged offset -> SetModelOffset {model with ToOffset = offset}    

let view (model:Model) (dispatch:Msg -> unit) = 
    let flags = 
        ImGuiWindowFlags.NoMove +
        ImGuiWindowFlags.NoCollapse +
        ImGuiWindowFlags.NoTitleBar +
        ImGuiWindowFlags.NoSavedSettings
    
    let gui =
        match model.Page with
        | 1 -> 
            Gui.app [
                Gui.window "Timezone Converter" flags [
                    
                    Gui.button "Switch Mode: Offset" (fun () -> PageChanged(2) |> dispatch)

                    Gui.text "Convert"

                    Gui.InputText "##input" (ref model.InputText) (fun a -> InputTextChanged(a) |> dispatch) (fun a -> InputTextChanged(a) |> dispatch)

                    Gui.text "From timezone" 

                    Gui.combobox "##from-timezone" (ref model.SelectedFromTimezone) (fun a -> SelectedFromTimezoneChanged(a)|>dispatch) (TimezoneNames model)

                    Gui.text "To timezone"

                    Gui.combobox "##to-timezone" (ref model.SelectedToTimezone) (fun a -> SelectedToTimezoneChanged(a)|>dispatch) (TimezoneNames model)

                    Gui.text ""

                    Gui.text model.Output
                ]
            ]
        | 2 ->
            Gui.app [
                Gui.window "Timezone Converter" flags [
                
                    Gui.button "Switch Mode: Timezone" (fun () -> PageChanged(1) |> dispatch)

                    Gui.text "Convert"

                    Gui.InputText "##input" (ref model.InputText) (fun a -> InputTextChanged(a) |> dispatch) (fun a -> InputTextChanged(a) |> dispatch)

                    Gui.text "From timezone" 

                    Gui.SliderInt "##from-offset" (ref model.FromOffset)  -11 +12 (fun a -> FromOffsetChanged(a)|>dispatch) (fun a -> FromOffsetChanged(a)|>dispatch)

                    Gui.text "To timezone"

                    Gui.SliderInt "##to-offset" (ref model.ToOffset)  -11 +12 (fun a -> ToOffsetChanged(a)|>dispatch) (fun a -> ToOffsetChanged(a)|>dispatch) 

                    Gui.text ""

                    Gui.text model.Output
                ]
            ]
        | _ ->
            Gui.app [
                Gui.window "Timezone Converter" flags [
                    Gui.text "Error: Unknown page"
                ]
            ]
    startOrUpdateGuiWith "Timezone Converter" gui |> ignore
    resizeGui(550,180)

Program.mkSimple init update view
|> Program.run