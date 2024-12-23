namespace ServerCore;

// 재귀적 락 허용 (X)
// 스핀락 정책 5000 -> yield
public class Lock
{
    private const int EMPTY_FLAG = 0x00000000;
    private const int WRITE_MASK = 0x7FFF0000;
    private const int READ_MASK = 0x0000FFFF;
    private const int MAX_SPIN_COUNT = 5000;
    
    // [Unused(1)] [WriteThreadId(15)] [ReadCount(16)]
    private int _flag;

    public void WriteLock()
    {
        while (true)
        {
            int desired = (Thread.CurrentThread.ManagedThreadId << 16) & WRITE_MASK;
            for (int i = 0; i < MAX_SPIN_COUNT; i++)
            {
                // 시도 성공하면 return
                if (Interlocked.CompareExchange(ref _flag, desired, EMPTY_FLAG) == EMPTY_FLAG) return;
            }
            
            Thread.Yield();
        }
    }

    public void WrithUnlock()
    {
        Interlocked.Exchange(ref _flag, EMPTY_FLAG);
    }

    public void ReadLock()
    {
        // 아무도 WriteLock을 획득하지 않고 있으면 , ReadCount를 1 늘린다.
        while (true)
        {
            for (int i = 0; i < MAX_SPIN_COUNT; i++)
            {
                int expected = (_flag & READ_MASK);
                if (Interlocked.CompareExchange(ref _flag, expected + 1, expected) == expected) return;
            }

            Thread.Yield();
        }
    }

    public void ReadUnlock()
    {   
        Interlocked.Decrement(ref _flag);    
    }
}