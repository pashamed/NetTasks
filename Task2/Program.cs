int[] sortedArray = GenerateSortedArray(50);
Console.WriteLine("Before removing");
foreach (int i in sortedArray)
{
    
    Console.Write(i + " ");
}

var noDuplicates = RemoveDuplicates(sortedArray);
Console.WriteLine();
Console.WriteLine("After removing");
foreach (int i in noDuplicates)
{
    
    Console.Write(i + " ");
}



static int[] GenerateSortedArray(int size)
{
    int[] array = new int[size];
    Random random = new Random();

    for (int i = 0; i < size; i++)
    {
        array[i] = random.Next(1, 20);
    }

    Array.Sort(array);
    return array;
}


//Time compelxity O(n)
//Space complexity O(1)
static int[] RemoveDuplicates(int[] sortedArray)
{
    if (sortedArray == null || sortedArray.Length == 0)
    {
        return new int[0];
    }

    int uniqueCount = 1;

    for (int i = 1; i < sortedArray.Length; i++)
    {
        if (sortedArray[i] != sortedArray[i - 1])
        {
            uniqueCount++;
        }
    }

    int[] uniqueArray = new int[uniqueCount];
    uniqueArray[0] = sortedArray[0];

    int index = 1;
    for (int i = 1; i < sortedArray.Length; i++)
    {
        if (sortedArray[i] != sortedArray[i - 1])
        {
            uniqueArray[index++] = sortedArray[i];
        }
    }

    return uniqueArray;
}

