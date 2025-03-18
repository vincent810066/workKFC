using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);



//Program.cs�]�wSession

// Add services to the container.
builder.Services.AddControllersWithViews();
//���UIHttpContextAccessor
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//�bAsp.Net
//�Ұ�Session ,�A������{IDistributedCache���f��cache store �ӧ@��session�����h�x�s
//�M��bConfigureServices ��k�U�ե�AddSession��k�N���JIOC�e��,
//�̫�bStartup.Configure ��k�U�ϥ�UseSession�N���J��request->response�ШD�޹D��
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(20 * 60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseSession();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
