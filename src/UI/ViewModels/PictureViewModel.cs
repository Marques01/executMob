namespace UI.ViewModels
{
    public class PictureViewModel
    {
        public Guid Id { get; private set; }

        public string Content { get; set; } = string.Empty;

        public long Size { get; set; }

        public string FileName { get; set; } = string.Empty;

        public byte[] Bytes { get; set; } = new byte[0];

        public PictureViewModel()
        {
            Id = Guid.NewGuid();
        }
    }
}
