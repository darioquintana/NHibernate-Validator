namespace NHibernate.Validator.Cfg.MappingSchema {
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("property", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmProperty {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("assertfalse", typeof(NhvmAssertfalse))]
        [System.Xml.Serialization.XmlElementAttribute("asserttrue", typeof(NhvmAsserttrue))]
        [System.Xml.Serialization.XmlElementAttribute("creditcardnumber", typeof(NhvmCreditcardnumber))]
        [System.Xml.Serialization.XmlElementAttribute("decimalmax", typeof(NhvmDecimalmax))]
        [System.Xml.Serialization.XmlElementAttribute("decimalmin", typeof(NhvmDecimalmin))]
        [System.Xml.Serialization.XmlElementAttribute("digits", typeof(NhvmDigits))]
        [System.Xml.Serialization.XmlElementAttribute("ean", typeof(NhvmEan))]
        [System.Xml.Serialization.XmlElementAttribute("email", typeof(NhvmEmail))]
        [System.Xml.Serialization.XmlElementAttribute("enum", typeof(NhvmEnum))]
        [System.Xml.Serialization.XmlElementAttribute("fileexists", typeof(NhvmFileexists))]
        [System.Xml.Serialization.XmlElementAttribute("future", typeof(NhvmFuture))]
        [System.Xml.Serialization.XmlElementAttribute("iban", typeof(NhvmIban))]
        [System.Xml.Serialization.XmlElementAttribute("ipaddress", typeof(NhvmIpaddress))]
        [System.Xml.Serialization.XmlElementAttribute("length", typeof(NhvmLength))]
        [System.Xml.Serialization.XmlElementAttribute("max", typeof(NhvmMax))]
        [System.Xml.Serialization.XmlElementAttribute("min", typeof(NhvmMin))]
        [System.Xml.Serialization.XmlElementAttribute("not-empty", typeof(NhvmNotEmpty))]
        [System.Xml.Serialization.XmlElementAttribute("not-null", typeof(NhvmNotNull))]
        [System.Xml.Serialization.XmlElementAttribute("notnull-notempty", typeof(NhvmNotnullNotempty))]
        [System.Xml.Serialization.XmlElementAttribute("past", typeof(NhvmPast))]
        [System.Xml.Serialization.XmlElementAttribute("pattern", typeof(NhvmPattern))]
        [System.Xml.Serialization.XmlElementAttribute("range", typeof(NhvmRange))]
        [System.Xml.Serialization.XmlElementAttribute("rule", typeof(NhvmRule))]
        [System.Xml.Serialization.XmlElementAttribute("size", typeof(NhvmSize))]
        [System.Xml.Serialization.XmlElementAttribute("valid", typeof(NhvmValid))]
        public object[] Items;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("assertfalse", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmAssertfalse {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tags;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("asserttrue", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmAsserttrue {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tags;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("creditcardnumber", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmCreditcardnumber {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tags;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("decimalmax", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmDecimalmax {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal value;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool valueSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tags;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("decimalmin", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmDecimalmin {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal value;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool valueSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tags;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("digits", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmDigits {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int integerDigits;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int fractionalDigits;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool fractionalDigitsSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tags;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("ean", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmEan {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tags;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("email", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmEmail {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tags;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("enum", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmEnum {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tags;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("fileexists", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmFileexists {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tags;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("future", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmFuture {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tags;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("iban", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmIban {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tags;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("ipaddress", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmIpaddress {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tags;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("length", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmLength {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int min;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool minSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int max;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool maxSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tags;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("max", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmMax {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public long value;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool valueSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tags;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("min", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmMin {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public long value;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool valueSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tags;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("not-empty", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmNotEmpty {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tags;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("not-null", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmNotNull {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tags;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("notnull-notempty", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmNotnullNotempty {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tags;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("past", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmPast {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tags;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("pattern", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmPattern {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string regex;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("regex-options")]
        public string regexoptions;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tags;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("range", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmRange {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public long min;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool minSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public long max;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool maxSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tags;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("rule", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmRule {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("param")]
        public NhvmParam[] param;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string attribute;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tags;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("param", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmParam {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("size", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmSize {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int min;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool minSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int max;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool maxSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string tags;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("valid", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmValid {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("nhv-mapping", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvMapping {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("class")]
        public NhvmClass[] @class;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string @namespace;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string assembly;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("class", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmClass {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("attributename")]
        public NhvmClassAttributename[] attributename;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("property")]
        public NhvmProperty[] property;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    public partial class NhvmClassAttributename {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string[] Text;
    }
}
