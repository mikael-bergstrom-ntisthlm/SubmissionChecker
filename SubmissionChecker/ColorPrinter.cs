using System;

namespace ColorPrinter;

public class Printer
{
  public static void Write(string s, ConsoleColor foreground, ConsoleColor background) =>
    WriteForegroundAndBackground(s, foreground, background, Console.Write);

  public static void WriteLine(string s, ConsoleColor foreground, ConsoleColor background) =>
    WriteForegroundAndBackground(s, foreground, background, Console.WriteLine);

  private static void WriteForegroundAndBackground(string s, ConsoleColor foreground, ConsoleColor background, Action<string> WriteAction)
  {
    ConsoleColor originForeground = Console.ForegroundColor;
    ConsoleColor originBackground = Console.BackgroundColor;

    Console.ForegroundColor = foreground;
    Console.BackgroundColor = background;

    WriteAction(s);

    Console.ForegroundColor = originForeground;
    Console.BackgroundColor = originBackground;
  }

  public static void Write(string s, ConsoleColor foreground) =>
    Write(s, foreground, Console.BackgroundColor);

  public static void WriteLine(string s, ConsoleColor foreground) => 
    WriteLine(s, foreground, Console.BackgroundColor);
}
