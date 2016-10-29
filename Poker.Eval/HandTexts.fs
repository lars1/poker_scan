namespace Poker.Eval

///<summary>
///Precomputes the string descriptions for the most common hands, 
///which is necessary for scoring hands quickly.
///</summary>
module HandTexts = 
    
    let private valueToString = 
        Map.ofArray [| for (v,_,_) in Main.valueOrdinalsAndStrings -> 
                        (v, v.ToString()) |]

    let private pairStrings =
        Map.ofArray [| for (v,_,_) in Main.valueOrdinalsAndStrings -> 
                        (v, (sprintf "Pair of %As" v)) |]

    // Many of these generated here are not possible, but there are only 169 generated so we can 
    // live with a little inefficiency:
    let private twoPairStrings = 
        seq { for (v,_,_) in Main.valueOrdinalsAndStrings do
                for (v2,_,_) in Main.valueOrdinalsAndStrings do
                    yield ((v,v2), (sprintf "Two pair of %As and %As" v v2)) }
        |> Map.ofSeq

    let private threeOfAKindStrings =
        Map.ofArray [| for (v,_,_) in Main.valueOrdinalsAndStrings -> 
                        (v, (sprintf "Three %As" v)) |]
        
    let private straightStrings = 
        Map.ofArray [| for (v,_,_) in Main.valueOrdinalsAndStrings -> 
                        (v, (sprintf "Straight from %A" v)) |]

    let private flushStrings = 
        seq { for (v,_,_) in Main.valueOrdinalsAndStrings do
                for (s,_) in Main.suitAndStrings do
                    yield ((v,s), (sprintf "Flush of %A from %A" s v)) }
        |> Map.ofSeq

    
    let highCard value = valueToString.[value]

    let pair value = pairStrings.[value]

    let twoPair val1 val2 = twoPairStrings.[(val1, val2)]

    let threeOfAKind value = threeOfAKindStrings.[value]

    let straight value = straightStrings.[value]

    let flush value suit = flushStrings.[(value, suit)]
