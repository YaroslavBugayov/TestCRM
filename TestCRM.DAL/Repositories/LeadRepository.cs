using TestCRM.DAL.Entities;
using TestCRM.DAL.Interfaces;
using Dapper;
using TestCRM.DAL.Data;

namespace TestCRM.DAL.Repositories
{
    public class LeadRepository(DapperContext context) : ILeadRepository
    {
        private readonly DapperContext _context = context;

        public async Task<int> CreateAsync(LeadEntity lead, CancellationToken ct = default)
        {
            const string sql = @"
                INSERT INTO Leads (Name, Email, Phone, CreatedAt)
                VALUES (@Name, @Email, @Phone, @CreatedAt);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            using var connection = _context.CreateConnection();
            var command = new CommandDefinition(sql, lead, cancellationToken: ct);
            return await connection.ExecuteScalarAsync<int>(command);
        }

        public async Task<int> DeleteAsync(int id, CancellationToken ct = default)
        {
            const string sql = "DELETE FROM Leads WHERE Id = @Id;";

            using var connection = _context.CreateConnection();
            var command = new CommandDefinition(sql, new { Id = id }, cancellationToken: ct);
            return await connection.ExecuteAsync(command);
        }

        public async Task<IEnumerable<LeadEntity>> GetAllAsync(CancellationToken ct = default)
        {
            const string sql = "SELECT Id, Name, Email, Phone, CreatedAt FROM Leads;";

            using var connection = _context.CreateConnection();
            var command = new CommandDefinition(sql, cancellationToken: ct);
            return await connection.QueryAsync<LeadEntity>(command);
        }

        public async Task<LeadEntity?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            const string sql = "SELECT Id, Name, Email, Phone, CreatedAt FROM Leads WHERE Id = @Id;";

            using var connection = _context.CreateConnection();
            var command = new CommandDefinition(sql, new { Id = id }, cancellationToken: ct);
            return await connection.QuerySingleOrDefaultAsync<LeadEntity>(command);
        }

        public async Task<IEnumerable<LeadEntity>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default)
        {
            int offset = (pageNumber - 1) * pageSize;

            const string sql = @"
                SELECT Id, Name, Email, Phone, CreatedAt 
                FROM Leads 
                ORDER BY CreatedAt DESC 
                OFFSET @Offset ROWS 
                FETCH NEXT @PageSize ROWS ONLY;";

            using var connection = _context.CreateConnection();
            var parameters = new { Offset = 0, PageSize = 10 };
            var command = new CommandDefinition(sql, parameters, cancellationToken: ct);
            return await connection.QueryAsync<LeadEntity>(command);
        }

        public async Task<int> UpdateAsync(LeadEntity lead, CancellationToken ct = default)
        {
            const string sql = @"
                UPDATE Leads
                SET Name = @Name,
                    Email = @Email,
                    Phone = @Phone
                WHERE Id = @Id;";

            using var connection = _context.CreateConnection();
            var command = new CommandDefinition(sql, lead, cancellationToken: ct);
            return await connection.ExecuteAsync(command);
        }
    }
}
