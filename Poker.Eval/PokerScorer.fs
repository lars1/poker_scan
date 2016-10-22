namespace Poker.Eval

module PokerScorer =
    let private highCardResult hand =
        match PokerRater.findHighCard hand with
        | Some(card) -> { Points = Main.cardToOrd card;
                          Desc = HandTexts.highCard (Main.cardValue card)}
        | None -> raise(PokerException("Ponder this: Nothing comes from an empty hand")) 

    // Parameterized Active Patterns:

    let private (|Pair|) hand =
        match PokerRater.findPair hand with
        | Some(value) -> 
            Some({ Points = 100 + Main.valueToOrd value; 
                   Desc = HandTexts.pair value })
        | _ -> None

    let private (|TwoPair|) hand =
        match PokerRater.findTwoPairs hand with
        | Some((val1, val2)) -> 
            Some({ Points = 200 + Main.valueToOrd val1 + Main.valueToOrd val2; 
                   Desc = (HandTexts.twoPair val1 val2) })
        | _ -> None

    let private (|ThreeOfAKind|) hand =
        match PokerRater.findThreeOfAKind hand with
        | Some(value) -> 
            Some({ Points = 300 + Main.valueToOrd value; 
                   Desc = HandTexts.threeOfAKind value })
        | _ -> None

    let private (|Straight|) hand =
        match PokerRater.findStraight hand with
        | Some(Card(value, suit)) -> 
            Some({ Points = 400 + Main.valueToOrd value; 
                   Desc = HandTexts.straight value })
        | _ -> None

    let private (|Flush|) hand =
        match PokerRater.findFlush hand with
        | Some(Card(value, suit)) -> 
            Some({ Points = 500 + Main.valueToOrd value; 
                   Desc = HandTexts.flush value suit })
        | _ -> None

    let private (|FullHouse|) hand =
        match PokerRater.findFullHouse hand with
        | Some((val1, val2)) -> 
            Some({ Points = 600 + Main.valueToOrd val1 + Main.valueToOrd val2; 
                   Desc = sprintf "Full house of %As and %As" val1 val2 })
        | _ -> None
    
    let private (|FourOfAKind|) hand =
        match PokerRater.findFourOfAKind hand with
        | Some(value) -> 
            Some({ Points = 700 + Main.valueToOrd value; 
                   Desc = sprintf "Four %As" value })
        | _ -> None

    let private (|StraightFlush|) hand = 
        match PokerRater.findStraightFlush hand with
        | Some(Card(value,_)) -> 
            Some({ Points = 800 + Main.valueToOrd value; 
                   Desc = (sprintf "Straight flush with %A" value) })
        | _ -> None

    
    let private scoreHandOfFiveCards (hand: PokerHand) =
        match hand with
        | StraightFlush(Some(s)) -> s
        | FourOfAKind(Some(s)) -> s
        | FullHouse(Some(s)) -> s
        | Flush(Some(s)) -> s
        | Straight(Some(s)) -> s
        | ThreeOfAKind(Some(s)) -> s
        | TwoPair(Some(s)) -> s
        | Pair(Some(s)) -> s
        | _ -> highCardResult hand

    let findHighestScore (hand: PokerHand) =
        let numToDrop = hand.Cards.Length - 5
        if numToDrop < 0 then
            raise(PokerException("Can't score hands of less than five cards"))

        let scorings = seq { for cardList in (ListTools.dropNFrom hand.Cards numToDrop) do
                                 yield scoreHandOfFiveCards (PokerHand(cardList,true))
                            }
        let scoringList = 
            scorings 
            |> Seq.toList 
            |> List.sortByDescending (fun score -> score.Points)
        
        List.head scoringList