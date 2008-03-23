using System;
using System.Collections.Generic;
using System.Text;
using NHibernate.Validator.MappingSchema;
using System.IO;

namespace NHibernate.Validator
{
    public interface IMappingDocumentParser
    {
        NhvValidator Parse(Stream stream);
    }
}
