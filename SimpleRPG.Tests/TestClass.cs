namespace SimpleRPG.Tests
{
    public class TestClass
    {
        public string Name { get; private set; } = string.Empty;

        public bool DoSomething(string value)
        {
            this.Name = value;
            return true;
        }
    }
}