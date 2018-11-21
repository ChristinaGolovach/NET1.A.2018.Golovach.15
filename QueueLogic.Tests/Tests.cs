using System;
using System.Collections.Generic;
using NUnit.Framework;
using QueueLogic;
using QueueLogic.Tests.ForTestsClesses;

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
        public void Queue_EnqueueThreeItemsInQueueofSizeOne_QueueSizeGrow()
        {
            // Arrange 
            var queueOfInt = new Queue<int>(1);

            // Act
            queueOfInt.Enqueue(1);
            queueOfInt.Enqueue(2);
            queueOfInt.Enqueue(3);

            // Assert
            Assert.IsTrue(queueOfInt.Count == 3);
            Assert.IsTrue(queueOfInt.Capacity == 4);
        }


        [Test]
        public void Queue_CircularBehavior_EnqueueThreeItemsInQueueofSizeOne_QueueSizeDontGrow()
        {
            // Arrange 
            var queueOfInt = new Queue<int>(3);

            // Act
            queueOfInt.Enqueue(1);
            queueOfInt.Enqueue(2);
            queueOfInt.Enqueue(3);
            queueOfInt.Dequeue();
            queueOfInt.Enqueue(5);

            // Assert
            Assert.IsTrue(queueOfInt.Count == 3);
            Assert.IsTrue(queueOfInt.Capacity == 3);
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
        public void Contains_CheckContinsIntItemTwo_ReturnTrue()
        {
            // Arrange
            var queueOfInt = new Queue<int>(new int[] { 1, 4, -9 });

            // Act - Assert
            Assert.IsTrue(queueOfInt.Contains(-9));
        }

        [Test]
        public void Contains_CheckContinsBookWithNameFirst_ReturnTrue()
        {
            // Arrange
            var queueOfBook = new Queue<BookIEquatable>(new List<BookIEquatable>() { new BookIEquatable() { Price = 1, Name = "First" }});

            // Act - Assert
            Assert.IsTrue(queueOfBook.Contains(new BookIEquatable() { Price = 1, Name = "First" }));
        }

        [Test]
        public void Contains_CheckContinsBookWithOverloadEquals_ReturnTrue()
        {
            // Arrange
            BookOverloadEquals book = new BookOverloadEquals() { Price = 2, Name = "Second" };
            var queueOfBooks = new Queue<BookOverloadEquals>(new List<BookOverloadEquals>() { new BookOverloadEquals() { Price = 1, Name = "First" }, book });

            // Act - Assert
            Assert.IsTrue(queueOfBooks.Contains(new BookOverloadEquals() { Price = 1, Name = "First" }));
            Assert.IsTrue(queueOfBooks.Contains(book));
        }

        [Test]
        public void Contains_CheckContinsBookWithNotOverloadEquals_ReturnTrue()
        {
            // Arrange
            Book book = new Book() { Price = 2, Name = "Second" };
            var queueOfBooks = new Queue<Book>(new List<Book>() { new Book() { Price = 1, Name = "First" }, book });

            // Act - Assert
            Assert.IsFalse(queueOfBooks.Contains(new Book() { Price = 1, Name = "First" }));
            Assert.IsTrue(queueOfBooks.Contains(book));
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
