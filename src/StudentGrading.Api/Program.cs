using StudentGrading.Api.Repositories.Interfaces;
using StudentGrading.Api.Repositories.Implementations;
using StudentGrading.Api.Services.Interfaces;
using StudentGrading.Api.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IStudentRepository, InMemoryStudentRepository>();
builder.Services.AddSingleton<ILessonRepository, InMemoryLessonRepository>();
builder.Services.AddSingleton<IGradeBoundaryRepository, InMemoryGradeBoundaryRepository>();

builder.Services.AddSingleton<IGradeEvaluator>(provider =>
{
    var configuration = provider.GetRequiredService<IConfiguration>();
    var gradeBoundaryRepo = provider.GetRequiredService<IGradeBoundaryRepository>();

    var gradeBoundaries = gradeBoundaryRepo.GetAllGradeBoundariesAsync().GetAwaiter().GetResult();

    var minimumPassingScore = configuration.GetValue<double>("Grading:MinimumPassingScore", 70.0f);

    if (gradeBoundaries == null || !gradeBoundaries.Any())
    {
        throw new InvalidOperationException("No grade boundaries found from repository. Check InMemoryGradeBoundaryRepository initialization.");
    }

    return new GradeEvaluator(gradeBoundaries, minimumPassingScore);
});

builder.Services.AddSingleton<IStudentGradeService, StudentGradeService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:8080", "http://localhost:5173") // Your Vue.js dev server URL(s)
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.MapControllers();


app.Run();

