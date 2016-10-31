# Poker scan
This is a solution to a programming challenge that was posted on cplus.about.com way back in 2008. There are quite a lot of puzzles there, but unfortunately the poker challenge disappeared a while ago. 

I don't play Poker much, but it seemed like a fun little programming challenge to do, and I also used it as a opportunity to learn more F#. The code is written with a functional mindset, with possibly a few minor patches of imperative code. I started out as a beginner in F#, and now I feel like I'm a little bit more skilled beginner :)

## Credits

Many thanks to David Bolton (www.dhbolton.com/) for creating the original challenge, and for kindly allowing me to publish the input data as part of this solution. 

All code in this project is written by me, Lars Warholm. 

## Description of the challenge

In short: figure out who won each of a thousand hands of Texas Hold'em as fast as possible using some simplified rules for rating hands in poker. 

There eight players, each with two cards, and five community cards. The supplied input file is contains 1000 lines of comma separated columns. The columns are: 

Round number (int)
Five community cards
Two cards for each of the eight players

Cards are represented by two letters, first the card's value (2-9,T,J,Q,K,A) and then the card's suit (H,D,C,S).

The rating rules are like in regular poker, except: 
- Royal (straight) flush is not a separate scoring category. Straight-Flush is the highest category. 
- Ace is always the highest value (14), and never a one. So A,2,3,4,5 is not a straight here.


## Comments

The problem boils down to:

For a thousand rounds, find the five best cards of seven for each of the eight players. 
Then rank those hands, and find the best hand or hands. There may be ties. 
Output info for each round on who won, by how many points, and textually what kind of hand they had.

I've implemented pretty much all the rating rules for poker hands, except royal flush is treated as a straight flush and ace always has a value of 14. So I actually did more than the challenge asked for, but it was good fun. I think the code has very good performance, often finishing in under a second on my machine. It can be better to use just two to three tasks, rather than say eight. 

On thing that took a little while to figure out was that string creation (sprintf) can be slow especially when you do over 20 million of them. Having precomputed strings in some maps sped things up a lot, from over 23s to 1.8s on one thread. Other than that I did not really see much perf penalty from doing things properly, instead of 
say making the most bare bones hacky implementation (although some savings would probably be made).