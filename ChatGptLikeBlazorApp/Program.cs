using Azure.Identity;
using ChatGptLikeBlazorApp;
using ChatGptLikeBlazorApp.Repositories;
using ChatGptLikeBlazorApp.Repositories.Interfaces;
using ChatGptLikeBlazorApp.Services;
using ChatGptLikeBlazorApp.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Azure.Cosmos;
using Microsoft.DeepDev;
using Microsoft.Extensions.Azure;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
    .AddMicrosoftIdentityConsentHandler();

builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration);
builder.Services.AddControllersWithViews()
    .AddMicrosoftIdentityUI();
builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy
    options.FallbackPolicy = options.DefaultPolicy;
});

// Add application services
builder.Services.AddSingleton<IChatService, ChatService>();
builder.Services.AddSingleton<IChatRepository, CosmosChatRepository>();
builder.Services.AddSingleton<IAssistantClient, OpenAIAssistantClient>();
builder.Services.Configure<OpenAIAssistantClientOptions>(builder.Configuration.GetSection(nameof(OpenAIAssistantClientOptions)));
builder.Services.Configure<ChatServiceOptions>(builder.Configuration.GetSection(nameof(ChatServiceOptions)));

// Utils
builder.Services.AddSingleton(p =>  TokenizerBuilder.CreateByModelName("gpt-3.5-turbo"));

// Add Azure clients
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddOpenAIClient(builder.Configuration.GetSection("OpenAI"));

    clientBuilder.AddClient<CosmosClient, CosmosDbOptions>(options =>
    {
        return new CosmosClient(options.ConnectionString, new() 
        { 
            ConnectionMode = ConnectionMode.Direct,
            SerializerOptions = new()
            {
                PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase,
            },
        });
    }).ConfigureOptions(builder.Configuration.GetSection("CosmosDb"));

    clientBuilder.UseCredential(
        builder.Environment.IsDevelopment() ?
            new AzureCliCredential() :
            new DefaultAzureCredential());
});

// Radzen services
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
