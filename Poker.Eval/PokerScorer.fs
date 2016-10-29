namespace Poker.Eval

module PokerScorer =

    let private scoringStep = 1000000

    // Base points for each kind of hand:
    let private highCard = 0
    let private pair = 1 * scoringStep
    let private twoPair = 2 * scoringStep
    let private threeOfAKind = 3 * scoringStep
    let private straight = 4 * scoringStep
    let private flush = 5 * scoringStep
    let private fullHouse = 6 * scoringStep
    let private fourOfAKind = 7 * scoringStep
    let private straightFlush = 8 * scoringStep

    // Multiplicators for scoring specific cards and ranking hands (eg find winner among competing pairs):
    let private multiplier = 15
    let private multipliers = [for i in 4 .. -1 .. 0 -> pown multiplier i]

    ///<Summary>Weighted scoring of cards by their value, like  c1*m^4 + c2*m^3 + c3*m^2 + c4*m + c5</Summary>
    let private getCardsValueScore (hand:PokerHand) =
        hand.Cards
        |> List.map Main.cardToOrd
        |> List.reduce (fun v0 v1 -> v0 * multiplier + v1)



    let private pointsForPair (hand:PokerHand) pairValue =
        pair 
            + (Main.valueToOrd pairValue) * multipliers.[0]
            + ((hand.RemoveValue pairValue) |> getCardsValueScore)

    let private pointsForTwoPair (hand:PokerHand) pairValue1 pairValue2 =
        let kicker = (hand.RemoveValue(pairValue1)).RemoveValue(pairValue2)
        twoPair
            + (Main.valueToOrd pairValue1) * multipliers.[0]
            + (Main.valueToOrd pairValue2) * multipliers.[1]
            + (getCardsValueScore kicker)
    
    let private pointsForThreeOfAKind (hand:PokerHand) threeVal =
        let kickers = (hand.RemoveValue threeVal)
        threeOfAKind 
            + (Main.valueToOrd threeVal) * multipliers.[0]
            + (kickers.Cards.[0] |> Main.cardToOrd) * multipliers.[3]
            + (kickers.Cards.[1] |> Main.cardToOrd) * multipliers.[4]

    let private pointsForStraight topVal =
        straight + Main.valueToOrd topVal

    let private pointsForFlush (hand:PokerHand) =
        flush + getCardsValueScore hand
    
    let private pointsForFullHouse tripleVal pairVal =
        fullHouse 
            + Main.valueToOrd tripleVal * multipliers.[0]
            + Main.valueToOrd pairVal * multipliers.[1]

    let private pointsForFourOfAKind (hand:PokerHand) quadVal =
        fourOfAKind
            + Main.valueToOrd quadVal * multipliers.[0]
            + Main.cardToOrd (hand.RemoveValue quadVal).Cards.[0]

    let private pointsForStraightFlush topVal =
        straightFlush + Main.valueToOrd topVal




    let private highCardResult hand =
        match PokerRater.findHighCard hand with
        | Some(card) -> { Points = Main.cardToOrd card;
                          Desc = HandTexts.highCard (Main.cardValue card)}
        | None -> raise(PokerException("Ponder this: Nothing comes from an empty hand")) 

    // Parameterized Active Patterns:

    let private (|Pair|) hand =
        match PokerRater.findPair hand with
        | Some(value) -> 
            Some({ Points = pointsForPair hand value; 
                   Desc = HandTexts.pair value })
        | _ -> None

    let private (|TwoPair|) hand =
        match PokerRater.findTwoPairs hand with
        | Some((val1, val2)) -> 
            Some({ Points = pointsForTwoPair hand val1 val2; 
                   Desc = (HandTexts.twoPair val1 val2) })
        | _ -> None

    let private (|ThreeOfAKind|) hand =
        match PokerRater.findThreeOfAKind hand with
        | Some(value) -> 
            Some({ Points = pointsForThreeOfAKind hand value;
                   Desc = HandTexts.threeOfAKind value })
        | _ -> None

    let private (|Straight|) hand =
        match PokerRater.findStraight hand with
        | Some(Card(value, suit)) -> 
            Some({ Points = pointsForStraight value; 
                   Desc = HandTexts.straight value })
        | _ -> None

    let private (|Flush|) hand =
        match PokerRater.findFlush hand with
        | Some(Card(value, suit)) -> 
            Some({ Points = pointsForFlush hand; 
                   Desc = HandTexts.flush value suit })
        | _ -> None

    let private (|FullHouse|) hand =
        match PokerRater.findFullHouse hand with
        | Some((val1, val2)) -> 
            Some({ Points = pointsForFullHouse val1 val2; 
                   Desc = sprintf "Full house of %As and %As" val1 val2 })
        | _ -> None
    
    let private (|FourOfAKind|) hand =
        match PokerRater.findFourOfAKind hand with
        | Some(value) -> 
            Some({ Points = pointsForFourOfAKind hand value; 
                   Desc = sprintf "Four %As" value })
        | _ -> None

    let private (|StraightFlush|) hand = 
        match PokerRater.findStraightFlush hand with
        | Some(Card(value,_)) -> 
            Some({ Points = pointsForStraightFlush value;
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

    
    ///<Summary>Reduce an internal score like 755 499 for AKQJ9 to output score 0 for high card Ace</Summary>
    let getSimpleScore points =         
        points / scoringStep

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