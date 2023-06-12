using System.Buffers;

namespace _SDK.Entities
{
    public interface IActivable
    {
        public bool IsActivated { get; }
        public void Activate();
        public void DeActivate();
    }
}