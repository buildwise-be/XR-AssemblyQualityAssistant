namespace _project.Scripts.Controllers.DTO
{
    public readonly struct AssemblyRemarkCollection : IRemarksCollection
    {
        private readonly string[] _messages;

        public AssemblyRemarkCollection(string[] messages)
        {
            _messages = messages;
        }

        public string[] GetRemarkMessages()
        {
            return _messages;
        }
    }
}