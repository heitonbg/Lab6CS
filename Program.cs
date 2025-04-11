using System;

public class SquareMatrix
{
    private int[,] _matrix;
    private int _size;

    public int Size => _size;
    public int[,] Matrix => _matrix;

    public SquareMatrix(int size)
    {
        _size = size;
        _matrix = new int[size, size];
        Random random = new Random();
        for (int row = 0; row < size; ++row)
        {
            for (int column = 0; column < size; ++column)
            {
                _matrix[row, column] = random.Next(1, 10);
            }
        }
    }

    public SquareMatrix(int[,] matrix)
    {
        if (matrix.GetLength(0) != matrix.GetLength(1))
        {
            throw new ArgumentException("Матрица должна быть квадратной.");
        }
        _size = matrix.GetLength(0);
        _matrix = (int[,])matrix.Clone();
    }

    public static SquareMatrix operator +(SquareMatrix firstMatrix, SquareMatrix secondMatrix)
    {
        if (firstMatrix._size != secondMatrix._size)
        {
            throw new ArgumentException("Матрицы должны быть одного размера для сложения.");
        }

        int[,] result = new int[firstMatrix._size, firstMatrix._size];
        for (int row = 0; row < firstMatrix._size; ++row)
        {
            for (int column = 0; column < firstMatrix._size; ++column)
            {
                result[row, column] = firstMatrix._matrix[row, column] + secondMatrix._matrix[row, column];
            }
        }
        return new SquareMatrix(result);
    }

    public static SquareMatrix operator *(SquareMatrix firstMatrix, SquareMatrix secondMatrix)
    {
        if (firstMatrix._size != secondMatrix._size)
        {
            throw new ArgumentException("Матрицы должны быть одного размера для умножения.");
        }

        int[,] result = new int[firstMatrix._size, firstMatrix._size];
        for (int row = 0; row < firstMatrix._size; ++row)
        {
            for (int column = 0; column < firstMatrix._size; ++column)
            {
                result[row, column] = 0;
                for (int innerIndex = 0; innerIndex < firstMatrix._size; ++innerIndex)
                {
                    result[row, column] += firstMatrix._matrix[row, innerIndex] * secondMatrix._matrix[innerIndex, column];
                }
            }
        }
        return new SquareMatrix(result);
    }

    public override string ToString()
    {
        string result = "";
        for (int row = 0; row < _size; ++row)
        {
            for (int column = 0; column < _size; ++column)
            {
                result += _matrix[row, column] + "\t";
            }
            result += "\n";
        }
        return result;
    }
}

// Расширяющие методы
public static class SquareMatrixExtensions
{
    // Транспонирование матрицы
    public static SquareMatrix Transpose(this SquareMatrix matrix)
    {
        int[,] transposed = new int[matrix.Size, matrix.Size];
        for (int i = 0; i < matrix.Size; i++)
        {
            for (int j = 0; j < matrix.Size; j++)
            {
                transposed[j, i] = matrix.Matrix[i, j];
            }
        }
        return new SquareMatrix(transposed);
    }

    // Нахождение следа матрицы (сумма диагональных элементов)
    public static int Trace(this SquareMatrix matrix)
    {
        int trace = 0;
        for (int i = 0; i < matrix.Size; i++)
        {
            trace += matrix.Matrix[i, i];
        }
        return trace;
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Калькулятор квадратных матриц");

        Console.Write("Введите размер матриц: ");
        int size = int.Parse(Console.ReadLine());

        SquareMatrix matrix1 = new SquareMatrix(size);
        SquareMatrix matrix2 = new SquareMatrix(size);

        while (true)
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine("1. Показать матрицы");
            Console.WriteLine("2. Сложить матрицы");
            Console.WriteLine("3. Умножить матрицы");
            Console.WriteLine("4. Транспонировать матрицу 1");
            Console.WriteLine("5. Найти след матрицы 1");
            Console.WriteLine("6. Создать новые матрицы");
            Console.WriteLine("7. Выход");

            Console.Write("Выберите действие: ");
            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    Console.WriteLine("\nМатрица 1:");
                    Console.WriteLine(matrix1);
                    Console.WriteLine("Матрица 2:");
                    Console.WriteLine(matrix2);
                    break;

                case 2:
                    try
                    {
                        SquareMatrix sum = matrix1 + matrix2;
                        Console.WriteLine("\nРезультат сложения:");
                        Console.WriteLine(sum);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }
                    break;

                case 3:
                    try
                    {
                        SquareMatrix product = matrix1 * matrix2;
                        Console.WriteLine("\nРезультат умножения:");
                        Console.WriteLine(product);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка: {ex.Message}");
                    }
                    break;

                case 4:
                    SquareMatrix transposed = matrix1.Transpose();
                    Console.WriteLine("\nТранспонированная матрица 1:");
                    Console.WriteLine(transposed);
                    break;

                case 5:
                    int trace = matrix1.Trace();
                    Console.WriteLine($"\nСлед матрицы 1: {trace}");
                    break;

                case 6:
                    Console.Write("Введите новый размер матриц: ");
                    size = int.Parse(Console.ReadLine());
                    matrix1 = new SquareMatrix(size);
                    matrix2 = new SquareMatrix(size);
                    Console.WriteLine("Созданы новые матрицы.");
                    break;

                case 7:
                    return;

                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }
        }
    }
}