using System.Diagnostics;

namespace lab22_26_siaod
{
    public partial class Form1 : Form
    {
        private Random random = new Random();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridView1.RowCount = 5;
            dataGridView1.ColumnCount = 6;

            dataGridView1.Rows[0].Cells[1].Value = "Простое 2ф";
            dataGridView1.Rows[1].Cells[1].Value = "Простое 1ф";
            dataGridView1.Rows[2].Cells[1].Value = "Естественное 2ф";
            dataGridView1.Rows[3].Cells[1].Value = "Естественное 1ф";
            dataGridView1.Rows[4].Cells[1].Value = "Поглощение";

            dataGridView1.Rows[0].Cells[0].Value = true;
            dataGridView1.Rows[1].Cells[0].Value = true;
            dataGridView1.Rows[2].Cells[0].Value = true;
            dataGridView1.Rows[3].Cells[0].Value = true;
            dataGridView1.Rows[4].Cells[0].Value = true;

            dataGridView1.RowHeadersVisible = false;
        }

        // Функция проверки упорядоченности массива 
        private bool IsSorted(int[] array)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                if (array[i] > array[i + 1])
                    return false;
            }
            return true;
        }

        // Алгоритм простого двухфазного слияния
        private (long comparisons, long assignments, long time) SimpleTwoPhaseMergeSort(int[] a)
        {
            long comparisons = 0;
            long assignments = 0;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int n = a.Length;
            int[] b = new int[n];
            int[] c = new int[n];
            int seriesLength = 1;

            while (seriesLength < n)
            {
                int bCount = 0;
                int cCount = 0;
                int i = 0;

                while (i < n)
                {
                    for (int j = 0; j < seriesLength && i < n; j++)
                    {
                        b[bCount++] = a[i++];
                        assignments++;
                    }
                    for (int j = 0; j < seriesLength && i < n; j++)
                    {
                        c[cCount++] = a[i++];
                        assignments++;
                    }
                }

                int aIdx = 0, bIdx = 0, cIdx = 0;

                while (bIdx < bCount || cIdx < cCount)
                {
                    int bEnd = Math.Min(bIdx + seriesLength, bCount);
                    int cEnd = Math.Min(cIdx + seriesLength, cCount);

                    while (bIdx < bEnd && cIdx < cEnd)
                    {
                        comparisons++;
                        if (b[bIdx] <= c[cIdx])
                        {
                            a[aIdx++] = b[bIdx++];
                        }
                        else
                        {
                            a[aIdx++] = c[cIdx++];
                        }
                        assignments++;
                    }

                    while (bIdx < bEnd)
                    {
                        a[aIdx++] = b[bIdx++];
                        assignments++;
                    }

                    while (cIdx < cEnd)
                    {
                        a[aIdx++] = c[cIdx++];
                        assignments++;
                    }
                }

                seriesLength *= 2;
            }

            stopwatch.Stop();
            return (comparisons, assignments, stopwatch.ElapsedMilliseconds);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Если галочка снята — стираем результаты 
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (dataGridView1.Rows[i].Cells[0].Value == null || !(bool)dataGridView1.Rows[i].Cells[0].Value)
                {
                    dataGridView1.Rows[i].Cells[2].Value = null;
                    dataGridView1.Rows[i].Cells[3].Value = null;
                    dataGridView1.Rows[i].Cells[4].Value = null;
                    dataGridView1.Rows[i].Cells[5].Value = null;
                }
            }

            int size = (int)numericUpDown1.Value;
            int[] originalArray = new int[size];

            for (int i = 0; i < size; i++)
                originalArray[i] = random.Next(size);

            if ((bool)dataGridView1.Rows[0].Cells[0].Value)
            {
                int[] sortingArray = (int[])originalArray.Clone();
                var result = SimpleTwoPhaseMergeSort(sortingArray);

                dataGridView1.Rows[0].Cells[2].Value = result.comparisons;
                dataGridView1.Rows[0].Cells[3].Value = result.assignments;
                dataGridView1.Rows[0].Cells[4].Value = result.time + " мс";
                dataGridView1.Rows[0].Cells[5].Value = IsSorted(sortingArray) ? "Да" : "Нет";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}