using TestCRM.DAL.Entities;
using TestCRM.DAL.Interfaces;
using Dapper;
using TestCRM.DAL.Data;

namespace TestCRM.DAL.Repositories
{
    public class LeadRepository(DapperContext context) : ILeadRepository
    {
        private readonly DapperContext _context = context;

        public async Task<int> CreateLeadAsync(LeadEntity lead, CancellationToken ct = default)
        {
            const string sql = @"
                INSERT INTO Leads (Name, Email, Phone, CreatedAt)
                VALUES (@Name, @Email, @Phone, @CreatedAt);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            using var connection = _context.CreateConnection();
            var command = new CommandDefinition(sql, lead, cancellationToken: ct);
            return await connection.ExecuteScalarAsync<int>(command);
        }

        public async Task<int> DeleteLeadAsync(int id, CancellationToken ct = default)
        {
            const string sql = "DELETE FROM Leads WHERE Id = @Id;";

            using var connection = _context.CreateConnection();
            var command = new CommandDefinition(sql, new { Id = id }, cancellationToken: ct);
            return await connection.ExecuteAsync(command);
        }

        public async Task<IEnumerable<LeadEntity>> GetAllLeadsAsync()
        {
            const string sql = "SELECT Id, Name, Email, Phone, CreatedAt FROM Leads;";

            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<LeadEntity>(sql);
        }

        public async Task<LeadEntity?> GetLeadByIdAsync(int id)
        {
            const string sql = "SELECT Id, Name, Email, Phone, CreatedAt FROM Leads WHERE Id = @Id;";

            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<LeadEntity>(sql, new { Id = id });
        }

        public async Task<int> UpdateLeadAsync(LeadEntity lead, CancellationToken ct = default)
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
