open System
open System.IO
open System.Text
open System.Diagnostics
open System.Threading.Tasks
open Poker.Eval

module Driver = 
    let digestibleSortableDateTime =
        (DateTime.Now.ToString "s").Replace(":", ".")

    let humanReadableTime (t: TimeSpan) = t.ToString("ss\.fff")


    ///<summary>Get score for a single round (input line)</summary>
    let private evaluateCards roundNumber communityCards (playerCardLists: Card list list) =
        let sortedCC = List.sortByDescending Main.cardToOrd communityCards

        RoundResult(
            roundNumber,
            playerCardLists 
            |> List.mapi (fun i c -> 
                    {
                        PlayerNumber = i;
                        Score = PokerHand.Create((c.[0])::(c.[1])::sortedCC) 
                                                 |> PokerScorer.findHighestScore
                    }))

    ///<summary>Evaluate a single line of input text</summary>
    let private processLine line =
        let lineNo, cardLists = PokerParser.parseInputLine line
        match cardLists with
        | communityCards::playerCardLists -> evaluateCards lineNo communityCards playerCardLists
        | [] -> raise(PokerException("Invalid input line " + line)) 
        
    ///<summary>Evaluate a list of poker hands (rounds) in a task, and return their RoundResults</summary>    
    let private taskProcess lineLst = async {
        return lineLst
               |> List.map processLine
    }
    

    let private winnersToString (winners: List<PlayerScore>) =
        match winners with
        | [] -> "none"
        | _ when List.length winners = 1 -> sprintf "Player %d" ((List.head winners).PlayerNumber + 1)
        | _ -> sprintf "Players %s" 
                (System.String.Join("+", 
                                    winners |> List.map (fun p -> string(p.PlayerNumber + 1))))

    ///<summary>Prepare the result of a round for output</summary>
    let private stringifyResult (roundResult: RoundResult) =
        match roundResult.Winners with
        | [] -> sprintf "%d,0,," roundResult.RoundNumber
        | w -> 
            let w1 = List.head w
            sprintf "%d,%d,%s,%s" 
                roundResult.RoundNumber 
                (w1.Score.Points |> PokerScorer.getSimpleScore) 
                (winnersToString w) 
                w1.Score.Desc

    ///<summary>Write the whole result out</summary>
    let private outputResults outputFilePath elapsedTime numTasks lines =
        use outputStream = new StreamWriter(File.OpenWrite outputFilePath, Encoding.UTF8)
        sprintf "%s seconds at %s using %d tasks" 
                elapsedTime 
                digestibleSortableDateTime 
                numTasks
        |> outputStream.WriteLine 
        List.iter (fun (ln:string) -> outputStream.WriteLine ln) lines
        
    ///<summary>Process an input file and write an output file</summary>
    let processFile inputFilePath outputFilePath nTasks =
        let allLines = File.ReadAllLines inputFilePath |> Array.toList
        let stopWatch = Stopwatch.StartNew()                            // start timing
        
        let sliceSize = allLines.Length / nTasks

        let results = match nTasks with
                        | n when n < 2 -> 
                            allLines |> List.map processLine            // just to make things clearer
                        | n -> 
                            Async.Parallel [for i in 0..(n-1) -> 
                                                    allLines 
                                                    |> List.skip (i*sliceSize) 
                                                    |> List.take sliceSize
                                                    |> taskProcess ]
                                                |> Async.RunSynchronously
                                                |> Array.fold (fun s x -> s @ x) []
        stopWatch.Stop()

        results 
        |> List.map stringifyResult 
        |> outputResults outputFilePath (stopWatch.Elapsed |> humanReadableTime) nTasks
        
        stopWatch.Elapsed


[<EntryPoint>]
let main argv = 
    // Moral: string creation (sprintf) can be slow especially when you do ten thousands of them.
    // Having precomputed strings in some maps sped things up a lot, from over 23s to 1.8s on one thread!
    // Other than that I did not really see much perf penalty from doing things properly, instead of 
    // say making the most bare bones hacky implementation (although some savings would probably be made)

    let outputPath = sprintf "1000results-%s.txt" Driver.digestibleSortableDateTime
    let numTasks = 8          // the number of concurrent threads to use 

    let calculationTime =
        Driver.processFile "1000hands.csv" outputPath numTasks
        
    let procRunningTime = DateTime.Now - Process.GetCurrentProcess().StartTime
    printf "Done\nPoker hand processing time: %ss \nProcess ran for a total of: %ss <-- This number is an approximation!\n\nPress Enter to quit" 
                (Driver.humanReadableTime calculationTime) 
                (Driver.humanReadableTime procRunningTime)

    Console.ReadLine() |> ignore
    0
