using BumaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BumaAPI.Routes;

public static class TimeLineItemRoutes
{
    static string modelName = "TimeLine";
    static string path = "/timelines";
    public static void MapTimeLineItemRoutes(this WebApplication app)
    {
        app.MapGet(path, async (DataBaseContext db) => await db.TimeLineItems.ToListAsync())
        .WithName($"Get{modelName}Items")
        .WithTags(modelName)
        .WithDescription($"Retornar todos {modelName}")
        .Produces<List<TimeLineItem>>(StatusCodes.Status200OK);

        app.MapGet(path + "/{id:int}", async (DataBaseContext db, int id) =>
            await db.TimeLineItems.FindAsync(id) is TimeLineItem model ?
            Results.Ok(model) :
            Results.NotFound())
            .WithName($"Get{modelName}ById")
            .WithTags(modelName)
            .WithDescription($"Retornar um {modelName} pelo ID")
            .Produces<TimeLineItem>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        app.MapPost(path, async (TimeLineItem model, DataBaseContext db) =>
        {
            if (model is null) return Results.BadRequest();
            db.TimeLineItems.Add(model);
            await db.SaveChangesAsync();
            return Results.Created($"{path}/{model.Id}", model);
        }).WithTags(modelName)
          .WithName($"Post{modelName}")
          .WithDescription($"Criar novo {modelName}")
          .Produces<TimeLineItem>(StatusCodes.Status201Created)
          .Produces(StatusCodes.Status400BadRequest);

        app.MapDelete(path + "/{id:int}", async (int id, DataBaseContext db) =>
        {
            var model = await db.TimeLineItems.FindAsync(id);
            if (model is null) return Results.NotFound();
            db.TimeLineItems.Remove(model);
            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithTags(modelName)
        .WithName($"Delete{modelName}")
        .WithDescription($"Deletar {modelName}")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        app.MapPut(path + "/{id:int}", async (int id, TimeLineItem model, DataBaseContext db) =>
        {
            var modelEdit = await db.TimeLineItems.FindAsync(id);

            if (modelEdit is null) return Results.NotFound();

            modelEdit.Title = model.Title;
            modelEdit.Position = model.Position;
            modelEdit.Local = model.Local;
            modelEdit.Description = model.Description;
            modelEdit.Icon = model.Icon;

            await db.SaveChangesAsync();
            return Results.Ok(modelEdit);
        })
        .WithTags(modelName)
        .WithName($"Put{modelName}")
        .WithDescription($"Atualizar {modelName}")
        .Produces<TimeLineItem>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }

}
