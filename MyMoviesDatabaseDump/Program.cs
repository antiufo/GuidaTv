using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using antiufo.TvGuide;

namespace MyMoviesDatabaseDump
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new Model1Container();
            if (!db.DatabaseExists()) db.CreateDatabase();

            var dump = new MyMoviesDump(db);
            for (int i= 2030; i >= 1800; i--)
            {
                dump.DumpYear(i);
            }
      

            var gen = new MoviesFileGenerator();
            dump.SaveGenreToBinaryFile(gen);
            gen.Save(@"..\..\..\TvGuide\Movies.dat");

            //update Movies set Genre=NULL WHERE Genre IN ('   Genere       ', ', ', 'Non definito') or Genre like '%genere%'

            

            Console.WriteLine("Done.");
      //      Console.ReadKey();
        }



    }
}
