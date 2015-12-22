
namespace TestPlant.SenseTalkLanguage
{
    using System.ComponentModel.Composition;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Classification;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.Language.StandardClassification;
    using Microsoft.VisualStudio.Utilities;

    [Export(typeof(IClassifierProvider))]
    [ContentType("sensetalk")]
    internal class SenseTalkClassifierProvider : IClassifierProvider
    {
        [Import]
        internal IClassificationTypeRegistryService ClassificationRegistry = null;

        public IClassifier GetClassifier(ITextBuffer buffer)
        {
            return buffer.Properties.GetOrCreateSingletonProperty(() => new SenseTalkClassifier(ClassificationRegistry));
        }
    }
}
