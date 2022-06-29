using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSample._02_Generic
{
    class Generic03
    {
        class Parent
        {
            public override string ToString()
            {
                return "Parent";
            }
        }

        class Child : Parent
        {
            public override string ToString()
            {
                return "Child";
            }
        }

        interface IOuter<out T>
        {
            T Create();
        }

        class ChildOuter : IOuter<Child>
        {
            public Child Create()
            {
                return new Child();
            }
        }

        interface IInputer<in T>
        {
            void Input(T item);
        }

        class ParentInputer : IInputer<Parent>
        {
            public void Input(Parent item)
            {
                Console.WriteLine(item.ToString());
            }
        }

        static public void Main()
        {
            // 공변성
            IEnumerable<Child> childs = new List<Child>();
            IEnumerable<Parent> parents = childs;

            IOuter<Child> cf = new ChildOuter();
            IOuter<Parent> pf = cf;

            // 공변성 covariance 위반
            // IOuter<Child> cf2 = pf; 

            Parent p = pf.Create();

            // 반공변성
            Action<Parent> ap = (Parent c) => { };
            Action<Child> ac = ap;
            IInputer<Parent> pi = new ParentInputer();
            IInputer<Child> ci = pi;

            // 반공변성 contravariance 위반
            // IInputer<Parent> pi = ci;
            ci.Input(p as Child);

            Parent[] pArrs = new Child[10];
            pArrs[0] = new Parent();
            ci.Input(pArrs[0] as Child);
        }
    }
}
