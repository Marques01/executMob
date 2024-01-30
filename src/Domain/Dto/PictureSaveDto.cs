namespace Domain.Dto
{
    public record PictureSaveDto
    {
        public long Size { get; init; }

        public string Name { get; init; } = string.Empty;

        public string Path { get; init; } = string.Empty;

        public bool IsCreated { get; init; }
    }
}
