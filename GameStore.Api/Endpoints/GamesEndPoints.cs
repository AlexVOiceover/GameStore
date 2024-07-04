using GameStore.Api.Contracts;

namespace GameStore.Api.Endpoints;

public static class GamesEndPoints
{
    const string  GetGameEndpointName = "GetGame";

private static readonly List<GameDto> games =
[
    new(
        1,
        "Street Fighter II",
        "Fighting",
        19.99M,
        new DateOnly(1992, 7, 15)
    ),
    new(
        2,
        "Final Fantasy XIV",
        "Roleplaying",
        59.99M,
        new DateOnly(2010, 9, 30)
    ),
    new(
        3,
        "The Legend of Zelda: Breath of the Wild",
        "Adventure",
        49.99M,
        new DateOnly(2017, 3, 3)
    ),
    new(
        4,
        "Super Mario Odyssey",
        "Platformer",
        59.99M,
        new DateOnly(2017, 10, 27)
    ),
    new(
        5,
        "Minecraft",
        "Sandbox",
        26.95M,
        new DateOnly(2011, 11, 18)
    ),
    new(
        6,
        "The Witcher 3: Wild Hunt",
        "Roleplaying",
        39.99M,
        new DateOnly(2015, 5, 19)
    ),
    new(
        7,
        "Red Dead Redemption 2",
        "Action",
        59.99M,
        new DateOnly(2018, 10, 26)
    )
];


    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app) 
    {
        var group=app.MapGroup("games");
        
        // GET /games
        group.MapGet("/", () => games);

        // GOST /games/1
        group.MapGet("/{id}", (int id) =>
            {
                GameDto? game = games.Find(game => game.Id == id);

                return game is null ? Results.NotFound() : Results.Ok(game);
            }
        ).WithName(GetGameEndpointName);

        // POST /games
        group.MapPost("/", (CreateGameDto newGame) => 
        {
        
            GameDto game = new (
                games.Count +1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate);

            games.Add(game); 

            return Results.CreatedAtRoute(GetGameEndpointName, new {id = game.Id}, game);
        });

        // PUT /games/1
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) => 
        {
            var index = games.FindIndex(game => game.Id == id);

            if (index == -1) {
                return Results.NotFound();
            }

            games[index] = new GameDto(
                id,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate
            );
            return Results.NoContent();
        });

        //DELETE /games/1
        group.MapDelete("/{id}", (int id) => 
        {
            games.RemoveAll(game => game.Id == id);
            return Results.NoContent();
        });
    return group;
    }

}
