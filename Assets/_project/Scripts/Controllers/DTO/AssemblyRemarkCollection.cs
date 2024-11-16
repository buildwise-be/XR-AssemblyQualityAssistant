namespace _project.Scripts.Controllers.DTO
{
    public readonly struct AssemblyRemarkCollection : IRemarksCollection
    {
        private readonly string[] _messages;
        private readonly IRemarksCollection.RemarksType[] _types;

        public AssemblyRemarkCollection(string[] messages, IRemarksCollection.RemarksType[] types)
        {
            _messages = messages;
            _types = types;
        }

        public string[] GetRemarkMessages()
        {
            return _messages;
        }

        public IRemarksCollection.RemarksType[] GetRemarkType()
        {
            return _types;
        }
    }
}