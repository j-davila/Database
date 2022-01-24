using Database.Repositories;
using MongoDB.Driver;
using Database.Settings;
using Database.Startup;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;

// added necessary startup code to be able to continue the tutorial. See official microsoft documentation, link located in startup.cs.

var builder = WebApplication.CreateBuilder(args);
var startup = new Startup(builder.Configuration);

// Add services to the container.

startup.ConfigureServices(builder.Services);

builder.Services.AddControllers(options => {
    options.SuppressAsyncSuffixInActionNames = false;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// serializers used for GUID and datetime to explicitly tell the database how to store the data.
BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

builder.Services.AddSingleton<IMongoClient>(ServiceProvider => 
{
   var settings = startup.Configuration.GetSection(nameof(MongoDbSettings)).Get<MongoDbSettings>();
   return new MongoClient(settings.ConnectionString);
});

builder.Services.AddSingleton<IItemsRepository, MongoDBRepository>(); // registering dependency

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

startup.Configure(app, app.Environment);

app.Run();
