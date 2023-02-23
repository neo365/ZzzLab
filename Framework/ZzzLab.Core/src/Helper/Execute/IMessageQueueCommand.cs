using ZzzLab.Logging;

namespace ZzzLab.Helper.Execute
{
    /// <summary>
    /// IMessageQueueCommand
    /// </summary>
    public interface IMessageQueueCommand
    {
        /// <summary>
        /// 로거설정
        /// </summary>
        IZLogger Logger { get; }

        /// <summary>
        /// Queue 실행시 실제 실행되는 함수
        /// </summary>
        void Execute();
    }
}