using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewTest
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 将有序数组B合并到有序数组A
            //int[] a = new int[100];
            //int[] b = { 1, 2, 3, 4, 5, 6, 7, 8 };
            //for (int i = 1; i < 11; i++)
            //{
            //    a[i - 1] = i * -3;
            //}

            //Merge(a, b, 10, 8);

            //for (int i = 0; a[i] != 0; i++)
            //{
            //    Console.WriteLine(a[i]);
            //}
            #endregion

            #region 取出字符数组中的第一个不重复字符
            //string str = "abbbccdefafgg ";
            //char[] charArr = str.ToArray();
            //char nonRepeatChar = FindFirstNonRepeatChar(charArr);
            //Console.WriteLine(nonRepeatChar);
            #endregion

            #region  将字符数组中的空格' '替换为'%20'
            //string str = "I am a man! ";
            //char[] arr = str.ToArray();
            //char[] newArr = new char[100];
            //for (int i = 0; i < arr.Length; i++)
            //{
            //    newArr[i] = arr[i];
            //}
            //ReplaceSpace(newArr, str.Length);
            //int k = 0;
            //while(newArr[k]!='\0')
            //{ 
            //    Console.Write(newArr[k]);
            //    k++;
            //}
            #endregion

            Console.ReadKey();
        }

        /// <summary>
        /// 将数组有序数组B中的值合并到有序数组A中。
        /// 数组A的空间无限，元素个数为m
        /// 数组B的空间有限，元素个数为n
        /// </summary>
        /// <param name="a">The a.</param>
        /// <param name="b">The b.</param>
        /// <param name="m">The m.</param>
        /// <param name="n">The n.</param>
        public static void Merge(int[] a, int[] b, int m, int n)
        {
            int tempIndex = 0;
            for (int i = 0; i < n; i++)
            {
                for (; tempIndex < m; tempIndex++)
                {
                    if (b[i] <= a[tempIndex])
                    {
                        Move(a, tempIndex, m++);
                        a[tempIndex] = b[i];
                        break;
                    }
                    else if (b[i] > a[tempIndex] && b[i] <= a[tempIndex + 1])
                    {
                        Move(a, tempIndex + 1, m++);
                        a[++tempIndex] = b[i];
                        break;
                    }
                    else if (b[i] > a[m - 1])
                    {
                        a[m++] = b[i];
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 对数组A中的元素进行迁移操作。
        /// 下标为startIndex的元素以及后面的元素依次向后移动一位。
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="m">The m.</param>
        public static void Move(int[] a, int startIndex, int m)
        {
            if (m == a.Length)
            {
                return;
            }

            for (int i = m; i > startIndex; i--)
            {
                a[i] = a[i - 1];
            }
        }


        public static char FindFirstNonRepeatChar(char[] str)
        {
            Dictionary<char, int> dict = new Dictionary<char, int>();
            for (int i = 0; i < str.Length; i++)
            {
                if (!dict.ContainsKey(str[i]))
                {
                    dict.Add(str[i], 1);
                }
                else
                {
                    dict[str[i]]++;
                }
            }

            for (int i = 0; i < str.Length; i++)
            {
                if (dict[str[i]] == 1)
                {
                    return str[i];
                }
            }

            return char.MinValue;
        }

        public static void ReplaceSpace(char[] str, int length)
        {
            int temp = 0;
            for (int i = 0; i < length; i++)
            {
                if (str[i] == ' ')
                {
                    temp = length;
                    while (temp > i)
                    {
                        str[temp + 1] = str[temp - 1];
                        temp--;
                    }
                    length += 2;

                    str[i] = '%';
                    str[i + 1] = '2';
                    str[i + 2] = '0';
                    i += 2;
                }
            }
        }
    }
}
