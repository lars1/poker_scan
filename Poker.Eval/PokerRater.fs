namespace Poker.Eval

open Main

module PokerRater = 
    
    ///<Summary>Check if all cards in a list are equivalent, using a custom comparison function</Summary>
    let private areCardsEqual (cmp: Card -> Card -> bool) (cards: list<Card>) =
        match cards with
        | [] -> raise(PokerException("Cannot compare empty list of cards"))
        | h::t -> cards 
                  |> List.filter (cmp h >> not)
                  |> List.isEmpty


    ///<Summary>Check if all cards in a list have the same value</Summary>
    let private isCardValuesEqual = areCardsEqual (fun c1 c2 -> Main.cardValue c1 = Main.cardValue c2)


    ///<Summary>Check if all cards in a list have the same suit</Summary>
    let private isCardSuitsEqual = areCardsEqual (fun c1 c2 -> Main.cardSuit c1 = Main.cardSuit c2)


    let private findNSimilarValues (n: int) (hand: PokerHand) =
        // Look for N similar cards, starting with card 0..n, then 1..(1+n), 2..(2..n) and so on:
        let rec findInner n (cards: list<Card>) len i =
            if i + n >= len then
                None
            elif isCardValuesEqual cards.[i..(i+n)] then
                Some(match cards.[i] with Card(v,_) -> v)
            else
                findInner n cards len (i+1)
        
        findInner (n-1) hand.Cards hand.Cards.Length 0

      
    ///<Summary>
    ///Search cards with a certain value, remove them, and search the 
    ///remainding for cards with another value. Then return the values of the first and second cards found.
    ///</Summary>
    let private findTwoThings (finder1: PokerHand->option<Value>) (finder2: PokerHand->option<Value>) (hand: PokerHand) =
        match (finder1 hand) with
        | None -> None
        | Some(val1) -> match hand.RemoveValue val1 |> finder2 with
                            | None -> None
                            | Some(val2) -> Some((val1, val2))


    ///<Summary>
    ///Look for a straight. The hand must have five cards.
    ///<Summary>
    let private isStraight (hand: PokerHand): bool =
        let rec isStraightInner cards lastCard =
            match cards with
            | h::t when Main.cardToOrd h = Main.cardToOrd lastCard - 1 -> isStraightInner t h
            | [] -> true
            | _ -> false
        match hand.Cards with
        | h::t when hand.Cards.Length = 5 -> isStraightInner t h
        | _ -> false


    ///<Summary>Return option with the value of the highest card</Summary>
    let findHighCard (hand: PokerHand) = hand.HighCard

    let findPair: PokerHand -> option<Value> = findNSimilarValues 2

    let findTwoPairs: PokerHand -> option<Value * Value> = findTwoThings findPair findPair

    let findThreeOfAKind: PokerHand -> option<Value> = findNSimilarValues 3

    let findStraight (hand:PokerHand) = 
        match (isStraight hand) with
        | true -> hand.HighCard
        | _ -> None

    let findFlush (hand: PokerHand) = 
        match (hand.Cards |> isCardSuitsEqual) with
        | true -> hand.HighCard
        | _ -> None

    let findFullHouse: PokerHand -> option<Value * Value> = findTwoThings findThreeOfAKind findPair
    
    let findFourOfAKind: PokerHand -> option<Value> = findNSimilarValues 4

    let findStraightFlush hand = 
        match isStraight hand && isCardSuitsEqual hand.Cards with
        | true -> hand.HighCard
        | _ -> None