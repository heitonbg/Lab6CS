using System;

namespace MatrixCalculator
{
  public class SquareMatrix
  {
    private readonly int[,] _matrixData;
    public int Size { get; }

    public SquareMatrix(int size)
    {
      Size = size;
      _matrixData = new int[size, size];
      var randomGenerator = new Random();
      
      for (int rowIndex = 0; rowIndex < size; ++rowIndex)
      {
        for (int columnIndex = 0; columnIndex < size; ++columnIndex)
        {
          _matrixData[rowIndex, columnIndex] = randomGenerator.Next(1, 10);
        }
      }
    }

    public int this[int row, int column]
    {
      get => _matrixData[row, column];
      set => _matrixData[row, column] = value;
    }

    public override string ToString()
    {
      var result = new System.Text.StringBuilder();
      
      for (int rowIndex = 0; rowIndex < Size; ++rowIndex)
      {
        for (int columnIndex = 0; columnIndex < Size; ++columnIndex)
        {
          result.Append($"{_matrixData[rowIndex, columnIndex]}\t");
        }
        result.AppendLine();
      }
      
      return result.ToString();
    }
  }

  public static class MatrixOperations
  {
    public static SquareMatrix AddMatrices(SquareMatrix firstMatrix, SquareMatrix secondMatrix)
    {
      if (firstMatrix.Size != secondMatrix.Size)
      {
        throw new ArgumentException("Размеры матрицы должны совпадать для сложения");
      }

      var resultMatrix = new SquareMatrix(firstMatrix.Size);
      
      for (int rowIndex = 0; rowIndex < firstMatrix.Size; ++rowIndex)
      {
        for (int columnIndex = 0; columnIndex < firstMatrix.Size; ++columnIndex)
        {
          resultMatrix[rowIndex, columnIndex] = firstMatrix[rowIndex, columnIndex] + secondMatrix[rowIndex, columnIndex];
        }
      }
      
      return resultMatrix;
    }

    public static SquareMatrix MultiplyMatrices(SquareMatrix firstMatrix, SquareMatrix secondMatrix)
    {
      if (firstMatrix.Size != secondMatrix.Size)
      {
        throw new ArgumentException("Размеры матрицы должны совпадать для умножения");
      }

      var resultMatrix = new SquareMatrix(firstMatrix.Size);
      
      for (int rowIndex = 0; rowIndex < firstMatrix.Size; ++rowIndex)
      {
        for (int columnIndex = 0; columnIndex < firstMatrix.Size; ++columnIndex)
        {
          for (int innerIndex = 0; innerIndex < firstMatrix.Size; ++innerIndex)
          {
            resultMatrix[rowIndex, columnIndex] += firstMatrix[rowIndex, innerIndex] * secondMatrix[innerIndex, columnIndex];
          }
        }
      }
      
      return resultMatrix;
    }

    public static SquareMatrix TransposeMatrix(SquareMatrix matrix)
    {
      var transposedMatrix = new SquareMatrix(matrix.Size);
      
      for (int rowIndex = 0; rowIndex < matrix.Size; ++rowIndex)
      {
        for (int columnIndex = 0; columnIndex < matrix.Size; ++columnIndex)
        {
          transposedMatrix[columnIndex, rowIndex] = matrix[rowIndex, columnIndex];
        }
      }
      
      return transposedMatrix;
    }

    public static int CalculateTrace(SquareMatrix matrix)
    {
      int trace = 0;
      
      for (int diagonalIndex = 0; diagonalIndex < matrix.Size; ++diagonalIndex)
      {
        trace += matrix[diagonalIndex, diagonalIndex];
      }
      
      return trace;
    }
  }

  public abstract class MatrixOperationHandler
  {
    protected MatrixOperationHandler _nextHandler;

    public void SetNextHandler(MatrixOperationHandler nextHandler)
    {
      _nextHandler = nextHandler;
    }

    public abstract void HandleRequest(int operationChoice, SquareMatrix firstMatrix, SquareMatrix secondMatrix);
  }

  public class MatrixDisplayHandler : MatrixOperationHandler
  {
    public override void HandleRequest(int operationChoice, SquareMatrix firstMatrix, SquareMatrix secondMatrix)
    {
      if (operationChoice == 1)
      {
        Console.WriteLine("Первая матрица:");
        Console.WriteLine(firstMatrix);
        Console.WriteLine("Вторая матрица:");
        Console.WriteLine(secondMatrix);
      }
      else if (_nextHandler != null)
      {
        _nextHandler.HandleRequest(operationChoice, firstMatrix, secondMatrix);
      }
    }
  }

