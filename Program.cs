using System;
using System.Collections.Generic;

public class Items<T>
{
	public T Value { get; set; }
	public Items<T> Next { get; set; }
	public Items<T> Prev { get; set; }
}

public class ItemsStack<T>
{
	public Items<T> Head { get; set; }
	public Items<T> Tail { get; set; }

	public void Push(T item)
	{
		if (Head == null)
			Tail = Head = new Items<T> { Value = item, Next = null, Prev = null };
		else
		{
			var newItem = new Items<T> { Value = item, Next = null, Prev = Tail };
			Tail.Next = newItem;
			Tail = newItem;
		}
	}

	public T Pop()
	{
		if (Tail == null) return default(T);
		var result = Tail.Value;
		Tail = Tail.Prev;
		if (Tail == null)
			Head = null;
		else
			Tail.Next = null;
		return result;
	}

	public T Peek() => Tail.Value;
}

public class Clone
{
	private ItemsStack<string> history;
	private ItemsStack<string> rollbacks;

	public Clone()
	{
		history = new ItemsStack<string>();
		rollbacks = new ItemsStack<string>();
	}

	public Clone(Clone clonedItem)
	{
		history = new ItemsStack<string>()
		{ Head = clonedItem.history.Head, Tail = clonedItem.history.Tail };
		rollbacks = new ItemsStack<string>()
		{ Head = clonedItem.rollbacks.Head, Tail = clonedItem.rollbacks.Tail };
	}

	public void Learn(string programm)
	{
		history.Push(programm);
		rollbacks = new ItemsStack<string>();
	}

	public void Rollback() =>
		rollbacks.Push(history.Pop());

	public void Relearn() =>
		history.Push(rollbacks.Pop());

	public string Check() =>
		history.Head == null ? "basic" : history.Peek();
}

public class CloneVersionSystem
{
	List<Clone> clones;

	public CloneVersionSystem() =>
		clones = new List<Clone>() { new Clone() };

	public string Execute(string query)
	{
		string[] command = query.Split();
		int.TryParse(command[1], out int cloneNumber);
		cloneNumber--;

		switch (command[0])
		{
			case "learn":
				clones[cloneNumber].Learn(command[2]);
				break;
			case "rollback":
				clones[cloneNumber].Rollback();
				break;
			case "relearn":
				clones[cloneNumber].Relearn();
				break;
			case "clone":
				clones.Add(new Clone(clones[cloneNumber]));
				break;
			case "check":
				return clones[cloneNumber].Check();
		}

		return null;
	}
}

class Program
{
	static void Main()
	{
		var cloneVersionSystem = new CloneVersionSystem();

		var s = Console.ReadLine().Split();
		int numCommand = int.Parse(s[0]);
		int numProgram = int.Parse(s[1]);

		var results = new List<string>();
		for (int i = 0; i < numCommand; i++)
		{
			var result = cloneVersionSystem.Execute(Console.ReadLine());
			if (result != null) results.Add(result);
		}

		foreach (var e in results)
			Console.WriteLine(e);
	}
}