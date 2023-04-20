using BumaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BumaAPI.Routes;

public static class PersonalInfoRoute
{
    static string modelName = "PersonalInfo";
    static string path = "/personals";
    public static void MapPersonalInfoRoute(this WebApplication app)
    {
        app.MapGet(path, async (DataBaseContext db) => await db.PersonalInfos.ToListAsync())
        .WithName($"Get{modelName}Items")
        .WithTags(modelName)
        .WithDescription($"Retornar todos {modelName}")
        .Produces<List<PersonalInfo>>(StatusCodes.Status200OK);

        app.MapGet(path + "/{id:int}", async (DataBaseContext db, int id) =>
            await db.PersonalInfos.FindAsync(id) is PersonalInfo model ?
            Results.Ok(model) :
            Results.NotFound())
            .WithName($"Get{modelName}ById")
            .WithTags(modelName)
            .WithDescription($"Retornar um {modelName} pelo ID")
            .Produces<PersonalInfo>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound);

        app.MapPost(path, async (PersonalInfo model, DataBaseContext db) =>
        {
            if (model is null) return Results.BadRequest();
            db.PersonalInfos.Add(model);
            await db.SaveChangesAsync();
            return Results.Created($"{path}/{model.Id}", model);
        }).WithTags(modelName)
          .WithName($"Post{modelName}")
          .WithDescription($"Criar novo {modelName}")
          .Produces<PersonalInfo>(StatusCodes.Status201Created)
          .Produces(StatusCodes.Status400BadRequest);

        app.MapDelete(path + "/{id:int}", async (int id, DataBaseContext db) =>
        {
            var model = await db.PersonalInfos.FindAsync(id);
            if (model is null) return Results.NotFound();
            db.PersonalInfos.Remove(model);
            await db.SaveChangesAsync();
            return Results.NoContent();
        })
        .WithTags(modelName)
        .WithName($"Delete{modelName}")
        .WithDescription($"Deletar {modelName}")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        app.MapPut(path + "/{id:int}", async (int id, PersonalInfo model, DataBaseContext db) =>
        {
            var modelEdit = await db.PersonalInfos.FindAsync(id);

            if (modelEdit is null) return Results.NotFound();

            modelEdit.FirstName = model.FirstName;
            modelEdit.LastName = model.LastName;
            modelEdit.Intro = model.Intro;
            modelEdit.Description = model.Description;
            modelEdit.Carrer = model.Carrer;

            await db.SaveChangesAsync();
            return Results.Ok(modelEdit);
        })
        .WithTags(modelName)
        .WithName($"Put{modelName}")
        .WithDescription($"Atualizar {modelName}")
        .Produces<PersonalInfo>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
