using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TadosCatFeeding
{
    public class TestClass : ITestClass
    {
        public int Number { get; set; }

        public TestClass(int a)
        {
            Number = a;
        }

        public int Double()
        {
            return Number * 2;
        }
    }
}
