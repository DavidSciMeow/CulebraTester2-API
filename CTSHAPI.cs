namespace CulebraTesterAPI
{
    public class CTSHAPI
    {
        public CTServer cts { get; private set; }
        public CTClient ctc { get; private set; }
        public CTSHAPI(int localport = 9987, bool logtoconsole = false)
        {
            cts = new CTServer(localport, logtoconsole);
            ctc = new CTClient("http://localhost", localport);
        }

    }
}
