using System;
using System.Collections;
namespace NHibernate.Validator.Util
{
	public static class EnumerableExtensions
	{
		public static bool Any(this IEnumerable source)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			var enumerator = source.GetEnumerator();
			var dispntor = enumerator as IDisposable;
			try
			{
				return enumerator.MoveNext();				
			}
			finally
			{
				if (dispntor != null)
				{
					dispntor.Dispose();
				}
			}
		}
	}
}