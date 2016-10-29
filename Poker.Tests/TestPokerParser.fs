namespace Poker.Tests
open NUnit.Framework
open Poker.Eval


[<TestFixture>]
type TestPokerParser() = 
    
    [<TestCase>]
    member t.ParseRun1() = 
        let actual = PokerParser.parseRun "QH"
        Assert.That(actual, Is.EqualTo([Card(Queen , Heart)]) )        

    [<TestCase>]
    member t.ParseRun2() = 
        let actual = PokerParser.parseRun "8D 9C AS QD TH 2S"
        Assert.That(actual, Is.EqualTo([Card(Eight, Diamond); Card(Nine, Club); 
                                        Card(Ace, Spade); Card(Queen, Diamond); 
                                        Card(Ten, Heart); Card(Two, Spade)]))        
    
    [<TestCase>]
    member t.ParseHands1() =
        let actual = PokerParser.parseHands "2H 3D 4S 5C,6H 7D"
        Assert.That(actual, 
            Is.EqualTo(
                [
                    [Card(Two, Heart); Card(Three, Diamond); Card(Four, Spade); Card(Five, Club)]; 
                    [Card(Six, Heart); Card(Seven, Diamond)]
                ]))
    
    [<TestCase>]
    member t.ParseHands2() = 
        let actual = PokerParser.parseHands "AH, 8S 9C,9H TD ,JS QC"
        Assert.That(actual,
            Is.EqualTo(
                [
                    [Card(Ace, Heart)];
                    [Card(Eight, Spade); Card(Nine, Club)];
                    [Card(Nine, Heart); Card(Ten, Diamond)];
                    [Card(Jack, Spade); Card(Queen, Club)];
                ]))

    [<TestCase>]
    member t.ParseInputLine1() =
        let actual = PokerParser.parseInputLine "1,KD"
        Assert.That(actual, Is.EqualTo(( 1, [
                                                [Card(King,Diamond)]
                                            ])))

    [<TestCase>]
    member t.ParseInputLine2() =
        let actual = PokerParser.parseInputLine "321,AS 2C,3D 4H"
        Assert.That(actual, Is.EqualTo((321, 
                                        [
                                            [Card(Ace,Spade); Card(Two, Club)];
                                            [Card(Three, Diamond); Card(Four, Heart)]
                                        ])))
