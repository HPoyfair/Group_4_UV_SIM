using System.IO;



public class Simulator
{

    private int[] memory = new int[100];


    public void Run()
    {
        
    }

    public void ReadFile(string path)
    {
        

        //check if file exists first
        if (!File.Exists(path))
    {
        Console.WriteLine($"File at path {path} not found.");
        return;
    }
        //read file and load instructions into memory 
        string[] lines = File.ReadAllLines(path);

        for (int i = 0; i < lines.Length && i < memory.Length; i++)
        {
            //stop reading if empty line exists
            if(lines[i].Trim() == "") break;
            //convert string to number and store in memory at the
            memory[i] = int.Parse(lines[i]);
        }

    }


    public void LogMemory()
    {
        for (int i = 0; i < memory.Length; i++)
        {
            Console.WriteLine($"Memory[{i}]: {memory[i]}");
        }
    }
}