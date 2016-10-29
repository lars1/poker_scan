namespace Poker.Tests
open NUnit.Framework
open Poker.Eval


[<TestFixture>]
type TestPokerScorer() = 

    [<TestCase>]
    member t.findHighestScore_straightFlush_fiveCards() = 
        let hand = "JC TC KC QC AC" |> PokerParser.parseRun |> PokerHand.Create
        let actual = PokerScorer.findHighestScore hand
        Assert.That(actual.Points, Is.EqualTo(8000014) )
    
    [<TestCase>]
    member t.findHighestScore_straightFlush_sevenCards() = 
        let hand = "JC TC KC QC AC 9C 8C" |> PokerParser.parseRun |> PokerHand.Create
        let actual = PokerScorer.findHighestScore hand
        Assert.That(actual.Points, Is.EqualTo(8000014) )

    [<TestCase>]
    member t.findHighestScore_fourOfAKind() = 
        let hand = "JD TC JH JS JC" |> PokerParser.parseRun |> PokerHand.Create
        let actual = PokerScorer.findHighestScore hand
        Assert.That(actual.Points, Is.EqualTo(7556885) )

    [<TestCase>]
    member t.findHighestScore_fullHouse_fiveCards() = 
        let hand = "5D QC 5H QS QC" |> PokerParser.parseRun |> PokerHand.Create
        let actual = PokerScorer.findHighestScore hand
        Assert.That(actual.Points, Is.EqualTo(6624375) )

    [<TestCase>]
    member t.findHighestScore_fullHouse_sevenCards() = 
        let hand = "5D QC 5H QS QC 4S 4H" |> PokerParser.parseRun |> PokerHand.Create
        let actual = PokerScorer.findHighestScore hand
        Assert.That(actual.Points, Is.EqualTo(6624375) )

    [<TestCase>]
    member t.findHighestScore_flush_fiveCards() = 
        let hand = "5D QD 6D 4D 2D" |> PokerParser.parseRun |> PokerHand.Create
        let actual = PokerScorer.findHighestScore hand
        Assert.That(actual.Points, Is.EqualTo(5628937) )

    [<TestCase>]
    member t.findHighestScore_flush_sevenCards() = 
        let hand = "5D QD 6D 4D 2D 7S KC" |> PokerParser.parseRun |> PokerHand.Create
        let actual = PokerScorer.findHighestScore hand
        Assert.That(actual.Points, Is.EqualTo(5628937) )

    [<TestCase>]
    member t.findHighestScore_straight_sevenCards() = 
        let hand = "TD 9C 8H 7S 6D 8S 2C" |> PokerParser.parseRun |> PokerHand.Create
        let actual = PokerScorer.findHighestScore hand
        Assert.That(actual.Points, Is.EqualTo(4000010) )

    [<TestCase>]
    member t.findHighestScore_threeOfAKind_sevenCards() = 
        let hand = "TD 8C 8H 7S 6D 8S 2C" |> PokerParser.parseRun |> PokerHand.Create
        let actual = PokerScorer.findHighestScore hand
        Assert.That(actual.Points, Is.EqualTo(3405157) )

    [<TestCase>]
    member t.findHighestScore_twoPair_sevenCards() = 
        let hand = "TD 8C 2H 7S 6D 8S 2C" |> PokerParser.parseRun |> PokerHand.Create
        let actual = PokerScorer.findHighestScore hand
        Assert.That(actual.Points, Is.EqualTo(2411760))
        
    [<TestCase>]
    member t.findHighestScore_pair_sevenCards() = 
        let hand = "TD 8C QH 7S 6D 8S 2C" |> PokerParser.parseRun |> PokerHand.Create
        let actual = PokerScorer.findHighestScore hand
        Assert.That(actual.Points, Is.EqualTo(1407857))

    [<TestCase>]
    member t.findHighestScore_trashCards_fiveCards() = 
        let hand = "2C 4S 5C 6D 7H" |> PokerParser.parseRun |> PokerHand.Create
        let actual = PokerScorer.findHighestScore hand
        Assert.That(actual.Points, Is.EqualTo(7) )

    [<TestCase>]
    member t.findHighestScore_trashCards_sevenCards() = 
        let hand = "2C 4S 5C 6D 7H 9S TS" |> PokerParser.parseRun |> PokerHand.Create
        let actual = PokerScorer.findHighestScore hand
        Assert.That(actual.Points, Is.EqualTo(10) )    