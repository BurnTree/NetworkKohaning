using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KohaningNeuralNetwork.component
{
    class Table
    {
        public double[,] mat;
        public double[,] norm;
        public double min, max;
        public int height, width;
        public List<double> listKl;

        public Table()
        {
            min = 1000;
            max = -1000;
            height = width = 0;
        }

        public void normalization()
        {
            norm = new double[height, width];
            for (int j = 0; j < width; j++)
            {
                for (int i = 0; i < height; i++)
                {
                    if (mat[i, j] < min) min = mat[i, j];
                    if (mat[i, j] > max) max = mat[i, j];
                }
                for (int i = 0; i < height; i++)
                    if (max != min)
                        norm[i, j] = (mat[i, j] - min) / (max - min);
                    else
                        norm[i, j] = 1;
                min = 1000;
                max = -1000;
            }
        }

        public int findKl(List<double> arr, double per)
        {
            for (int i = 0; i < arr.Count; i++)
            {
                if (arr[i] == per)
                    return i;
            }
            return 0;
        }
    }
}
