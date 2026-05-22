# Comparison of Merge Sort Variations & Absorption Sort – Windows Forms App

This Windows Forms application implements and compares **five different sorting algorithms**:

- Simple two‑phase merge sort
- Simple one‑phase merge sort
- Natural two‑phase merge sort
- Natural one‑phase merge sort
- Absorption sort (external sorting with limited memory)

For each algorithm, the program measures:
- Number of **comparisons**
- Number of **assignments** (element moves)
- **Execution time** (milliseconds)

All algorithms are tested on **the same random array** (cloned from the original) to ensure fair comparison.  
The results are displayed in a table, along with a verification column indicating whether the array was successfully sorted.

<img width="622" height="240" alt="image" src="https://github.com/user-attachments/assets/ca282650-f745-47e4-ac2c-6cb0e94674f1" />

## ✨ Features

- **Select any combination of algorithms** – five checkboxes allow you to enable/disable individual sorts.
- **Adjustable array size** – set the number of elements (default 1 000 000).
- **Memory percentage for absorption sort** – controls how much “RAM” (as a percentage of array size) the absorption algorithm may use (default 10%).
- **One‑click benchmark** – the same random array is cloned and passed to each enabled algorithm.
- **Detailed metrics**:
  - Number of comparisons
  - Number of assignments (data movements)
  - Total time in milliseconds
  - “Sorted?” – Yes/No (validates correctness)
- **Simple exit button** – closes the application.

## 🧮 Implemented Algorithms

| Algorithm | Description | Key characteristics |
|-----------|-------------|----------------------|
| **Simple two‑phase merge sort** | Classical merge sort that always splits into fixed‑length series (powers of two). | Two auxiliary arrays (`b`, `c`), series length doubles each pass. |
| **Simple one‑phase merge sort** | One‑phase variant that distributes initial elements alternately, then repeatedly merges runs. | Four auxiliary arrays, alternates between two sets of buffers. |
| **Natural two‑phase merge sort** | Uses existing ascending runs (natural series) instead of fixed length. | Detects already sorted subsequences, reduces passes on partially sorted data. |
| **Natural one‑phase merge sort** | Natural series + one‑phase merging (alternating between buffer pairs). | Efficient for data with long natural runs. |
| **Absorption sort** | Simulates external sorting with limited internal memory. | Parameters: percentage of array that can be held in memory. Performs repeated “absorption” of sorted blocks. **Uses `Array.Sort` for in‑memory block sorting** (the rest of the algorithm is manual). |

All merge‑sort variants are implemented **from scratch** (no built‑in sorting methods) and count comparisons and assignments directly in the code.  
The absorption sort counts only the comparisons and assignments performed by the user‑written code (block reads, writes, merging), but relies on .NET's `Array.Sort` for sorting each memory‑resident block.

## 🚀 How to Use

1. **Launch the application** – open the solution in Visual Studio and press `F5`.
2. **Select algorithms** – check the boxes next to the sorts you want to compare.
3. **Set array size** – use the “Размер массива” numeric up‑down (default 1 000 000).
4. **Set memory percentage** – only relevant for absorption sort, default 10%.
5. **Click “Сортировать”** – the program generates a random integer array of the specified size, clones it for each enabled algorithm, runs the sorts, and fills the table.
6. **View results** – the table shows comparisons, assignments, time (ms), and whether the result is sorted.
7. **Exit** – click the red exit button (or close the window).

> **Note:** For large array sizes (e.g., 10 million), the program may take several seconds or minutes, depending on your hardware.

## 🖥️ User Interface

| Column | Content |
|--------|---------|
| ✔ (checkbox) | Enable/disable the algorithm |
| (Algorithm name) | Simple 2‑phase, Simple 1‑phase, Natural 2‑phase, Natural 1‑phase, Absorption |
| Сравнения | Total number of key comparisons performed |
| Присвоения | Total number of element assignments (copies/moves) |
| Время | Execution time in milliseconds |
| Отсортировано? | “Да” if the resulting array is sorted in non‑descending order, otherwise “Нет” |

Below the table there are:
- **Сортировать** button – start benchmarking
- **Размер массива** – array size (positive integer)
- **% ОП** – percentage of memory for absorption sort (1‑100)
- **Exit button** – closes the app

## 🛠️ Requirements

- **.NET 6.0** or higher (Windows Forms)
- **Visual Studio 2022** (or any C# IDE with Windows Forms designer)
- **Operating System**: Windows (Windows Forms dependency)

## 📦 Build & Run

1. Open the solution file (`.slnx`) in Visual Studio.
2. Set the startup project to the one containing `Form1.cs`.
3. Build the solution (`Ctrl+Shift+B`).
4. Run with `F5`.

Alternatively, compile from the command line using `dotnet build` (if the project is SDK‑style).

## 📊 Example Output (Array Size = 100,000)

| Algorithm | Comparisons | Assignments | Time (ms) | Sorted? |
|-----------|-------------|-------------|-----------|---------|
| Simple two‑phase | 1,546,882 | 2,400,000 | 125 | Yes |
| Simple one‑phase | 1,546,882 | 2,800,000 | 142 | Yes |
| Natural two‑phase | 1,432,109 | 2,350,000 | 98 | Yes |
| Natural one‑phase | 1,430,876 | 2,700,000 | 110 | Yes |
| Absorption (10%) | 2,987,654 | 3,200,000 | 310 | Yes |

*Values are illustrative; actual numbers depend on the random seed and hardware.*

## 🔍 Implementation Details

- **Random array** – generated with `Random.Next(size)`; the same array is cloned using `(int[])originalArray.Clone()` for each algorithm.
- **Comparison & assignment counting** – every time two keys are compared (`if (a[i] <= a[j])`) a counter is incremented; every time an element is moved (`dest[k] = src[m]`) an assignment counter is incremented.
- **Timing** – `System.Diagnostics.Stopwatch` measures wall‑clock time in milliseconds.
- **Correctness check** – `IsSorted(int[] array)` verifies non‑descending order.
- **Absorption sort** – simulates external sorting with a limited “memory” of `ceil(percent/100 * n)` elements. It repeatedly reads chunks, sorts them in‑memory, and merges them into the main array.

## 📝 Notes

- The absorption sort’s memory percentage is relative to the array size. For example, 10% of a 1 000 000‑element array means an internal buffer of 100 000 integers.
- All algorithms are implemented iteratively/recursively as shown in the code; they sort in **ascending order**.
- The natural merge algorithms detect **non‑decreasing runs** (`a[i] >= a[i-1]`). This makes them faster on already sorted or partially sorted data.
- The simple merge sorts always use fixed‑length series (powers of two), which may perform extra passes compared to natural variants.
- If the array size is very large (e.g., > 10 million), the program may consume significant memory (multiple clones). Be careful on memory‑constrained systems.

## 📄 License

Project created for educational purposes. Free use and modification are permitted with attribution.

---

**Author:** Aleksandr Chernetsov  
**Group:** 24VP1  
**Laboratory work:** 22–26
