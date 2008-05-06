namespace NHibernate.Validator.Cfg.MappingSchema {
    
    
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("property", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmProperty {
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("assertfalse", typeof(NhvmAssertfalse))]
        [System.Xml.Serialization.XmlElementAttribute("asserttrue", typeof(NhvmAsserttrue))]
        [System.Xml.Serialization.XmlElementAttribute("creditcardnumber", typeof(NhvmCreditcardnumber))]
        [System.Xml.Serialization.XmlElementAttribute("digits", typeof(NhvmDigits))]
        [System.Xml.Serialization.XmlElementAttribute("ean", typeof(NhvmEan))]
        [System.Xml.Serialization.XmlElementAttribute("email", typeof(NhvmEmail))]
        [System.Xml.Serialization.XmlElementAttribute("fileexists", typeof(NhvmFileexists))]
        [System.Xml.Serialization.XmlElementAttribute("future", typeof(NhvmFuture))]
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
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name;
    }
    
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("assertfalse", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmAssertfalse {
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
    }
    
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("asserttrue", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmAsserttrue {
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
    }
    
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("creditcardnumber", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmCreditcardnumber {
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
    }
    
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("digits", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmDigits {
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int integerDigits;
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int fractionalDigits;
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool fractionalDigitsSpecified;
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
    }
    
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("ean", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmEan {
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
    }
    
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("email", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmEmail {
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
    }
    
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("fileexists", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmFileexists {
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
    }
    
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("future", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmFuture {
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
    }
    
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("ipaddress", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmIpaddress {
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
    }
    
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("length", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmLength {
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int min;
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool minSpecified;
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int max;
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool maxSpecified;
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
    }
    
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("max", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmMax {
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public long value;
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool valueSpecified;
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
    }
    
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("min", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmMin {
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public long value;
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool valueSpecified;
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
    }
    
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("not-empty", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmNotEmpty {
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
    }
    
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("not-null", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmNotNull {
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
    }
    
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("notnull-notempty", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmNotnullNotempty {
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
    }
    
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("past", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmPast {
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
    }
    
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("pattern", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmPattern {
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string regex;
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute("regex-options")]
        public string regexoptions;
    }
    
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("range", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmRange {
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public long min;
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool minSpecified;
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public long max;
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool maxSpecified;
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
    }
    
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("rule", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmRule {
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("param")]
        public NhvmParam[] param;
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string attribute;
    }
    
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("param", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmParam {
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name;
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value;
    }
    
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("size", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmSize {
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int min;
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool minSpecified;
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public int max;
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool maxSpecified;
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
    }
    
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("valid", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmValid {
    }
    
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("nhv-mapping", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvMapping {
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("class")]
        public NhvmClass[] @class;
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string @namespace;
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string assembly;
    }
    
    /// <comentarios/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("class", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvmClass {
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("attributename")]
        public string[] attributename;
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlElementAttribute("property")]
        public NhvmProperty[] property;
        
        /// <comentarios/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name;
    }
}
