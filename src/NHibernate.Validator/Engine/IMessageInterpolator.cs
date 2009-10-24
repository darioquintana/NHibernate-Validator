namespace NHibernate.Validator.Engine
{
	/// <summary>
	/// Responsible for validator message interpolation (variable replacement etc)
	/// this extension point is useful if the call has some contextual informations to
	/// interpolate in validator messages
	/// </summary>
	public interface IMessageInterpolator
	{
		string Interpolate(InterpolationInfo info);
	}
}