using System;
using Dapper;
using Npgsql;

namespace ConsoleApplication
{   
    public class Beer
    {
        public int id;
        public Guid lovely_uuid;
        public string name;
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            using (var conn = new NpgsqlConnection("Host=elevate.postgres.local;Username=elevate_recruit_user;Password=elevate;Database=elevate_recruit"))
            {
                conn.Open();
                var x = conn.Query<Beer>("select * from beers");
                Console.WriteLine(ObjectDumper.Dump(x));
            }
        }
    }
}
