using System;

namespace ConsoleMenu
{
  public class Menu<T>
  {
    private List<MenuItem> _items;
    private string _title;
    private int _currentSelection;

    private ConsoleColor originalBackground;
    private ConsoleColor originalForeground;

    public Menu(string title)
    {
      _items = new List<MenuItem>();
      _title = title;
      _currentSelection = 0;

      originalBackground = Console.BackgroundColor;
      originalForeground = Console.ForegroundColor;
    }

    public void AddMenuItem(string text, T value)
    {
      _items.Add(new MenuItem(text, value));
    }

    public T GetChoice()
    {
      // Clear screen
      Console.Clear();
      Console.CursorVisible = false;


      // Display title
      Console.WriteLine(_title);

      // Display menu items
      for (int i = 0; i < _items.Count; i++)
      {
        PrintMenuItem(i);
      }

      // Input loop

      while (true)
      {
        int prevSelection = _currentSelection;
        ConsoleKeyInfo key = Console.ReadKey(true);

        switch (key.Key)
        {
          case ConsoleKey.DownArrow:
            _currentSelection++;
            if (_currentSelection >= _items.Count)
            {
              _currentSelection = 0;
            }
            break;
          case ConsoleKey.UpArrow:
            _currentSelection--;
            if (_currentSelection < 0)
            {
              _currentSelection = _items.Count - 1;
            }
            break;
          case ConsoleKey.Enter:
            Console.Clear();
            return _items[_currentSelection].Value;
        }
        PrintMenuItem(prevSelection);
        PrintMenuItem(_currentSelection);
      }
    }

    public List<T> GetChoices()
    {
      List<T> choices = new List<T>();

      string choice = "";
      while (choice != "q")
      {
        choices.Add(GetChoice());
      }

      return choices;
    }

    public string GetTextOfValue(T value)
    {
      return _items.Find(x => x.Value.Equals(value)).Text;
    }

    public void PrintMenuItem(int i)
    {
      if (i == _currentSelection)
      {
        Console.BackgroundColor = originalForeground;
        Console.ForegroundColor = originalBackground;
      }
      else
      {
        Console.BackgroundColor = originalBackground;
        Console.ForegroundColor = originalForeground;
      }

      Console.CursorTop = i + 2;
      Console.CursorLeft = 0;
      Console.WriteLine($"  {_items[i].Text}  ");

      Console.BackgroundColor = originalBackground;
      Console.ForegroundColor = originalForeground;
    }

    private class MenuItem
    {
      public string Text { get; private set; }
      public T Value { get; private set; }
      public bool Selected { get; set; }

      public MenuItem(string text, T value)
      {
        Text = text;
        Value = value;
        Selected = false;
      }
    }
  }
}