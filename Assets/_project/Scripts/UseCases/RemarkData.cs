namespace _project.Scripts.UseCases
{
    public readonly struct RemarkData : IRemarkDto
    {
        private readonly string[] _messages;
        private readonly IRemarkDto.RemarkType[] _types;

        public RemarkData(string[] remarkMessage, IRemarkDto.RemarkType[] types)
        {
            _messages = remarkMessage;
            _types = types;
        }

        public string[] GetMessages()
        {
            return _messages;
        }

        public IRemarkDto.RemarkType[] GetRemarkType()
        {
            return _types;
        }
    }
}