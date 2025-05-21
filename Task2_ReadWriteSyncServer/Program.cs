using Task2_ReadWriteSyncServer;

class Program // 10 читателей и 3 писателя для теста
{
    static void Main() 
    {
        for (int i = 0; i < 10; i++)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    int val = Server.GetCount();
                    Console.WriteLine($"[Reader] Count = {val}");
                    Thread.Sleep(100);
                }
            });
        }

        for (int i = 0; i < 3; i++)
        {
            Task.Run(() =>
            {
                Random rnd = new Random();
                while (true)
                {
                    int toAdd = rnd.Next(1, 5);
                    Server.AddToCount(toAdd);
                    Console.WriteLine($"[Writer] Added {toAdd}");
                    Thread.Sleep(500);
                }
            });
        }

        Console.ReadLine();
    }
}
