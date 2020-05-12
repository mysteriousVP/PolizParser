namespace LexicalAnalyzer
{
    class CustomErrorHandler : State
    {
        public string Message { get; }

        public CustomErrorHandler(int id, string message)
            : base(id, true, false)
        {
            this.Message = message;
        }
    }
}