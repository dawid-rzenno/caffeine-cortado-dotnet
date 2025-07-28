using System.Text;
using cortado.Repositories;
using cortado.Services;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace cortado;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddSingleton<DapperContext>();
        builder.Services.AddSingleton<PasswordService>();
        builder.Services.AddSingleton<JwtTokenService>();
        
        builder.Services.AddScoped<IUsersRepository, UsersRepository>();
        builder.Services.AddScoped<IGoalsRepository, GoalsRepository>();
        builder.Services.AddScoped<IMilestonesRepository, MilestonesRepository>();
        builder.Services.AddScoped<IUserRolesRepository, UserRolesRepository>();
        builder.Services.AddScoped<IDietsRepository, DietsRepository>();
        builder.Services.AddScoped<IMealsRepository, MealsRepository>();
        builder.Services.AddScoped<IIngredientsRepository, IngredientsRepository>();
        builder.Services.AddScoped<INutrientsRepository, NutrientsRepository>();
        builder.Services.AddScoped<INutrientTypesRepository, NutrientTypesRepository>();
        builder.Services.AddScoped<ITrainingsRepository, TrainingsRepository>();
        builder.Services.AddScoped<IExercisesRepository, ExercisesRepository>();
        builder.Services.AddScoped<IMotivationalQuotesRepository, MotivationalQuotesRepository>();
        builder.Services.AddScoped<IMassUnitsRepository, MassUnitsRepository>();
        
        builder.Services.AddScoped<IDietMealsRepository, DietMealsRepository>();
        builder.Services.AddScoped<IMealIngredientsRepository, MealIngredientsRepository>();
        builder.Services.AddScoped<IIngredientNutrientsRepository, IngredientNutrientsRepository>();
        
        builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

        builder.Services.AddControllers(options =>
        {
            options.Conventions.Add(new RouteTokenTransformerConvention(new OutboundParameterTransformer()));
        }).AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            options.SerializerSettings.Formatting = Formatting.Indented;
            options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            options.SerializerSettings.MaxDepth = 16;
        });
        
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                    ValidAudience = builder.Configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
                };
            });
        
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.MapControllers();

        app.Run();
    }
}