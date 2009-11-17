using System;
using System.Collections.Generic;
using System.Linq;

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
			return Inspectors.Select(inspector => inspector.GuessType(entityInstance)).FirstOrDefault(result => result != null);
		}

		#endregion
	}
}