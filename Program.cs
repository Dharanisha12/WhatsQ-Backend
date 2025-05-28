using WhatsQ.Providers;
using WhatsQ.Repositories;

var builder = WebApplication.CreateBuilder(args);



// ✅ Register Repositories and Providers

builder.Services.AddScoped<IPatientStatusProvider, PatientStatusProvider>();
builder.Services.AddScoped<IPatientStatusRepository, PatientStatusRepository>();

// ✅ Read WhatsApp settings from appsettings.json


// ✅ Configure CORS (for React app)
//builder.Services.AddCors(options =>
//{
//  options.AddPolicy("AllowReactApp", builder =>
//{
//  builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
//});
//});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", builder =>
    {
        builder.WithOrigins("http://localhost:3000")  // Add React app URL here
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// ✅ Add Controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

// ✅ Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowReactApp");
app.UseAuthorization();
app.MapControllers();
app.Run();