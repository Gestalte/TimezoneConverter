module Converter

open System

type Sign =
    | Plus
    | Minus

type Offset =
    { 
        Sign:Sign
        Hours:int
        Minutes:int
    }

type Timezone =
    { 
        Name:string
        Abbreviation:string
        Location:string
        Offset:Offset
    }

type Time =
    {
        Hours:int
        Minutes:int
    }

module Timezones =

    let PST = { 
        Name = "Pacific Standard Time "
        Abbreviation = "PST"
        Location = "Western United States"
        Offset =
        {
            Sign=Minus
            Hours = 8
            Minutes = 0
        }
    }

    let PDT = { 
        Name = "Pacific Daylight Time"
        Abbreviation = "PDT"
        Location = "Western United States"
        Offset =
        {
            Sign=Minus
            Hours = 7
            Minutes = 0
        }
    }

    let CST = {
        Name = "Central Standard Time"
        Abbreviation = "CST"
        Location = "Central United States and Canada"
        Offset = 
        {
            Sign = Minus
            Hours = 6
            Minutes = 0
        }
    }

    let CDT = {
        Name = "Central Daylight Time"
        Abbreviation = "CDT"
        Location = "Central United States and Canada"
        Offset = 
        {
            Sign = Minus
            Hours = 6
            Minutes = 0
        }
    }

    let EST = { 
        Name = "Eastern Standard Time"
        Abbreviation = "EST"
        Location = "Eastern United States"
        Offset =
        {
            Sign=Minus
            Hours = 5
            Minutes = 0
        }
    }

    let EDT = { 
        Name = "Eastern Daylight Time"
        Abbreviation = "EDT"
        Location = "Eastern United States"
        Offset =
        {
            Sign=Minus
            Hours = 4
            Minutes = 0
        }
    }

    let Z = { 
        Name = "Zulu Time"
        Abbreviation = "Z"
        Location = "Military"
        Offset =
        { 
            Sign=Minus
            Hours = 0
            Minutes = 0
        }
    }

    let A = { 
        Name = "Alpha"
        Abbreviation = "A"
        Location = "Military"
        Offset =
        { 
            Sign=Plus
            Hours = 1
            Minutes = 0
        }
    }

    let CET = { 
        Name = "Central European Time"
        Abbreviation = "CET"
        Location = "Central Europe"
        Offset =
        {
            Sign=Plus
            Hours = 1
            Minutes = 0
        }
    }

    let CEST = { 
        Name = "Central European Summer Time"
        Abbreviation = "CEST"
        Location = "Central Europe"
        Offset = 
        { 
            Sign = Plus; 
            Hours = 2;
            Minutes = 0; 
        }
    }

    let SAST = { 
        Name = "South African Standard Time"
        Abbreviation = "SAST"
        Location = "South Africa"
        Offset =
        { 
            Sign=Plus
            Hours = 2
            Minutes = 0
        }
    }

    let JST = { 
        Name = "Japan Standard Time"
        Abbreviation = "JST"
        Location = "Japan"
        Offset =
        {
            Sign=Plus
            Hours = 9
            Minutes = 0
        }
    }

    let Timezones = [| A; CDT; CET; CST; CEST; EDT; EST; JST; PDT;  PST; SAST; Z |]

module Conversions =
    let under amount maximum =
        match amount < 0 with
        | true ->  amount + maximum
        | false -> amount

    let over amount maximum = 
        match amount >= maximum with
        | true -> amount - maximum
        | false -> amount
    
    let calculateVector maximum amount  =
        over (under amount maximum) maximum

    let calculateHourVector = calculateVector 24
    let calculateMinuteVector = calculateVector 60

    let ConvertToUTCTime time offset =
        match offset.Sign with
            | Plus ->
                {
                    Hours = calculateHourVector (time.Hours - offset.Hours)
                    Minutes = calculateMinuteVector (time.Minutes - offset.Minutes)
                }
            | Minus -> 
                {
                    Hours = calculateHourVector (time.Hours + offset.Hours)
                    Minutes = calculateMinuteVector (time.Minutes + offset.Minutes)
                }

    let ConvertUCTToOther uctTime targetTimeZoneOffset =
        match targetTimeZoneOffset.Sign with
            | Plus ->
                {
                    Hours = calculateHourVector (uctTime.Hours + targetTimeZoneOffset.Hours)
                    Minutes = calculateMinuteVector (uctTime.Minutes + targetTimeZoneOffset.Minutes)
                }
            | Minus -> 
                {
                    Hours = calculateHourVector (uctTime.Hours - targetTimeZoneOffset.Hours)
                    Minutes = calculateMinuteVector (uctTime.Minutes - targetTimeZoneOffset.Minutes)
                }

    let convertBetweenTimeZones time fromZone toZone =
        let uctTime = ConvertToUTCTime time fromZone.Offset
        ConvertUCTToOther uctTime toZone.Offset

    let convertBetweenTimeOffsets time fromOffset toOffset =
        let uctTime = ConvertToUTCTime time fromOffset
        ConvertUCTToOther uctTime toOffset
    
    let AddLeading0 timePart =
        if timePart < 10 then
            "0" + timePart.ToString()
        else
            timePart.ToString()

    let CalculateTime inputTime fromTimezone toTimezone =       
        let result = convertBetweenTimeOffsets inputTime fromTimezone.Offset toTimezone.Offset
        Some $"{AddLeading0(inputTime.Hours)}:{AddLeading0(inputTime.Minutes)} {fromTimezone.Name} is {AddLeading0(result.Hours)}:{AddLeading0(result.Minutes)} in {toTimezone.Name}"

    let DisplayOffSetWithSign offset =
        let sign =
            match offset.Sign with
            | Plus -> "+"
            | Minus -> "-"

        "UTC" + sign + Math.Abs(offset.Hours).ToString()

    let CalculateTimeBetweenOffsets inputTime fromOffset toOffset =       
        let result = convertBetweenTimeOffsets inputTime fromOffset toOffset
        Some $"{AddLeading0(inputTime.Hours)}:{AddLeading0(inputTime.Minutes)} in {DisplayOffSetWithSign(fromOffset)} is {AddLeading0(result.Hours)}:{AddLeading0(result.Minutes)} in {DisplayOffSetWithSign(toOffset)}"

    let GetTimezoneFromName (name:string) =
        Timezones.Timezones
        |> Array.filter(fun t -> t.Name = name)
        |> Array.tryHead
