module Tests

open System
open System.IO
open System.Text.Json
open FoolGame.Test
open Giraffe
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.TestHost
open Microsoft.Extensions.DependencyInjection
open Xunit
open HttpFunc
open Xunit.Abstractions

[<Fact>]
let ``My test`` () = Assert.True(true)


let createHost () =
    WebHostBuilder()
        .UseContentRoot(Directory.GetCurrentDirectory())
        .UseEnvironment("Test")
        .Configure(Action<IApplicationBuilder> FoolGame.Web.App.configureApp)
        .ConfigureServices(Action<IServiceCollection> FoolGame.Web.App.configureServices)

let shouldContains actual expected = Assert.Contains(actual, expected)
let shouldEqual expected actual = Assert.Equal(expected, actual)
let shouldNotNull expected = Assert.NotNull(expected)

type ApiTests(output: ITestOutputHelper) =

    [<Fact>]
    member _.``GET /api/new/2 should responce id``() =
        use server = new TestServer(createHost ())
        use client = server.CreateClient()

        get client "api/new/2"
        |> ensureSuccess
        |> readText
        |> JsonSerializer.Deserialize<Guid>

    [<Fact>]
    member _.``GET /api/list show response list of games``() =
        use server = new TestServer(createHost ())
        use client = server.CreateClient()

        get client "api/new/2" |> ensureSuccess |> ignore
        get client "api/list"
        |> ensureSuccess
        |> readText
        |> output.WriteLine