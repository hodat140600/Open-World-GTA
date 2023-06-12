using System.Buffers;

namespace _SDK.Entities
{
    public interface IOwnable
    {
        public bool IsOwned { get; }
        public void Own();

        public void DisOwn();

    }
}