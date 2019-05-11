using System;

/*
Numerous number of people have created library projects for discriminated unions in C#, many of which are publicly available on
nuget. Undoubtedly many more exist internally at numerous companies.

C# 7 features pattern matching built in, and the compiler in C# 8 will tell you if a match statement isn't exhaustive, which hopefully removes the need for all this. But even before then, someone named Juliet uploaded this extremely succinct implementation to pastebin, and linked it in a brief comment on a questionon Stack Overflow. I've copied it here, because I like it.

The original is linked in a comment at:
https://stackoverflow.com/questions/3151702/discriminated-union-in-c-sharp
 */
namespace Juliet
{
    class Program
    {
        static void Main(string[] args)
        {
            Union3<int, char, string>[] unions = new Union3<int,char,string>[]
                {
                    new Union3<int, char, string>(5),
                    new Union3<int, char, string>('x'),
                    new Union3<int, char, string>("Juliet")
                };
 
            foreach (Union3<int, char, string> union in unions)
            {
                string value = union.Match(
                    num => num.ToString(),
                    character => new string(new char[] { character }),
                    word => word);
                Console.WriteLine("Matched union with value '{0}'", value);
            }
 
            Console.ReadLine();
        }
    }
 
    public sealed class Union3<A, B, C>
    {
        readonly A Item1;
        readonly B Item2;
        readonly C Item3;
        int tag;
 
        public Union3(A item) { Item1 = item; tag = 0; }
        public Union3(B item) { Item2 = item; tag = 1; }
        public Union3(C item) { Item3 = item; tag = 2; }
 
        public T Match<T>(Func<A, T> f, Func<B, T> g, Func<C, T> h)
        {
            switch (tag)
            {
                case 0: return f(Item1);
                case 1: return g(Item2);
                case 2: return h(Item3);
                default: throw new Exception("Unrecognized tag value: " + tag);
            }
        }
    }
}