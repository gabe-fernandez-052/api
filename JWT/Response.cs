namespace JWT
{
    public record Response<T>
    {
        public T? Content { get; set; }
        public string Error { get; set; } = string.Empty;
    }
}