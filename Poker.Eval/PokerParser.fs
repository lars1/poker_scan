namespace Poker.Eval

module PokerParser =
    let private parseValue str = Main.stringToValueMap.[str]

    let private parseSuit str = Main.stringToSuitMap.[str]
        
    let private parseCard (str: string) = 
        let vs = str.Substring(0,1) 
        let ss = str.Substring(1,1)
        Card(parseValue(vs), parseSuit(ss))

    let parseRun (s: string) =
        s.Split(' ') 
        |> Seq.filter (fun s1 -> s1 <> "") 
        |> Seq.map parseCard 
        |> List.ofSeq

    let parseHands (hands: string) =
        hands.Split(',') |> Seq.map parseRun |> List.ofSeq
        
    let parseInputLine (line: string) =
        match line.IndexOf(",") with
        | x when 1 <= x -> (int(line.Substring(0, x)), 
                            (parseHands (line.Substring(x + 1))))
        | _ -> raise(PokerException("Malformed line: \"" + line + "\""))        