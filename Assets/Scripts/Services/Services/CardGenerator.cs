using System.Text;
using UnityEngine;

public static class CardGenerator
{
    public static string GenerateCardNumber()
    {
        int length = 16;
        int[] digits = new int[length];

        for (int i = 0; i < length - 1; i++)
        {
            digits[i] = Random.Range(0, 10);
        }

        int sum = 0;
        for (int i = length - 2; i >= 0; i--)
        {
            int digit = digits[i];
            int positionFromRight = (length - 1 - i);

            if (positionFromRight % 2 == 1)
            {
                digit *= 2;
                if (digit > 9)
                    digit -= 9;
            }
            sum += digit;
        }

        int checkDigit = (10 - (sum % 10)) % 10;
        digits[length - 1] = checkDigit;

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < length; i++)
        {
            sb.Append(digits[i]);
        }
        return sb.ToString();
    }

    public static string GenerateCVV()
    {
        int cvv = Random.Range(0, 1000);
        return cvv.ToString("D3");
    }

    public static string GenerateExpirationDate()
    {
        System.DateTime now = System.DateTime.Now;
        int month = now.Month; 
        int year = now.Year + 4; 
        int shortYear = year % 100; 
        return month.ToString("D2") + "/" + shortYear.ToString("D2");
    }

}