  public class MatrixAdditionHandler : MatrixOperationHandler
  {
    public override void HandleRequest(int operationChoice, SquareMatrix firstMatrix, SquareMatrix secondMatrix)
    {
      if (operationChoice == 2)
      {
        try
        {
          Console.WriteLine("Сумма матриц:");
          Console.WriteLine(MatrixOperations.AddMatrices(firstMatrix, secondMatrix));
        }
        catch (Exception exception)
        {
          Console.WriteLine($"Ошибка: {exception.Message}");
        }
      }
      else if (_nextHandler != null)
      {
        _nextHandler.HandleRequest(operationChoice, firstMatrix, secondMatrix);
      }
    }
  }

  public class MatrixMultiplicationHandler : MatrixOperationHandler
  {
    public override void HandleRequest(int operationChoice, SquareMatrix firstMatrix, SquareMatrix secondMatrix)
    {
      if (operationChoice == 3)
      {
        try
        {
          Console.WriteLine("Произведение матриц:");
          Console.WriteLine(MatrixOperations.MultiplyMatrices(firstMatrix, secondMatrix));
        }
        catch (Exception exception)
        {
          Console.WriteLine($"Ошибка: {exception.Message}");
        }
      }
      else if (_nextHandler != null)
      {
        _nextHandler.HandleRequest(operationChoice, firstMatrix, secondMatrix);
      }
    }
  }

  public class MatrixTranspositionHandler : MatrixOperationHandler
  {
    public override void HandleRequest(int operationChoice, SquareMatrix firstMatrix, SquareMatrix secondMatrix)
    {
      if (operationChoice == 4)
      {
        Console.WriteLine("Транспонированная первая матрица:");
        Console.WriteLine(MatrixOperations.TransposeMatrix(firstMatrix));
      }
      else if (_nextHandler != null)
      {
        _nextHandler.HandleRequest(operationChoice, firstMatrix, secondMatrix);
      }
    }
  }

  public class MatrixTraceHandler : MatrixOperationHandler
  {
    public override void HandleRequest(int operationChoice, SquareMatrix firstMatrix, SquareMatrix secondMatrix)
    {
      if (operationChoice == 5)
      {
        Console.WriteLine($"След первой матрицы: {MatrixOperations.CalculateTrace(firstMatrix)}");
      }
      else if (_nextHandler != null)
      {
        _nextHandler.HandleRequest(operationChoice, firstMatrix, secondMatrix);
      }
    }
  }

  public class MatrixRecreationHandler : MatrixOperationHandler
  {
    public override void HandleRequest(int operationChoice, SquareMatrix firstMatrix, SquareMatrix secondMatrix)
    {
      if (operationChoice == 6)
      {
        Console.Write("Введите размер новых матриц: ");
        int newSize = int.Parse(Console.ReadLine());
        firstMatrix = new SquareMatrix(newSize);
        secondMatrix = new SquareMatrix(newSize);
        Console.WriteLine("Новые матрицы созданы");
      }
      else if (_nextHandler != null)
      {
        _nextHandler.HandleRequest(operationChoice, firstMatrix, secondMatrix);
      }
    }
  }

  public class Program
  {
    public static void Main(string[] args)
    {
      Console.WriteLine("Матричный калькулятор");
      Console.Write("Введите размер матрицы: ");
      int matrixSize = int.Parse(Console.ReadLine());

      SquareMatrix firstMatrix = new SquareMatrix(matrixSize);
      SquareMatrix secondMatrix = new SquareMatrix(matrixSize);

      var displayHandler = new MatrixDisplayHandler();
      var additionHandler = new MatrixAdditionHandler();
      var multiplicationHandler = new MatrixMultiplicationHandler();
      var transpositionHandler = new MatrixTranspositionHandler();
      var traceHandler = new MatrixTraceHandler();
      var recreationHandler = new MatrixRecreationHandler();

      displayHandler.SetNextHandler(additionHandler);
      additionHandler.SetNextHandler(multiplicationHandler);
      multiplicationHandler.SetNextHandler(transpositionHandler);
      transpositionHandler.SetNextHandler(traceHandler);
      traceHandler.SetNextHandler(recreationHandler);

      while (true)
      {
        Console.WriteLine("\nМеню:");
        Console.WriteLine("1. Показать матрицы");
        Console.WriteLine("2. Сложить матрицы");
        Console.WriteLine("3. Перемножение матриц");
        Console.WriteLine("4. Транспонирование первой матрицы");
        Console.WriteLine("5. Посчитать след первой матрицы");
        Console.WriteLine("6. Создать новые матрицы");
        Console.WriteLine("7. Выход");

        Console.Write("Выбор действия: ");
        int selectedOperation = int.Parse(Console.ReadLine());

        if (selectedOperation == 7)
        {
          break;
        }

        displayHandler.HandleRequest(selectedOperation, firstMatrix, secondMatrix);
      }
    }
  }
}