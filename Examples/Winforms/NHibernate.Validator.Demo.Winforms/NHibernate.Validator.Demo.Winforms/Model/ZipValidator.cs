namespace NHibernate.Validator.Demo.Winforms.Model
{
	public class ZipValidator : Validator<ZipAttribute>
	{
		public override bool IsValid(object value)
		{
			string zip = (string) value;

			return true;
		}

		public override void Initialize(ZipAttribute parameters)
		{
		}
	}
}