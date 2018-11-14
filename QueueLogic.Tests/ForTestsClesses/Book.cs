using System;

namespace QueueLogic.Tests.ForTestsClesses
{
    public class Book
    {
        private string name;

        public int Price { get; set; }

        public string Name
        {
            get => name;
            set => name = value ?? throw new ArgumentNullException($"The {nameof(Name)} can not be null.");
        }
    }
}
