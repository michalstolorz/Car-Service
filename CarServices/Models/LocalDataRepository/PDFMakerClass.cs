using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CarServices.Models.LocalDataRepository
{
    public class PDFMakerClass
    {
        public enum CountryCodes
        {
            US,
            AX,
            AL,
            DZ,
            AS,
            AD,
        }
        public enum CurrencyCodes
        {
            AED = 784,
            AFN = 971,
            ALL = 8,
            AMD = 51,
            ARS = 32,
            AUD = 36,
            AZN = 944,
            BAM = 977,
            BDT = 50,
            BGN = 975,
            BHD = 48,
            BND = 96,
            BOB = 68,
            BRL = 986,
            BYR = 974,
            BZD = 84,
            CAD = 124,
            CHF = 756,
            CLP = 152,
            CNY = 156,
            COP = 170,
            CRC = 188,
            CZK = 203,
            DKK = 208,
            DOP = 214,
            DZD = 12,
            EEK = 233,
            EGP = 818,
            ETB = 230,
            EUR = 978,
            GBP = 826,
            GEL = 981,
            GTQ = 320,
            HKD = 344,
            HNL = 340,
            HRK = 191,
            HUF = 348,
            IDR = 360,
            ILS = 376,
            INR = 356,
            IQD = 368,
            IRR = 364,
            ISK = 352,
            JMD = 388,
            JOD = 400,
            JPY = 392,
            KES = 404,
            KGS = 417,
            KHR = 116,
            KRW = 410,
            KWD = 414,
            KZT = 398,
            LAK = 418,
            LBP = 422,
            LKR = 144,
            LTL = 440,
            LVL = 428,
            LYD = 434,
            MAD = 504,
            MKD = 807,
            MNT = 496,
            MOP = 446,
            MVR = 462,
            MXN = 484,
            MYR = 458,
            NIO = 558,
            NOK = 578,
            NPR = 524,
            NZD = 554,
            OMR = 512,
            PAB = 590,
            PEN = 604,
            PHP = 608,
            PKR = 586,
            PLN = 985,
            PYG = 600,
            QAR = 634,
            RON = 946,
            RSD = 941,
            RUB = 643,
            RWF = 646,
            SAR = 682,
            SEK = 752,
            SGD = 702,
            SYP = 760,
            THB = 764,
            TJS = 972,
            TND = 788,
            TRY = 949,
            TTD = 780,
            TWD = 901,
            UAH = 980,
            USD = 840,
            UYU = 858,
            UZS = 860,
            VEF = 937,
            VND = 704,
            XOF = 952,
            YER = 886,
            ZAR = 710,
            ZWL = 932,
            Unknown = 0
        }

        public enum InvoiceType
        {
            Unknown = 0,
            Invoice = 380,
            Correction = 1380,
            CreditNote = 381,
            DebitNote = 383,
            SelfBilledInvoice = 389
        }

        public class Product
        {
            public string ProductID { get; set; }

            public string ProductName { get; set; }

            public float Price { get; set; }

            public float Quantity { get; set; }

            public float Total { get; set; }


        }

        public class UserDetails
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string ContactName { get; set; }
            public string City { get; set; }
            public string Postcode { get; set; }
            public CountryCodes Country { get; set; }
            public string Street { get; set; }
        }

        public class ZugferdInvoice
        {
            public string InvoiceNumber { get; set; }
            public DateTime InvoiceDate { get; set; }
            public CurrencyCodes Currency { get; set; }
            public InvoiceType Type { get; set; }

            public UserDetails Buyer { get; set; }
            public UserDetails Seller { get; set; }

            public ZugferdProfile Profile { get; set; }

            public float TotalAmount { get; set; }

            List<Product> products = new List<Product>();

            public void AddProduct(Product product)
            {
                products.Add(product);
            }

            public void AddProduct(string productID, string productName, float price, float quantity, float totalPrice)
            {
                Product product = new Product()
                {
                    ProductID = productID,
                    ProductName = productName,
                    Price = price,
                    Quantity = quantity,
                    Total = totalPrice
                };

                products.Add(product);
            }

            public ZugferdInvoice(string invoiceNumber, DateTime invoiceDate, CurrencyCodes currency)
            {
                InvoiceNumber = invoiceNumber;
                InvoiceDate = invoiceDate;
                Currency = currency;

            }

            public Stream Save(Stream stream)
            {
                XmlTextWriter writer = new XmlTextWriter(stream, Encoding.UTF8);
                writer.Formatting = Formatting.Indented;

                writer.WriteStartDocument();

                #region Header
                writer.WriteStartElement("rsm:CrossIndustryDocument");
                writer.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                writer.WriteAttributeString("xmlns", "rsm", null, "urn:ferd:CrossIndustryDocument:invoice:1p0");
                writer.WriteAttributeString("xmlns", "ram", null, "urn:un:unece:uncefact:data:standard:ReusableAggregateBusinessInformationEntity:12");
                writer.WriteAttributeString("xmlns", "udt", null, "urn:un:unece:uncefact:data:standard:UnqualifiedDataType:15");
                #endregion

                #region SpecifiedExchangedDocumentContext
                writer.WriteStartElement("rsm:SpecifiedExchangedDocumentContext");
                writer.WriteStartElement("ram:TestIndicator");
                writer.WriteElementString("udt:Indicator", "true");
                writer.WriteEndElement();
                writer.WriteStartElement("ram:GuidelineSpecifiedDocumentContextParameter");
                writer.WriteElementString("ram:ID", "urn:ferd:CrossIndustryDocument:invoice:1p0:" + Profile.ToString().ToLower());
                writer.WriteEndElement();
                writer.WriteEndElement();

                #endregion

                WriteHeaderExchangeDocument(writer);

                writer.WriteStartElement("rsm:SpecifiedSupplyChainTradeTransaction");

                writer.WriteStartElement("ram:ApplicableSupplyChainTradeAgreement");

                //Seller details.
                WriteUserDetails(writer, "ram:SellerTradeParty", Seller);

                //Buyer details
                WriteUserDetails(writer, "ram:BuyerTradeParty", Buyer);

                //End of ApplicableSupplyChainTradeAgreement
                writer.WriteEndElement();

                writer.WriteStartElement("ram:ApplicableSupplyChainTradeSettlement");

                writer.WriteElementString("ram:InvoiceCurrencyCode", Currency.ToString("g"));

                writer.WriteStartElement("ram:SpecifiedTradeSettlementMonetarySummation");

                WriteOptionalAmount(writer, "ram:GrandTotalAmount", TotalAmount);

                writer.WriteEndElement();

                writer.WriteEndElement();

                AddTradeLineItems(writer);

                writer.WriteEndDocument();
                writer.Flush();
                stream.Position = 0;
                return stream;
            }

            private void AddTradeLineItems(XmlTextWriter writer)
            {
                foreach (Product product in this.products)
                {
                    writer.WriteStartElement("ram:IncludedSupplyChainTradeLineItem");


                    if (Profile != ZugferdProfile.Basic)
                    {
                        writer.WriteStartElement("ram:SpecifiedSupplyChainTradeAgreement");
                        writer.WriteStartElement("ram:GrossPriceProductTradePrice");

                        WriteAttribute(writer, "ram:BasisQuantity", "unitCode", "KGM", product.Quantity.ToString());

                        writer.WriteEndElement();

                        writer.WriteEndElement();
                    }

                    writer.WriteStartElement("ram:SpecifiedSupplyChainTradeDelivery");
                    WriteAttribute(writer, "ram:BilledQuantity", "unitCode", "KGM", product.Quantity.ToString());

                    writer.WriteEndElement();


                    writer.WriteStartElement("ram:SpecifiedSupplyChainTradeSettlement");

                    writer.WriteStartElement("ram:SpecifiedTradeSettlementMonetarySummation");

                    WriteAttribute(writer, "ram:LineTotalAmount", "currencyID", this.Currency.ToString("g"), FormatValue(product.Price * product.Quantity));
                    writer.WriteEndElement();
                    writer.WriteEndElement();

                    writer.WriteStartElement("ram:SpecifiedTradeProduct");

                    WriteOptionalElement(writer, "ram:Name", product.ProductName);

                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }

            }

            private void WriteAttribute(XmlTextWriter writer, string tagName, string attributeName, string attributeValue, string nodeValue)
            {
                writer.WriteStartElement(tagName);
                writer.WriteAttributeString(attributeName, attributeValue);
                writer.WriteValue(nodeValue);
                writer.WriteEndElement();
            }



            private void WriteOptionalElement(XmlTextWriter writer, string tagName, string value)
            {
                if (!String.IsNullOrEmpty(value))
                {
                    writer.WriteElementString(tagName, value);
                }
            }
            private void WriteOptionalAmount(XmlTextWriter writer, string tagName, float value, int numDecimals = 2)
            {
                if (value != float.MinValue)
                {
                    writer.WriteStartElement(tagName);
                    writer.WriteAttributeString("currencyID", Currency.ToString("g"));
                    writer.WriteValue(FormatValue(value, numDecimals));
                    writer.WriteEndElement();
                }
            }


            private void WriteUserDetails(XmlTextWriter writer, string customerTag, UserDetails user)
            {
                if (user != null)
                {
                    writer.WriteStartElement(customerTag);

                    if (!String.IsNullOrEmpty(user.ID))
                    {
                        writer.WriteElementString("ram:ID", user.ID);
                    }

                    if (!String.IsNullOrEmpty(user.Name))
                    {
                        writer.WriteElementString("ram:Name", user.Name);
                    }

                    writer.WriteStartElement("ram:PostalTradeAddress");
                    writer.WriteElementString("ram:PostcodeCode", user.Postcode);
                    writer.WriteElementString("ram:LineOne", string.IsNullOrEmpty(user.ContactName) ? user.Street : user.ContactName);
                    if (!string.IsNullOrEmpty(user.ContactName))
                        writer.WriteElementString("ram:LineTwo", user.Street);
                    writer.WriteElementString("ram:CityName", user.City);
                    writer.WriteElementString("ram:CountryID", user.Country.ToString("g"));
                    writer.WriteEndElement();


                    writer.WriteEndElement();
                }
            }



            private void WriteHeaderExchangeDocument(XmlTextWriter writer)
            {
                #region HeaderExchangedDocument
                writer.WriteStartElement("rsm:HeaderExchangedDocument");
                writer.WriteElementString("ram:ID", InvoiceNumber);
                writer.WriteElementString("ram:Name", GetInvoiceTypeName(Type));
                writer.WriteElementString("ram:TypeCode", String.Format("{0}", GetInvoiceTypeCode(Type)));


                writer.WriteStartElement("ram:IssueDateTime");
                writer.WriteStartElement("udt:DateTimeString");
                writer.WriteAttributeString("format", "102");
                writer.WriteValue(ConvertDateFormat(InvoiceDate, "102"));
                writer.WriteEndElement();
                writer.WriteEndElement();

                // AddNotes(writer, Notes);

                writer.WriteEndElement();

                #endregion
            }




            private string ConvertDateFormat(DateTime date, String format = "102")
            {
                if (format.Equals("102"))
                {
                    return date.ToString("yyyyMMdd");
                }
                else
                {
                    return date.ToString("yyyy-MM-ddTHH:mm:ss");
                }
            }

            private string GetInvoiceTypeName(InvoiceType type)
            {
                switch (type)
                {
                    case InvoiceType.Invoice: return "RECHNUNG";
                    case InvoiceType.Correction: return "KORREKTURRECHNUNG";
                    case InvoiceType.CreditNote: return "GUTSCHRIFT";
                    case InvoiceType.DebitNote: return "";
                    case InvoiceType.SelfBilledInvoice: return "";
                    default: return "";
                }
            }
            private int GetInvoiceTypeCode(InvoiceType type)
            {
                if ((int)type > 1000)
                {
                    type -= 1000;
                }

                return (int)type;
            }

            private string FormatValue(float value, int numDecimals = 2)
            {
                string formatString = "0.";
                for (int i = 0; i < numDecimals; i++)
                {
                    formatString += "0";
                }
                return value.ToString(formatString).Replace(",", ".");
            }

        }

        public enum ZugferdProfile
        {
            Unknown = 0,
            Basic = 1,
            Comfort = 2,
            Extended = 3
        }
    }
}
