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
            dataGridView1.Rows[4].Cells[0].Value = false;

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

        // Алгоритм однофазной сортировки простым слиянием
        private (long comparisons, long assignments, long time) SimpleOnePhaseMergeSort(int[] a)
        {
            long comparisons = 0;
            long assignments = 0;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int n = a.Length;

            int[] b = new int[n];
            int[] c = new int[n];
            int[] d = new int[n];
            int[] e = new int[n];

            int bCount = 0, cCount = 0;
            for (int i = 0; i < n; i++)
            {
                if (i % 2 == 0) b[bCount++] = a[i];
                else c[cCount++] = a[i];
                assignments++;
            }

            int seriesLength = 1;
            bool directDir = true;

            while (seriesLength < n)
            {
                if (directDir)
                    MergePhase(b, c, bCount, cCount, d, e, out bCount, out cCount, seriesLength, ref comparisons, ref assignments);
                else
                    MergePhase(d, e, bCount, cCount, b, c, out bCount, out cCount, seriesLength, ref comparisons, ref assignments);

                seriesLength *= 2;
                directDir = !directDir;
            }

            int[] finalSrc = directDir ? b : d;
            Array.Copy(finalSrc, a, n);
            assignments += n;

            stopwatch.Stop();
            return (comparisons, assignments, stopwatch.ElapsedMilliseconds);
        }

        // Вспомогательный метод для выполнения одной фазы слияния-распределения
        private void MergePhase(int[] src1, int[] src2, int n1, int n2, int[] dest1, int[] dest2,
                                out int d1Count, out int d2Count, int len, ref long comp, ref long assig)
        {
            d1Count = 0; d2Count = 0;
            int i = 0, j = 0;
            bool writeToFirst = true;

            while (i < n1 || j < n2)
            {
                int limit1 = Math.Min(i + len, n1);
                int limit2 = Math.Min(j + len, n2);
                int[] currentDest = writeToFirst ? dest1 : dest2;
                ref int currentCounter = ref (writeToFirst ? ref d1Count : ref d2Count);

                while (i < limit1 && j < limit2)
                {
                    comp++;
                    currentDest[currentCounter++] = (src1[i] <= src2[j]) ? src1[i++] : src2[j++];
                    assig++;
                }
                while (i < limit1) { currentDest[currentCounter++] = src1[i++]; assig++; }
                while (j < limit2) { currentDest[currentCounter++] = src2[j++]; assig++; }

                writeToFirst = !writeToFirst;
            }
        }

        // Алгоритм двухфазной сортировки естественным слиянием 
        private (long comparisons, long assignments, long time) NaturalTwoPhaseMergeSort(int[] a)
        {
            long comparisons = 0;
            long assignments = 0;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int n = a.Length;
            int[] b = new int[n];
            int[] c = new int[n];

            while (true)
            {
                int bCount = 0, cCount = 0, i = 0;
                bool toB = true;
                int seriesCount = 0;

                while (i < n)
                {
                    seriesCount++;
                    if (toB)
                    {
                        b[bCount++] = a[i++]; assignments++;
                        while (i < n && a[i] >= a[i - 1]) { comparisons++; b[bCount++] = a[i++]; assignments++; }
                    }
                    else
                    {
                        c[cCount++] = a[i++]; assignments++;
                        while (i < n && a[i] >= a[i - 1]) { comparisons++; c[cCount++] = a[i++]; assignments++; }
                    }
                    if (i < n) comparisons++;
                    toB = !toB;
                }

                if (seriesCount <= 1) break;

                int aIdx = 0, bIdx = 0, cIdx = 0;
                while (bIdx < bCount && cIdx < cCount)
                {
                    bool bEnd = false, cEnd = false;
                    while (!bEnd && !cEnd)
                    {
                        comparisons++;
                        if (b[bIdx] <= c[cIdx])
                        {
                            a[aIdx++] = b[bIdx++]; assignments++;
                            if (bIdx >= bCount || b[bIdx] < b[bIdx - 1]) bEnd = true;
                        }
                        else
                        {
                            a[aIdx++] = c[cIdx++]; assignments++;
                            if (cIdx >= cCount || c[cIdx] < c[cIdx - 1]) cEnd = true;
                        }
                    }
                    while (!bEnd) { a[aIdx++] = b[bIdx++]; assignments++; if (bIdx >= bCount || b[bIdx] < b[bIdx - 1]) bEnd = true; }
                    while (!cEnd) { a[aIdx++] = c[cIdx++]; assignments++; if (cIdx >= cCount || c[cIdx] < c[cIdx - 1]) cEnd = true; }
                }
                while (bIdx < bCount) { a[aIdx++] = b[bIdx++]; assignments++; }
                while (cIdx < cCount) { a[aIdx++] = c[cIdx++]; assignments++; }
            }
            stopwatch.Stop();
            return (comparisons, assignments, stopwatch.ElapsedMilliseconds);
        }

        // Алгоритм однофазной сортировки естественным слиянием
        private (long comparisons, long assignments, long time) NaturalOnePhaseMergeSort(int[] a)
        {
            long comparisons = 0;
            long assignments = 0;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            int n = a.Length;
            int[] b = new int[n];
            int[] c = new int[n];
            int[] d = new int[n];
            int[] e = new int[n];

            int bCount = 0, cCount = 0, i = 0;
            bool toB = true;
            while (i < n)
            {
                int[] target = toB ? b : c;
                ref int count = ref (toB ? ref bCount : ref cCount);

                target[count++] = a[i++]; assignments++;
                while (i < n && a[i] >= a[i - 1])
                {
                    comparisons++;
                    target[count++] = a[i++];
                    assignments++;
                }
                if (i < n) comparisons++;
                toB = !toB;
            }

            bool directDir = true;
            int totalRuns;

            do
            {
                if (directDir)
                    NaturalMergePhase(b, c, bCount, cCount, d, e, out bCount, out cCount, out totalRuns, ref comparisons, ref assignments);
                else
                    NaturalMergePhase(d, e, bCount, cCount, b, c, out bCount, out cCount, out totalRuns, ref comparisons, ref assignments);

                directDir = !directDir;
            } while (totalRuns > 1);

            Array.Copy(directDir ? b : d, a, n);
            assignments += n;

            stopwatch.Stop();
            return (comparisons, assignments, stopwatch.ElapsedMilliseconds);
        }

        // Вспомогательный метод для однофазного естественного слияния
        private void NaturalMergePhase(int[] src1, int[] src2, int n1, int n2, int[] dest1, int[] dest2,
                                        out int d1Count, out int d2Count, out int totalRuns, ref long comp, ref long assig)
        {
            d1Count = 0; d2Count = 0; totalRuns = 0;
            int i = 0, j = 0;
            bool writeToFirst = true;

            while (i < n1 || j < n2)
            {
                totalRuns++;
                int[] target = writeToFirst ? dest1 : dest2;
                ref int counter = ref (writeToFirst ? ref d1Count : ref d2Count);

                bool run1End = i >= n1, run2End = j >= n2;

                while (!run1End && !run2End)
                {
                    comp++;
                    if (src1[i] <= src2[j])
                    {
                        target[counter++] = src1[i++]; assig++;
                        run1End = (i >= n1 || src1[i] < src1[i - 1]);
                    }
                    else
                    {
                        target[counter++] = src2[j++]; assig++;
                        run2End = (j >= n2 || src2[j] < src2[j - 1]);
                    }
                }

                while (!run1End) { target[counter++] = src1[i++]; assig++; run1End = (i >= n1 || src1[i] < src1[i - 1]); }
                while (!run2End) { target[counter++] = src2[j++]; assig++; run2End = (j >= n2 || src2[j] < src2[j - 1]); }

                writeToFirst = !writeToFirst;
            }
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

            if ((bool)dataGridView1.Rows[1].Cells[0].Value)
            {
                int[] sortingArray = (int[])originalArray.Clone();
                var result = SimpleOnePhaseMergeSort(sortingArray);

                dataGridView1.Rows[1].Cells[2].Value = result.comparisons;
                dataGridView1.Rows[1].Cells[3].Value = result.assignments;
                dataGridView1.Rows[1].Cells[4].Value = result.time + " мс";
                dataGridView1.Rows[1].Cells[5].Value = IsSorted(sortingArray) ? "Да" : "Нет";
            }

            if ((bool)dataGridView1.Rows[2].Cells[0].Value)
            {
                int[] sortingArray = (int[])originalArray.Clone();
                var result = NaturalTwoPhaseMergeSort(sortingArray);

                dataGridView1.Rows[2].Cells[2].Value = result.comparisons;
                dataGridView1.Rows[2].Cells[3].Value = result.assignments;
                dataGridView1.Rows[2].Cells[4].Value = result.time + " мс";
                dataGridView1.Rows[2].Cells[5].Value = IsSorted(sortingArray) ? "Да" : "Нет";
            }

            if ((bool)dataGridView1.Rows[3].Cells[0].Value)
            {
                int[] sortingArray = (int[])originalArray.Clone();
                var result = NaturalOnePhaseMergeSort(sortingArray);

                dataGridView1.Rows[3].Cells[2].Value = result.comparisons;
                dataGridView1.Rows[3].Cells[3].Value = result.assignments;
                dataGridView1.Rows[3].Cells[4].Value = result.time + " мс";
                dataGridView1.Rows[3].Cells[5].Value = IsSorted(sortingArray) ? "Да" : "Нет";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}