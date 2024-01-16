using System;
using System.Collections.Generic;
using System.Threading;

namespace ZzzLab.Helper.Execute
{
    /// <summary>
    /// 순차적으로 Execute하기 위한 Queue.
    /// </summary>
    public sealed class MessageQueue : IDisposable
    {
        private readonly Queue<IMessageQueueCommand> HighestList = new Queue<IMessageQueueCommand>();
        private readonly Queue<IMessageQueueCommand> NormalList = new Queue<IMessageQueueCommand>();
        private readonly Queue<IMessageQueueCommand> LowestList = new Queue<IMessageQueueCommand>();
        private readonly ManualResetEvent Controller = new ManualResetEvent(false);
        private readonly Thread QueueThread;

        /// <summary>
        /// 동작여부
        /// </summary>
        public bool IsAlive { get; private set; }

        /// <summary>
        /// Queue의 전체 개수를 가져온다.
        /// </summary>
        public int Count => this.HighestList.Count + this.NormalList.Count + this.LowestList.Count;

        /// <summary>
        /// 우선순위 낮음의 Queue의 전체 개수를 가져온다.
        /// </summary>
        public int CountLowest => this.LowestList.Count;

        /// <summary>
        /// 우선순위 높음의 Queue의 전체 개수를 가져온다.
        /// </summary>
        public int CountHighest => this.HighestList.Count;

        /// <summary>
        /// 우선순위 보통의 Queue의 전체 개수를 가져온다.
        /// </summary>
        public int CountNormal => this.NormalList.Count;

        /// <summary>
        /// MessageQueue Class
        /// </summary>
        public MessageQueue()
        {
            this.QueueThread = new Thread(this.ExecuteMessage)
            {
                IsBackground = true,
                Name = "MessageQueue"
            };
            this.IsAlive = true;
            this.QueueThread.Start();
        }

        //--------------------------------
        // Command 를 메세지큐에 넣음
        //--------------------------------

        /// <summary>
        /// Command를 Queue에 넣어 준다.
        /// </summary>
        /// <param name="command"></param>
        public void Enqueue(IMessageQueueCommand command)
            => this.Enqueue(command, QueuePriority.Normal);

        public void EnqueueLowest(IMessageQueueCommand command)
            => this.Enqueue(command, QueuePriority.Lowest);

        /// <summary>
        /// Command를 Queue에 넣어 준다. 다른 것을 보다 우선 처리된다.
        /// </summary>
        public void EnqueueHighest(IMessageQueueCommand command)
            => this.Enqueue(command, QueuePriority.Highest);

        /// <summary>
        /// Command를 Queue에 넣어 준다.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="priority">우선순위</param>
        private void Enqueue(IMessageQueueCommand command, QueuePriority priority = QueuePriority.Normal)
        {
            // Queue 에 Command 를 Enqueue
            switch (priority)
            {
                case QueuePriority.Highest:
                    this.HighestList.Enqueue(command);
                    break;

                case QueuePriority.Normal:
                    this.NormalList.Enqueue(command);
                    break;

                case QueuePriority.Lowest:
                    this.LowestList.Enqueue(command);
                    break;
            }

            // 대기상태에 빠져 있을지 모르니, Thread 를 동작하게 만듦
            this.Controller.Set();
        }

        /// <summary>
        /// Queue를 모두 삭제 한다.
        /// </summary>
        public void Clear()
        {
            this.HighestList.Clear();
            this.NormalList.Clear();
            this.LowestList.Clear();
        }

        /// <summary>
        /// 우선순위 낮음의 Queue를 모두 삭제 한다.
        /// </summary>
        public void ClearLowest()
            => this.LowestList.Clear();

        /// <summary>
        /// 우선순위 높음의 Queue를 모두 삭제 한다.
        /// </summary>
        public void ClearHighest()
            => this.HighestList.Clear();

        /// <summary>
        /// 우선순위 보통의 Queue를 모두 삭제 한다.
        /// </summary>
        public void ClearNormal()
            => this.NormalList.Clear();

        //--------------------------------
        // Queue 에 쌓여있는 Command 를 수행
        //--------------------------------
        private void ExecuteMessage()
        {
            try
            {
                while (IsAlive)
                {
                    // Queue 에 있는 모든 Command 를 수행
                    while (this.Count > 0)
                    {
                        bool IsContinue = false;
                        while (this.HighestList.Count > 0)
                        {
                            IMessageQueueCommand command = this.HighestList.Dequeue();
                            command?.Execute();
                            IsContinue = true;
                        }

                        if (IsContinue) continue;

                        if (this.NormalList.Count > 0)
                        {
                            IMessageQueueCommand command = this.NormalList.Dequeue();
                            command?.Execute();
                        }

                        if (this.NormalList.Count > 0) continue;

                        if (this.LowestList.Count > 0)
                        {
                            IMessageQueueCommand command = this.LowestList.Dequeue();
                            command?.Execute();
                        }
                    }

                    // Queue 에 있는 모든 Command 를 수행

                    // 다 수행하고 나면, 대기상태로 진입
                    this.Controller.Reset();
                    this.Controller.WaitOne(Timeout.Infinite);
                }
            }
            catch (ThreadAbortException) { }
        }

        #region IDisposable Support

        private bool disposedValue = false; // 중복 호출을 검색하려면

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    this.IsAlive = false;
                    this.Controller.Set();
                    //this.QueueThread.Abort();
                    this.Clear();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Support
    }
}