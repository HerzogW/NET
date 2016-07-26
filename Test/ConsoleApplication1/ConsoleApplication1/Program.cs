using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    //class Test
    //{
    //    static List<string> tempList = new List<string>();

    //    static void Main(string[] args)
    //    {
    //        //Parent();
    //        AddToList1();
    //        Console.ReadKey();
    //    }

    //    private static void Parent()
    //    {
    //        Task parent = new Task(AddToList1);
    //        parent.Start();
    //        parent.ContinueWith((obj) =>
    //        {
    //            foreach (string elem in tempList)
    //            {
    //                Console.WriteLine(elem);
    //            }
    //        });
    //    }

    //    private static void AddToList1()
    //    {
    //        for (int i = 0; i < 500; i++)
    //        {
    //            new Task(() => { tempList.Add(i.ToString()); }).Start();
    //        }
    //        //new Task(() => { for (int i = 0; i < 100; i++) tempList.Add(i.ToString()); }, TaskCreationOptions.AttachedToParent).Start();
    //        //new Task(() => { for (int i = 0; i < 100; i++) tempList.Add(i.ToString()); }, TaskCreationOptions.AttachedToParent).Start();
    //        //new Task(() => { for (int i = 0; i < 100; i++) tempList.Add(i.ToString()); }, TaskCreationOptions.AttachedToParent).Start();
    //        //new Task(() => { for (int i = 0; i < 100; i++) tempList.Add(i.ToString()); }, TaskCreationOptions.AttachedToParent).Start();
    //        //new Task(() => { for (int i = 0; i < 100; i++) tempList.Add(i.ToString()); }, TaskCreationOptions.AttachedToParent).Start();
    //        //new Task(() => { for (int i = 0; i < 100; i++) tempList.Add(i.ToString()); }, TaskCreationOptions.AttachedToParent).Start();
    //        //new Task(() => { for (int i = 0; i < 100; i++) tempList.Add(i.ToString()); }, TaskCreationOptions.AttachedToParent).Start();
    //        //new Task(() => { for (int i = 0; i < 100; i++) tempList.Add(i.ToString()); }, TaskCreationOptions.AttachedToParent).Start();
    //        //new Task(() => { for (int i = 0; i < 100; i++) tempList.Add(i.ToString()); }, TaskCreationOptions.AttachedToParent).Start();
    //    }
    //}

    //    class Test
    //    {

    //        static void Main(string[] args)
    //        {
    //            int[] arr = { 3, 2, 1, 4, 7, 6, 5, 4, 3, 6 };
    //            GetTheMaxLengthSortedArray(arr);

    //            Console.ReadKey();
    //        }
    //        public static void GetTheMaxLengthSortedArray(int[] arr)
    //        {
    //            int startindex = 0;
    //            int maxlength = 2;
    //            int tempStart = 0;
    //            int templength = 1;
    //            bool flag = false; //false:减少 ； true:增加

    //            if (arr.Length == 0)
    //            {
    //                //长度为0
    //            }
    //            else if (arr.Length <= 2)
    //            {
    //                //最大有序长度为arr.length；
    //            }

    //            for (int i = 1; i < arr.Length; i++)
    //            {
    //                if (!flag)//前面的序列为递减
    //                {
    //                    if (arr[i - 1] > arr[i])
    //                    {
    //                        templength++;
    //                    }
    //                    else if (arr[i - 1] < arr[i])
    //                    {
    //                        if (templength > maxlength)
    //                        {
    //                            maxlength = templength;
    //                            startindex = tempStart;
    //                        }

    //                        tempStart = i - 1;
    //                        templength = 2;
    //                        flag = true;
    //                    }
    //                    else
    //                    {
    //                        if (templength > maxlength)
    //                        {
    //                            maxlength = templength;
    //                            startindex = tempStart;
    //                        }
    //                        tempStart = i;
    //                        templength = 1;
    //                    }
    //                }
    //                else//前面的序列为递增
    //                {
    //                    if (arr[i - 1] > arr[i])
    //                    {
    //                        if (templength > maxlength)
    //                        {
    //                            maxlength = templength;
    //                            startindex = tempStart;
    //                        }

    //                        tempStart = i - 1;
    //                        templength = 2;
    //                        flag = false;
    //                    }
    //                    else if (arr[i - 1] < arr[i])
    //                    {
    //                        templength++;
    //                    }
    //                    else
    //                    {
    //                        if (templength > maxlength)
    //                        {
    //                            maxlength = templength;
    //                            startindex = tempStart;
    //                        }
    //                        tempStart = i;
    //                        templength = 1;
    //                    }
    //                }
    //            }

    //            Console.WriteLine("Start:{0},Length:{1}", startindex, maxlength);
    //        }
    //    }

    //class Test
    //{
    //    static void Main(string[] args)
    //    {
    //        string str = "-123456789012346789846464";
    //        int? result = ConvertStringIntoInt32(str);

    //        Console.WriteLine(result);
    //        Console.ReadKey();
    //    }

    //    public static int? ConvertStringIntoInt32(string originalStr)
    //    {
    //        char[] arr = originalStr.ToCharArray();
    //        int result = 0;
    //        bool negative = false;


    //        for (int i = 0; i < arr.Length; i++)
    //        {
    //            if (arr[i] == '-')
    //            {
    //                negative = true;
    //            }
    //            else if (arr[i] == '+')
    //            {
    //                negative = false;
    //            }
    //            else if (arr[i] - '0' < 0 || arr[i] - '0' > 9)
    //            {
    //                return null;
    //            }
    //            else
    //            {
    //                if (result > (int.MaxValue - (arr[i] - '0')) / 10)
    //                {
    //                    return null;
    //                }

    //                result = result * 10 + (arr[i] - '0');

    //            }
    //        }

    //        if (negative)
    //        {
    //            result = -result;
    //        }

    //        return result;
    //    }

    //}

    class Test
    {
        static void Main(string[] args)
        {
            int[] array = { 9, 8, 7, 6, 5, 4, 3, 2, 1 };
            BubbleSort(array);

            Console.WriteLine(array.ToString());
            Console.ReadKey();
        }

        public static void BubbleSort(int[] array)
        {
            if (array == null)
            {
                return;
            }

            if (array.Length == 0)
            {
                return;
            }

            int temp;
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = 0; j < array.Length - i - 1; j++)
                {
                    if (array[j] > array[j + 1])
                    {
                        temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
            }
        }
    }
}
