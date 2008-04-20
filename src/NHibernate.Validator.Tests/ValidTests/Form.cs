namespace NHibernate.Validator.Tests.ValidTests
{
	public class Form
	{
		private Member _member;

		[Valid]
		public Member Member
		{
			get { return _member; }
			set { _member = value; }
		}
	}
}