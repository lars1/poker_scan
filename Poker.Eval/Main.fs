namespace Poker.Eval

type Suit =
| Club
| Diamond
| Heart
| Spade

type Value =
| Two
| Three
| Four
| Five
| Six
| Seven
| Eight
| Nine
| Ten
| Jack
| Queen
| King
| Ace

type Card = Card of Value * Suit

type Score = { Points: int; Desc: string }

type PlayerScore = { PlayerNumber: int; Score: Score }

type RoundResult(roundNumber: int, scores: List<PlayerScore>) =
    let roundNumber = roundNumber
    let scores = scores |> List.sortByDescending (fun s -> s.Score.Points)
    member r.RoundNumber = roundNumber
    member r.Winners =
        match scores with
        | [] -> []
        | h::t -> scores |> List.where (fun e -> e.Score = h.Score)

module Main = 
    let suitAndStrings = [|
            (Club, "C");
            (Diamond, "D");
            (Heart, "H");
            (Spade, "S")
            |]
                
    let suitToStringMap = Map.ofArray suitAndStrings
    let stringToSuitMap = Map.ofArray [| for (s, x) in suitAndStrings -> (x, s) |]

    let valueOrdinalsAndStrings = [|
            (Two, 2, "2"); 
            (Three, 3, "3"); 
            (Four, 4, "4"); 
            (Five, 5, "5"); 
            (Six, 6, "6"); 
            (Seven, 7, "7");
            (Eight, 8, "8");
            (Nine, 9, "9");
            (Ten, 10, "T");
            (Jack, 11, "J");
            (Queen, 12, "Q");
            (King, 13, "K");
            (Ace, 14, "A"); 
            |]

    let valueToOrdinalMap = Map.ofArray [| for (v,o,s) in valueOrdinalsAndStrings -> (v,o) |]
    let ordinalToValueMap = Map.ofArray [| for (v,o,s) in valueOrdinalsAndStrings -> (o,v) |]
    let stringToValueMap = Map.ofArray [| for (v,o,s) in valueOrdinalsAndStrings -> (s,v) |]
    
    let cardToOrd card =
        match card with
        | Card(value, _) -> valueToOrdinalMap.[value]

    let valueToOrd (value: Value) = valueToOrdinalMap.[value]

    let ordToValue (ordinal: int) = ordinalToValueMap.[ordinal]

    let cardValue (c: Card) =
        match c with
        | Card(v,_) -> v

    let cardSuit (c: Card) =
        match c with
        | Card(_,s) -> s


type PokerException =
    inherit System.Exception
    new () = { inherit System.Exception() }
    new (message) = { inherit System.Exception(message) }
    new (message: string, innerException) = { inherit System.Exception(message, innerException) }
    new (si: System.Runtime.Serialization.SerializationInfo, sc) = { inherit System.Exception(si, sc) }


type PokerHand(cards: list<Card>, sorted: bool) =
    // Getter-only auto property:
    member val Cards = if sorted then cards else PokerHand.SortCards cards with get 

    ///<Summary>Return a new PokerHand that is a copy of this, except for cards with a specific value
    ///</Summary>
    member x.RemoveValue value = 
        PokerHand(x.Cards
                  |> List.filter (fun c -> Main.cardValue c <> value),
                  true)

    member x.HighCard = 
        match x.Cards with
        | h::t -> Some(h)
        | _ -> None

    static member public Create cards = PokerHand(cards,false)

    static member private SortCards cards =
        cards
        |> List.sortByDescending (fun card -> Main.cardToOrd card)

