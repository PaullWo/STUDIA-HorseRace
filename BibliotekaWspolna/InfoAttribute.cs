using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotekaWspolna
{
    public class InfoAttribute : Attribute
    {
		private string _nazwaPakietu;

		public string NazwaPakietu
		{
			get { return _nazwaPakietu; }
			set { _nazwaPakietu = value; }
		}

	}
}
