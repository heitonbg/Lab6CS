using System;

public class SquareMatrix
{
    private int[,] _matrix;
    public int Size { get; }

    public SquareMatrix(int size)
    {
        Size = size;
        _matrix = new int[size, size];
        var random = new Random();
        for (int i = 0; i < size; i++)
            for (int j = 0; j < size; j++)
                _matrix[i, j] = random.Next(1, 10);
    }

    public int this[int i, int j]
    {
        get => _matrix[i, j];
        set => _matrix[i, j] = value;
    }

    public override string ToString()
    {
        string result = "";
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
                result += $"{_matrix[i, j]}\t";
            result += "\n";
        }
        return result;
    }
}

public static class MatrixOperations
{
    public static SquareMatrix Add(SquareMatrix a, SquareMatrix b)
    {
        if (a.Size != b.Size) throw new ArgumentException("Размеры матриц не совпадают");

        var result = new SquareMatrix(a.Size);
        for (int i = 0; i < a.Size; i++)
            for (int j = 0; j < a.Size; j++)
                result[i, j] = a[i, j] + b[i, j];
        return result;
    }

    public static SquareMatrix Multiply(SquareMatrix a, SquareMatrix b)
    {
        if (a.Size != b.Size) throw new ArgumentException("Размеры матриц не совпадают");

        var result = new SquareMatrix(a.Size);
        for (int i = 0; i < a.Size; i++)
            for (int j = 0; j < a.Size; j++)
                for (int k = 0; k < a.Size; k++)
                    result[i, j] += a[i, k] * b[k, j];
        return result;
    }

    public static SquareMatrix Transpose(SquareMatrix m)
    {
        var result = new SquareMatrix(m.Size);
        for (int i = 0; i < m.Size; i++)
            for (int j = 0; j < m.Size; j++)
                result[j, i] = m[i, j];
        return result;
    }

    public static int Trace(SquareMatrix m)
    {
        int trace = 0;
        for (int i = 0; i < m.Size; i++)
            trace += m[i, i];
        return trace;
    }
}

public abstract class MatrixHandler
{
    protected MatrixHandler successor;

    public void SetSuccessor(MatrixHandler successor)
    {
        this.successor = successor;
    }

    public abstract void HandleRequest(int choice, SquareMatrix m1, SquareMatrix m2);
}

public class ShowHandler : MatrixHandler
{
    public override void HandleRequest(int choice, SquareMatrix m1, SquareMatrix m2)
    {
        if (choice == 1)
        {
            Console.WriteLine("Матрица 1:");
            Console.WriteLine(m1);
            Console.WriteLine("Матрица 2:");
            Console.WriteLine(m2);
        }
        else if (successor != null)
        {
            successor.HandleRequest(choice, m1, m2);
        }
    }
}

public class AddHandler : MatrixHandler
{
    public override void HandleRequest(int choice, SquareMatrix m1, SquareMatrix m2)
    {
        if (choice == 2)
        {
            try
            {
                Console.WriteLine("Сумма матриц:");
                Console.WriteLine(MatrixOperations.Add(m1, m2));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
        else if (successor != null)
        {
            successor.HandleRequest(choice, m1, m2);
        }
    }
}

public class MultiplyHandler : MatrixHandler
{
    public override void HandleRequest(int choice, SquareMatrix m1, SquareMatrix m2)
    {
        if (choice == 3)
        {
            try
            {
                Console.WriteLine("Произведение матриц:");
                Console.WriteLine(MatrixOperations.Multiply(m1, m2));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
        else if (successor != null)
        {
            successor.HandleRequest(choice, m1, m2);
        }
    }
}

public class TransposeHandler : MatrixHandler
{
    public override void HandleRequest(int choice, SquareMatrix m1, SquareMatrix m2)
    {
        if (choice == 4)
        {
            Console.WriteLine("Транспонированная матрица 1:");
            Console.WriteLine(MatrixOperations.Transpose(m1));
        }
        else if (successor != null)
        {
            successor.HandleRequest(choice, m1, m2);
        }
    }
}

public class TraceHandler : MatrixHandler
{
    public override void HandleRequest(int choice, SquareMatrix m1, SquareMatrix m2)
    {
        if (choice == 5)
        {
            Console.WriteLine($"След матрицы 1: {MatrixOperations.Trace(m1)}");
        }
        else if (successor != null)
        {
            successor.HandleRequest(choice, m1, m2);
        }
    }
}

public class NewMatricesHandler : MatrixHandler
{
    public override void HandleRequest(int choice, SquareMatrix m1, SquareMatrix m2)
    {
        if (choice == 6)
        {
            Console.Write("Введите новый размер матриц: ");
            int size = int.Parse(Console.ReadLine());
            m1 = new SquareMatrix(size);
            m2 = new SquareMatrix(size);
            Console.WriteLine("Созданы новые матрицы");
        }
        else if (successor != null)
        {
            successor.HandleRequest(choice, m1, m2);
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Калькулятор матриц (Цепочка ответственности)");
        Console.Write("Введите размер матриц: ");
        int size = int.Parse(Console.ReadLine());

        SquareMatrix matrix1 = new SquareMatrix(size);
        SquareMatrix matrix2 = new SquareMatrix(size);

        // Создаем цепочку обработчиков
        var showHandler = new ShowHandler();
        var addHandler = new AddHandler();
        var multiplyHandler = new MultiplyHandler();
        var transposeHandler = new TransposeHandler();
        var traceHandler = new TraceHandler();
        var newHandler = new NewMatricesHandler();

        showHandler.SetSuccessor(addHandler);
        addHandler.SetSuccessor(multiplyHandler);
        multiplyHandler.SetSuccessor(transposeHandler);
        transposeHandler.SetSuccessor(traceHandler);
        traceHandler.SetSuccessor(newHandler);

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

            if (choice == 7) break;

            showHandler.HandleRequest(choice, matrix1, matrix2);
        }
    }
}