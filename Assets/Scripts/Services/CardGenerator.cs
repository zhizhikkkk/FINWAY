using System.Text;
using UnityEngine;

public static class CardGenerator
{
    /// <summary>
    /// ���������� 16-������� ����� �����, ��������������� ��������� ����.
    /// </summary>
    public static string GenerateCardNumber()
    {
        int length = 16;
        int[] digits = new int[length];

        // ����������� ������ 15 ���� ��������� �������
        for (int i = 0; i < length - 1; i++)
        {
            digits[i] = Random.Range(0, 10);
        }

        // ��������� ����������� ����� �� ��������� ���� ��� ���������� 15 ����
        int sum = 0;
        // ���� � ����� � ������, �������� ��������� ����� (check digit)
        // � ��������� ����, ������� � ������ (��� �����������) �����, ��������� ������ ������ �����.
        for (int i = length - 2; i >= 0; i--)
        {
            int digit = digits[i];
            int positionFromRight = (length - 1 - i); // ������� �� ������� ���� (������� � 1)

            // ���� ������� ������ (�� ���� ������ ������ �����), ���������
            if (positionFromRight % 2 == 1)
            {
                digit *= 2;
                if (digit > 9)
                    digit -= 9;
            }
            sum += digit;
        }

        // ����������� ����� � ��� �����, ������� ����� ���������, ����� ����� ����� ������� 10
        int checkDigit = (10 - (sum % 10)) % 10;
        digits[length - 1] = checkDigit;

        // �������� ����� � ������
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < length; i++)
        {
            sb.Append(digits[i]);
        }
        return sb.ToString();
    }

    /// <summary>
    /// ���������� ��������� 3-������� CVV.
    /// </summary>
    public static string GenerateCVV()
    {
        int cvv = Random.Range(0, 1000);
        return cvv.ToString("D3");
    }

    /// <summary>
    /// ���������� ���� ��������� � ������� "MM/YY".
    /// ��������� ����� �� 1 �� 12, ��� �� �������� +3 �� �������� +7.
    /// </summary>
    public static string GenerateExpirationDate()
    {
        System.DateTime now = System.DateTime.Now;
        int month = now.Month; // ����� ������� ����
        int year = now.Year + 4; // ��� ����� 4 ����
        int shortYear = year % 100; // ��������� ��� ����� ����
        return month.ToString("D2") + "/" + shortYear.ToString("D2");
    }

}
