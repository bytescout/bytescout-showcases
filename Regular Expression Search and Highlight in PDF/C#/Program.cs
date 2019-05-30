//*******************************************************************************************//
//                                                                                           //
// Download Free Evaluation Version From: https://bytescout.com/download/web-installer       //
//                                                                                           //
// Also available as Web API! Get Your Free API Key: https://app.pdf.co/signup               //
//                                                                                           //
// Copyright © 2017-2019 ByteScout, Inc. All rights reserved.                                //
// https://www.bytescout.com                                                                 //
// https://pdf.co                                                                            //
//                                                                                           //
//*******************************************************************************************//


using System.Diagnostics;
using System.Drawing;
using Bytescout.PDFExtractor;
using Bytescout.PDF;
using Pen = Bytescout.PDF.Pen;
using SolidBrush = Bytescout.PDF.SolidBrush;

namespace SearchAndHighlightExample
{
	class Program
	{
		static void Main(string[] args)
		{
			const string inputFile = @"sample.pdf";
			const int pageIndex = 0;
			const string searchPattern = "\\d+\\.\\d+";

			// Prepare TextExtractor
			using (TextExtractor textExtractor = new TextExtractor("demo", "demo"))
			{
				textExtractor.RegexSearch = true;
				textExtractor.LoadDocumentFromFile(inputFile);

				// Load document with PDF SDK
				using (Document pdfDocument = new Document(inputFile))
				{
					pdfDocument.RegistrationName = "demo";
                	pdfDocument.RegistrationKey = "demo";

					Page pdfDocumentPage = pdfDocument.Pages[pageIndex];
					Canvas canvas = pdfDocumentPage.Canvas;

					SolidBrush fillBrush = new SolidBrush(new ColorRGB(255, 0, 0));
					fillBrush.Opacity = 50; // make the brush transparent

					// Search for pattern and highlight found pieces
					if (textExtractor.Find(pageIndex, searchPattern, caseSensitive: false))
					{
						do
						{
							foreach (var foundPiece in textExtractor.FoundText.Elements)
							{
								// Inflate the rectangle a bit
								RectangleF rect = RectangleF.Inflate(foundPiece.Bounds, 1, 2);
								// Draw rectangle over the PDF page
								canvas.DrawRectangle(fillBrush, rect);
							}
						} while (textExtractor.FindNext());
					}

					// Save as new PDF document
					pdfDocument.Save("result.pdf");

					// Open result document in default associated application (for demo purposes)
					Process.Start("result.pdf");
				}
			}
		}
	}
}
