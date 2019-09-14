using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;

namespace FPinCSharp.Example8
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            /*
                Populating the profile page of a user in a social media application
            */

            //// User input
            var userIds = new[] { 1, 2, 3, 4, 5 };

            {
                var sw = new Stopwatch();
                sw.Start();

                var profileTasks = userIds.Select(async userId =>
                {
                    try
                    {
                        var userTask = UserService.FetchUser(userId);
                        var albumsTask = UserService.FetchAlbums(userId);
                        var connectionIdsTask = UserService.FetchConnectionIds(userId);

                        await userTask;
                        await albumsTask;
                        await connectionIdsTask;

                        var connections = await Task.WhenAll(
                            connectionIdsTask.Result.Select(UserService.FetchUser)
                        );

                        return UserService.GenerateProfile(userTask.Result, connections, albumsTask.Result);
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                });

                var profiles = (await Task.WhenAll(profileTasks))
                    .Where(x => !string.IsNullOrEmpty(x));

                Console.WriteLine(string.Join("\n", profiles));
                Console.WriteLine($"Took {sw.ElapsedMilliseconds}ms");
                sw.Stop();
            }

            Console.WriteLine("----------------------------------");

            {
                var sw = new Stopwatch();
                sw.Start();

                var profiles = await userIds
                    .Select(userId =>
                        TryAsync(() => Task
                            .FromResult(UserService.GenerateProfile)
                            .Apply(UserService.FetchUser(userId))
                            .Apply(UserService.FetchConnections(userId))
                            .Apply(UserService.FetchAlbums(userId))
                        )
                        // .ToOption()
                        .ToEither()
                    )
                    .Sequence()
                    // .Map(xs => xs.Somes());
                    .Map(xs => xs.Rights());
                Console.WriteLine(string.Join("\n", profiles));
                Console.WriteLine($"Took {sw.ElapsedMilliseconds}ms");
                sw.Stop();
            }
        }
    }

    public static class UserService
    {
        public static async Task<User> FetchUser(int userId)
        {
            await Task.Delay(1000);
            return new User { Id = userId, Name = $"User {userId}" };
        }

        public static async Task<Album[]> FetchAlbums(int userId)
        {
            if (userId % 2 == 0)
            {
                throw new Exception("Internal Error!");
            }

            await Task.Delay(1000);

            return new[] {
                new Album { Id = userId * 1000, Name = $"Album {userId * 1000}" },
                new Album { Id = userId * 2000, Name = $"Album {userId * 2000}" },
                new Album { Id = userId * 3000, Name = $"Album {userId * 3000}" },
            };
        }

        public static async Task<int[]> FetchConnectionIds(int userId)
        {
            await Task.Delay(1000);
            return Enumerable.Range(userId + 1, 5).ToArray();
        }

        public static Task<User[]> FetchConnections(int userId)
        {
            return (
                from connectionIds in FetchConnectionIds(userId)
                from connections in connectionIds.Select(FetchUser).Sequence()
                select connections.ToArray()
            );
        }

        public static Func<User, User[], Album[], string> GenerateProfile
            = GetProfile;

        private static string GetProfile(
                User user,
            User[] connections,
            Album[] albums)
        {
            var photos = albums.Select(x => $"\t<li>{x.Name}</li>");

            return $@"
<h1>{user.Name}</h1>
<p> Connected with {connections[0].Name} and {connections.Length - 1} others </p>
<ul>
{string.Join("\n", photos)}
</ul>
		    ";
        }
    }

    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }

    public class Album
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int[] PhotoIds { get; set; }
    }
}
