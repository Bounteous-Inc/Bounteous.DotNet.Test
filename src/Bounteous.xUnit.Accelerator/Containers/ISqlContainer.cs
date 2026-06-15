using System.Threading.Tasks;

namespace Bounteous.xUnit.Accelerator.Containers;

public interface ISqlContainer
{
    Task<ISqlContainer> WithDatabase(string schema);
    Task<ISqlContainer>  RunSql(string sql);
    string ConnectionString { get; }
}