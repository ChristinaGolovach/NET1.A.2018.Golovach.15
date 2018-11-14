using System;

namespace QueueLogic.Tests
{
    public class BookIEquatable : IComparable<BookIEquatable>, IEquatable<BookIEquatable>
    {
        private string name;

        public int Price { get; set; }  
        
        public string Name
        {
            get => name;
            set => name = value ?? throw new ArgumentNullException($"The {nameof(Name)} can not be null.");
        }

        public static int CompareByName(BookIEquatable firstBook, BookIEquatable secondBook)
        {
            CheckBooks(firstBook, secondBook);

            return firstBook.Name.CompareTo(secondBook.Name);
        }

        public int CompareTo(BookIEquatable other)
        {
            if (ReferenceEquals(other, null))
            {
                throw new ArgumentNullException($"The {nameof(other)} can not be null.");
            }

            return this.Price.CompareTo(other.Price);
        }

        public bool Equals(BookIEquatable other)
        {
            return this.Price == other.Price && this.Name == other.Name;
        }

        //public override bool Equals(object obj)
        //{
        //    return this.Equals(obj as BookIEquatable);
        //}

        private static void CheckBooks(BookIEquatable firstBook, BookIEquatable secondBook)
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
