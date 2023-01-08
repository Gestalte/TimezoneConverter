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

let GetTime (input:string) : Converter.Time Option =
    let time = input
    let split = time.Split(':')

    let parseTimePart (arr:string array) index =
        try
            tryParseInt arr[index]
        with :? Exception -> 
            None

    let hour = parseTimePart split 0

    let minute = parseTimePart split 1

    if (hour.IsSome=false || minute.IsSome=false) then
        None
    else
        Some {
            Hours = hour.Value
            Minutes = minute.Value
        }

let Calc model =
    let time = GetTime model.InputText

    let makeZone input =
        match input with
        | 0 -> None
        | _ -> Converter.Conversions.GetTimezoneFromName (model.Timezones[input-1]).Name

    let fromZone = makeZone model.SelectedFromTimezone

    let toZone = makeZone model.SelectedToTimezone

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

    let getSign offset =
        match offset with
        | a when a >= 0 -> Converter.Sign.Plus
        | _ -> Converter.Sign.Minus

    let makeOffset (input:int) :Converter.Offset =
        {
            Hours = Math.Abs(input)
            Minutes = 0
            Sign = getSign input
        }

    let fOffset:Converter.Offset = makeOffset model.FromOffset
    let tOffset:Converter.Offset = makeOffset model.ToOffset

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
    | InputTextChanged input -> SetModelOffset( SetModel {model with InputText = input})
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
        Gui.app [
                Gui.window "Timezone Converter" flags [
                    
                    match model.Page with
                    | 1 -> Gui.button "Switch Mode: Offset" (fun () -> PageChanged(2) |> dispatch)
                    | 2 -> Gui.button "Switch Mode: Timezone" (fun () -> PageChanged(1) |> dispatch)
                    | _ -> (fun x -> x)

                    Gui.text "Convert"

                    Gui.InputText "##input" (ref model.InputText) (fun a -> InputTextChanged(a) |> dispatch) (fun a -> InputTextChanged(a) |> dispatch)

                    Gui.text "From timezone" 

                    match model.Page with
                    | 1 -> 
                        Gui.combobox 
                            "##from-timezone" 
                            (ref model.SelectedFromTimezone) 
                            (fun a -> SelectedFromTimezoneChanged(a)|>dispatch) 
                            (TimezoneNames model)
                    | 2 -> 
                        Gui.SliderInt 
                            "##from-offset" 
                            (ref model.FromOffset)  
                            -11 
                            +12 
                            (fun a -> FromOffsetChanged(a)|>dispatch) 
                            (fun a -> FromOffsetChanged(a)|>dispatch)
                    | _ -> (fun x -> x)

                    Gui.text "To timezone"

                    match model.Page with
                    | 1 -> 
                        Gui.combobox 
                            "##to-timezone" 
                            (ref model.SelectedToTimezone) 
                            (fun a -> SelectedToTimezoneChanged(a)|>dispatch) 
                            (TimezoneNames model)
                    | 2 -> 
                        Gui.SliderInt 
                            "##to-offset" 
                            (ref model.ToOffset)  
                            -11 
                            +12 
                            (fun a -> ToOffsetChanged(a)|>dispatch) 
                            (fun a -> ToOffsetChanged(a)|>dispatch) 
                    | _ -> (fun x -> x)

                    Gui.text ("\n" + model.Output)
                ]
            ]

    startOrUpdateGuiWith "Timezone Converter" gui |> ignore
    resizeGui(550,250)
    Styles.setGreenColorScheme()

Program.mkSimple init update view
|> Program.run