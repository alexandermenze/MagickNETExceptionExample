namespace MagickNETExceptionExample.App
{
    public class DebugStream : Stream
    {
        private readonly Stream _innerStream;
        private long _read = 0;
        private int _seekCount = 0;

        public override bool CanRead => _innerStream.CanRead;

        public override bool CanSeek => _innerStream.CanSeek;

        public override bool CanWrite => _innerStream.CanWrite;

        public override long Length => _innerStream.Length;

        public long ReadBytes => _read;
        public long SeekCount => _seekCount;

        public override long Position
        {
            get => _innerStream.Position;
            set => _innerStream.Position = value;
        }

        public DebugStream(Stream innerStream)
        {
            _innerStream = innerStream;
        }

        public override void Flush()
            => _innerStream.Flush();

        public override int Read(byte[] buffer, int offset, int count)
        {
            var bytesRead = _innerStream.Read(buffer, offset, count);
            _read += bytesRead;
            return bytesRead;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            var pos = _innerStream.Seek(offset, origin);
            _seekCount++;
            return pos;
        }

        public override void SetLength(long value) => 
            _innerStream.SetLength(value);

        public override void Write(byte[] buffer, int offset, int count) => 
            _innerStream.Write(buffer, offset, count);
    }
}
