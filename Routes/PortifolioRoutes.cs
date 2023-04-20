using BumaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BumaAPI.Routes;

public static class PortifolioRoutes
{
    static string modelName = "Portifolio"; 
    static string path = "/portifolios";
    public static void MapPortifolioRoutes(this WebApplication app)
    {
        app.MapGet(path, async (DataBaseContext db) => await db.PortifolioItems.ToListAsync())
        .WithName($"Get{modelName}Items")
        .WithTags(modelName)
        .WithDescription($"Retornar todos {modelName}")
        .Produces<List<PortifolioItem>>(StatusCodes.Status200OK);

        app.MapGet(path + "/{id:int}", async (DataBaseContext db, int id) =>
            await db.PortifolioItems.FindAsync(id) is PortifolioItem model ?
            Results.Ok(model) :
            Results.NotFound())
            .WithName($"Get{modelName}ById")
            .WithTags(modelName)
            .WithDescription($"Retornar um {modelName} pelo ID")
            .Produces<PortifolioItem>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        app.MapPost(path, async (PortifolioItem model, DataBaseContext db) =>
        {
            if (model is null) return Results.BadRequest();
            db.PortifolioItems.Add(model);
            await db.SaveChangesAsync();
            return Results.Created($"{path}/{model.Id}", model);
        }).WithTags(modelName)
          .WithName($"Post{modelName}")
          .WithDescription($"Criar novo {modelName}")
          .Produces<PortifolioItem>(StatusCodes.Status201Created)
          .Produces(StatusCodes.Status400BadRequest);

        app.MapDelete(path + "/{id:int}", async (int id, DataBaseContext db) =>
        {
            var model = await db.PortifolioItems.FindAsync(id);
            if (model is null) return Results.NotFound();
            db.PortifolioItems.Remove(model);
            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithTags(modelName)
        .WithName($"Delete{modelName}")
        .WithDescription($"Deletar {modelName}")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        app.MapPut(path + "/{id:int}", async (int id, PortifolioItem model, DataBaseContext db) =>
        {
            var modelEdit = await db.PortifolioItems.FindAsync(id);

            if (modelEdit is null) return Results.NotFound();

            modelEdit.Image=model.Image;
            modelEdit.Url=model.Url;
            modelEdit.UrlGitHub=model.UrlGitHub;

            await db.SaveChangesAsync();
            return Results.Ok(modelEdit);
        })
        .WithTags(modelName)
        .WithName($"Put{modelName}")
        .WithDescription($"Atualizar {modelName}")
        .Produces<PortifolioItem>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }

}
