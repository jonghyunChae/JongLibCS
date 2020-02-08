using System;

namespace Jong2D
{
    public interface IResource : IDisposable
    {
        void Close();
    }

    public abstract class Resource : IResource
    {
        public abstract void Close();

        #region IDisposable Support
        private bool disposedValue = false; // 중복 호출을 검색하려면

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }

                this.Close();

                disposedValue = true;
            }
        }

        ~Resource()
        {
            this.Dispose(false);
        }

        // 삭제 가능한 패턴을 올바르게 구현하기 위해 추가된 코드입니다.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }

}
