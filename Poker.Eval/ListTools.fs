namespace Poker.Eval

module ListTools =
    // terseness can be good:
    let rev = List.rev
    let tail = List.tail
    let head = List.head
    let length = List.length
    let toSeq = List.toSeq

    ///<summary>Remove item at index i and return the new list</summary>
    let removeAtIndex (lst:list<'A>) (i:int) =
        let rec loop (back:list<'A>) (front:list<'A>) (i:int) =
            if i < 1 then
                (rev front) @ (tail back)                          // construct result, dropping the item
            else
                loop (tail back) ((head back)::front) (i-1)        // keep looking

        match i with
        | _ when 0 <= i && i < length lst -> loop lst [] i         // "i" is good so start looking
        | _ -> lst                                                 // "i" was out of bounds

    
    ///<summary>Drop n items from list in all possible combinations</summary>
    let dropNFrom (lst:list<'A>) (n:int) =
        let rec iter from n (lst:list<'A>) =
            match n with
            | 0 -> toSeq [lst]
            | _ -> 
                seq { for i in from .. (length lst) - 1 do 
                        yield! iter i (n-1) (removeAtIndex lst i) }
        
        match n with 
        | _ when n < 0 || (length lst) < n -> toSeq [lst]          // n out of bounds 
        | _ -> (iter 0 n lst)
