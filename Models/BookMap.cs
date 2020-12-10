using FluentNHibernate.Mapping;

namespace nhibernate_mvc.Models
{
    public class BookMap : ClassMap<Book>
    {

        public BookMap()
        {
            Id(x => x.Id).Column("id").Not.Nullable();
            Map(x => x.Title).Column("title").Not.Nullable();
            Map(x => x.Author).Column("author").Not.Nullable();
            Map(x => x.Genre).Column("genre").Not.Nullable();
            Map(x => x.Name).Column("name").Not.Nullable();
            Map(x => x.MajorVersion).Column("majorversion").Not.Nullable();
            Map(x => x.MinorVersion).Column("minorversion").Not.Nullable();
            Table("book");
        }
    }
}
