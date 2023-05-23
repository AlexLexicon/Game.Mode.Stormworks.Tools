using Game.Mode.Stormworks.Tools.Swtpkg.Application.Extensions;
using Lexicom.AspNetCore.Controllers.Extensions;
using Lexicom.ConsoleApp.DependencyInjection;
using Lexicom.ConsoleApp.Tui.Extensions;
using Lexicom.Logging.ConsoleApp.Extensions;

var builder = ConsoleApplication.CreateBuilder();

builder.Lexicom(options =>
{
    options.AddTui<Program>();
    options.AddLogging();
});

builder.Services.AddSwtpkgApplication();

var app = builder.Build();

await app.RunLexicomTuiAsync();
