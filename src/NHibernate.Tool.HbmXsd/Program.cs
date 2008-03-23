using System;

namespace NHibernate.Tool.NhvXsd
{
	public class Program
	{
		private static void Main(string[] args)
		{
			// For debugging: ..\..\..\NHibernate\Cfg\MappingSchema\Hbm.generated.cs

			if (args.Length == 1)
				new NhvCodeGenerator().Execute(args[0]);
			else
				Console.WriteLine("usage: HbmXsd <outputfile>");
		}
	}
}