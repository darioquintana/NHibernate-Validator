using System;
using System.Collections.Generic;

namespace NHibernate.Validator.Engine
{
	/// <summary>
	/// Allow multiple <see cref="IEntityTypeInspector"/> to work just a <see cref="IEntityTypeInspector"/>.
	/// </summary>
	/// <remarks>
	/// The <see cref="MultiEntityTypeInspector"/> iterate a list of <see cref="IEntityTypeInspector"/>
	/// for the first position to the last and return the first match.
	/// </remarks>
	public class MultiEntityTypeInspector : IEntityTypeInspector
	{
		public MultiEntityTypeInspector(IEnumerable<IEntityTypeInspector> inspectors)
		{
			if (inspectors == null)
			{
				throw new ArgumentNullException("inspectors");
			}
			Inspectors = inspectors;
		}

		public IEnumerable<IEntityTypeInspector> Inspectors { get; private set; }

		#region Implementation of IEntityTypeInspector

		public System.Type GuessType(object entityInstance)
		{
			foreach (var inspector in Inspectors)
			{
				System.Type result = inspector.GuessType(entityInstance);
				if (result != null)
					return result;
			}

			return null;
		}

		#endregion
	}
}