using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scanapp
{
    internal class ConsoleTable
    {
        private List<List<string>> data;
        private List<string>? headers;


        public ConsoleTable(List<List<string>> data, List<string>? headers = null)
        {
            int nRows =  data.Count;
            if (nRows == 0)
                throw new Exception("List cannot be empty");
            int nCols = data[0].Count;
            data.ForEach(row =>
            {
                if (row.Count != nCols)
                    throw new Exception("Column length must be constant");
            });
            if (headers != null && headers.Count != nCols)
                throw new Exception("Column length must be equal to header length");

            this.data = data;
            this.headers = headers;
            PrintData();
        }

        private void PrintData()
        {
            int[] rowSizes = this.getRowSizes();
            printSeparatingLine(rowSizes);
            if(this.headers != null)
            {
                printDataLine(this.headers, rowSizes);
                printSeparatingLine(rowSizes);
            }
            this.data.ForEach(row => printDataLine(row, rowSizes));
        }

        private void printDataLine(List<string> dataline, int[] rowSizes)
        {
            Console.Write("|");
            for(int i = 0; i<dataline.Count; i++)
            {
                Console.Write(" ");
                Console.Write(dataline[i]);
                for (int j = 0; j < rowSizes[i] - dataline[i].Length + 1; j++)
                    Console.Write(" ");
                Console.Write("|");
            }
            Console.Write("\n");
            printSeparatingLine(rowSizes);
        }

        private void printSeparatingLine(int[] rowSizes)
        {
            Console.Write("+");
            for(int col = 0; col < rowSizes.Length; col++)
            {
                for (int colchar = 0; colchar < rowSizes[col]+2; colchar++)
                    Console.Write("-");
                Console.Write("+");
            }
            Console.Write("\n");
        }

        private int[] getRowSizes()
        {
            int[] maxCharacterCount = new int[this.data[0].Count];
            if(this.headers != null)
            {
                for (int i = 0; i < this.headers.Count; i++)
                {
                    maxCharacterCount[i] = Math.Max(maxCharacterCount[i], this.headers[i].Length);
                }
            }
            for(int i = 0; i < data.Count; i++)
            {
                for (int j = 0; j < data[0].Count; j++)
                    maxCharacterCount[j] = Math.Max(maxCharacterCount[j], data[i][j].Length);
            }
            return maxCharacterCount;
        }

    }
}
