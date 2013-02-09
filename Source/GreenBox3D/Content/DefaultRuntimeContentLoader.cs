using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GreenBox3D.Content
{
    internal class DefaultRuntimeContentLoader : IRuntimeContentLoader
    {
        private class ReaderDescriptor
        {
            public readonly Type Type;
            public readonly Type Loadee;
            public readonly string Extension;

            public ReaderDescriptor(Type type)
            {
                Type = type;
                Loadee = type.BaseType.GenericTypeArguments[0];
                Extension = type.GetCustomAttribute<ContentTypeReaderAttribute>().Extension;
            }
        }

        private readonly Dictionary<Type, ReaderDescriptor> _readers;

        public DefaultRuntimeContentLoader()
        {
            AssemblyName[] references = Assembly.GetEntryAssembly().GetReferencedAssemblies();

            _readers = new Dictionary<Type, ReaderDescriptor>();

            foreach (ReaderDescriptor descriptor in from assembly in references.Select(Assembly.Load) 
                                                    from type in assembly.GetExportedTypes() 
                                                    where Attribute.IsDefined(type, typeof(ContentTypeReaderAttribute)) 
                                                    select new ReaderDescriptor(type))
                _readers.Add(descriptor.Loadee, descriptor);
        }

        public T LoadContent<T>(ContentManager manager, string filename) where T : class
        {
            ReaderDescriptor descriptor;

            if (!_readers.TryGetValue(typeof(T), out descriptor))
                return null;

            IContentTypeReader reader = (IContentTypeReader)Activator.CreateInstance(descriptor.Type);
            Stream stream = FileManager.OpenFile(filename + descriptor.Extension);

            object result = reader.Load(manager, stream);

            if (result != null)
                manager.CacheObject(filename, result);

            return (T)result;
        }
    }
}
