# Poker scan
This is a solution to a pretty old programming challenge that was posted on cplus.about.com way back in 2008. There used to be quite a lot of puzzles there, but unfortunately the poker challenge disappeared from there a while ago. 

I don't really play Poker, but it seemed like a fun little programming challenge to do, and I also used it as a opportunity to learn more F#. The code is written with a functional mindset, but there are a few small patches imperative code in it (unless I already took those out). 


## Description of the challenge

In short: figure out who won each of a thousand hands of Texas Hold'em as fast as possible using some simplified rules for rating hands in poker. 

There eight players, each with two cards, and five community cards. The supplied input file is contains 1000 lines of comma separated columns. The columns are: 

Round number (int)
Five community cards
Two cards for each of the eight players

Cards are represented by two letters, first the card's value (2-9,T,J,Q,K,A) and then the card's suit (H,D,C,S).

The rating rules are like in regular poker, except: 
- Royal (straight) flush is not a separate scoring category. Straight-Flush is the highest category. 
- Generally the highest hand card break ties, then the second highest and so on. 
- Ace is always the highest value (13), and never a one. So A,2,3,4,5 is not a straight here.

## Comments

The problem boils down to:

For a thousand rounds, find the five best cards of seven for each of the eight players. 
Then rank those hands, and find the best hand or hands. There may be ties. 
Output info for each round on who won, by how many points, and textually what kind of hand they had.

And most importantly: do it blazingly fast! ;-)



