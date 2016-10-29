namespace Poker.Tests
open NUnit.Framework
open Poker.Eval


[<TestFixture>]
type TestPokerRater() = 
    
    [<TestCase>]
    member t.constructHand() = 
        let actual = PokerHand([Card(Eight, Heart); Card(Queen, Heart); Card(Two, Heart)], false)
        Assert.That(actual.Cards, Is.EqualTo([Card(Queen, Heart); Card(Eight, Heart); Card(Two, Heart)]) )

    [<TestCase>]
    member t.findHighCard1() = 
        let hand = PokerHand.Create [Card(Eight, Heart); Card(Queen, Heart); Card(Two, Heart)]
        let actual = PokerRater.findHighCard hand
        Assert.That( actual, Is.EqualTo(Some(Card(Queen, Heart))))

    [<TestCase>]
    member t.findPairAtStartOfHand() = 
        let hand = PokerHand.Create [Card(Eight, Heart); Card(Eight, Club); Card(Two, Spade)]
        let actual = PokerRater.findPair hand
        Assert.That( actual, Is.EqualTo(Some(Eight)) )

    [<TestCase>]
    member t.findPairAtMiddleOfHand() = 
        let hand = PokerHand.Create [Card(Ace, Heart); Card(Jack, Club); Card(Jack, Spade); Card(Ten, Spade)]
        let actual = PokerRater.findPair hand
        Assert.That( actual, Is.EqualTo(Some(Jack)) )

    [<TestCase>]
    member t.findPairAtEndOfHand() = 
        let hand = PokerHand.Create [Card(Eight, Heart); Card(Two, Heart); Card(Two, Spade)]
        let actual = PokerRater.findPair hand
        Assert.That( actual, Is.EqualTo(Some(Two)) )
    
    [<TestCase>]
    member t.findTwoPairs1() = 
        let hand = PokerHand.Create [Card(Eight, Heart); Card(Two, Heart); Card(Two, Spade); Card(King, Diamond); Card(Eight, Club)]
        let actual = PokerRater.findTwoPairs hand
        Assert.That( actual, Is.EqualTo(Some((Eight, Two))) )

    [<TestCase>]
    member t.findNoThreeOfAKind() =
        let hand = PokerHand.Create [Card(Ace, Heart); Card(Queen, Club); Card(Jack, Spade); Card(Jack, Spade)]
        let actual = PokerRater.findThreeOfAKind hand
        Assert.That( actual, Is.EqualTo(None) )

    [<TestCase>]
    member t.findThreeOfAKind() =
        let hand = PokerHand.Create [Card(Ace, Heart); Card(Jack, Club); Card(Jack, Spade); Card(Jack, Spade)]
        let actual = PokerRater.findThreeOfAKind hand
        Assert.That( actual, Is.EqualTo(Some(Jack)) )

    [<TestCase>]
    member t.findNoFourOfAKind() =
        let hand = PokerHand.Create [Card(Ace, Heart); Card(Jack, Club); Card(Jack, Spade); Card(Jack, Diamond); Card(Nine, Heart); Card(Five, Club)]
        let actual = PokerRater.findFourOfAKind hand
        Assert.That( actual, Is.EqualTo(None) )

    [<TestCase>]
    member t.findFourOfAKind() =
        let hand = PokerHand.Create [Card(Ace, Heart); Card(Jack, Club); Card(Jack, Spade); Card(Jack, Diamond); Card(Jack, Heart); Card(Five, Club)]
        let actual = PokerRater.findFourOfAKind hand
        Assert.That( actual, Is.EqualTo(Some(Jack)) )

    [<TestCase>]
    member t.findNoFullHouse() =
        let hand = PokerHand.Create [Card(Ace, Heart); Card(Jack, Club); Card(Jack, Spade); Card(Jack, Diamond); Card(Jack, Heart); Card(Five, Club)]
        let actual = PokerRater.findFullHouse hand
        Assert.That( actual, Is.EqualTo(None) )

    [<TestCase>]
    member t.findFullHouse1() =
        let hand = PokerHand.Create [Card(Ace, Heart); Card(Ace, Club); Card(Ace, Spade); Card(Nine, Diamond); Card(Five, Heart); Card(Five, Club)]
        let actual = PokerRater.findFullHouse hand
        Assert.That( actual, Is.EqualTo(Some(Ace, Five)) )

    [<TestCase>]
    member t.findFullHouse2() =
        let hand = PokerHand.Create [Card(Ace, Heart); Card(Queen, Club); Card(Queen, Spade); Card(Nine, Diamond); Card(Five, Heart); Card(Five, Club); Card(Five, Club)]
        let actual = PokerRater.findFullHouse hand
        Assert.That( actual, Is.EqualTo(Some(Five, Queen)) )

    [<TestCase>]
    member t.findNoFlush() =
        let hand = PokerHand.Create [Card(Ace, Heart); Card(Ace, Club); Card(Ace, Heart); Card(Nine, Heart); Card(Five, Heart)]
        let actual = PokerRater.findFlush hand
        Assert.That( actual, Is.EqualTo(None) )

    [<TestCase>]
    member t.findFlush() =
        let hand = PokerHand.Create [Card(Ace, Heart); Card(King, Heart); Card(Queen, Heart); Card(Nine, Heart); Card(Five, Heart)]
        let actual = PokerRater.findFlush hand
        Assert.That( actual, Is.EqualTo(Some(Card(Ace, Heart))) )

    [<TestCase>]
    member t.find_no_straight_in_five_cards() =
        let hand = PokerHand.Create [Card(Ace, Heart); Card(Queen, Club); Card(Jack, Heart); Card(Ten, Heart); Card(Nine, Heart)]
        let actual = PokerRater.findStraight hand
        Assert.That( actual, Is.EqualTo(None) )

    [<TestCase>]
    member t.find_no_straight_in_four_cards() =
        let hand = PokerHand.Create [Card(Ace, Heart); Card(King, Heart); Card(Queen, Heart); Card(Jack, Heart)]
        let actual = PokerRater.findStraight hand
        Assert.That( actual, Is.EqualTo(None) )

    [<TestCase>]
    member t.find_straight_1() =
        let hand = PokerHand.Create [Card(Ace, Heart); Card(King, Heart); Card(Queen, Heart); Card(Jack, Heart); Card(Ten, Club)]
        let actual = PokerRater.findStraight hand
        Assert.That( actual, Is.EqualTo(Some(Card(Ace, Heart))) )

    [<TestCase>]
    member t.find_straight_2() =
        let hand = PokerHand.Create [Card(Nine, Heart); Card(Eight, Heart); Card(Seven, Heart); Card(Six, Heart); Card(Five, Club)]
        let actual = PokerRater.findStraight hand
        Assert.That( actual, Is.EqualTo(Some(Card(Nine, Heart))) )

    [<TestCase>]
    member t.find_straight_3() =
        let hand = PokerHand.Create [Card(Six, Heart); Card(Five, Heart); Card(Four, Heart); Card(Three, Heart); Card(Two, Club)]
        let actual = PokerRater.findStraight hand
        Assert.That( actual, Is.EqualTo(Some(Card(Six, Heart))) )


    [<TestCase>]
    member t.find_no_straight_flush_in_flush_missing_straight() =
        let hand = PokerHand.Create [Card(King, Heart); Card(Queen, Heart); Card(Jack, Heart); Card(Ten, Heart); Card(Eight, Heart)]
        let actual = PokerRater.findStraightFlush hand
        Assert.That( actual, Is.EqualTo(None) )

    [<TestCase>]
    member t.find_no_straight_flush_in_straight_missing_flush() =
        let hand = PokerHand.Create [Card(King, Heart); Card(Queen, Heart); Card(Jack, Heart); Card(Ten, Heart); Card(Nine, Club)]
        let actual = PokerRater.findStraightFlush hand
        Assert.That( actual, Is.EqualTo(None) )
        
    [<TestCase>]
    member t.find_no_straight_flush_in_four_cards() =
        let hand = PokerHand.Create [Card(Ace, Heart); Card(King, Heart); Card(Queen, Heart); Card(Jack, Heart)]
        let actual = PokerRater.findStraightFlush hand
        Assert.That( actual, Is.EqualTo(None) )

    [<TestCase>]
    member t.find_straight_flush_1() =
        let hand = PokerHand.Create [Card(Ace, Heart); Card(King, Heart); Card(Queen, Heart); Card(Jack, Heart); Card(Ten, Heart)]
        let actual = PokerRater.findStraightFlush hand
        Assert.That( actual, Is.EqualTo(Some(Card(Ace, Heart))) )

    [<TestCase>]
    member t.find_straight_flush_2() =
        let hand = PokerHand.Create [Card(Nine, Spade); Card(Eight, Spade); Card(Seven, Spade); Card(Six, Spade); Card(Five, Spade)]
        let actual = PokerRater.findStraightFlush hand
        Assert.That( actual, Is.EqualTo(Some(Card(Nine, Spade))) )