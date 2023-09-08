using CoreEndpointAPI.Models;
using System.Xml;

namespace CoreEndpointAPI.Repositories
{
    public class CurrencyRepository
    {
        public async Task<IEnumerable<CurrencyModel>> CurrenciesAsync()
        {
            var results = new List<CurrencyModel>();
            string url = "https://www.tcmb.gov.tr/kurlar/today.xml";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(url);
            var root = xmlDoc.DocumentElement;

            if (root != null)
            {
                XmlNodeList currencies = root.ChildNodes;
                

                foreach (XmlNode currency in currencies)
                {
                    decimal.TryParse(currency.ChildNodes[3]?.InnerText.Replace(".",","), out decimal buying); //3rd row in XML
                    decimal.TryParse(currency.ChildNodes[4]?.InnerText.Replace(".",","), out decimal selling); //4th row in XML
                    CurrencyModel c = new CurrencyModel()
                    {
                        Kod = currency.Attributes?["CurrencyCode"]?.Value,
                        Name = currency.ChildNodes[2]?.InnerText, //2nd row at XML source code 
                        Buying = buying,
                        Selling = selling
                    };
                    results.Add(c);  
                }


            }

            return await Task.Run(()=>results);
        }

    }
}
