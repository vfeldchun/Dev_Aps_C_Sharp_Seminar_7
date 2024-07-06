

namespace Task1
{
    internal class TestClass
    {
        [CustomName("CustomFieldName")]
        public int J = 0;
        public int I { get; set; }
        public string? S { get; set; }
        public decimal D { get; set; }
        public char[]? C { get; set; }

        public TestClass()
        { }

        private TestClass(int i)
        {
            this.I = i;
        }

        public TestClass(int i, string s, decimal d, char[] с) : this(i)
        {
            this.S = s;
            this.D = d;
            this.C = с;
        }
    }
}
