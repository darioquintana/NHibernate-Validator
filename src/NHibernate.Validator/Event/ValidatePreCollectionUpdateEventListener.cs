using System;
using NHibernate.Event;

namespace NHibernate.Validator.Event
{
	[Serializable]
	public class ValidatePreCollectionUpdateEventListener : ValidateEventListener, IPreCollectionUpdateEventListener
	{
		public void OnPreUpdateCollection(PreCollectionUpdateEvent @event)
		{
			var owner = @event.AffectedOwnerOrNull;
			if(!ReferenceEquals(null,owner))
			{
				Validate(owner, @event.Session.EntityMode);
			}
		}
	}
}