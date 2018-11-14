using System;

namespace QueueLogic.Tests.ForTestsClesses
{
    public class BookOverloadEquals
    {
        private string name;

        public int Price { get; set; }

        public string Name
        {
            get => name;
            set => name = value ?? throw new ArgumentNullException($"The {nameof(Name)} can not be null.");
        }

        public bool Equals(BookOverloadEquals other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }
            return this.Price == other.Price && this.Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as BookOverloadEquals);
        }

        public override int GetHashCode()
        {
            return Price.GetHashCode() + Name.GetHashCode();
        }

        private static void CheckBooks(BookOverloadEquals firstBook, BookOverloadEquals secondBook)
        {
            if (ReferenceEquals(firstBook, null))
            {
                throw new ArgumentNullException($"The {nameof(firstBook)} can not be null.");
            }

            if (ReferenceEquals(secondBook, null))
            {
                throw new ArgumentNullException($"The {nameof(secondBook)} can not be null.");
            }
        }
    }
}
