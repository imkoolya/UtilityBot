class Functions
{
    public string Sum(string text)
    {
        string[] arr = text.Split(" ");
        int sum = 0;
        foreach (var item in arr)
        {
            if (int.TryParse(item, out int number)) sum += number;
        }
        return $"Сумма чисел: {sum}";
    }
    public string SplitNumbers(string str)
    {
        List<int> arr = str.Split(' ').Select(x => Convert.ToInt32(x)).ToList();

        return this.Sum(str);
    }
}