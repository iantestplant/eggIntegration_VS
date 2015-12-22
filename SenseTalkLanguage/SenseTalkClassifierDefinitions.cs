
namespace TestPlant.SenseTalkLanguage
{
    using System.ComponentModel.Composition;
    using System.Windows.Media;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Utilities;

    internal static class SenseTalkClassifierDefinitions
    {
        //Content Type and File Extension Definitions

        [Export]
        [Name("sensetalk")]
        [BaseDefinition("code")]
        internal static ContentTypeDefinition senseTalkContentTypeDefinition = null;

        [Export]
        [FileExtension(".script")]
        [ContentType("senseTalk")]
        internal static FileExtensionToContentTypeDefinition senseTalkFileExtensionDefinition = null;

        //Classification Type Definitions
        [Export]
        [Name("sensetalk")]
        //[BaseDefinition("code")]
        internal static ClassificationTypeDefinition senseTalkClassificationDefinition = null;


        #region Format definition

        [Export]
        [Name("sensetalk.comment")]
        [BaseDefinition("sensetalk")]
        internal static ClassificationTypeDefinition commentDefinition = null;

        [Export]
        [Name("sensetalk.function")]
        [BaseDefinition("sensetalk")]
        internal static ClassificationTypeDefinition functionDefinition = null;

        [Export]
        [Name("sensetalk.quote")]
        [BaseDefinition("sensetalk")]
        internal static ClassificationTypeDefinition quoteDefinition = null;

        [Export]
        [Name("sensetalk.keyword")]
        [BaseDefinition("sensetalk")]
        internal static ClassificationTypeDefinition keywordDefinition = null;

        [Export]
        [Name("sensetalk.qualifier")]
        [BaseDefinition("sensetalk")]
        internal static ClassificationTypeDefinition qualifierDefinition = null;

        [Export]
        [Name("sensetalk.number")]
        [BaseDefinition("sensetalk")]
        internal static ClassificationTypeDefinition numberDefinition = null;

        [Export]
        [Name("sensetalk.conditional")]
        [BaseDefinition("sensetalk")]
        internal static ClassificationTypeDefinition conditionalDefinition = null;

        [Export]
        [Name("sensetalk.operator")]
        [BaseDefinition("sensetalk")]
        internal static ClassificationTypeDefinition operatorDefinition = null;

        [Export]
        [Name("sensetalk.var")]
        [BaseDefinition("sensetalk")]
        internal static ClassificationTypeDefinition varDefinition = null;

        #endregion

        #region Classification Format Productions

        [Export(typeof(EditorFormatDefinition))]
        [ClassificationType(ClassificationTypeNames = "sensetalk.comment")]
        [Name("sensetalk.comment")]
        [Order(After = Priority.High)]
        //[UserVisible(true)] // If set to true, this item will show up in the Fonts and Colors option page
        internal sealed class SenseTalkComment : ClassificationFormatDefinition
        {
            public SenseTalkComment()
            {
                this.DisplayName = "SenseTalk comments"; // Name of this item in Fonts and Colors options
                this.ForegroundColor = Colors.Green;
            }
        }

        [Export(typeof(EditorFormatDefinition))]
        [ClassificationType(ClassificationTypeNames = "sensetalk.function")]
        [Name("sensetalk.function")]
        internal sealed class SenseTalkFunction : ClassificationFormatDefinition
        {
            public SenseTalkFunction()
            {
                this.ForegroundColor = Color.FromRgb(86, 19, 166); //darker purple
            }
        }

        [Export(typeof(EditorFormatDefinition))]
        [ClassificationType(ClassificationTypeNames = "sensetalk.quote")]
        [Name("sensetalk.quote")]
        internal sealed class SenseTalkQuote : ClassificationFormatDefinition
        {
            public SenseTalkQuote()
            {
                this.ForegroundColor = Color.FromRgb(163, 21, 21); //red
            }
        }

        [Export(typeof(EditorFormatDefinition))]
        [ClassificationType(ClassificationTypeNames = "sensetalk.keyword")]
        [Name("sensetalk.keyword")]
        internal sealed class KeywordFormat : ClassificationFormatDefinition
        {
            public KeywordFormat()
            {
                this.ForegroundColor = Color.FromRgb(128,9,128);  // purple
                this.IsBold = true;
            }
        }

        [Export(typeof(EditorFormatDefinition))]
        [ClassificationType(ClassificationTypeNames = "sensetalk.qualifier")]
        [Name("sensetalk.qualifier")]
        internal sealed class SenseTalkQualifier : ClassificationFormatDefinition
        {
            public SenseTalkQualifier()
            {
                this.ForegroundColor = Color.FromRgb(43,145,175);  // light Blue
                this.IsBold = true;
            }
        }

        [Export(typeof(EditorFormatDefinition))]
        [ClassificationType(ClassificationTypeNames = "sensetalk.number")]
        [Name("sensetalk.number")]
        internal sealed class SenseTalkNumber : ClassificationFormatDefinition
        {
            public SenseTalkNumber()
            {
                this.ForegroundColor = Color.FromRgb(210,121,0);  // Orange
            }
        }

        [Export(typeof(EditorFormatDefinition))]
        [ClassificationType(ClassificationTypeNames = "sensetalk.operator")]
        [Name("sensetalk.operator")]
        internal sealed class SenseTalkOperator : ClassificationFormatDefinition
        {
            public SenseTalkOperator()
            {
                this.ForegroundColor = Color.FromRgb(0,21,110);  // dark blue
                this.IsBold = true;
            }
        }


        [Export(typeof(EditorFormatDefinition))]
        [ClassificationType(ClassificationTypeNames = "sensetalk.var")]
        [Name("sensetalk.var")]
        internal sealed class SenseTalkVar : ClassificationFormatDefinition
        {
            public SenseTalkVar()
            {
                this.ForegroundColor = Color.FromRgb(0, 157, 166);  // magenta?
                this.IsBold = true;
            }
        }

        [Export(typeof(EditorFormatDefinition))]
        [ClassificationType(ClassificationTypeNames = "StringFormat")]
        [Name("StringFormat")] //The name of the Format
        [UserVisible(true)] //this should be visible to the end user
        [Order(Before = Priority.Default)] //set the priority to be after the default classifiers
        internal sealed class StringFormat : ClassificationFormatDefinition
        {

            public const string Name = "StringFormat";

            /// <summary>
            /// Defines the visual format for the "StringClassifier" classification type
            /// </summary>
            public StringFormat()
            {
                DisplayName = Name;
                ForegroundColor = Colors.DarkRed;
            }
        }

        #endregion //Format definition
    }
}
