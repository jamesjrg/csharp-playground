using System;

/*
Investigating whether the preview release of the C# 8 compiler now warns about non-exhaustive pattern matching
It does! The implementation is/was at https://github.com/dotnet/roslyn/blob/master/src/Compilers/CSharp/Portable/Binder/SwitchExpressionBinder.cs#L56
*/

namespace version_8
{
    public abstract class A
    {
    }

    public class B : A
    {
        public int b { get; }
    }

    public class C : A
    {
        public int c { get; }
    }

    public class D : A
    {
        public int d { get; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            A o = new D();
            var foo = o switch
            {
                B x => "b",
                C x => "c"
            };
            
        }
    }
}
