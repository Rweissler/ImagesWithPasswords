using System;

namespace ImagesWithPasswords.Data1
{
    public class Image
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Password { get; set; }
        public int ViewCount { get; set; }
    }
}
