using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace QueueLogic
{
    /// <summary>
    /// The class that implements the data structure of the queue.
    /// </summary>
    /// <typeparam name="T">
    /// Any type.
    /// </typeparam>
    public class Queue<T> : IEnumerable<T>
    {
        private T[] queueItems;
        private int head;
        private int tail;
        private int count;
        private int version;

        private const int DEFAULTCAPACITY = 4;
        
        public int Count
        {
            get => count;
            private set => count = value;
        }

        public int Capacity
        {
            get => queueItems.Length;
        }

        #region Constructors
        /// <summary>
        /// Create instance of Queue with default capacity = 4.
        /// </summary>
        public Queue() : this (DEFAULTCAPACITY) { }

        /// <summary>
        /// Create instance of Queue with the given capacity.
        /// </summary>
        /// <param name="capacity">
        /// The capacity queue.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="capacity"/> less than zero.
        /// </exception>
        public Queue(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException($"The {nameof(capacity)} can not be less zero.");
            }

            if (capacity == 0)
            {
                queueItems = new T[DEFAULTCAPACITY];
            }
            else
            {
                queueItems = new T[capacity];
            }
            
            Count = 0;
            tail = -1;
        }

        /// <summary>
        /// Create a queue from the given collection.
        /// </summary>
        /// <param name="collection">
        /// The target collection for the queue.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="collection"/> is null.
        /// </exception>
        public Queue(IEnumerable<T> collection)
        {
            if (ReferenceEquals(collection, null))
            {
                throw new ArgumentNullException($"The {nameof(collection)} can not be null.");
            }

            T[] inputCollection = collection.ToArray();

            if (inputCollection.Length == 0)
            {
                queueItems = new T[DEFAULTCAPACITY];
                Count = 0;
                tail = -1;
            }
            
            else
            {
                queueItems = new T[inputCollection.Length];
                inputCollection.CopyTo(queueItems, 0);
                tail = inputCollection.Length - 1;
                Count = inputCollection.Length;
            }           
        }
        #endregion Constructors

        /// <summary>
        /// Insert an item at the end of the queue.
        /// </summary>
        /// <param name="item">
        /// The target item for the inserting.
        /// </param>
        public void Enqueue(T item)
        {
            if (IsFull())
            {
                int length =  (int)(queueItems.Length * 2);
                T[] newQueueItems = new T[length];
                Array.Copy(queueItems, head, newQueueItems, 0, Count);
                queueItems = newQueueItems;
                head = 0;
            }

            tail = (tail + 1) % queueItems.Length;
            queueItems[tail] = item;          
            version++;
            Count++;
        }

        /// <summary>
        /// Get an item from the head of the queue.
        /// </summary>
        /// <returns>
        /// The first item in queue.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the queue is empty.
        /// </exception>
        public T Dequeue()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException($"The queue is empty.");
            }

            T headItem = queueItems[head];
            queueItems[head] = default(T);
            head = (head + 1) / queueItems.Length;
            version++;
            Count--;

            return headItem;
        }

        /// <summary>
        /// Show the first item of queue.
        /// </summary>
        /// <returns>
        /// The first item of queue
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the queue is empty.
        /// </exception>
        public T Peek()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException($"The queue is empty.");
            }

            return queueItems[head];
        }

        /// <summary>
        /// Check if does exists given item in the queue.
        /// </summary>
        /// <param name="item">
        /// The item for checking 
        /// </param>
        /// <returns>
        /// true - if the given item exists in the queue, otherwice - false.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the queue is empty.
        /// </exception>
        public bool Contains(T item)
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("The queue is empty.");
            }

            EqualityComparer<T> comparer = EqualityComparer<T>.Default;  
            
            for(int i = 0; i <= tail; i++)
            {
                if (ReferenceEquals(item, null) && ReferenceEquals(queueItems[i], null))
                {
                    return true;
                }
                else if (!ReferenceEquals(item, null) && comparer.Equals(queueItems[i], item))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Check for emptiness of queue.
        /// </summary>
        /// <returns>
        /// true - if queue is empty. false - if queue is not empty.
        /// </returns>
        public bool IsEmpty() => Count == 0;
       
        /// <summary>
        /// Clear queue.
        /// </summary>
        public void Clear()
        {
            Array.Clear(queueItems, 0, tail);
            Count = 0;
            version++;
        }

        /// <summary>
        /// Get iterator for the queue.
        /// </summary>
        /// <returns>
        /// Instance of IEnumerator.
        /// </returns>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
           return GetEnumerator();
        }

        /// <summary>
        /// Check for filling  of queue.
        /// </summary>
        /// <returns>
        /// true - if queue is full. false - if queue is not full.
        /// </returns>
        private bool IsFull() => Count == queueItems.Length;

        #region Iterator implementation

        public struct Enumerator : IEnumerator<T>
        {
            private readonly Queue<T> queue;
            private readonly int version;

            private int currentIndex;
            private T currentItem;

            public Enumerator(Queue<T> queue)
            {
                this.queue = queue;
                version = queue.version;
                currentIndex = -1;
                currentItem = default(T);
            }

            public T Current
            {
                get
                {
                    if (currentIndex == -1)
                    {
                        throw new InvalidOperationException("The iteration does not started or ended.");
                    }

                    return currentItem;                    
                }
            }

            object IEnumerator.Current => Current;
            
            public bool MoveNext()
            {
                if (version != queue.version)
                {
                    throw new InvalidOperationException($"The queue was chaged.");
                }

                currentIndex++;

                if (currentIndex == queue.Count)
                {
                    currentIndex = -1;
                    currentItem = default(T);
                    return false;
                }

                currentItem = queue.queueItems[currentIndex];

                return true;
            }

            public void Reset()
            {
                if (version != queue.version)
                {
                    throw new InvalidOperationException($"The queue was chaged.");
                }

                currentIndex = -1;
                currentItem = default(T);
            }

            public void Dispose()
            {
                currentIndex = -1;
                currentItem = default(T);
            }
        }

        #endregion Iterator implementation
    }
}
