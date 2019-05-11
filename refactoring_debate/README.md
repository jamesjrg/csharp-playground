### What does good code look like?

In early 2016, someone wrote an [article](http://www.codeproject.com/Articles/1083348/Csharp-BAD-PRACTICES-Learn-how-to-make-a-good-code) on refactoring bad C# code into good C# code. It currently has an average rating of 4.82/5, with 470 votes, so it seems reasonable to say that in general people think it's a good article. It explains how to turn very basic imperative code into modern SOLID code.

After it was written numerous other people responded with their own alternative refactorings that they thought did a better job. I'm ignoring the various refactorings in F# that people wrote, because I'm more interested about what good code looks like than language evangelism.

##### Before refactoring

The folder BeforeRefactor has the original imperative code to be refactored.

##### After refactoring to enterprise-ready SOLID code

The folder SOLIDRefactor is pretty much a copy of the Code Project refactored example. It looks a lot like most of the code I've seen in commercial software development, and is the sort of code that is often expected when completing C# coding exercises when applying for jobs.

To make it look more like standard Best Practice C# enterprise code I've made the slight change of replacing the account kind enum in the Code Project version with a more substantive class. This class includes a description attribute and an implementation of IEquatable.

##### Fluent builder for functional pattern matching

Numerous people have written add-on libraries for C# that add pattern matching and other functional concepts borrowed from functional languages like F# and Scala. These libraries are often designed to use a fluent-builder type syntax.

One of the many people to respond to the refactoring article (David Arno) wrote their own version using their own functional add-on library, and I've included their version as an example of what this sort of code looks like. I made the extremely minor change of replacing the NUnit test attributes with XUnit attributes instead.

I personally don't like this style of code much - I think there's too much magic, it requires learning a new set of keywords that replace standard C# keywords, and it's harder to reason about exactly what's going on unless you develop a detailed knowledge of the underlying add-on library.  But a large number of bright and capable developers seem to like this style, so maybe they're right and I'm wrong.
 
##### Super functiony refactor

Some people think the best thing is not the SOLID approach of breaking code up into as many classes as possible, but rather to focus on breaking things up into lots of very small, self-contained static functions. In some ways the end result is the same - each function has a single responsiblity in the same way each class does in the SOLID version - but it's far, far, far less verbose. This approach, like the previous one, is influenced by functional languages like F# and Scala, but results in very different code to fluent matching with a custom library.

Again, one of the many people to respond to the article (Pete Smith) used this approach, and so I've included that for comparison too.

##### My refactor, obviously the best one

My refactor is very similar to the super-functiony refactor by Pete. The main difference is that in the case of discounts given due to account type, I've used a static dictionary instead of a whole bunch of very similar one line functions. The idea of representing logic as a dictionary is pretty common in Python, and as I understand it very common in Lisp and its variants where the boundary between code and data is much more blurred. In this particular example it removes a tiny bit of repetition when compared to Pete's version (multiplying the discount by the amount can be extracted into a common method when compared to the previous version), but more importantly I would argue it also makes it easier to see at a glance how a particular account type results in a particular type of discount.  