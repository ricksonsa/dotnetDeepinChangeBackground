using Topshelf;

namespace cbservice
{
    class Program
    {
        static void Main(string[] args)
        {
            var bgService = new BackgroundService();

            while (true)
            {
                try
                {
                    bgService.Start();
                }
                catch (System.Exception ex)
                {
                    bgService.Stop();
                    System.Console.WriteLine(ex.Message);
                }

                System.Console.Read();
            }

        }
    }
}