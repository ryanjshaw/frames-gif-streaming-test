namespace core;

public class SyncToAsyncStream(Stream inner) : Stream
{

    public override void Flush()
    {
        inner.FlushAsync().GetAwaiter().GetResult();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        throw new NotImplementedException();
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new NotImplementedException();
    }

    public override void SetLength(long value)
    {
        throw new NotImplementedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        inner.WriteAsync(buffer, offset, count).GetAwaiter().GetResult();
    }

    public override bool CanRead => inner.CanRead;
    public override bool CanSeek => inner.CanSeek;
    public override bool CanWrite => inner.CanWrite;
    public override long Length => inner.Length;

    public override long Position
    {
        get { return inner.Position; }
        set { inner.Position = value; }
    }

    public void Dispose()
    {
        inner.Dispose();
    }
}