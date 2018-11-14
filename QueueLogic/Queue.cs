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
            queueItems = new T[capacity];
            Count = 0;
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
            //TODO IF item is null

            if (IsFull())
            {
                int length = queueItems.Length == 0 ? DEFAULTCAPACITY : (int)(queueItems.Length * 1.5);
                T[] newQueueItems = new T[length];
                queueItems.CopyTo(newQueueItems, 0);
                queueItems = newQueueItems;
            }

            queueItems[tail] = item;
            tail++;
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
            head++;
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

        //TODO Contains

        /// <summary>
        /// Check for emptiness of queue.
        /// </summary>
        /// <returns>
        /// true - if queue is empty. false - if queue is not empty.
        /// </returns>
        public bool IsEmpty()
        {
            return Count == 0 ? true : false;
        }

        /// <summary>
        /// Check for filling  of queue.
        /// </summary>
        /// <returns>
        /// true - if queue is full. false - if queue is not full.
        /// </returns>
        public bool IsFull()
        {
            return Count == queueItems.Length ? true : false;
        }

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
        public IEnumerator<T> GetEnumerator()
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

        #region Iterator implementation

        private struct Enumerator : IEnumerator<T>
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
