
namespace nhibernate_mvc.Models
{
    public class Book
    {
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string Author { get; set; }
        public virtual string Genre { get; set; }

        public virtual string Name { get; set; }
        public virtual string MajorVersion { get; set; }
        public virtual string MinorVersion { get; set; }
    }
}