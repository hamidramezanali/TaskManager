// See https://aka.ms/new-console-template for more information
using Processes.Manager;
using System.Diagnostics;
using System.Text.Json;

//var result = await Cli.Wrap(@"C:\Users\hamid\source\repos\Proccess.Manager\Process.Counter\bin\Debug\net6.0\Process.Counter.exe")
//    //.WithArguments("--foo bar")
//    //.WithWorkingDirectory("work/dir/path")
//    .ExecuteBufferedAsync();
//Console.WriteLine(result.StandardOutput);


//* Create your Process
Process process = new Process();
process.StartInfo.FileName = @"C:\Users\hamid\source\repos\Proccess.Manager\ProcessManager.ManageServices\bin\Debug\net6.0\ProcessManager.ManageServices.exe";
process.StartInfo.Arguments = "/c DIR";
process.StartInfo.UseShellExecute = false;
process.StartInfo.RedirectStandardOutput = true;
process.StartInfo.RedirectStandardError = true;
//* Set your output and error (asynchronous) handlers
process.OutputDataReceived += new DataReceivedEventHandler(OutputHandler);
process.ErrorDataReceived += new DataReceivedEventHandler(OutputHandler);
//* Start process and handlers
process.Start();
process.BeginOutputReadLine();
process.BeginErrorReadLine();
process.WaitForExit();



static void OutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
{
    try
    {
        //* Do your stuff with the output (write to console/log/StringBuilder)
        Console.WriteLine(outLine.Data);
        var weatherForecast = JsonSerializer.Deserialize<WeatherForecast>(outLine.Data);
        Console.WriteLine(weatherForecast.Date.ToString());
    }
    catch (Exception)
    {

    }

}