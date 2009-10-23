using System.Collections.Generic;
using NHibernate.Validator.Interpolator;
using NUnit.Framework;
using SharpTestsEx;

namespace NHibernate.Validator.Tests.Interpolation
{
	[TestFixture]
	public class MessageTranslatorTest
	{
		[Test]
		public void ParseTokensToTranslate()
		{
			var mt = new MessageTranslator("blah {validator.const} ${Property} ${Property.Prop} {Const}");
			mt.RelevantTokens.Should().Have.SameValuesAs("{validator.const}", "${Property}", "${Property.Prop}", "{Const}");
		}

		[Test]
		public void RelevantTokens_DoesNotReturnDuplications()
		{
			var mt = new MessageTranslator("blah {Const} ${Property} ${Property} {Const}");
			mt.RelevantTokens.Should().Have.SameValuesAs("{Const}", "${Property}").And.Have.Count.EqualTo(2);
		}

		[Test]
		public void RelevantTokens_ExcludeEscaped()
		{
			var mt = new MessageTranslator("blah {Const} #{Const} $#{Property} #$#{Property}");
			mt.RelevantTokens.Should().Have.SameSequenceAs("{Const}");
		}

		[Test]
		public void RelevantTokens_ExcludeNotValidVariableNames()
		{
			var mt = new MessageTranslator(@"blah {Const} {Co_nst} ${Pro""perty} #$#{Prop'erty}");
			mt.RelevantTokens.Should().Have.SameValuesAs("{Const}", "{Co_nst}");
		}

		[Test]
		public void Replace_TokensToTranslate()
		{
			var mt = new MessageTranslator("blah {validator.const} ${Property} ${Property.Prop} {Const}");
			var replacements = new Dictionary<string, string>
			                   	{
			                   		{"{validator.const}", "1"}, 
														{"${Property}", "2"}, 
														{"${Property.Prop}", "3"}, 
														{"{Const}", "4"}
			                   	};
			mt.Replace(replacements).Should().Be.EqualTo("blah 1 2 3 4");
		}

		[Test]
		public void Replace_Duplications()
		{
			var mt = new MessageTranslator("blah {Const} ${Property} ${Property} {Const}");
			var replacements = new Dictionary<string, string> { { "${Property}", "2" }, { "{Const}", "1" } };
			mt.Replace(replacements).Should().Be.EqualTo("blah 1 2 2 1");
		}

		[Test]
		public void Replace_IgnoreReplacementsNotRequired()
		{
			var mt = new MessageTranslator("blah {validator.const} ${Property} ${Property.Prop} {Const}");
			var replacements = new Dictionary<string, string> {{"${Property}", "1"}, {"{Const}", "2"}};
			mt.Replace(replacements).Should().Be.EqualTo("blah {validator.const} 1 ${Property.Prop} 2");
		}

		[Test]
		public void Replace_NullWithEmpty()
		{
			var mt = new MessageTranslator("blah {validator.const} ${Property} ${Property.Prop} {Const}");
			var replacements = new Dictionary<string, string> { { "${Property}", null }, { "{Const}", "2" } };
			mt.Replace(replacements).Should().Be.EqualTo("blah {validator.const}  ${Property.Prop} 2");
		}

		[Test]
		public void Replace_Escaped()
		{
			var mt = new MessageTranslator("blah {Const} #{Const} $#{Property} $#{Property}");
			mt.Replace(null).Should().Be.EqualTo("blah {Const} {Const} ${Property} ${Property}");
		}
	}
}