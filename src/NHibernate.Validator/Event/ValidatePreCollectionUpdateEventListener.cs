using System;
using System.Threading;
using System.Threading.Tasks;
using NHibernate.Event;

namespace NHibernate.Validator.Event
{
	[Serializable]
	public class ValidatePreCollectionUpdateEventListener : ValidateEventListener, IPreCollectionUpdateEventListener
	{
		public Task OnPreUpdateCollectionAsync(PreCollectionUpdateEvent @event, CancellationToken cancellationToken)
		{
			OnPreUpdateCollection(@event);
			return Task.CompletedTask;
		}

		public void OnPreUpdateCollection(PreCollectionUpdateEvent @event)
		{
			var owner = @event.AffectedOwnerOrNull;
			if (!ReferenceEquals(null, owner))
			{
				Validate(owner, @event.Session.GetEntityPersister(@event.Session.GetEntityName(owner), owner).EntityMode);
			}
		}
	}
}