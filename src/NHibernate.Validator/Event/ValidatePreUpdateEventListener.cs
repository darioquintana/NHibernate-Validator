using System;
using System.Threading;
using System.Threading.Tasks;
using NHibernate.Event;

namespace NHibernate.Validator.Event
{
	/// <summary>
	/// Before update, executes the validator framework
	/// </summary>
	/// <remarks>
	/// Because, usually, we validate on insert and on update we are
	/// using the same environment for PreInsert and  PreUpdate event listeners,
	/// the initialization of the environment (the ValidatorEngine) was or will be done in 
	/// ValidatePreInsertEventListener by NH.
	/// This give us better performance on NH startup.
	/// </remarks>
	[Serializable]
	public class ValidatePreUpdateEventListener : ValidateEventListener, IPreUpdateEventListener
	{
		#region IPreUpdateEventListener Members

		public Task<bool> OnPreUpdateAsync(PreUpdateEvent @event, CancellationToken cancellationToken)
		{
			OnPreUpdate(@event);
			return Task.FromResult(false);
		}

		public bool OnPreUpdate(PreUpdateEvent @event)
		{
			Validate(@event.Entity, @event.Persister.EntityMode);
			return false;
		}

		#endregion
	}
}