using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class B:A
    {
        public B()
        {
            Console.WriteLine("B()");
        }

        public override void Fun()
        {
            Console.WriteLine("B.Fun()");
        }


        static void Main(string[] args)
        {
            A a = new B();
            a.Fun();
            Console.ReadKey();
        }
    }

    abstract class A
    {
        public A()
        {
            Console.WriteLine("A()");
        }

        public virtual void Fun()
        {
            Console.WriteLine("A.Fun()");
        } 
    }
}
