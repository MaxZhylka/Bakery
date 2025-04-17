using System.Diagnostics;
using backend.Core.DTOs;
using backend.Core.Enums;
using backend.Infrastructure.Database;
using backend.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Xunit.Abstractions;



namespace backend.Tests
{
    public class LoanApplicationRepositoryTests
    {
        private readonly LoanApplicationRepository _applicationRepository;
        private readonly ITestOutputHelper _output;

        public LoanApplicationRepositoryTests(ITestOutputHelper output)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer("Server=localhost\\MSSQLSERVER01;Database=Lab_5;Trusted_Connection=True;Encrypt=False;")
                .Options;

            var context = new AppDbContext(options);

            _applicationRepository = new LoanApplicationRepository(context);
            _output = output;
        }


        private static CreateLoanApplicationDTO CreateMockApplication(Guid userId)
        {
            return new CreateLoanApplicationDTO
            {
                UserId = userId,
                Value = 1000m,
                Term = LoanTerm.OneYear
            };
        }

        [Theory]
        [InlineData(100)]
        [InlineData(1000)]
        public async Task CreateMultipleApplications_PerformanceTest(int count)
        {
            var userId = new Guid("BAD4581F-14E7-4453-A148-F749AD3A9F97") ;
            var createdIds = new List<Guid>();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < count; i++)
            {
                var created = await _applicationRepository.CreateLoanApplicationAsync(CreateMockApplication(userId));
                createdIds.Add(created.Id);
            }

            stopwatch.Stop();
            _output.WriteLine($"Created {count} applications in {stopwatch.ElapsedMilliseconds} ms");

            foreach (var id in createdIds)
            {
                await _applicationRepository.DeleteLoanApplicationAsync(id);
            }
        }
    }
}
