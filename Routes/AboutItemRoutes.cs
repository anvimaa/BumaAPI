using BumaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BumaAPI.Routes;
public static class AboutItemRoutes
{
    static string modelName = "About";
    static string path = "/abouts";
    public static void MapAboutItemRoutes(this WebApplication app)
    {
        app.MapGet(path, async (DataBaseContext db) => await db.AboutItems.ToListAsync())
        .WithName($"Get{modelName}Items")
        .WithTags(modelName)
        .WithDescription($"Retornar todos {modelName}")
        .Produces<List<AboutItem>>(StatusCodes.Status200OK);

        app.MapGet(path + "/{id:int}", async (DataBaseContext db, int id) =>
            await db.AboutItems.FindAsync(id) is AboutItem model ?
            Results.Ok(model) :
            Results.NotFound())
            .WithName($"Get{modelName}ById")
            .WithTags(modelName)
            .WithDescription($"Retornar um {modelName} pelo ID")
            .Produces<AboutItem>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        app.MapPost(path, async (AboutItem model, DataBaseContext db) =>
        {
            if (model is null) return Results.BadRequest();
            db.AboutItems.Add(model);
            await db.SaveChangesAsync();
            return Results.Created($"{path}/{model.Id}", model);
        }).WithTags(modelName)
          .WithName($"Post{modelName}")
          .WithDescription($"Criar novo {modelName}")
          .Produces<AboutItem>(StatusCodes.Status201Created)
          .Produces(StatusCodes.Status400BadRequest);

        app.MapDelete(path + "/{id:int}", async (int id, DataBaseContext db) =>
        {
            var model = await db.AboutItems.FindAsync(id);
            if (model is null) return Results.NotFound();
            db.AboutItems.Remove(model);
            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithTags(modelName)
        .WithName($"Delete{modelName}")
        .WithDescription($"Deletar {modelName}")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        app.MapPut(path + "/{id:int}", async (int id, AboutItem model, DataBaseContext db) =>
        {
            var modelEdit = await db.AboutItems.FindAsync(id);

            if (modelEdit is null) return Results.NotFound();

            modelEdit.Title = model.Title;
            modelEdit.PrimaryText = model.PrimaryText;
            modelEdit.SecundaryText = model.SecundaryText;

            await db.SaveChangesAsync();
            return Results.Ok(modelEdit);
        })
        .WithTags(modelName)
        .WithName($"Put{modelName}")
        .WithDescription($"Atualizar {modelName}")
        .Produces<AboutItem>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
