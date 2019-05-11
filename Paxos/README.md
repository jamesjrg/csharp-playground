## A very basic Paxos implementation in C#

Implementing paxos by approximately following the Dojo at https://github.com/DaveCTurner/paxos-dojo

It diverges slightly from the instructions above by having the participants directly send HTTP messages to all other relevant parties. In the original an intermediary Haskell server is provided which handles storing messages sent with HTTP POST in internal queues, and replying with relevant messages from other participants when polled with HTTP GET. 

Furthermore, in this implementation all the participants run inside a single C# process. Put together, this means the program is extremely contrived - the process sends several identical HTTP messages to itself, then copies them to concurrent in-memory queues to be consumed by several identical background tasks.

I decided to make everything to run in one process because when all the actors are running on the same machine using multiple processes would add a certain amount of awkwardness without adding anything interesting.

I decided to use peer-to-peer HTTP, rather than having the participants communicate simply by sharing data in memory:

1. because independently of Paxos this is a very small learning exercise in configuring .NET Core Web APIS

2. because independently of Paxos this is also a very small learning exercise in peer-to-peer networking

It would be an interesting exercise to add things like artificial failures and/or service discovery, both of which were delegated to the separate Haskell server in the original Dojo, but I haven't yet done this.