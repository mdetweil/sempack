using System;
using sempacklib;

namespace sempack
{
    class Program
    {
        static void Main(string[] args)
        {
            var sempack = new SempackLibrary(args);
           	sempack.ParseArguments();
        }
    }
}
