using BumaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BumaAPI.Routes;

public static class ProgressBarRoutes
{
    static string modelName = "ProgressBar";
    static string path = "/progressbars";
    public static void MapProgressBarRoutes(this WebApplication app)
    {
        app.MapGet(path, async (DataBaseContext db) => await db.ProgressBars.ToListAsync())
        .WithName($"Get{modelName}Items")
        .WithTags(modelName)
        .WithDescription($"Retornar todos {modelName}")
        .Produces<List<ProgressBar>>(StatusCodes.Status200OK);

        app.MapGet(path + "/{id:int}", async (DataBaseContext db, int id) =>
            await db.ProgressBars.FindAsync(id) is ProgressBar model ?
            Results.Ok(model) :
            Results.NotFound())
            .WithName($"Get{modelName}ById")
            .WithTags(modelName)
            .WithDescription($"Retornar um {modelName} pelo ID")
            .Produces<ProgressBar>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        app.MapPost(path, async (ProgressBar model, DataBaseContext db) =>
        {
            if (model is null) return Results.BadRequest();
            db.ProgressBars.Add(model);
            await db.SaveChangesAsync();
            return Results.Created($"{path}/{model.Id}", model);
        }).WithTags(modelName)
          .WithName($"Post{modelName}")
          .WithDescription($"Criar novo {modelName}")
          .Produces<ProgressBar>(StatusCodes.Status201Created)
          .Produces(StatusCodes.Status400BadRequest);

        app.MapDelete(path+"/{id:int}", async (int id, DataBaseContext db) =>
        {
            var model = await db.ProgressBars.FindAsync(id);
            if (model is null) return Results.NotFound();
            db.ProgressBars.Remove(model);
            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithTags(modelName)
        .WithName($"Delete{modelName}")
        .WithDescription($"Deletar {modelName}")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        app.MapPut(path + "/{id:int}", async (int id, ProgressBar model, DataBaseContext db) =>
        {
            var modelEdit = await db.ProgressBars.FindAsync(id);

            if (modelEdit is null) return Results.NotFound();

            modelEdit.Title = model.Title;
            modelEdit.Percent = model.Percent;

            await db.SaveChangesAsync();
            return Results.Ok(modelEdit);
        })
        .WithTags(modelName)
        .WithName($"Put{modelName}")
        .WithDescription($"Atualizar {modelName}")
        .Produces<ProgressBar>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }

}
