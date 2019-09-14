using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;

namespace FPinCSharp.Example5
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            /*
                Populating the profile page of a user in a social media application
            */

            // User input
            var userId = 1;

            {
                var sw = new Stopwatch();
                sw.Start();
                var user = await UserService.FetchUser(userId);
                var connectionIds = await UserService.FetchConnectionIds(userId);
                var connections = await Task.WhenAll(
                    connectionIds.Select(id => UserService.FetchUser(id))
                );
                var albums = await UserService.FetchAlbums(userId);
                var profile = UserService.GenerateProfile(user, connections, albums);
                Console.WriteLine(profile);
                Console.WriteLine($"Took {sw.ElapsedMilliseconds}ms");
                sw.Stop();
            }

            Console.WriteLine("----------------------------------");

            {
                var sw = new Stopwatch();
                sw.Start();
                var profile = await (
                    from user in UserService.FetchUser(userId)
                    from connectionIds in UserService.FetchConnectionIds(userId)
                    from connections in connectionIds.Select(UserService.FetchUser).Sequence()
                    from albums in UserService.FetchAlbums(userId)
                    select UserService.GenerateProfile(user, connections.ToArray(), albums)
                );

                Console.WriteLine(profile);
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

        public static string GenerateProfile(
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
