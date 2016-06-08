using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Test
    {
        static List<string> tempList = new List<string>();

        static void Main(string[] args)
        {
            //Parent();
            AddToList1();
            Console.ReadKey();
        }

        private static void Parent()
        {
            Task parent = new Task(AddToList1);
            parent.Start();
            parent.ContinueWith((obj) =>
            {
                foreach (string elem in tempList)
                {
                    Console.WriteLine(elem);
                }
            });
        }

        private static void AddToList1()
        {
            for (int i = 0; i < 500; i++)
            {
                new Task(() => { tempList.Add(i.ToString()); }).Start();
            }
            //new Task(() => { for (int i = 0; i < 100; i++) tempList.Add(i.ToString()); }, TaskCreationOptions.AttachedToParent).Start();
            //new Task(() => { for (int i = 0; i < 100; i++) tempList.Add(i.ToString()); }, TaskCreationOptions.AttachedToParent).Start();
            //new Task(() => { for (int i = 0; i < 100; i++) tempList.Add(i.ToString()); }, TaskCreationOptions.AttachedToParent).Start();
            //new Task(() => { for (int i = 0; i < 100; i++) tempList.Add(i.ToString()); }, TaskCreationOptions.AttachedToParent).Start();
            //new Task(() => { for (int i = 0; i < 100; i++) tempList.Add(i.ToString()); }, TaskCreationOptions.AttachedToParent).Start();
            //new Task(() => { for (int i = 0; i < 100; i++) tempList.Add(i.ToString()); }, TaskCreationOptions.AttachedToParent).Start();
            //new Task(() => { for (int i = 0; i < 100; i++) tempList.Add(i.ToString()); }, TaskCreationOptions.AttachedToParent).Start();
            //new Task(() => { for (int i = 0; i < 100; i++) tempList.Add(i.ToString()); }, TaskCreationOptions.AttachedToParent).Start();
            //new Task(() => { for (int i = 0; i < 100; i++) tempList.Add(i.ToString()); }, TaskCreationOptions.AttachedToParent).Start();
        }
    }

}
