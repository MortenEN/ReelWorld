using ReelWorld.DataAccessLibrary.Interfaces;
using ReelWorld.DataAccessLibrary.SqlServer;

namespace ReelWorld.Api
{
    public class Program
    {
        private const string _connectionString = "Data Source=hildur.ucn.dk;Initial Catalog=DMA-CSD-S241_10632087;Persist Security Info=True;User ID=DMA-CSD-S241_10632087;Password=Password1!;Encrypt=True;Trust Server Certificate=True";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            


            // Dependency Injection for UserDao
            builder.Services.AddScoped<IProfileDaoAsync>(sp =>
            new ProfileDao(_connectionString));
            builder.Services.AddScoped<IEventDaoAsync>(sp =>
            new EventDao(_connectionString));
            builder.Services.AddScoped<IRegistrationDaoAsync>(sp =>
            new RegistrationDao(_connectionString));
            builder.Services.AddScoped<ILoginDao>(sp =>
            new LoginDao(_connectionString));






            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
