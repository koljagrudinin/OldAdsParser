using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace _074_Parser.Enigne.Parser
{
    class ImageEncoder
    {
        private static List<bool[,]> dataMassive = new List<bool[,]>();

        private static List<bool[,]> ClearMassive = new List<bool[,]>();
 
        public static string RemakeImage(Bitmap bmp)
        {
            EncodeToBool(bmp);
            RemoveTrash();
            dataMassive = new List<bool[,]>();
            return"";
        }

        private static void EncodeToBool(Bitmap bi)
        {
             //1. split image to one chars
            List<Bitmap> temp = new List<Bitmap>();
            List<int> columnsToRemove = new List<int>();

            for (int i = 0; i < bi.Width; i++)
            {
                bool ignored=false;
                for (int j = 0; j < bi.Height; j++)
                {
                    Color c = bi.GetPixel(i, j);
                    if (c.R < 20 && c.G < 20 || c.B < 20)
                    {
                        ignored = true;
                        break;
                    }
                }
                if (ignored == false)
                {
                    columnsToRemove.Add(i);
                }
            }

            int a = 0;
            foreach (var item in columnsToRemove)
            {
                if (item > a + 1)
                {
                    Bitmap newBMP = new Bitmap(item - a, bi.Height);
                    for (int i = a; i < item; i++)
                    {
                        for (int j = 0; j < bi.Height; j++)
                        {
                            newBMP.SetPixel(i - a, j, bi.GetPixel(i, j));
                        }
                    }
                    temp.Add(newBMP);
                    a = item;
                }
                else
                    a = item;
            }

            //REMOVE TRASH

            List<int> trash = new List<int>();
            foreach (var item in temp)
            {
                if (item.Width == 6)
                {
                    trash.Add(temp.IndexOf(item));
                }
            }
            foreach (var item in trash)
            {
                temp.RemoveAt(item);
            }

            foreach (var item in temp)
            {
                AddBoolModel(item);
            }
        }

        private static void AddBoolModel(Bitmap btm)
        {
            int h = btm.Height;
            int w = btm.Width;
            bool[,] temp = new bool[h, w];
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    Color c = btm.GetPixel(i, j);
                    if (c.B < 20 && c.G < 20 && c.R < 20)
                    {
                        temp[i, j] = true;
                    }
                    else
                        temp[i, j] = false;
                }
            }

            dataMassive.Add(temp);
        }

        private static void RemoveTrash()
        {
            ClearMassive = new List<bool[,]>();
            foreach (var item in dataMassive)
            {
                
            }
        }
    }
}
