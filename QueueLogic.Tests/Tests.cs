using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using QueueLogic;

namespace QueueLogic.Tests
{
    [TestFixture]
    public class Tests
    { 
        [Test]
        public void Queue_CreateQueue_InstanceIsNotNull()
        {
            // Act
            var queueOfInt = new Queue<int>();

            // Assert
            Assert.IsNotNull(queueOfInt);
        }

        [Test]
        public void Queue_EnqueueTwoItemsInQueueofInt_QueueHasTwoItems()
        {
            // Arrange 
            var queueOfInt = new Queue<int>();

            // Act
            queueOfInt.Enqueue(1);
            queueOfInt.Enqueue(2);

            // Assert
            Assert.IsTrue(queueOfInt.Count == 2);
        }

        [Test]
        public void CreateQueue_WithGivenCollection_InstanceIsNotNullAndHaveTwoItems()
        {
            //Arrange
            IEnumerable <BookIEquatable> books = new List<BookIEquatable>() { new BookIEquatable() { Price = 1, Name = "First" }, new BookIEquatable() { Price = 2, Name = "Second" }};

            // Act
             var queueOfBooks = new Queue<BookIEquatable>(books);

            // Assert
            Assert.IsNotNull(queueOfBooks);
            Assert.IsTrue(queueOfBooks.Count == 2);
        }

        [Test]
        public void Dequeue_OneItemFromQueueOfBooks_QueueHasOneItem()
        {
            // Arrange 
            IEnumerable<BookIEquatable> books = new List<BookIEquatable>() { new BookIEquatable() { Price = 1, Name = "First" }, new BookIEquatable() { Price = 2, Name = "Second" } };
            var queueOfBooks = new Queue<BookIEquatable>(books);

            // Act
            BookIEquatable book = queueOfBooks.Dequeue();

            // Assert
            Assert.IsTrue(queueOfBooks.Count == 1);
            Assert.IsTrue(book.Name == "First");
        }

        [Test]
        public void Peek_GetItemFromQueueOfBooks_ItemIsSecondBook()
        {
            // Arrange
            IEnumerable<BookIEquatable> books = new List<BookIEquatable>() { new BookIEquatable() { Price = 1, Name = "First" }, new BookIEquatable() { Price = 2, Name = "Second" } };
            var queueOfBooks = new Queue<BookIEquatable>(books);
            
            // Act
            BookIEquatable book = queueOfBooks.Peek();
          
            // Assert
            Assert.IsTrue(book.Name == "First");
        }


        [Test]
        public void CetEnumeratorFromQueueOfBooks_OneStepMoveNext_AndCurrentBook_IteratorIsNotNull_CurrentBookNameisFirst()
        {
            // Arrange
            var books = new List<BookIEquatable>() { new BookIEquatable() { Price = 1, Name = "First" }, new BookIEquatable() { Price = 2, Name = "Second" }};
            var queueOfBooks = new Queue<BookIEquatable>(books);

            // Act
            var enumerator = queueOfBooks.GetEnumerator();
            enumerator.MoveNext();
            BookIEquatable currentBook =  enumerator.Current;

            // Assert
            Assert.IsNotNull(enumerator);
            Assert.AreEqual(currentBook.Name, "First");
        }

        [Test]
        public void Peek_GetItemFromEmptyQueue_ThrownException()
        {
            // Arrange
            Queue<int> queue = new Queue<int>();

            // Act - Assert
            Assert.Throws<InvalidOperationException>(() => queue.Peek());
        }

        [Test]
        public void Dequeue_GetItemFromEmptyQueue_ThrownException()
        {
            // Arrange
            Queue<int> queue = new Queue<int>();

            // Act - Assert
            Assert.Throws<InvalidOperationException>(() => queue.Dequeue());
        }

    }
}
