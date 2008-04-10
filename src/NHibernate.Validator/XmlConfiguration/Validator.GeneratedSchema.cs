namespace NHibernate.Validator.MappingSchema {
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("property", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvProperty {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("asserttrue", typeof(NhvAsserttrue))]
        [System.Xml.Serialization.XmlElementAttribute("digits", typeof(NhvDigits))]
        [System.Xml.Serialization.XmlElementAttribute("email", typeof(NhvEmail))]
        [System.Xml.Serialization.XmlElementAttribute("future", typeof(NhvFuture))]
        [System.Xml.Serialization.XmlElementAttribute("ipAddress", typeof(NhvIpAddress))]
        [System.Xml.Serialization.XmlElementAttribute("length", typeof(NhvLength))]
        [System.Xml.Serialization.XmlElementAttribute("max", typeof(NhvMax))]
        [System.Xml.Serialization.XmlElementAttribute("min", typeof(NhvMin))]
        [System.Xml.Serialization.XmlElementAttribute("not-empty", typeof(NhvNotEmpty))]
        [System.Xml.Serialization.XmlElementAttribute("not-null", typeof(NhvNotNull))]
        [System.Xml.Serialization.XmlElementAttribute("notnullorempty", typeof(NhvNotnullorempty))]
        [System.Xml.Serialization.XmlElementAttribute("past", typeof(NhvPast))]
        [System.Xml.Serialization.XmlElementAttribute("pattern", typeof(NhvPattern))]
        [System.Xml.Serialization.XmlElementAttribute("range", typeof(NhvRange))]
        [System.Xml.Serialization.XmlElementAttribute("rule", typeof(NhvRule))]
        [System.Xml.Serialization.XmlElementAttribute("size", typeof(NhvSize))]
        [System.Xml.Serialization.XmlElementAttribute("valid", typeof(NhvValid))]
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
    [System.Xml.Serialization.XmlRootAttribute("asserttrue", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvAsserttrue {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("digits", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvDigits {
        
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
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("email", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvEmail {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("future", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvFuture {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("ipAddress", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvIpAddress {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("length", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvLength {
        
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
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("max", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvMax {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public long value;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool valueSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("min", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvMin {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public long value;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool valueSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("not-empty", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvNotEmpty {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("not-null", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvNotNull {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("notnullorempty", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvNotnullorempty {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("past", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvPast {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("pattern", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvPattern {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string regex;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string message;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("range", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvRange {
        
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
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("rule", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvRule {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("param")]
        public NhvParam[] param;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string attribute;
        
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
    [System.Xml.Serialization.XmlRootAttribute("param", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvParam {
        
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
    public partial class NhvSize {
        
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
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("valid", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvValid {
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("validator", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvValidator {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("class")]
        public NhvClass[] @class;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-1.0")]
    [System.Xml.Serialization.XmlRootAttribute("class", Namespace="urn:nhibernate-validator-1.0", IsNullable=false)]
    public partial class NhvClass {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("attributename")]
        public string[] attributename;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("property")]
        public NhvProperty[] property;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string @namespace;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string assembly;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name;
    }
}
