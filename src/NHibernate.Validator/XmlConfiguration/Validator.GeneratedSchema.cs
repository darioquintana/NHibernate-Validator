namespace NHibernate.Validator.MappingSchema {
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-0.1")]
    [System.Xml.Serialization.XmlRootAttribute("rules", Namespace="urn:nhibernate-validator-0.1", IsNullable=false)]
    public partial class NhvRules {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("future", typeof(NhvFuture))]
        [System.Xml.Serialization.XmlElementAttribute("length", typeof(NhvLength))]
        [System.Xml.Serialization.XmlElementAttribute("not-empty", typeof(NhvNotEmpty))]
        [System.Xml.Serialization.XmlElementAttribute("not-null", typeof(NhvNotNull))]
        [System.Xml.Serialization.XmlElementAttribute("past", typeof(NhvPast))]
        public object[] Items;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-0.1")]
    [System.Xml.Serialization.XmlRootAttribute("future", Namespace="urn:nhibernate-validator-0.1", IsNullable=false)]
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-0.1")]
    [System.Xml.Serialization.XmlRootAttribute("length", Namespace="urn:nhibernate-validator-0.1", IsNullable=false)]
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-0.1")]
    [System.Xml.Serialization.XmlRootAttribute("not-empty", Namespace="urn:nhibernate-validator-0.1", IsNullable=false)]
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-0.1")]
    [System.Xml.Serialization.XmlRootAttribute("not-null", Namespace="urn:nhibernate-validator-0.1", IsNullable=false)]
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-0.1")]
    [System.Xml.Serialization.XmlRootAttribute("past", Namespace="urn:nhibernate-validator-0.1", IsNullable=false)]
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
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-0.1")]
    [System.Xml.Serialization.XmlRootAttribute("property", Namespace="urn:nhibernate-validator-0.1", IsNullable=false)]
    public partial class NhvProperty {
        
        /// <remarks/>
        public NhvRules rules;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string name;
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("NhvXsd", "0.0.0.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="urn:nhibernate-validator-0.1")]
    [System.Xml.Serialization.XmlRootAttribute("validator", Namespace="urn:nhibernate-validator-0.1", IsNullable=false)]
    public partial class NhvValidator {
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("property")]
        public NhvProperty[] property;
    }
}
