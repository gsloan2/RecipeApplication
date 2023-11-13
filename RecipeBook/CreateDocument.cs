using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml;
using System.Windows.Forms;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;
using System;

namespace RecipesApp.RecipeBook
{
    internal class CreateDocument

    {

        public static void CreateRecipeDocument(Recipe recipe)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Word Document|*.docx";
                saveFileDialog.Title = "Save Recipe Document";
                if (saveFileDialog.ShowDialog() == DialogResult.OK && saveFileDialog.FileName != "")
                {
                    using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(saveFileDialog.FileName, WordprocessingDocumentType.Document))
                    {
                        MainDocumentPart mainPart = wordDocument.AddMainDocumentPart();
                        mainPart.Document = new Document();
                        Body body = mainPart.Document.AppendChild(new Body());

                        // Add the title
                        Paragraph titleParagraph = CreateParagraph(recipe.Title, false, "48");
                        body.AppendChild(titleParagraph);

                        // Add the ingredients
                        Paragraph ingredientsHeading = CreateParagraph("Ingredients", true, "36");
                        body.AppendChild(ingredientsHeading);

                        AddFormattedText(body, recipe.Ingredients, true);

                        // Add the instructions
                        Paragraph stepsHeading = CreateParagraph("Steps", true, "36");
                        body.AppendChild(stepsHeading);

                        AddFormattedText(body, recipe.Instructions, true);

                        wordDocument.Dispose();
                    }
                }
            }
        }

        private static void AddFormattedText(Body body, string text, bool isLastSection = false)
        {
            string[] paragraphs = text.Split(new string[] { "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string paragraph in paragraphs)
            {
                string[] lines = paragraph.Split('\n');
                foreach (string line in lines)
                {
                    if (!string.IsNullOrWhiteSpace(line)) 
                    {
                        Paragraph para = CreateParagraph("• " + line.Trim(), false, "28");
                        body.AppendChild(para);
                    }
                }
            
                if (!isLastSection)
                {
                    body.AppendChild(new Paragraph(new Run(new Break())));
                }
            }
        }

        private static Paragraph CreateParagraph(string text, bool isUnderlined, string fontSize)
        {
            Paragraph paragraph = new Paragraph();
            Run run = new Run();
            RunProperties properties = new RunProperties();

            FontSize size = new FontSize() { Val = new StringValue(fontSize) };
            properties.Append(size);

            RunFonts font = new RunFonts() { Ascii = "Times New Roman" };
            properties.Append(font);

            if (isUnderlined)
            {
                Underline underline = new Underline() { Val = UnderlineValues.Single };
                properties.Append(underline);
            }

            run.Append(properties);
            run.Append(new Text(text));
            paragraph.Append(run);

            return paragraph;
        }
    }
}
