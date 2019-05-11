namespace Paxos.Messages
{
    public class Prepare
    {
        public string Type { get { return "prepare"; } }
        public int TimePeriod { get; set; }
        public string Value { get; set; }
    }
}