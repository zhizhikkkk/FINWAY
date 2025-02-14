using System.Text;
using UnityEngine;

public static class CardGenerator
{
    /// <summary>
    /// Генерирует 16-значный номер карты, удовлетворяющий алгоритму Луна.
    /// </summary>
    public static string GenerateCardNumber()
    {
        int length = 16;
        int[] digits = new int[length];

        // Сгенерируем первые 15 цифр случайным образом
        for (int i = 0; i < length - 1; i++)
        {
            digits[i] = Random.Range(0, 10);
        }

        // Вычисляем контрольную цифру по алгоритму Луна для оставшихся 15 цифр
        int sum = 0;
        // Идем с конца к началу, исключая последнюю цифру (check digit)
        // В алгоритме Луна, начиная с правой (без контрольной) цифры, удваиваем каждую вторую цифру.
        for (int i = length - 2; i >= 0; i--)
        {
            int digit = digits[i];
            int positionFromRight = (length - 1 - i); // позиция от правого края (начиная с 1)

            // Если позиция четная (то есть каждая вторая цифра), удваиваем
            if (positionFromRight % 2 == 1)
            {
                digit *= 2;
                if (digit > 9)
                    digit -= 9;
            }
            sum += digit;
        }

        // Контрольная цифра — это число, которое нужно прибавить, чтобы сумма стала кратной 10
        int checkDigit = (10 - (sum % 10)) % 10;
        digits[length - 1] = checkDigit;

        // Собираем номер в строку
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < length; i++)
        {
            sb.Append(digits[i]);
        }
        return sb.ToString();
    }

    /// <summary>
    /// Генерирует случайный 3-значный CVV.
    /// </summary>
    public static string GenerateCVV()
    {
        int cvv = Random.Range(0, 1000);
        return cvv.ToString("D3");
    }

    /// <summary>
    /// Генерирует дату истечения в формате "MM/YY".
    /// Случайный месяц от 1 до 12, год от текущего +3 до текущего +7.
    /// </summary>
    public static string GenerateExpirationDate()
    {
        System.DateTime now = System.DateTime.Now;
        int month = now.Month; // Месяц текущей даты
        int year = now.Year + 4; // Год через 4 года
        int shortYear = year % 100; // последние две цифры года
        return month.ToString("D2") + "/" + shortYear.ToString("D2");
    }

}
