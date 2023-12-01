using System.Diagnostics;

int n = 10;
Stopwatch sw = Stopwatch.StartNew();
sw.Start();
GetFibonacciIterative(n);
sw.Stop();
Console.WriteLine("Itterative time: " + sw.ElapsedTicks);

sw.Restart();
GetFibonacciRecursive(n);
sw.Stop();
Console.WriteLine("Recursive time: " + sw.ElapsedTicks);



//Time complexity O(n)
//Space complexity O(1)
static long GetFibonacciIterative(int n)
{
    if (n <= 1)
    {
        return n;
    }

    long a = 0, b = 1, result = 0;
    for (int i = 2; i <= n; i++)
    {
        result = a + b;
        a = b;
        b = result;
    }
    return result;
}
 
//Time complexity O(n^2)
//Space complexity O(n)
static long GetFibonacciRecursive(int n)
{
    
    if (n <= 1)
    {
        return n;
    }
    return GetFibonacciRecursive(n - 1) + GetFibonacciRecursive(n - 2);
}