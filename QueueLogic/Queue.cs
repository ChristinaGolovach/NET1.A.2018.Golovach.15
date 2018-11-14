using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QueueLogic
{
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

        public Queue() : this (DEFAULTCAPACITY) { }

        public Queue(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentNullException($"The {nameof(capacity)} can not be less zero.");
            }
            queueItems = new T[capacity];
            Count = 0;
        }

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

        public void Enqueue(T item)
        {
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

        public T Dequeue(T item)
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

        public T Peek()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException($"The queue is empty.");
            }

            return queueItems[head];
        }

        public bool IsEmpty()
        {
            return Count == 0 ? true : false;
        }

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
    }
}
