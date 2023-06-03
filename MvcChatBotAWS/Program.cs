using Amazon.LexRuntimeV2;

var builder = WebApplication.CreateBuilder(args);

// Registro del servicio de Amazon Lex
//builder.Services.AddSingleton<AmazonLexRuntimeV2Client>(sp =>
//{
//    var credentials = new Amazon.Runtime.BasicAWSCredentials("AKIASED2MTKXGT6F337H", "aXQ6RPK5dtKee4caVZey0kITgjJq6OguDtPW3xv0");
//    var config = new AmazonLexRuntimeV2Config
//    {
//        RegionEndpoint = Amazon.RegionEndpoint.USEast1 // Virginia
//    };

//    return new AmazonLexRuntimeV2Client(credentials, config);
//});
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
