using Game.Mode.Stormworks.Tools.Swtpkg.Application.Extensions;
using Game.Mode.Stormworks.Tools.Swtpkg.Application.Factories;
using Game.Mode.Stormworks.Tools.Swtpkg.ConsoleApp.Factories;
using Lexicom.AspNetCore.Controllers.Extensions;
using Lexicom.ConsoleApp.DependencyInjection;
using Lexicom.ConsoleApp.Tui.Extensions;
using Lexicom.Logging.ConsoleApp.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

/*
 * Control the working folder
 * 
 * New package
 * -Delete everything from folder
 * -Create metadata file (swtpkg.json)
 * --Current pass/progress info
 * --Created time
 * --Created by
 * --Modified time
 * --Modified by
 * --Package version
 * --Game version
 * 
 * Copy images / missions
 * 
 * Copy actual game data tile files
 * 
 * Clean folder
 * -Remove subfolders
 * -Rename files
 * -Everything in root
 * 
 * Crop satellite images
 * 
 * Generate [.metadata.json]
 * -Associated Package version
 * -Associated other tile file names
 * -Created date time
 * -created by
 * -modified date time
 * -modified by
 * 
 * Generate [.data.json] for each tile
 * 
 * Populate [.data.json] from app files (manual collection)
 * -IsStartable
 * -Features (Docks, runways, biomes)
 * 
 * Generate [.workbench.json]
 * 
 * Generate [.position.json]
 * 
 * Remove all non used images/files
 * 
 * Zip and complete package
 */


var builder = ConsoleApplication.CreateBuilder();

builder.Lexicom(options =>
{
    options.AddTui<Program>();
    options.AddLogging();
});

builder.Services.AddLogging(options =>
{
    options.AddConsole(options =>
    {

    });
});



builder.Services.AddSwtpkgApplication();

builder.Services.AddSingleton<IImageFactory, DrawingImageFactory>();

var app = builder.Build();

await app.RunLexicomTuiAsync();
