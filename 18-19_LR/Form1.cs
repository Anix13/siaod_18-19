using System;
using System.Windows.Forms;

namespace _18_19_LR
{
    public partial class Form1 : Form
    {
        // Очередь с приоритетом представлена в виде массива размером 16
        private int[] priorityQueue = new int[16];
        private int queueSize = 0; // Текущий размер очереди
        private Random random = new Random(); // Генератор случайных чисел

        public Form1()
        {
            InitializeComponent();
            InitializeDataGridViews(); // Инициализация таблиц
        }

        private void InitializeDataGridViews()
        {
            // Настройка первой таблицы
            dataGridView1.RowCount = 1;
            dataGridView1.ColumnCount = 15;
            dataGridView1.ColumnHeadersVisible = false;
            dataGridView1.RowHeadersVisible = false;
            for (int i = 0; i < 15; i++) dataGridView1.Columns[i].Width = 40;

            // Настройка второй таблицы
            dataGridView2.RowCount = 4;
            dataGridView2.ColumnCount = 15;
            dataGridView2.ColumnHeadersVisible = false;
            dataGridView2.RowHeadersVisible = false;
            for (int i = 0; i < 15; i++) dataGridView2.Columns[i].Width = 40;

            // Настройка третьей таблицы (для максимального элемента)
            dataGridView3.RowCount = 1;
            dataGridView3.ColumnCount = 1;
            dataGridView3.ColumnHeadersVisible = false;
            dataGridView3.RowHeadersVisible = false;
            dataGridView3.Columns[0].Width = 80;
        }

        // Кнопка "Создать очередь"
        private void button1_Click(object sender, EventArgs e)
        {
            CreateQueue(); // Создание очереди
            Print(priorityQueue); // Вывод в таблицы
            textBox1.Text = "Очередь создана и отображена";
        }

        // Кнопка "Очистить очередь"
        private void button2_Click(object sender, EventArgs e)
        {
            if (queueSize == 0)
            {
                textBox1.Text = "Очередь уже пуста";
                MessageBox.Show("Очередь уже пуста", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Clear_Tab(); // Очистка таблиц
            queueSize = 0; // Обнуление размера очереди
            textBox1.Text = "Все массивы очищены";
        }

        // Кнопка "Извлечь максимальный элемент"
        private void button3_Click(object sender, EventArgs e)
        {
            if (queueSize == 0)
            {
                textBox1.Text = "Очередь пуста";
                return;
            }

            // Извлечение максимального элемента (корня пирамиды)
            int max = priorityQueue[1];
            dataGridView3.Rows[0].Cells[0].Value = max.ToString();

            // Перемещение последнего элемента в корень
            priorityQueue[1] = priorityQueue[queueSize];
            priorityQueue[queueSize] = 0;
            queueSize--;

            // Восстановление свойств кучи
            for (int i = 1; i <= queueSize; i++)
            {
                fixUp(i);
            }

            UpdateArrayGridView(); // Обновление таблицы
            Print(priorityQueue);
            textBox1.Text = $"Извлечен максимальный элемент: {max}";
        }

        // Кнопка "Добавить элемент"
        private void button4_Click(object sender, EventArgs e)
        {
            if (queueSize >= 15)
            {
                MessageBox.Show("Очередь переполнена", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox1.Text = "Очередь переполнена";
                return;
            }

            // Генерация случайного числа
            int newElement = random.Next(10, 100);
            queueSize++;
            priorityQueue[queueSize] = newElement;
            fixUp(queueSize); // Восстановление свойств кучи

            UpdateArrayGridView();
            Print(priorityQueue);
            textBox1.Text = $"Добавлен элемент: {newElement}";
        }

        // Кнопка "Изменить приоритет" (не реализована)
        private void button5_Click(object sender, EventArgs e)
        {
        }

        // Создание очереди из 15 случайных элементов
        private void CreateQueue()
        {
            queueSize = 15;
            for (int i = 1; i <= 15; i++)
                priorityQueue[i] = random.Next(10, 100);

            // Построение кучи
            for (int i = 2; i <= queueSize; i++)
            {
                fixUp(i);
            }

            UpdateArrayGridView();
        }

        // Восстановление свойств пирамиды (при добавлении элемента)
        private void fixUp(int index)
        {
            // Пока текущий элемент больше родителя, меняем их местами
            while (index > 1 && priorityQueue[index] > priorityQueue[index / 2])
            {
                Swap(index, index / 2);
                index /= 2;
            }
        }

        // Обмен значений в массиве
        private void Swap(int i, int j)
        {
            int temp = priorityQueue[i];
            priorityQueue[i] = priorityQueue[j];
            priorityQueue[j] = temp;
        }

        // Вывод массива в таблицы
        private void Print(int[] A)
        {
            Clear_Tab(); // Очистка таблиц перед выводом

            // Вывод в первую таблицу
            for (int i = 1; i <= queueSize; i++)
            {
                dataGridView1.Rows[0].Cells[i - 1].Value = A[i].ToString();
            }

            // Координаты для отображения кучи во второй таблице
            int[,] positions =
            {
                {0, 7},
                {1, 3}, {1, 11},
                {2, 1}, {2, 5}, {2, 9}, {2, 13},
                {3, 0}, {3, 2}, {3, 4}, {3, 6}, {3, 8}, {3, 10}, {3, 12}, {3, 14}
            };

            // Вывод в виде пирамиды во второй таблице
            for (int i = 1; i <= queueSize; i++)
            {
                if (i - 1 >= positions.GetLength(0)) break;
                int row = positions[i - 1, 0];
                int col = positions[i - 1, 1];
                dataGridView2.Rows[row].Cells[col].Value = A[i].ToString();
            }
        }

        // Очистка таблиц
        private void Clear_Tab()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
                foreach (DataGridViewCell cell in row.Cells)
                    cell.Value = "";

            foreach (DataGridViewRow row in dataGridView2.Rows)
                foreach (DataGridViewCell cell in row.Cells)
                    cell.Value = "";

            dataGridView3.Rows[0].Cells[0].Value = "";
        }

        // Обновление первой таблицы (массив)
        private void UpdateArrayGridView()
        {
            for (int i = 0; i < 15; i++)
                dataGridView1.Rows[0].Cells[i].Value = (i + 1 <= queueSize) ? priorityQueue[i + 1].ToString() : "";
        }

        // Закрытие формы
        private void button6_Click_1(object sender, EventArgs e) => this.Close();

        // Пустые обработчики событий
        private void label1_Click(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
        private void Form1_Load(object sender, EventArgs e) { }
    }
}
