using Xunit;
using SqlKata;
using SqlKata.Compilers;

namespace wasp.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Query q = new Query().From("Project as p")
                .Join("Project as p1", "p.Id", "p1.ProjectId")
                .Select("p.Id", "p.Name");
            
            var c = new SqlServerCompiler().Compile(q);

            Assert.Equal("SELECT [id], [name] FROM [users]", c.RawSql);
        }
    }
}