var builder = WebApplication.CreateBuilder(args);

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
        options.AddPolicy(name: MyAllowSpecificOrigins,
                builder =>
                {
                        builder.WithOrigins("http://localhost:63343")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                });
});


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
        app.UseSwagger();
        app.UseSwaggerUI();
}
app.UseRouting();
app.UseCors(MyAllowSpecificOrigins);
app.MapControllers();
app.Run();
