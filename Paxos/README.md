## A very basic Paxos implementation in C#

Implementing Paxos (or at least Synod, the consensus algorithm used by Paxos) by approximately following the Dojo at https://github.com/DaveCTurner/paxos-dojo

It diverges slightly from the instructions above by having the participants directly send HTTP messages to all other relevant parties. In the original an intermediary Haskell server is provided which handles storing messages sent with HTTP POST in internal queues, and then correctly routing those messages when participants poll it for stored messages with HTTP GET.

Furthermore, in this implementation all the participants run inside a single C# process - the original was designed that different people at the Dojo could write their own parts of the system running on their own laptop. Put together, replacing the intermediary server and putting everything in one process makes my version extremely contrived - the process sends several identical HTTP messages to itself, then copies them to concurrent in-memory queues to be consumed by several identical background tasks.

I decided to make everything to run in one process because when all the actors are running on the same machine and not a bunch of laptops, using multiple processes would add awkwardness without revealing anything interesting.

I could then have had the actors communicate in-memory and dispensed with HTTP entirely, but I wanted to use peer-to-peer HTTP because:

1. independently of Paxos this is a very small learning exercise in configuring .NET Core Web APIs (using what was at the time the latest .NET Core preview)

2. independently of Paxos this is also a very small learning exercise in making me think through how peer to peer HTTP networking works

It would be an interesting exercise to add things like artificial failures and/or service discovery, both of which were delegated to the separate Haskell server in the original Dojo, but I haven't yet done this.

Note: The message routing is still a bit of a work in progress.
